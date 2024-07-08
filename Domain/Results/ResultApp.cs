namespace Repository.Results
{
    public class ResultApp
    {
        public bool? Succeeded { get; set; }
        public ErrorResult? errors { get; set; }
        public string? message { get; set; }
        public object? objectResult { get; set; }
    }
}