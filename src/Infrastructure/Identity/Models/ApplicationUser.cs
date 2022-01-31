using Microsoft.AspNetCore.Identity;
using System;

namespace Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public override int Id { get; set; }
        public bool IsDelete { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }
    }
}
