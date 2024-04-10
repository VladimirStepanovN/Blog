using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.BLL.BusinessModels.Responses.UserResponses;
using Blog.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.IServices
{
    public interface IUserService
    {
        Task<IdentityResult> Register(AddUserRequest addUserRequest);
        Task<IdentityResult> Update(UpdateUserRequest updateUserRequest);
        Task<GetUserResponse[]> GetAll();
        Task<IdentityResult> Delete(DeleteUserRequest deleteUserRequest);
    }
}
