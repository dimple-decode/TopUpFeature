using TopUpService.Database.Entities;

namespace TopUpService.DTO.Response
{
    public class GetBeneficiaryResponse : HttpResponseModel<List<BeneficiaryDetails>>
    {
    }

    public class BeneficiaryDetails
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }

    }
}
