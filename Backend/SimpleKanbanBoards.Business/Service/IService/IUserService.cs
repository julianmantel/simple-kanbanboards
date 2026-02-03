using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.User;

namespace SimpleKanbanBoards.Business.Service.IService
{
    public interface IUserService
    {
        Task<UserModel> GetUserByIdAsync(int id);
        Task CreateUserAsync(CreateUserModel user);
        Task<string> LoginAsync(LoginRequestModel request);
        Task ResetPasswordAsync(ResetPassRequestModel request);
        Task ChangePasswordAsync(ChangePasswordModel request);
        Task DeleteUserAsync(int id);
    }
}
