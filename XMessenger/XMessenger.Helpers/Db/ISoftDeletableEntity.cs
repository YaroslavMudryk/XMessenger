namespace XMessenger.Helpers.Db
{
    public interface ISoftDeletableEntity
    {
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
    }
}