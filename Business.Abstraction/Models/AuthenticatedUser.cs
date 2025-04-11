namespace Business.Abstraction.Models
{
    /// <summary>
    /// Authenticated user model
    /// </summary>
    public class AuthenticatedUser
    {
        /// <summary>
        /// User Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// User Role
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}