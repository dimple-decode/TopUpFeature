namespace TransactionService.DTO.Response
{
    /// <summary>
    /// Http Response Model
    /// </summary>
    public class HttpResponseModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Http Response Model For Dynamic Object Response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpResponseModel<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
    }
}
