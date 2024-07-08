namespace Repository.Results
{
    public class ErrorResult
    {
        public string message { get; set; }
        public string tittle { get; set; }
        public string status { get; set; }

        public string traceId { get; set; }

        public Dictionary<string, List<string>> result { get; set; } = new Dictionary<string, List<string>>();

    }
}
