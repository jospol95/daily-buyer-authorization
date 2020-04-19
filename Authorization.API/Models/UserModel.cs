using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authorization.API.Models
{
    [Table("User")]
    public class UserModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        
        [Column("username")]
        [Required]
        public string Username { get; set; }
        
        [Column("firstname")]
        public string FirstName { get; set; }
        
        [Column("lastname")]
        public string LastName { get; set; }
        
        [Column("email")]
        public string Email { get; set; }
        
        [Column("password_hash")]
        public byte[] PasswordHash { get; set; }
        
        [Column("password_salt")]
        public byte[] PasswordSalt { get; set; }
        
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }
        
        [Column("user_status_id")]
        public int UserStatusId { get; set; }
        
        [Column("authentication_type_id")]
        public int AuthenticationTypeId { get; set; }
    }
}