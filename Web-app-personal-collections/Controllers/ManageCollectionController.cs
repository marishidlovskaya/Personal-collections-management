using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using Web_app_personal_collections.Data;
using Web_app_personal_collections.Migrations;
using Web_app_personal_collections.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Web_app_personal_collections.Controllers
{
    [Authorize]
    public class ManageCollectionController : Controller
    {
        private readonly CollectionDbContext _collectionDbContext;
        private readonly CollectionService _collectionService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ManageCollectionController(CollectionDbContext collectionDbContext, IHostingEnvironment hostingEnvironment)
        {
            _collectionDbContext = collectionDbContext;
            _collectionService = new CollectionService(collectionDbContext);
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(string id)
        {
            var categories = _collectionService.GetAllCategories();
            List<SelectListItem> listOfCategories = new List<SelectListItem>();
            foreach (var cat in categories)
            {
                listOfCategories.Add(new SelectListItem { Text = cat.Name });
            }
            ViewBag.categories = listOfCategories;
            return View();
        }
        public JsonResult GetAllCollectionsByUserId(string userId = null)
        {
            if(userId == null)
                userId = HttpContext.User.Claims.First().Value;
            var model = _collectionService.GetCollectionsByUserId(userId);
            return Json(model);
        }
        public JsonResult GetAllCollectionsByUserId_(string userId)
        { 
            var model = _collectionService.GetCollectionsByUserId(userId);
            return Json(model);
        }
        public JsonResult SaveCollection(string data, int collectionId)
        {
            var data_ = JsonSerializer.Deserialize<CollectionInfoModel>(data);
            var userId = HttpContext.User.Claims.First().Value;             
            _collectionService.SaveCollection(data_, userId, collectionId);
            return Json("");
        }
        public JsonResult DeleteCollectionById(int id)
        {
            _collectionService.DeleteCollectionById(id);
            return Json("");
        }

        public JsonResult GetCollectionInfoById(int id)
        {
            var data = _collectionService.GetCollectionInfoById(id);
            string additionalColumns = _collectionService.GetAdditionalColumnsByColId(id);
            data.AdditionalFields = additionalColumns;
            return Json(data);
        }

        public JsonResult GetTags()
        {
            return Json(_collectionService.GetAllTags().Select((s, i) => new
            {
                id = i,
                text = s.Name,
            }));
        }
    }
}
