namespace Web_app_personal_collections.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }

        public DateTime DateTimeOfComment { get; set; }
        public int CollectionId { get; set; }
        public Collection Collection { get; set; }
    }
}
