using Web_app_personal_collections.Models.Entities;

namespace Web_app_personal_collections.ViewModels
{
    public class CollectionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }

        public DateTime DateTimeCollectionAdded { get; set; }

        public int CategoryId { get; set; }

        public string Image { get; set; }

        public int NumberOfItems { get; set; }

        public int NumberOfLikes { get; set; }

        public List<TagModel> Tags { get; set; }


    }
}
