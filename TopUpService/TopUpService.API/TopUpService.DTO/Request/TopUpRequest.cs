namespace TopUpService.DTO.Request
{
    public class TopUpRequest
    {
        public int UserId { get; set; }
        public int BeneficiaryId { get; set; }
        public decimal Amount { get; set; }
    }
}
