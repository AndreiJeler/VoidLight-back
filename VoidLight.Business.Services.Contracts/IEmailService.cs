using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Data.Entities;

namespace VoidLight.Business.Services.Contracts
{
    public interface IEmailService
    {
        Task SendActivationEmail(User user, string token);
        Task SendResetPasswordEmail(User user, bool isForgotten);
    }
}
