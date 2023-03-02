namespace Web_app_personal_collections.Models.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime DateTimeItemAdded { get; set; }

        public int CollectionId { get; set; }
        public Collection Collection { get; set; }

        public string? text1 { get; set; }
        public string? text2 { get; set; }
        public string? text3 { get; set; }
        public decimal? number1 { get; set; }
        public decimal? number2 { get; set; }
        public decimal? number3 { get; set; }

        public DateTime? date1 { get; set; }
        public DateTime? date2 { get; set; }
        public DateTime? date3 { get; set; }

        public Boolean? bool1 { get; set; }
        public Boolean? bool2 { get; set; }
        public Boolean? bool3 { get; set; }

    }
}
