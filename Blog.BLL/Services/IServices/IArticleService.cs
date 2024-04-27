using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.BusinessModels.Responses.ArticleResponses;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.IServices
{
    public interface IArticleService
    {
        Task<IdentityResult> Create(AddArticleRequest addArticleRequest);
        Task<IdentityResult> Update(UpdateArticleRequest updateArticleRequest, string login);
        Task<GetArticleResponse[]> GetAll();
        Task<IdentityResult> Delete(int articleId, string login);
        Task<GetArticleResponse[]> GetAllByUserId(int userId);
		Task<GetArticleResponse> GetById(int articleId);
	}
}
