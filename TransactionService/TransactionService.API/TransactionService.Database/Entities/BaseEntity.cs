using System.ComponentModel.DataAnnotations;

namespace TransactionService.Database.Entities
{
    /// <summary>
    /// Base Entity Class
    /// </summary>
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
