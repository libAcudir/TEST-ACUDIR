namespace Acudir.Test.Apis.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Object Data { get; set; }
        public List<string> Errors { get; set; }


        public ApiResponse()
        {
            Errors = new List<string>();
        }
    }
}
