using AutoMapper;
using Blog.BLL.BlogConfiguration;
using Blog.BLL.BusinessModels.Requests.ArticleRequests;
using Blog.BLL.BusinessModels.Responses.ArticleResponses;
using Blog.BLL.Services.IServices;
using Blog.DAL.Entities;
using Blog.DAL.Repositories;
using Blog.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public ArticleService(ConnectionSettings settings)
        {
            _articleRepository = new ArticleRepository(settings.DefaultConnection);
            var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new BusinessMappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        /// <summary>
        /// Логика сервиса срздания статьи
        /// </summary>
        /// <param name="addArticleRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Create(AddArticleRequest addArticleRequest)
        {
            var article = _mapper.Map<Article>(addArticleRequest);
            await _articleRepository.Add(article);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса удаления статьи
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Delete(int articleId)
        {
            var article = await _articleRepository.Get(articleId);
            if (article == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Статьи не существует"
                });
            }

            await _articleRepository.Delete(articleId);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса получения всех статей
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public async Task<GetArticleResponse[]> GetAll()
        {
            var articles = await _articleRepository.GetArticles();
            var getArticleResponse = _mapper.Map<Article[], GetArticleResponse[]>(articles);
            return getArticleResponse;
        }

        /// <summary>
        /// Логика сервиса получения всех статей определенного автора
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<GetArticleResponse[]> GetAllByUserId(int userId)
        {
            var articles = await _articleRepository.GetArticles(userId);
            var getArticleResponse = _mapper.Map<Article[], GetArticleResponse[]>(articles);
            return getArticleResponse;
        }

        /// <summary>
        /// Логика сервиса редактирования статьи
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="updateArticleRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Update(int articleId, UpdateArticleRequest updateArticleRequest)
        {
            var article = _mapper.Map<Article>(updateArticleRequest);
            await _articleRepository.Update(articleId, article);
            return IdentityResult.Success;
        }
    }
}
