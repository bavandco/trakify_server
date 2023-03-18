namespace AdminEndpoint.Models
{
    public class AdminGetUserNotesDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string UserId { get; set; }
    }
}
