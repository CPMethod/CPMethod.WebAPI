namespace CPMethod.DataModel.DTOs
{
    public class CPMResponse
    {
        public string svg { get; set; } = string.Empty;
        public IEnumerable<Node> nodes { get; set; } = Enumerable.Empty<Node>();
    }
}
