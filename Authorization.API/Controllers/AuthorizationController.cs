using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Authorization.API.Objects;
using Authorization.API.Services;
using Authorization.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        #region Private Fields
        private readonly IUserService _userService;


        #endregion
        
        #region Constructor

        public AuthorizationController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion
        
        
        #region Public Methods
        // GET
        [HttpPost("index")]
        public IActionResult Index()
        {
            return null;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(NewUserViewModel newUserViewModel)
        {
            var isUsernameTaken = await _userService.IsUsernameTaken(newUserViewModel.Username);
            if (isUsernameTaken) return BadRequest("This username already exists, pick a different one");
            var newUser = await _userService.RegisterNewUser(newUserViewModel);
            if (newUser == null) return BadRequest("Something bad occurred");
            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var loggedUser = await _userService.Login(loginViewModel);
            // if (loggedUser == null) return BadRequest("Username or password incorrect."); // or Unauthorized
            if (loggedUser == null) return Unauthorized(); // or Unauthorized

            //token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = _userService.GetTokenForLoggedUser(loggedUser);
            
            // return Ok(loggedUser);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
        
        [HttpPost("loginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string googleIdToken)
        {
            var googleUser = await _userService.GetUserViewModelUserFromGoogleToken(googleIdToken);
            //if it's an existing one, check authenticationType
            var isUsernameTaken = await _userService.IsUsernameTaken(googleUser.Username);
            if (isUsernameTaken)
            {
                //if authType is not googleAuth, return a this is an existing user
                var existingUser = await  _userService.GetUserByUsername(googleUser.Username);
                if (existingUser.AuthenticationTypeId == (int) AuthenticationType.GoogleApi)
                {
                    return BadRequest("This email is already registered, please try a different login method");
                }
                return Ok(existingUser);
            }
            //if it's a new user, save it, log that user
            var addedUser = await _userService.RegisterNewExternalUser(googleUser);
            return Ok(addedUser);
        }
        
        [HttpPost("loginWithFacebook")]
        public Task<IActionResult> LoginWithFacebook([FromBody] string facebookIdToken)
        {
            return null;
        }
        
        [HttpPost("loginWithApple")]
        public Task<IActionResult> LoginWithApple([FromBody] string appleIdToken)
        {
            return null;
        }
        
        #endregion
        
    }
}