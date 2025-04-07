namespace AuthSystem.DataModel.DTOs
{
    public class UserDto
    {
        /// <summary>
        /// User's e-mail address.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// User's username.
        /// </summary>
        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
