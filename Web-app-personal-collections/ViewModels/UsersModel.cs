using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_app_personal_collections.Data;

namespace Web_app_personal_collections.ViewModels
{
    public class UsersModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}

