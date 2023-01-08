namespace XMessenger.Domain.Models.Database
{
    public class Faculty : BaseModel<int>
    {
        public string Name { get; set; }
        public int UniversityId { get; set; }
        public University University { get; set; }
        public List<Specialty> Specialties { get; set; }
    }
}
