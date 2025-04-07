using System.ComponentModel.DataAnnotations;

namespace AuthSystem.DataModel.DTOs
{
    /// <summary>
    /// Data transfer object for registering new user.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// User's user name.
        /// </summary>
        [Required(ErrorMessage = "User name is required.")]
        [StringLength(
            maximumLength: 16,
            MinimumLength = 4,
            ErrorMessage = "Invalid user name length.")]
        public string? UserName { get; set; }

        /// <summary>
        /// User's email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(
            maximumLength: 64, 
            MinimumLength = 8, 
            ErrorMessage = "Invalid password lenght.")]
        public string? Password { get; set; }

        /// <summary>
        /// User's first name.
        /// </summary>
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(
            maximumLength: 32,
            MinimumLength = 2,
            ErrorMessage = "First name has invalid length.")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Users's last name.
        /// </summary>
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(
            maximumLength: 32,
            MinimumLength = 2,
            ErrorMessage = "Last name has invalid length.")]
        public string? LastName { get; set; }
    }
}
