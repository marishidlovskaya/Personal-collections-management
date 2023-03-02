namespace Web_app_personal_collections.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Collection> Collections { get; set; }

    }
}
