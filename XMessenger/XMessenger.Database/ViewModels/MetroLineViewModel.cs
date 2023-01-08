namespace XMessenger.Database.ViewModels
{
    public class MetroLineViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public List<MetroStationViewModel> Stations { get; set; }
    }
}