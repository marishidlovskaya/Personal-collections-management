

using System.ComponentModel.DataAnnotations;

namespace Web_app_personal_collections.Models.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public ICollection<Collection> Collections { get; set; }
    }
}
