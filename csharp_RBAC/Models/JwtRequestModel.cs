namespace csharp_RBAC.Models
{
    public class JwtRequestModel
    {
        public string Email { get; set; }
        public List<string> Scopes { get; set; }
        public string Role { get; set; }
    }
}
