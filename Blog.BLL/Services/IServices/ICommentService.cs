using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.BusinessModels.Responses.CommentResponses;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.IServices
{
    public interface ICommentService
    {
        Task<IdentityResult> Create(AddCommentRequest addCommentRequest);
        Task<GetCommentResponse[]> Get(int commentId);
        Task<GetCommentResponse[]> GetAll();
        Task<IdentityResult> Update(int commentId, UpdateCommentRequest updateCommentRequest);
        Task<IdentityResult> Delete(int commentId);
    }
}
