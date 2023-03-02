namespace Web_app_personal_collections.Models.Entities
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime DateTimeCollectionAdded { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Image { get; set; }


        public ICollection<Tag> Tags { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
