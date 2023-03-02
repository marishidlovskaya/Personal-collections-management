using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_app_personal_collections.Data;
using Web_app_personal_collections.ViewModels;

namespace Web_app_personal_collections.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CollectionDbContext _collectionDbContext;

        public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, CollectionDbContext collectionDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _collectionDbContext = collectionDbContext;
            _userService = new UserService(_signInManager, _userManager, _roleManager, collectionDbContext);
        }
        public IActionResult Index()
        {
            var roles = _userService.GelAllRoles();
            List<SelectListItem> listOfRoles = new List<SelectListItem>();
            foreach (var role in roles)
            {
                listOfRoles.Add(new SelectListItem { Text = role.Name });
            }
            ViewBag.roles = listOfRoles;
            return View();
        }
        public JsonResult GetUsers()
        {
            var users = _userService.GetAllUsers();
            return Json(users);
        }
        public JsonResult GetUserData(string userId)
        {
            var user = _userService.GetUserData(userId);
            return Json(user);
        }
        public JsonResult UpdateUserData(string userstatus, string userrole, string userid)
        {
            UsersModel user = new UsersModel() { Id = userid, Role = userrole, Status = userstatus };
            _userService.UpdateUserData(user).Wait();
            return Json("");
        }
        public JsonResult DeleteUserById(string userId)
        {            
            _userService.DeleteUserById(userId).Wait();           
            return Json("");
        }
        public async Task<IActionResult> BlockUsers(string[] userIds)
        {
            await _userService.BlockUsers(userIds);
            return Redirect("Index");
        }
        public async Task<IActionResult> UnBlockUsers(string[] userIds)
        {
            await _userService.UnBlockUsers(userIds);
            return Redirect("Index");
        }
        public async Task<IActionResult> ChangeUserRole(string[] userIds, string Role)
        {
            await _userService.ChangeUserRole(userIds, Role);
            return Redirect("Index");
        }
    }
}
