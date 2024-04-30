namespace TopUpService.Database.Entities
{
    /// <summary>
    /// User Entity
    /// </summary>
    public class User:BaseEntity
    {
        public bool IsVerified { get; set; }

        public List<Beneficiary> Beneficiaries { get; }
        public List<TopUpTransaction> TopUpTransactions { get; }

    }
}
