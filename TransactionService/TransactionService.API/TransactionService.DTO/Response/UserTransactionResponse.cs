namespace TransactionService.DTO.Response
{
    /// <summary>
    /// User Transaction Response
    /// </summary>
    public class UserTransactionResponse: HttpResponseModel<UserBalance>
    {
    }

    /// <summary>
    /// User Balance
    /// </summary>
    public class UserBalance
    {
        public decimal? Balance { get; set; }
    }
}
