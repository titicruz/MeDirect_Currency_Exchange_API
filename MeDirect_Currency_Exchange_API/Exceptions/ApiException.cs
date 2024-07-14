namespace MeDirect_Currency_Exchange_API.Exceptions {
    public class ApiException : Exception {
        public int ErrorCode { get; }
        public ApiException(int errorCode, string message) : base(message) {
            ErrorCode = errorCode;
        }
    }
}
