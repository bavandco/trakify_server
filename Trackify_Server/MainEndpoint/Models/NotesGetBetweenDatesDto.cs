namespace MainEndpoint.Models
{
    public class NotesGetBetweenDatesDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
    }
}
