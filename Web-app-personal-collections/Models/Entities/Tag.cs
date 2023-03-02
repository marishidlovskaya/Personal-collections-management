namespace Web_app_personal_collections.Models.Entities
{
    public class Tag
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public int CollectionId { get; set; }
        public Collection Collection { get; set; }

    }
}
