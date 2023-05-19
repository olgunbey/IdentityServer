namespace IdentityOrnek.Areas.Admin.Models
{
    public class AssignRoleToUserModelView
    {
        public string? Id { get; set; }
        public string RoleName { get; set; } = null;
        public bool Exist { get; set; }
    }
}
