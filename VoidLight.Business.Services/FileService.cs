using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Infrastructure.Common.Exceptions;

namespace VoidLight.Business.Services
{
    public class FileService : IFileService
    {
        private readonly FirebaseSettings _firebaseSettings;

        public FileService(IOptions<FirebaseSettings> appSettings)
        {
            _firebaseSettings = appSettings.Value;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file is null || file.Length <= 0)
            {
                throw new NoFileException("No file sent!");
            }

            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseSettings.API));
            var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseSettings.AuthEmail, _firebaseSettings.AuthPassword);

            var cancellation = new CancellationTokenSource();

            try
            {
                var timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                var name = timeStamp+ "_" + file.FileName;

                var upload = new FirebaseStorage(
                        _firebaseSettings.Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true

                        }
                    )
                    .Child("Media")
                    .Child(name)
                    .PutAsync(file.OpenReadStream(), cancellation.Token);

                var path = await upload;

                return path;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }
    }
}
