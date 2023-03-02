namespace Web_app_personal_collections.ViewModels
{
    public class CollectionInfoModel
    {
        public int Id { get; set; }
        public string CollectionName { get; set; }
        public string CollectionDescription { get; set; }
        public DateTime DateTimeCollectionAdded { get; set; }
        public string Image { get; set; }
        public List<string> Tags { get; set; }

        public int numberOfLikes { get; set; }

        public UsersModel Users { get; set; }


        //public List<string> Comments { get; set; }

        public string CategoryName { get; set; }

        public List<ItemModel> Items { get; set; }

        public List<CommentModel> Comments { get; set; }


        public int NumberOfItems { get; set; }
        public int NumberOfComments { get; set; }

        public bool colIsLikedByCurrentUser { get; set; } 
        
        public string AdditionalFields { get; set; }

    }
}
