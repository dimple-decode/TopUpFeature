namespace TopUpService.Common
{
    public enum TransactionType
    {
        DEBIT,
        CREDIT
    }

    public static class Constants
    {
        public static List<decimal> TopUpOptions = new List<decimal>()
            {
                    5,10,20,30,50,75,100
            };

        public const string ContentTypeHeader = "Content-Type";
    }

  

}