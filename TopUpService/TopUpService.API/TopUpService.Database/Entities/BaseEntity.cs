using System.ComponentModel.DataAnnotations;

namespace TopUpService.Database.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
