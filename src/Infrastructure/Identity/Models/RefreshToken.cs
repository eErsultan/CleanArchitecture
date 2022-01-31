using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public int UserId { get; set; }
    }
}
