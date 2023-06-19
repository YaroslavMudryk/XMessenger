namespace XMessenger.Database.Export.Models
{
    public class FacultyModel : ExportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SpecialtyModel> Specialties { get; set; }
    }
}
