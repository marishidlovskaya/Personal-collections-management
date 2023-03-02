namespace Web_app_personal_collections.Models.Entities
{
    public class Like
    {
        public int Id { get; set; }

        public int CollectionId { get; set; }
        public Collection Collection { get; set; }

        public string UserId { get; set; }
    }
}
