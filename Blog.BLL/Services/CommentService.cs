using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.BusinessModels.Responses.CommentResponses;
using Blog.BLL.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services
{
    public class CommentService : ICommentService
    {
        public Task<IdentityResult> Create(AddCommentRequest addCommentRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> Delete(int commentId)
        {
            throw new NotImplementedException();
        }

        public Task<GetCommentResponse[]> Get(int commentId)
        {
            throw new NotImplementedException();
        }

        public Task<GetCommentResponse[]> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> Update(int commentId, UpdateCommentRequest updateCommentRequest)
        {
            throw new NotImplementedException();
        }
    }
}
