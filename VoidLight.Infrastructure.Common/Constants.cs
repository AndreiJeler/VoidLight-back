using System;
using System.Collections.Generic;
using System.Text;

namespace VoidLight.Infrastructure.Common
{
    public static class Constants
    {
        public const string EXAMPLE = "example text for sth";

        #region URL
        public const string APP_URL = "https://localhost:44324/";
        public const string CLIENT_URL = "https://localhost:4200/";

        #endregion

        #region HttpStatus
        public const string HTTP_CREATED = "created";
        public const string HTTP_UPDATED = "updated";
        #endregion HttpStatus

        #region Images
        public const string DEFAULT_IMAGE_USER = "Images\\Default\\defaultUserPicture.gif";

        #endregion


        #region Exceptions
        public const string AUTHENTICATION_EXCEPTION = "Authentication failed";
        public const string SEND_EMAIL_EXCEPTION = "Can't send the email";
        public const string INVALID_PARAMETER_EXCEPTION = "Invalid Parameter";
        public const string UNAUTHORIZED_EXCEPTION = "Unauthorized access";
        public const string ACCOUNT_ALREADY_CONFIRMED_EXCEPTION = "Account already confirmed";

        #endregion


        #region Email
        public const string ACCOUNT_ACTIVATION_SUBJECT = "Account activation";
        public const string ACCOUNT_ACTIVATION_BODY = "Click on the bellow link for activate your account \n";
        public const string ACCOUNT_ACTIVATION_LINK = CLIENT_URL + "account-activation?token=";
        public const string SMTP_CLIENT = "smtp.gmail.com";
        public const int SMTP_PORT = 587;
        public const string TEMPORARY_PASSWORD_MESSAGE = "There is your temporary pessword. You MUST change it in your Profile section after login. ";
        public const string RESET_PASSWORD_MESSAGE = "Your password has been changed";
        public const string RESET_PASSWORD_SUBJECT = "Reset password";
        #endregion Email

        #region User
        public const string PASSWORD_TEMPLATE = "000000";
        public const string AUTHORIZATION_HEADER = "Authorization";
        public const string AUTHORIZATION_ITEM = "User";
        public const string AUTHORIZATION_ID = "id";
        public const string UNAUTHORIZED_MESSAGE = "Unauthorized";

        #endregion

    }
}
