
namespace CommonBoilerPlateEight.Domain.Models.Permission
{
    public class Permission
    {
        public const string PermissionClaimType = "Permission";

        public Dictionary<string, List<string>> PermissionDictionary { get; set; }
        = new Dictionary<string, List<string>>
        {
                {
                    "User", new List<string>
                    {
                        "View",
                        "Create",
                        "Update",
                        "Lock",
                        "Unlock",
                        "ResetPassword"
                    }
                },
                {
                    "Role", new List<string>
                    {
                        "View",
                        "Create",
                        "Update"
                    }
                },
                {
                    "AuditLog", new List<string>
                    {
                        "View",
                    }
                },
                {
                    "ActivityLog", new List<string>
                    {
                        "View",
                    }
                },
                {
                    "Customer", new List<string>
                    {
                        "View",
                    }
                }
        };


        public IEnumerable<string> Permissions =>
           PermissionDictionary.SelectMany(
               p =>
                   p.Value.Select(i => $"{p.Key}-{i}")
           );
    }
}
