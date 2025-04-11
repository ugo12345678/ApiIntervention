
using System.Security.Claims;
using Business.Abstraction.Models;

namespace Business.Extensions
{
    /// <summary>
    /// <see cref="ClaimsPrincipal"/> extension class
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// NameIdenfifier Claim Type (standard xmlsoap)
        /// </summary>
        public const string NameIdentifierClaimType = "preferred_username";
        /// <summary>
        /// FirstName Claim Type (standard xmlsoap)
        /// </summary>
        public const string FirstNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
        /// <summary>
        /// Last Name Claim Type (custom LiveTool)
        /// </summary>
        public const string LastNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        /// <summary>
        /// Last Name Claim Type (custom LiveTool)
        /// </summary>
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        /// <summary>
        /// Get the current <see cref="IUser"/> from claims
        /// </summary>
        /// <returns></returns>
        public static AuthenticatedUser GetUser(this ClaimsPrincipal claimsPrincipal)
        {
            AuthenticatedUser result = null;

            if (claimsPrincipal is not null)
            {
                IEnumerable<Claim> claims = claimsPrincipal.Claims;
                result = new AuthenticatedUser()
                {
                    Id = Guid.Empty,
                    Username = GetClaimTypeValue(claims, NameIdentifierClaimType),
                    FirstName = GetClaimTypeValue(claims, FirstNameClaimType),
                    LastName = GetClaimTypeValue(claims, LastNameClaimType),
                    Role = GetClaimTypeValue(claims, Role)
                };
            }

            return result;
        }

        /// <summary>
        /// Returns specified <see cref="Claim"/> type value from specified <see cref="Claim"/> list
        /// </summary>
        /// <param name="claims"><see cref="Claim"/> list to search in</param>
        /// <param name="claimType"><see cref="Claim"/> type to search for</param>
        /// <returns><see cref="Claim"/> value if found, null otherwise</returns>
        private static string GetClaimTypeValue(IEnumerable<Claim> claims, string claimType)
        {
            return claims.FirstOrDefault(p => p.Type == claimType)?.Value;
        }
    }
}
