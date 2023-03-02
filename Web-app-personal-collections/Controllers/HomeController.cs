using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_app_personal_collections.Data;
using Web_app_personal_collections.Models;
using Web_app_personal_collections.Models.Entities;

namespace Web_app_personal_collections.Controllers
{

    public class HomeController : Controller
    {
        private readonly CollectionDbContext _collectionDbContext;
        private readonly CollectionService _collectionService;
        private readonly SearchService _searchService;

        public HomeController(CollectionDbContext collectionDbContext)
        {
            _collectionDbContext = collectionDbContext;
            _collectionService = new CollectionService(collectionDbContext);
            _searchService = new SearchService(collectionDbContext);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public JsonResult GetAllCollections(int catId, string ordering = "lastadded")
        {
            var collections = _collectionService.GetAllCollections();
            if (catId != 0)
            {
                collections = collections.Where(c => c.CategoryId == catId).OrderByDescending(x => x.NumberOfLikes).ToList();
            }          
            switch (ordering)
            {
                case "thebiggest":
                    collections = collections.OrderByDescending(x => x.NumberOfItems).ToList();
                    break;
                case "themostpopular":
                    collections = collections.OrderByDescending(x => x.NumberOfLikes).ToList();
                    break;
                case "lastadded":
                    collections = collections.OrderByDescending(x => x.DateTimeCollectionAdded).ToList();
                    break;
                default:
                    collections = collections.OrderByDescending(x => x.NumberOfLikes).ToList();
                    break;
            }
            return Json(collections);
        }
        public JsonResult CheckIfLikeWasPut(string userId, int collectionId)
        {
            var result = CheckIfLikeWasPut(userId, collectionId);
            return Json(result);
        }
        public JsonResult GetSearchResult(string searchinput)
        {
            var result = _searchService.Search(searchinput);
            return Json(result);
        }
        public JsonResult GetOnlyTagsSearchResult(string searchinput)
        {
            var result = _searchService.SearchByTag(searchinput);
            return Json(result);
        }
        public JsonResult GetAllCategories()
        {
            var result = _collectionService.GetAllCategories();
            return Json(result);
        }
    }
}