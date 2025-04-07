using AuthSystem.DataModel;

namespace AuthSystem.Client.DataStore
{
    public class LogInData : BaseModel
    {
        public string? BearerToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
