using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduCrew.Models
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Category { get; set; }

    }
}