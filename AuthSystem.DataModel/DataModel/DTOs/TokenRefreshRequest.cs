using System.ComponentModel.DataAnnotations;

namespace AuthSystem.DataModel.DTOs
{
    /// <summary>
    /// Data Transfer Object for refreshing JWT.
    /// </summary>
    public class TokenRefreshRequest
    {
        /// <summary>
        /// Expired (or simply requested to be refreshed) JSON Web Token.
        /// </summary>
        [Required(ErrorMessage = "Bearer token is required.")]
        public string? BearerToken { get; set; }

        /// <summary>
        /// Refresh token.
        /// </summary>
        [Required(ErrorMessage = "Refresh token is required.")]
        public string? RefreshToken { get; set; }
    }
}
