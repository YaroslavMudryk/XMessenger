namespace XMessenger.Database.ViewModels
{
    public class MetroViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Description { get; set; }
        public List<MetroLineViewModel> Lines { get; set; }
    }
}