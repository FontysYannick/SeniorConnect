﻿
using System.ComponentModel.DataAnnotations;

namespace SeniorConnect.API.Models.Users
{
    public abstract class AbstractAuthRequest
    {
        [Required, EmailAddress(ErrorMessage = "Uw email Address is niet correct.")]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "Gebruik minimaal 6 en maximaal 100 tekens.")]
        public string Password { get; set; } = string.Empty;
    }
}
