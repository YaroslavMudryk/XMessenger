namespace XMessenger.Database.ViewModels
{
    public class FacultyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SpecialtyViewModel> Specialties { get; set; }
    }
}