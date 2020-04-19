using System.Threading.Tasks;
using Authorization.API.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.API.Services
{
    public interface IUserService
    {
        public Task<UserViewModel> RegisterNewUser(NewUserViewModel newUserViewModel);
        public Task<UserViewModel> RegisterNewExternalUser(NewExternalUserViewModel newExternalUserViewModel);
        public Task<UserViewModel> Login(LoginViewModel loginViewModel);
        public Task<bool> IsUsernameTaken(string username);
        public Task<NewExternalUserViewModel> GetUserViewModelUserFromGoogleToken(string googleIdToken);
        public Task<UserViewModel> GetUserByUsername(string username);
        public SecurityToken GetTokenForLoggedUser(UserViewModel userViewModel);
    }
}