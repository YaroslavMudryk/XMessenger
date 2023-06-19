namespace XMessenger.Helpers.Db
{
    public interface IAuditEntity
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
}