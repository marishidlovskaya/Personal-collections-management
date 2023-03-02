using MessagePack;

namespace Web_app_personal_collections.Models.Entities
{
    public class CollectionConfiq
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public Collection Collection { get; set; }

        public string AdditionalFields { get; set; }

    }
}
