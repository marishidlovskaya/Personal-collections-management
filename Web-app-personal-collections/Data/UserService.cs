using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Web_app_personal_collections.Areas.Identity.Pages.Account;
using Web_app_personal_collections.Data.Migrations;
using Web_app_personal_collections.Models.Entities;
using Web_app_personal_collections.ViewModels;

namespace Web_app_personal_collections.Data
{
    public class UserService
    {
        private readonly CollectionDbContext _collectionDbContext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, CollectionDbContext collectionDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _collectionDbContext = collectionDbContext;
        }
        public List<IdentityUser> Users
        {
            get
            {
                List<IdentityUser> users = new List<IdentityUser>();
                var listOfAllUsers = _userManager.Users.ToList();
                return listOfAllUsers;
            }
        }

        public List<UsersModel> GetAllUsers()
        {
            List<UsersModel> model = new List<UsersModel>();
            foreach (var user in Users)
            {
                model.Add(new UsersModel() { Id = user.Id, Email = user.UserName, Role = GetRole(user).Result, Status = GetStatus(user).Result });
            }
            return model;
        }

        public UsersModel GetUserData(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            return new UsersModel() { Id = userId, Email = user.UserName, Role = GetRole(user).Result, Status = GetStatus(user).Result};
        }

        public List<IdentityRole> GelAllRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task UpdateUserData(UsersModel usersModel)
        {
            string[] userIds = usersModel.Id.Split(',');
            //var user = await _userManager.FindByIdAsync(usersModel.Id);
            await ChangeUserRole(userIds, usersModel.Role);
            if(usersModel.Status == "Active")
            {
                await UnBlockUsers(userIds);
            }
            else
            {
                await BlockUsers(userIds);  
            }
        }

        public async Task ChangeUserRole(string[] userId, string toRole)
        {
            foreach (var id in userId)
            {

                var userToChangeRole = Users.Where(x => x.Id == id).FirstOrDefault();

                var roleBefore = _userManager.GetRolesAsync(userToChangeRole);
                await _userManager.RemoveFromRolesAsync(userToChangeRole, roleBefore.Result);
                await _userManager.AddToRoleAsync(userToChangeRole, toRole);
            }

        }


        public async Task DeleteUserById(string userId)
        {
                var userToDelete = Users.Where(x => x.Id == userId).FirstOrDefault();
                await _userManager.DeleteAsync(userToDelete);
        }

        public async Task BlockUsers(string[] userId)
        {
            foreach (var id in userId)
            {
                var userToBlock = Users.Where(x => x.Id == id).FirstOrDefault();
                userToBlock.LockoutEnabled = false;
                await _userManager.UpdateAsync(userToBlock);
            }
        }
        public async Task UnBlockUsers(string[] userId)
        {
            foreach (var id in userId)
            {
                var userToUnBlock = Users.Where(x => x.Id == id).FirstOrDefault();
                userToUnBlock.LockoutEnabled = true;
                await _userManager.UpdateAsync(userToUnBlock);
            }
        }

        private async Task<string> GetStatus(IdentityUser user)
        {
            var status = await _userManager.GetLockoutEnabledAsync(user);

            return status ? Status.Active.ToString() : Status.Blocked.ToString();
        }

        private async Task<string> GetRole(IdentityUser user)
        {
            var role = await _userManager.GetRolesAsync(user);
            return role.FirstOrDefault();

        }

        public async Task AddUserToDBCollection(User user)
        {
            _collectionDbContext.Users.Add(user);
            await _collectionDbContext.SaveChangesAsync();
        }


    }
    public enum Status
    {
        Blocked,
        Active


    }
}
