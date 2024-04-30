using System.ComponentModel.DataAnnotations;

namespace TopUpService.Database.Entities
{
    /// <summary>
    /// Top Up Beneficiary Entity
    /// </summary>
    public class Beneficiary:BaseEntity
    {
        [MaxLength(20)]
        public string NickName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<TopUpTransaction> TopUpTransactions { get; }

    }
}
