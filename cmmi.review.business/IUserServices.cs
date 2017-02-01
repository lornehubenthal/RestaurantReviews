using cmmi.review.entities;
using System.Collections.Generic;

namespace cmmi.review.business
{
    public interface IUserServices
    {
        ICollection<UserEntity> GetAllUsers();
        UserEntity GetUserById(int userId);
        int CreateUser(UserEntity userEntity, string password);
        bool UpdateUser(int userId, UserEntity userEntity, string password);
        bool DeleteUser(int userId);
    }
}
