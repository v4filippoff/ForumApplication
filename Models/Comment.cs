using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int DiscussionId { get; set; }
        public Discussion Discussion { get; set; }
    }
}
