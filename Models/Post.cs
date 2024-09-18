using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduCrew.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }  

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}