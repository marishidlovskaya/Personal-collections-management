using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System.Xml.Linq;
using Web_app_personal_collections.Migrations;
using Web_app_personal_collections.Models.Entities;
using Web_app_personal_collections.ViewModels;
using static Humanizer.In;

namespace Web_app_personal_collections.Data
{
    public class SearchService
    {
        private readonly CollectionDbContext _collectionDbContext;
        public SearchService(CollectionDbContext collectionDbContext)
        {
            _collectionDbContext = collectionDbContext;
        }

        public List<SearchModel> Search(string input)
        {
            input = "\"" + input + "*\"";
            var query = from col in _collectionDbContext.Collections
                        join cat in _collectionDbContext.Categories on col.CategoryId equals cat.Id into CategoriesGroup 
                        from c in CategoriesGroup.DefaultIfEmpty()
                        join com in _collectionDbContext.Comments on col.Id equals com.CollectionId into CommentsGroup
                        from co in CommentsGroup.DefaultIfEmpty()
                        join it in _collectionDbContext.Items on col.Id equals it.CollectionId into ItemsGroup 
                        from i in ItemsGroup.DefaultIfEmpty()
                        join tgs in _collectionDbContext.Tags on col.Id equals tgs.CollectionId into TagsGroup
                        from tg in TagsGroup.DefaultIfEmpty()

                        where EF.Functions.Contains(col.Name, input) ||
                                EF.Functions.Contains(col.Description, input) ||
                                EF.Functions.Contains(c.Name, input) ||
                                EF.Functions.Contains(i.Name, input) ||
                                EF.Functions.Contains(co.Text, input) ||
                                EF.Functions.Contains(tg.Name, input)
                        group col by new { col.Name, col.Description, catName = c.Name, col.Id, col.Image } into gr

                        select new  
                        {
                            Id = gr.Key.Id,
                            CategoryName = gr.Key.catName,
                            CollectionDescription = gr.Key.Description,
                            CollectionName = gr.Key.Name,
                            ItemNames = gr.Select(s => s.Items).FirstOrDefault(),
                            Comments = gr.Select(s => s.Comments).FirstOrDefault(),
                            Tags = gr.Select(s => s.Tags).FirstOrDefault(),
                            Image = gr.Key.Image

                        };
            
            var result = query.ToList().Select(s => new SearchModel
            {
                Id = s.Id,
                CategoryName = s.CategoryName,
                CollectionDescription = s.CollectionDescription,
                CollectionName = s.CollectionName,
                Image = s.Image,
                ItemNames = s.ItemNames.Select(s => s.Name).ToList(),
                Comments = s.Comments.Select(s => s.Text).ToList(),
                Tags = s.Tags.Select(s => s.Name).ToList()
            });

            return result.ToList();
        }

        public List<SearchModel> SearchByTag(string inputTag)
        {
            inputTag = "\"" + inputTag + "*\"";
            var query = from col in _collectionDbContext.Collections
                        join cat in _collectionDbContext.Categories on col.CategoryId equals cat.Id into CategoriesGroup
                        from c in CategoriesGroup.DefaultIfEmpty()
                        join com in _collectionDbContext.Comments on col.Id equals com.CollectionId into CommentsGroup
                        from co in CommentsGroup.DefaultIfEmpty()
                        join it in _collectionDbContext.Items on col.Id equals it.CollectionId into ItemsGroup
                        from i in ItemsGroup.DefaultIfEmpty()
                        join tgs in _collectionDbContext.Tags on col.Id equals tgs.CollectionId into TagsGroup
                        from tg in TagsGroup.DefaultIfEmpty()

                        where EF.Functions.Contains(tg.Name, inputTag)
                        group col by new { col.Name, col.Description, catName = c.Name, col.Id, col.Image } into gr

                        select new
                        {
                            Id = gr.Key.Id,
                            CategoryName = gr.Key.catName,
                            CollectionDescription = gr.Key.Description,
                            CollectionName = gr.Key.Name,
                            ItemNames = gr.Select(s => s.Items).FirstOrDefault(),
                            Comments = gr.Select(s => s.Comments).FirstOrDefault(),
                            Tags = gr.Select(s => s.Tags).FirstOrDefault(),
                            Image = gr.Key.Image

                        };

            var result = query.ToList().Select(s => new SearchModel
            {
                Id = s.Id,
                CategoryName = s.CategoryName,
                CollectionDescription = s.CollectionDescription,
                CollectionName = s.CollectionName,
                Image = s.Image,
                ItemNames = s.ItemNames.Select(s => s.Name).ToList(),
                Comments = s.Comments.Select(s => s.Text).ToList(),
                Tags = s.Tags.Select(s => s.Name).ToList()
            });

            return result.ToList();


        }
    }
}
