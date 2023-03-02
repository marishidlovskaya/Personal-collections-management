namespace Web_app_personal_collections.ViewModels
{
    public class SearchModel
    {
        public int Id { get; set; } 
        public string CollectionName { get; set; }
        public string CollectionDescription { get; set; }

        public List<string> ItemNames { get; set; }

        public string CategoryName { get; set; }

        public string Image { get; set; }

        public List<string> Comments { get; set; }

        public List<string> Tags { get; set; }

    }
}
