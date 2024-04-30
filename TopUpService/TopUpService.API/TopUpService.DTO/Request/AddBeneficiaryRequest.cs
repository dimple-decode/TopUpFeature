namespace TopUpService.DTO.Request
{
    /// <summary>
    /// Add Beneficiary Request 
    /// </summary>
    public class AddBeneficiaryRequest
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
