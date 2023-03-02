namespace Web_app_personal_collections.ViewModels
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateTimeOfComment { get; set; }
        public int CollectionId { get; set; }
    }
}
