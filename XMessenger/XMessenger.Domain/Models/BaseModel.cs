using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace XMessenger.Domain.Models
{
    public class BaseModel
    {
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        [Required]
        public string CreatedFromIP { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
        public int LastUpdatedBy { get; set; }
        public string LastUpdatedFromIP { get; set; }

        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSoftDelete { get; set; }
        public int DeletedBy { get; set; }

        public int Version { get; set; }
    }

    public class BaseModel<T> : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
    }

    public class BaseModelWithoutIdentity<T>: BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public T Id { get; set; }
    }
}