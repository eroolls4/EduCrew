using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduCrew.Models
{
    public class UserRolesViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
    }

}