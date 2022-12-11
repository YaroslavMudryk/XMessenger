namespace XMessenger.Identity.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNormalized { get; set; }
        public List<ClaimViewModel> Claims { get; set; }
    }
}