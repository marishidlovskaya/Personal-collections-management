using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Web_app_personal_collections.Data.Migrations;
using Web_app_personal_collections.Migrations;
using Web_app_personal_collections.Models.Entities;
using Web_app_personal_collections.ViewModels;

namespace Web_app_personal_collections.Data
{
    public class CollectionService
    {
        private readonly CollectionDbContext _collectionDbContext;
        public CollectionService(CollectionDbContext collectionDbContext)
        {
            _collectionDbContext = collectionDbContext;
        }

        public List<ItemModel> GetAllItemsByColId(int id)
        {
            List<ItemModel> model = new List<ItemModel>();
            foreach (var item in _collectionDbContext.Items.Where(x=>x.CollectionId == id))
            {
                model.Add(new ItemModel() {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image,
                    DateTimeItemAdded = item.DateTimeItemAdded, 
                    CollectionId = item.CollectionId,
                    Bool1 = item.bool1,
                    Bool2 = item.bool2,
                    Bool3 = item.bool3,
                    Date1 = item.date1,
                    Date2 = item.date2,
                    Date3 = item.date3,
                    Number1 = item.number1,
                    Number2 = item.number2,
                    Number3 = item.number3,
                    Text1 = item.text1,
                    Text2 = item.text2,
                    Text3 = item.text3
                              
                });
            }
            return model;
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            foreach (var category in _collectionDbContext.Categories)
            {
                categories.Add(new Category() { Id = category.Id, Name = category.Name });
            }
            return categories;
        }

        public List<CollectionModel> GetAllCollections()
        {
            var query = from collection in _collectionDbContext.Collections
                        select new CollectionModel()
                        {
                            Id = collection.Id,
                            Name = collection.Name,
                            Image = collection.Image,
                            Description = collection.Description,
                            UserId = collection.UserId,
                            CategoryId = collection.CategoryId,
                            DateTimeCollectionAdded = collection.DateTimeCollectionAdded,
                            NumberOfItems = _collectionDbContext.Items.Where(f => f.CollectionId == collection.Id).Count(),
                            NumberOfLikes = _collectionDbContext.Likes.Where(f => f.CollectionId == collection.Id).Count(),
                            Tags = _collectionDbContext.Tags.Where(f => f.CollectionId == collection.Id).Select(s => new TagModel()
                            {
                                Id = s.Id,
                                CollectionId = s.CollectionId,
                                Name = s.Name,
                            }).ToList(),
                            
                        };
            return query.ToList();
        }


        public bool CheckIfLikeWasPut(string userId, int collectionId)
        {
            var query = from likes in _collectionDbContext.Likes
                        where likes.UserId == userId && likes.CollectionId == collectionId
                        select likes;
            return query.Any();
        }

        public List<TagModel> GetTagsByCollectionId(int id)
        {
            var query = from tags in _collectionDbContext.Tags
                        where tags.CollectionId == id
                        select new TagModel()
                        {
                            Id = tags.Id,
                            Name = tags.Name,
                            CollectionId = tags.CollectionId
                        };
            return query.ToList();
        }

        public List<TagModel> GetAllTags()
        {
            var query = from tags in _collectionDbContext.Tags
                        select new TagModel()
                        {
                            Id = tags.Id,
                            Name = tags.Name,
                        };
            return query.ToList();
        }

        public List<CommentModel> GetAllCommentsByCollectionId(int colID)
        {
            var query = from comment in _collectionDbContext.Comments
                        join us in _collectionDbContext.Users on comment.UserId equals us.Id
                        where comment.CollectionId == colID
                        select new CommentModel()
                        {
                            Id = comment.Id,
                            CollectionId = comment.CollectionId,
                            DateTimeOfComment = comment.DateTimeOfComment,
                            Text = comment.Text,
                            UserId = comment.UserId,
                            UserName = us.UserName,
                        };

            return query.OrderByDescending(x=>x.DateTimeOfComment).ToList();
        }

        public CollectionInfoModel GetCollectionInfoById(int id)
        {
            var queryCollectionInfo = (from col in _collectionDbContext.Collections

                                       where col.Id == id
                                       select new CollectionInfoModel()
                                       {
                                           Id = col.Id,
                                           CollectionName = col.Name,
                                           CollectionDescription = col.Description,
                                           DateTimeCollectionAdded = col.DateTimeCollectionAdded,
                                           Image = col.Image,
                                           CategoryName = col.Category.Name,
                                           NumberOfComments = _collectionDbContext.Comments.Where(x => x.CollectionId == col.Id).Count(),
                                           NumberOfItems = _collectionDbContext.Items.Where(x => x.CollectionId == col.Id).Count(),
                                           numberOfLikes = _collectionDbContext.Likes.Where(x => x.CollectionId == col.Id).Count(),
                                           Comments = GetAllCommentsByCollectionId(id),
                                           Tags = _collectionDbContext.Tags.Where(f => f.CollectionId == id).Select(s => s.Name).ToList(),
                                           Items = _collectionDbContext.Items.Where(f => f.CollectionId == id).Select(s => new ItemModel()
                                           {
                                               Id = s.Id,
                                               Name = s.Name,
                                               Image = s.Image,
                                               CollectionId = s.CollectionId,
                                               Text1 = s.text1,
                                               Text2 = s.text2,
                                               Text3 = s.text3,
                                               Number1 = s.number1,
                                               Number2 = s.number2,
                                               Number3 = s.number3,
                                               Bool1 = s.bool1,
                                               Bool2 = s.bool2,
                                               Bool3 = s.bool3,
                                               Date1 = s.date1,
                                               Date2 = s.date2,
                                               Date3 = s.date3,
                                           }).ToList(),
                                       }).FirstOrDefault();
            return queryCollectionInfo;
        }

        public List<CollectionInfoModel> GetCollectionsByUserId(string userId)
        {
            var allCollections = (from col in _collectionDbContext.Collections
                                 where col.UserId == userId
                                 select new CollectionInfoModel()
                                 {
                                     Id = col.Id,
                                     CollectionName = col.Name,
                                     CollectionDescription = col.Description,
                                     DateTimeCollectionAdded = col.DateTimeCollectionAdded,
                                     Image = col.Image,
                                     CategoryName = col.Category.Name,
                                     Tags = _collectionDbContext.Tags.Where(f => f.CollectionId == col.Id).Select(s => s.Name).ToList(),
                                     numberOfLikes = _collectionDbContext.Likes.Where(x=>x.CollectionId == col.Id).Count(),
                                     Items = _collectionDbContext.Items.Where(f => f.CollectionId == col.Id).Select(s => new ItemModel()
                                     {
                                         Id = s.Id,
                                         Name = s.Name,
                                         Image = s.Image,
                                         CollectionId = s.CollectionId,
                                         Text1 = s.text1,
                                         Text2 = s.text2,
                                         Text3 = s.text3,
                                         Number1 = s.number1,
                                         Number2 = s.number2,
                                         Number3 = s.number3,
                                         Bool1 = s.bool1,
                                         Bool2 = s.bool2,
                                         Bool3 = s.bool3,
                                         Date1 = s.date1,
                                         Date2 = s.date2,
                                         Date3 = s.date3,
                                     }).ToList(),
                                 }).ToList();
            return allCollections;
        }


        public void PutLikeOrDislike(int id, string userId, bool isLike)
        {
            var numOfLikes = _collectionDbContext.Likes.Where(x => x.UserId == userId && x.CollectionId == id).Select(s => s.Id).Count();
            var like = _collectionDbContext.Likes.Where(x => x.UserId == userId && x.CollectionId == id).FirstOrDefault();

            if (isLike == true && numOfLikes < 1)
            {
                _collectionDbContext.Likes.Add(new Like() { CollectionId = id, UserId = userId });

            }
            if (isLike == false && numOfLikes > 0)
            {
                _collectionDbContext.Likes.Remove(like);
            }
            _collectionDbContext.SaveChanges();
        }

        public CommentModel AddComment(int colId, string currentUserId, string comment)
        {
            Comment _comment = new Comment() { 
                CollectionId = colId,
                DateTimeOfComment = DateTime.Now,  
                Text = comment,
                UserId = currentUserId,
            };

            _collectionDbContext.Comments.Add(_comment);
            _collectionDbContext.SaveChanges();

            var result =  new CommentModel() { 
 
                UserName = _collectionDbContext.Users.Where(x=>x.Id == currentUserId).Select(s=>s.UserName).FirstOrDefault(),
                Text = comment,
                DateTimeOfComment= _comment.DateTimeOfComment,
            };
            return result;  
        } 
        public string GetAdditionalColumnsByColId(int colId)
        {
            var query = _collectionDbContext.collectionConfiqs
                .Where(x=>x.CollectionId == colId).Select(x=>x.AdditionalFields).FirstOrDefault();
            return query;
        }

        public void SaveCollection(CollectionInfoModel model, string userId, int collectionId)
        {
            if(collectionId > 0)
            {
                UpdateCollection(model, collectionId);
                return;
            }
            var categoryId = _collectionDbContext.Categories.Where(x=>x.Name == model.CategoryName).Select(x=>x.Id).FirstOrDefault();
            Collection collection = new Collection();
            List<Item> item = new List<Item>();
            List<Tag> tag = new List<Tag>();


            foreach (var i in model.Items)
            {
                item.Add(new Item
                {
                    Name = i.Name,
                    Image = i.Image,
                    DateTimeItemAdded = DateTime.Now,
                    text1 = i.Text1,
                    text2 = i.Text2,
                    text3 = i.Text3,
                    number1 = i.Number1,
                    number2 = i.Number2,
                    number3 = i.Number3,
                    bool1 = i.Bool1,
                    bool2 = i.Bool2,
                    bool3 = i.Bool3,
                    date1 = i.Date1,
                    date2 = i.Date2,
                    date3 = i.Date3,
                });
               
             }

            foreach (var i in model.Tags)
            {
                tag.Add(new Tag
                {
                    Name = i
                });

            }

            collection.CategoryId = categoryId;
            collection.Name = model.CollectionName;
            collection.Description = model.CollectionDescription;
            collection.DateTimeCollectionAdded = DateTime.Now;
            collection.UserId = userId;
            collection.Image = model.Image;
            collection.Items = item;
            collection.Tags = tag;




            _collectionDbContext.Add(collection);
            _collectionDbContext.SaveChanges();

            CollectionConfiq collectionConfiq = new CollectionConfiq();
            collectionConfiq.AdditionalFields = model.AdditionalFields;
            collectionConfiq.CollectionId = collection.Id;

            _collectionDbContext.Add(collectionConfiq);
            _collectionDbContext.SaveChanges();

        }

        private void UpdateCollection(CollectionInfoModel model, int collectionId)
        {
            var collection = _collectionDbContext.Collections
            .FirstOrDefault(s => s.Id.Equals(collectionId));
            var categoryId = _collectionDbContext.Categories.Where(x => x.Name == model.CategoryName).Select(x => x.Id).FirstOrDefault();
            collection.CategoryId = categoryId;
            collection.Name = model.CollectionName;
            collection.Description = model.CollectionDescription;
            collection.DateTimeCollectionAdded = DateTime.Now;
            collection.Image = model.Image;
            _collectionDbContext.SaveChanges();

            _collectionDbContext.Items.RemoveRange(_collectionDbContext.Items.Where(f => f.CollectionId == collectionId));
            _collectionDbContext.Tags.RemoveRange(_collectionDbContext.Tags.Where(f => f.CollectionId == collectionId));
            _collectionDbContext.collectionConfiqs.RemoveRange(_collectionDbContext.collectionConfiqs.Where(f => f.CollectionId == collectionId));

            foreach (var i in model.Items)
            {
                _collectionDbContext.Items.Add(new Item
                {
                    Name = i.Name,
                    Image = i.Image,
                    DateTimeItemAdded = DateTime.Now,
                    text1 = i.Text1,
                    text2 = i.Text2,
                    text3 = i.Text3,
                    number1 = i.Number1,
                    number2 = i.Number2,
                    number3 = i.Number3,
                    bool1 = i.Bool1,
                    bool2 = i.Bool2,
                    bool3 = i.Bool3,
                    date1 = i.Date1,
                    date2 = i.Date2,
                    date3 = i.Date3,
                    CollectionId = collectionId
                });
            }

            foreach (var i in model.Tags)
            {
                _collectionDbContext.Tags.Add(new Tag
                {
                    Name = i,
                    CollectionId = collectionId
                });

            }

            _collectionDbContext.SaveChanges();

            CollectionConfiq collectionConfiq = new CollectionConfiq();
            collectionConfiq.AdditionalFields = model.AdditionalFields;
            collectionConfiq.CollectionId = collectionId;

            _collectionDbContext.Add(collectionConfiq);
            _collectionDbContext.SaveChanges();

        }

        public void DeleteCollectionById(int id)
        {
            Collection collectionToDelete = _collectionDbContext.Collections.Where(x => x.Id == id).FirstOrDefault();
            _collectionDbContext.Remove(collectionToDelete);
            _collectionDbContext.SaveChanges();
        }

    }
}
