namespace Web_app_personal_collections.ViewModels
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime DateTimeItemAdded { get; set; }

        public int CollectionId { get; set; }

        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public decimal? Number1 { get; set; }
        public decimal? Number2 { get; set; }
        public decimal? Number3 { get; set; }

        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }

        public Boolean? Bool1 { get; set; }
        public Boolean? Bool2 { get; set; }
        public Boolean? Bool3 { get; set; }
    }
}
