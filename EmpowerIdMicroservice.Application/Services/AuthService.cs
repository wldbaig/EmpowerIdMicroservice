using EmpowerIdMicroservice.Domain.AggregateModules.ApplicationUserAggregate;
using Microsoft.AspNetCore.Identity;

namespace EmpowerIdMicroservice.Application.Services
{
    public class AuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            { 
                var result = await _signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // User successfully signed in
                    return true;
                } 
            } 

            return false;
        }

        public async Task<IdentityResult> RegisterUser(string fullname, string username, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                FullName = fullname 
            };

            var result = await _userManager.CreateAsync(user, password);
              
            return result;
        }
    }
}
