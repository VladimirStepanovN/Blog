using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.BLL.BusinessModels.Responses.UserResponses;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.IServices
{
    public interface IUserService
    {
        Task<IdentityResult> Register(AddUserRequest addUserRequest);
        Task<IdentityResult> Update(int userId, UpdateUserRequest updateUserRequest, string login);
        Task<GetUserResponse[]> GetAll();
        Task<IdentityResult> Delete(int userId, string login);
        Task<GetUserResponse> Get(int userId);
        Task<AuthenticateResponse> GetByLogin(string login);
        Task<GetUserResponse> GetByEmail(string email);
    }
}
