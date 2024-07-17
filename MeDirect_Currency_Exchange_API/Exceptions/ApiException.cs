using MeDirect_Currency_Exchange_API.Models.DTOs;

namespace MeDirect_Currency_Exchange_API.Exceptions {
    public class ApiException : Exception {
        public int ErrorCode { get; }
        public ErrorResponse ErrorResponse { get; }
        public ApiException(int errorCode, string message,string title, string? details = null, Dictionary<string, List<string>>? errors = null, string? traceId = null) : base(message) {
            errors = new Dictionary<string, List<string>>();
            errors.Add("Error", [message ]);
            ErrorResponse = new ErrorResponse(errorCode, title, "Error", errors,traceId);
            ErrorCode = errorCode;
        }
    }
}
