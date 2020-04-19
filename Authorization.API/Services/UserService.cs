using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Authorization.API.Models;
using Authorization.API.Objects;
using Authorization.API.ViewModels;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.API.Services
{
    public class UserService : IUserService
    {
        #region Private Fields

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        
        #endregion

        #region Constructor
        
        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        
        #endregion

        #region public methods

        public SecurityToken GetTokenForLoggedUser(UserViewModel userViewModel)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userViewModel.Id.ToString()),
                new Claim(ClaimTypes.Name, userViewModel.Username),
                new Claim(ClaimTypes.AuthenticationMethod, userViewModel.AuthenticationTypeId.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        } 
        
        public async Task<UserViewModel> RegisterNewUser(NewUserViewModel newUserViewModel)
        {
            CreatePasswordHash(newUserViewModel.Password,out var userPasswordHash,out var userPasswordSalt);
            var newUser = await _userRepository.AddUser(newUserViewModel.Username, newUserViewModel.FirstName,
                newUserViewModel.LastName, newUserViewModel.Email, userPasswordHash, userPasswordSalt);
            var mappedUser = _mapper.Map<UserViewModel>(newUser);
            return mappedUser;
        }
        
        public async Task<UserViewModel> RegisterNewExternalUser(NewExternalUserViewModel newExternalUserViewModel)
        {
            CreatePasswordHash(newExternalUserViewModel.Password,out var userPasswordHash,out var userPasswordSalt);
            var newUser = await _userRepository.AddUser(newExternalUserViewModel.Username, newExternalUserViewModel.FirstName,
                newExternalUserViewModel.LastName, newExternalUserViewModel.Email, userPasswordHash, userPasswordSalt, newExternalUserViewModel.AuthenticationTypeId);
            var mappedUser = _mapper.Map<UserViewModel>(newUser);
            return mappedUser;
        }

        public async Task<UserViewModel> Login(LoginViewModel loginViewModel)
        {
            var user = await _userRepository.GetUserByUsername(loginViewModel.Username);
            if (user == null) return null;
            if (!VerifyPasswordHash(loginViewModel.Password, user.PasswordHash, user.PasswordSalt)) return null;
            var mappedUser = _mapper.Map<UserViewModel>(user);
            return mappedUser;
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            return user != null;
        }

        public async Task<NewExternalUserViewModel> GetUserViewModelUserFromGoogleToken(string googleIdToken)
        {
            var googleTokenSettings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>()
                {
                    "680869894998-hujg19vbq0lhmhc8vf8ldmc1dnepvrti.apps.googleusercontent.com"
                }
            };
            try
            {
                var validToken = await GoogleJsonWebSignature.ValidateAsync(googleIdToken, googleTokenSettings);
                var user = new NewExternalUserViewModel()
                {
                    AuthenticationTypeId = (int)AuthenticationType.GoogleApi,
                    Email = validToken.Email,
                    FirstName = validToken.Name,
                    LastName = validToken.GivenName,
                    Password = GenerateRandomPasswordForSocialUser(),
                    Username = validToken.Email
                };
                return user;
            }
            catch (InvalidJwtException ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                throw;
            }
        }

        public async Task<UserViewModel> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var mappedUser = _mapper.Map<UserViewModel>(user);
            return mappedUser;
        }
        
        #endregion
        
        #region Private Stacic Methods
        private static bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(userPasswordSalt))
            {
                var computerHash = hmac.ComputeHash((System.Text.Encoding.UTF8.GetBytes(password)));
                if (computerHash.Where((value, count) => value != userPasswordHash[count]).Any())
                {
                    return false;
                }
            }

            return true;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash((System.Text.Encoding.UTF8.GetBytes(password)));
        }
        
        private static string GenerateRandomPasswordForSocialUser()
        {
            return "abc";
        }
        
        #endregion
        
        
    }
}