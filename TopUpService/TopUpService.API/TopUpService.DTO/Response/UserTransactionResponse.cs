namespace TopUpService.DTO.Response
{
    public class UserTransactionResponse : HttpResponseModel<UserBalance>
    {
    }

    public class UserBalance
    {
        public decimal? Balance { get; set; }
    }
}
