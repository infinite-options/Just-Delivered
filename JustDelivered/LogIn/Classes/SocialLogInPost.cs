﻿using System;
namespace JustDelivered.LogIn.Classes
{
    public class SocialLogInPost
    {
        public string email { get; set; }
        public string password { get; set; }
        public string social_id { get; set; }
        public string signup_platform { get; set; }
    }
}
