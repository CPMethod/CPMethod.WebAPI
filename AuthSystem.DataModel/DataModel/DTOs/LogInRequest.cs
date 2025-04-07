using System.ComponentModel.DataAnnotations;

namespace AuthSystem.DataModel.DTOs
{
    public class LogInRequest
    {
        /// <summary>
        /// User's password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(
            maximumLength: 64,
            MinimumLength = 8, 
            ErrorMessage = "Invalid password length.")]
        public string? Password { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }
    }
}
