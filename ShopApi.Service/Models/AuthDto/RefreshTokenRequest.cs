﻿namespace ShopApi.Service.Models.AuthDto
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}