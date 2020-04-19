using System;
using System.Threading.Tasks;
using Authorization.API.Objects;

namespace Authorization.API.Models
{
    public interface IUserRepository
    {
        public Task<UserModel> AddUser(string username,
            string firstName, string lastName, string email,
            byte[] passwordHash, byte[] passwordSalt, 
            DateTime? createdDate = null);
        
        public Task<UserModel> AddUser(string username,
            string firstName, string lastName, string email,
            byte[] passwordHash, byte[] passwordSalt, int authenticationTypeId, 
            DateTime? createdDate = null);
        
        public Task<UserModel> GetUserByUsername(string username);
    }
}