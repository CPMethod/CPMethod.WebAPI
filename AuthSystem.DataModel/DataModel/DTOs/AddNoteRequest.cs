using System.ComponentModel.DataAnnotations;

namespace AuthSystem.DataModel.DTOs
{
    public class AddNoteRequest
    {
        public string? Title { get; set; }

        public string? Content { get; set; }
    }
}
