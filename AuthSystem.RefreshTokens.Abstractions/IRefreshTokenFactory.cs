using AuthSystem.DataModel;
using AuthSystem.RefreshTokens.DataModel;

namespace AuthSystem.RefreshTokens.Abstractions
{
    /// <summary>
    /// Generating refresh tokens.
    /// </summary>
    public interface IRefreshTokenFactory
    {
        /// <summary>
        /// Creates new refresh token.
        /// </summary>
        /// <param name="jti">ID of JSON Web Token to connect with this refresh token.</param>
        /// <param name="userId">ID of user that token is generated for.</param>
        /// <returns>Newly created refresh token.</returns>
        RefreshToken CreateToken(Guid jti, string userId);
    }
}