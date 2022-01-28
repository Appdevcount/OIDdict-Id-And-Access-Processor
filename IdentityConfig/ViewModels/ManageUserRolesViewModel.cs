using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_And_Access_Management
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }//new
        public IList<UserRolesViewModel> UserRoles { get; set; }//new
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
    //public class ManageUserRolesViewModel
    //{
    //    public string UserId { get; set; }
    //    public IList<UserRolesViewModel> UserRoles { get; set; }
    //}

    //public class UserRolesViewModel
    //{
    //    public string RoleName { get; set; }
    //    public bool Selected { get; set; }
    //}
}
