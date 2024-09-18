using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduCrew.Models
{
    public class EduCrewUser : IdentityUser
    {
     
        public string Bio {  get; set; }
        public string Description {  get; set; }

        public string ProfileImage { get; set; }

    }
}