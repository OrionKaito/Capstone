using System.Collections.Generic;

namespace Capstone.ViewModel
{
    public class AuthorizationVM
    {
        public IEnumerable<string> Roles;
        public IEnumerable<string> Groups;
        public IEnumerable<string> Permissions;
    }
}
