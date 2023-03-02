using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Web_app_personal_collections.Data;
using Web_app_personal_collections.ViewModels;

namespace Web_app_personal_collections.Controllers
{
    public class CollectionController : Controller
    {
        private readonly CollectionDbContext _collectionDbContext;
        private readonly CollectionService _collectionService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CollectionController(CollectionDbContext collectionDbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _collectionDbContext = collectionDbContext;
            _collectionService = new CollectionService(collectionDbContext);
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index(int id)
        {
            var model = _collectionService.GetCollectionInfoById(id);
            if (_signInManager.IsSignedIn(User))
            {
                model.colIsLikedByCurrentUser = _collectionService.CheckIfLikeWasPut(_userManager.GetUserAsync(HttpContext.User)
                    .Result.Id, id);
            }
            return View(model);
        }
        public JsonResult PutLikeOrDislike(int id, bool isLike)
        {
            var currentUserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            _collectionService.PutLikeOrDislike(id, currentUserId, isLike);
            return Json("");
        }
        public JsonResult CheckIfUserLogIn()
        {
            if (!HttpContext.User.Claims.Any())
            {
                return Json(Response.StatusCode = 401);
            }
            return Json("");
        }
        public JsonResult AddComment(int id, string comment)
        {
            var currentUserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            var result = _collectionService.AddComment(id, currentUserId, comment);
            return Json(result);
        }
        public JsonResult GetAllItemsByColId(int id)
        {
            var data = _collectionService.GetAllItemsByColId(id);
            Collection_ItemsModel model = new Collection_ItemsModel();
            model.Items = data;
            model.Columns = new List<ColumsOfItemTable> {
                new ColumsOfItemTable() { data = "image", title = "Image" },
                new ColumsOfItemTable() { data = "name", title = "Item name" },
            };
            string additionalColumns = _collectionService.GetAdditionalColumnsByColId(id);
            if(additionalColumns != null)
            {
                var columnsInfo = JsonSerializer.Deserialize<List<ColumsOfItemTable>>(additionalColumns);
                foreach (var item in columnsInfo)
                {

                    model.Columns.Add(item);
                }
            }
            return Json(model);
        }
    }
}
