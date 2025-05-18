namespace CPMethod.DataModel.DTOs
{
    public class Result
    {
        public string task { get; set; } = string.Empty;
        public int earliestStart { get; set; }
        public int earliestFinish { get; set; }
        public int latestStart { get; set; }
        public int latestFinish { get; set; }
        public int slack { get; set; }
        public int duration { get; set; }
        public bool isCritical { get; set; }
    }
}
