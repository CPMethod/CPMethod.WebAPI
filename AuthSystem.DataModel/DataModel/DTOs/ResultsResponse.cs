namespace CPMethod.DataModel.DTOs
{
    public class ResultsResponse
    {
        public IEnumerable<Result> results { get; set; } = Enumerable.Empty<Result>();
    }
}
