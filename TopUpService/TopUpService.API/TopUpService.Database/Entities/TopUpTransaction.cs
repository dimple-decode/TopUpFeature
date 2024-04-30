namespace TopUpService.Database.Entities
{

    public class TopUpTransaction:BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public int BeneficiaryId { get; set; }
        public User User { get; set; }
        public Beneficiary Beneficiary { get; set; }
    }
}
