namespace MeDirect_Currency_Exchange_API.Models.DTOs {
    public class ErrorResponse {

        public string type {  get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
        public string? TraceId { get; set; }
        public ErrorResponse(int statusCode, string Title,string Type = "Error", Dictionary<string, List<string>>? errors = null, string? traceId = null) {
            type = Type;
            status = statusCode;
            title = Title;
            Errors = errors;
            TraceId = traceId;
        }
    }
}
