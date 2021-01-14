﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Business;
using VoidLight.Data.Entities;
using VoidLight.Data.Mappers;

namespace VoidLight.Business.Services
{
    public class PostService : IPostService
    {
        private readonly VoidLightDbContext _context;
        private IFileService _fileService;

        public PostService(VoidLightDbContext context, IFileService service)
        {
            _context = context;
            _fileService = service;
        }

        public async Task<PostDto> AddPost(string postJSON, IFormFileCollection files)
        {

            var post = DeserializePost(postJSON);

            var userPost = PostMapper.ConvertDtoToEntity(post);
            userPost.IsShared = false;
            var dbGame = await _context.Games.FirstOrDefaultAsync(game => game.Name == post.Game);
            var dbUser = await _context.Users.FirstOrDefaultAsync(user => user.Id == post.UserId);

            var contents = new List<Content>();

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    var splits = file.FileName.Split('.');
                    var extension = splits[splits.Length - 1];

                    var path = await this._fileService.UploadFileAsync(file);

                    if (extension == "jpg" || extension == "png")
                    {
                        contents.Add(new ImageContent() { ContentPath = path });
                    }

                    else
                    {
                        contents.Add(new VideoContent() { ContentPath = path });
                    }
                }
            }

            var dbPost = new Post()
            {
                Comments = new List<PostComment>(),
                Likes = new List<PostLike>(),
                Content = contents,
                Game = dbGame,
                Text = post.Text,
                Time = DateTime.Now.ToUniversalTime(),
            };


            userPost.Post = dbPost;
            userPost.User = dbUser;
            userPost.Timestamp = dbPost.Time;

            await _context.UserPosts.AddAsync(userPost);

            await _context.SaveChangesAsync();

            return PostMapper.ConvertEntityToDto(userPost, post.UserId);
        }

        public async Task DeletePost(int postId, int userId)
        {
            var post = await _context.Posts.Include(p => p.UserPosts).ThenInclude(up => up.User).FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
            {
                throw new Exception("Post does not exist");
            }
            if (post.UserPosts.FirstOrDefault(up => up.IsShared == false).UserId == userId)
            {
                _context.Posts.Remove(post);
            }
            else
            {
                var userPost = post.UserPosts.FirstOrDefault(up => up.UserId == userId);
                _context.UserPosts.Remove(userPost);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Post> FindPost(int postId)
        {
            return _context.Posts
                .Include(post => post.Comments)
                .Include(post => post.UserPosts).ThenInclude(up => up.User)
                .Include(post => post.Likes)
                .FirstOrDefaultAsync(post => post.Id == postId);
        }

        public async Task<ICollection<PostDto>> GetGamePosts(int gameId, int userId)
        {
            var dbGame = await _context.Games.Include(g => g.Posts).FirstOrDefaultAsync(game => game.Id == gameId);

            var postsForGame = await _context.Posts
                .Include(post => post.Game)
                .Include(post => post.Likes)
                .Include(post => post.Content)
                .Include(post => post.UserPosts).ThenInclude(up => up.User)
                .Where(post => post.Game == dbGame).ToListAsync();

            return await _context.UserPosts
                .Include(up => up.Post).ThenInclude(up => up.Game)
                .Include(up => up.User)
                .Include(up => up.Post).ThenInclude(p => p.Likes)
                .Include(up => up.Post).ThenInclude(p => p.Content)
                .Include(up => up.Post).ThenInclude(p => p.Comments).ThenInclude(c => c.User).ThenInclude(u => u.Role)
                .Where(up => postsForGame.Contains(up.Post))
                .OrderByDescending(up => up.Post.Time)
                .Select(up => PostMapper.ConvertEntityToDto(up, userId))
                .ToListAsync();
        }

        public async Task<ICollection<PostDto>> GetGamePublisherPosts(int gamePublisherId)
        {
            HashSet<PostDto> posts = new HashSet<PostDto>(new PostDtoComparer());

            var gamePublisher = await _context.GamePublishers.FirstOrDefaultAsync(gp => gp.Id == gamePublisherId);

            /*            foreach (var game in gamePublisher.Games)
                        {
                            posts.UnionWith(await GetGamePosts(game.Id));
                        }*/


            return posts.OrderByDescending(p => p.Time).ToList();
        }

        public async Task<IEnumerable<CommentDto>> GetPostComments(int postId)
        {
            var post = await _context.Posts.Include(p => p.Comments).ThenInclude(c => c.User).ThenInclude(u => u.Role).FirstOrDefaultAsync(p=>p.Id==postId); 
            return post.Comments.Select(comm => CommentMapper.ConvertEntityToDto(comm)).AsEnumerable();

        }

        public ICollection<PostDto> GetPostsByUser(int userId, int feedUserId)
        {
            var userPosts= _context.UserPosts
                .Include(up => up.User)
                .Include(up => up.Post).ThenInclude(p => p.Game)
                .Include(up => up.Post).ThenInclude(p => p.Content)
                .Include(up => up.Post).ThenInclude(p => p.Comments).ThenInclude(c => c.User).ThenInclude(u => u.Role)
                .Include(up => up.Post).ThenInclude(p => p.Likes)
                .ToList();
            return userPosts.Where(up=>up.UserId==userId).Select(up => PostMapper.ConvertEntityToDto(up, feedUserId)).ToList().OrderByDescending(up=>up.Time).ToList();
        }

        public async Task<ICollection<PostDto>> GetPostsForUserFeed(int userId)
        {
            var user = await _context.Users
                .Include(u => u.GameUsers).ThenInclude(ug => ug.Game).ThenInclude(g => g.Posts)
                .Include(u => u.FriendsList).ThenInclude(f => f.FriendUser)
                .FirstOrDefaultAsync(user => user.Id == userId);

            var games = user.GameUsers.Select(gu=>gu.Game.Id);
            var friends = user.FriendsList.Select(f => f.FriendUserId);

            var posts = _context.UserPosts
                .Include(up => up.Post).ThenInclude(up => up.Game)
                .Include(up => up.User)
                .Include(up => up.Post).ThenInclude(p => p.Likes)
                .Include(up => up.Post).ThenInclude(p => p.Content)
                .Where(up => games.Contains(up.Post.Game.Id) || friends.Contains(up.UserId) || user.Id == up.UserId)
                .Select(up => PostMapper.ConvertEntityToDto(up, user.Id))
                .ToList();

            return posts.OrderByDescending(up => up.Time).ToList();
        }


        public async Task<CommentDto> PostComment(int postId, int userId, string commentText)
        {
            var post = await FindPost(postId);
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(us => us.Id == userId);
            var comment = new PostComment()
            {
                Post = post,
                PostId = postId,
                User = user,
                UserId = userId,
                Text = commentText,
                TimeStamp = DateTime.Now.ToUniversalTime()
            };
            post.Comments.Add(comment);
            _context.Update(post);
            await _context.SaveChangesAsync();
            return CommentMapper.ConvertEntityToDto(comment);
        }


        public async Task<CommentDto> PostComment(int postId, int userId, string commentText)
        {
            var post = await FindPost(postId);
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(us => us.Id == userId);
            var comment = new PostComment()
            {
                Post = post,
                PostId = postId,
                User = user,
                UserId = userId,
                Text = commentText,
                TimeStamp = DateTime.Now.ToUniversalTime()
            };
            post.Comments.Add(comment);
            _context.Update(post);
            await _context.SaveChangesAsync();
            return CommentMapper.ConvertEntityToDto(comment);
        }

        public async Task<PostDto> PostShare(int postId, int userId)
        {
            var post = await FindPost(postId);
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(us => us.Id == userId);
            if (post.UserPosts.FirstOrDefault(p => p.IsShared == false).UserId==userId)
            {
                throw new Exception("You cannot share your own post!");
            }
            try
            {
                var userPost = new UserPost()
                {
                    Post = post,
                    PostId = postId,
                    User = user,
                    UserId = userId,
                    IsShared = true,
                    Timestamp = DateTime.Now.ToUniversalTime()
                };
                post.UserPosts.Add(userPost);
                _context.Update(post);
                await _context.SaveChangesAsync();
                return PostMapper.ConvertEntityToDto(userPost, userId);
            }
            catch
            {
                throw new Exception("You have already shared this post!");
            }
            }

        public async Task<int> UserLikePost(int postId, int userId)
        {
            var dbPost = await _context.Posts.Include(p => p.Likes).FirstOrDefaultAsync(post => post.Id == postId);
            var dbUser = await _context.Users.Include(u => u.UserPostLikes).FirstOrDefaultAsync(user => user.Id == userId);

            PostLike postLike = new PostLike()
            {
                Post = dbPost,
                PostId = postId,
                User = dbUser,
                UserId = userId
            };

            var like = await _context.Likes.FirstOrDefaultAsync(like => like.UserId == userId && like.PostId == postId);

            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
                return dbPost.Likes.Count;
            }
            else
            {
                await _context.Likes.AddAsync(postLike);
                await _context.SaveChangesAsync();
                return postLike.Post.Likes.Count;
            }
        }

        private static PostDto DeserializePost(string postJSON)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<PostDto>(postJSON, options);
        }
    }
}
