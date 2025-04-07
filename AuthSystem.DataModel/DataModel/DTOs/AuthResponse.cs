namespace AuthSystem.DataModel.DTOs
{
    /// <summary>
    /// Pair of tokens issued after succesfull auth.
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// JWT Bearer token issued to user after succesful auth.
        /// </summary>
        public string? BearerToken { get; set; }

        /// <summary>
        /// Refresh token issued to user along JWT after succesful auth.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
