namespace XMessenger.Identity.Dtos
{
    public class RoleDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int[] ClaimIds { get; set; }
    }
}