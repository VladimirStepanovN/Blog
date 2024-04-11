﻿using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.BusinessModels.Responses.ArticleResponses;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.IServices
{
    public interface IArticleService
    {
        Task<IdentityResult> Create(AddArticleRequest addArticleRequest);
        Task<IdentityResult> Update(int articleId, UpdateArticleRequest updateArticleRequest);
        Task<GetArticleResponse[]> GetAll();
        Task<IdentityResult> Delete(int articleId);
        Task<GetArticleResponse[]> GetAllByUserId(int userId);
    }
}
