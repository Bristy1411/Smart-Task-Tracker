namespace SmartTaskTracker.API.DTOs
{
    public class UserSummaryDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public int TaskCount { get; set; } = 0;
    }
}
