using System;
using System.Threading.Tasks;
using Authorization.API.Data;
using Authorization.API.Models;
using Authorization.API.Objects;
using Microsoft.EntityFrameworkCore;

namespace Authorization.API.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Private variables
        private readonly AuthorizationDbContext _authorizationDb;
        
        #endregion

        #region Constructor

        public UserRepository(AuthorizationDbContext authorizationDb)
        {
            _authorizationDb = authorizationDb;
        }
        
        #endregion
        
        #region Public Methods
        
        public async Task<UserModel> AddUser(string username,
            string firstName, string lastName, string email,
            byte[] passwordHash, byte[] passwordSalt, DateTime? createdDate)
        {
            var newUser = new UserModel()
            {
                Username = username.ToLower(),
                FirstName = firstName,
                LastName = lastName,
                Email = lastName.ToLower(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserStatusId = (int) UserStatus.Active,
                AuthenticationTypeId = (int) AuthenticationType.AuthApi,
                CreatedDate = createdDate?? DateTime.Now
            };
            await _authorizationDb.Users.AddAsync(newUser);
            await _authorizationDb.SaveChangesAsync();
            return newUser;
        }
        
        public async Task<UserModel> AddUser(string username,
            string firstName, string lastName, string email,
            byte[] passwordHash, byte[] passwordSalt, int authenticationTypeId, DateTime? createdDate = null)
        {
            var newUser = new UserModel()
            {
                Username = username.ToLower(),
                FirstName = firstName,
                LastName = lastName,
                Email = email.ToLower(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserStatusId = (int) UserStatus.Active,
                AuthenticationTypeId = authenticationTypeId,
                CreatedDate = createdDate?? DateTime.Now
            };
            await _authorizationDb.Users.AddAsync(newUser);
            await _authorizationDb.SaveChangesAsync();
            return newUser;
        }
        
        public async Task<UserModel> GetUserByUsername(string username)
        {
            var user = await _authorizationDb.Users.FirstOrDefaultAsync(u => u.Username.Equals(username.ToLower()));
            return user;
        }
        
        #endregion
        
    }
}