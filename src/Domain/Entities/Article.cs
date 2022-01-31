using Domain.Common;

namespace Domain.Entities
{
    public class Article : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
