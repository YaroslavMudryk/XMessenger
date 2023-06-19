namespace XMessenger.Helpers
{
    public class BaseModel : IAuditEntity
    {
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        [Required]
        public string CreatedFromIP { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
        public int? LastUpdatedBy { get; set; }
        public string LastUpdatedFromIP { get; set; }

        public int Version { get; set; }
    }

    public class BaseSoftDeletableModel<T> : BaseModel<T>, ISoftDeletableEntity
    {
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
    }

    public class BaseModel<T> : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
    }
}