using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Discussion
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public Discussion()
        {
            Comments = new List<Comment>();
        }
    }
}
