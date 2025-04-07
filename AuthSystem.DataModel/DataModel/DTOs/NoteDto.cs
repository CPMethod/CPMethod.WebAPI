namespace AuthSystem.DataModel.DTOs
{
    public class NoteDto : BaseModel
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
