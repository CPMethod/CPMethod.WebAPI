namespace AuthSystem.DataModel
{
    public class Note : BaseModel
    {
        public string? Title { get; set; }
        
        public string? Content { get; set; }   

        public DateTime? CreatedAt { get; set; }


        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
