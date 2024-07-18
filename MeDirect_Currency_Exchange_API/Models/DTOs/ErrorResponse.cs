using System.Text.Json.Serialization;

namespace MeDirect_Currency_Exchange_API.Models.DTOs {
    public class ErrorResponse {
        [JsonPropertyName("type")]
        public string Type {  get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("Errors")]
        public Dictionary<string, List<string>>? Errors { get; set; }
        [JsonPropertyName("TraceId")]
        public string? TraceId { get; set; }
        public ErrorResponse(int statusCode, string title,string type = "Error", Dictionary<string, List<string>>? errors = null, string? traceId = null) {
            Type = type;
            Status = statusCode;
            Title = title;
            Errors = errors;
            TraceId = traceId;
        }
    }
}
