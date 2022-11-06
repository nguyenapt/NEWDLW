using System.Web.Security;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Dlw.EpiBase.Content.Infrastructure.Data.EnsureDatabase
{
    [InitializableModule]
    [ModuleDependency(typeof(EnsureDatabaseInitializableModule))]
    public class EnsureAdminUserAndRoles : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var adminUser = Membership.GetUser("admin");

            if (adminUser != null) return;

            Membership.CreateUser("admin", "default", "admin@host.local");

            EnsureRole("Administrators");
            EnsureRole("WebEditors");
            EnsureRole("WebAdmins");

            Roles.AddUserToRoles("admin", new[] { "Administrators", "WebAdmins", "WebEditors" });
        }

        public void Uninitialize(InitializationEngine context)
        {
            // do nothing
        }

        private void EnsureRole(string roleName)
        {
            if (Roles.RoleExists(roleName)) return;

            Roles.CreateRole(roleName);
        }
    }
}