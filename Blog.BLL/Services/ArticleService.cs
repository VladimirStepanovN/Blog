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
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public ArticleService(ConnectionSettings settings)
        {
            _articleRepository = new ArticleRepository(settings.DefaultConnection);
            _userRepository = new UserRepository(settings.DefaultConnection);
            _roleRepository = new RoleRepository(settings.DefaultConnection);
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
        public async Task<IdentityResult> Delete(int articleId, string login)
        {
            var initiator = await _userRepository.GetByLogin(login);
            var role = await _roleRepository.GetRoleById(initiator.RoleId);
            var entity = await _articleRepository.Get(articleId);

            if (entity != null)
            {
                if (role.RoleName == "Модератор")
                {
                    await _articleRepository.Delete(articleId);
                    return IdentityResult.Success;
                }

                if (role.RoleName == "Пользователь" && initiator.UserId == entity.UserId)
                {
                    await _articleRepository.Delete(articleId);
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed(new IdentityError
            {
                Description = "Статьи не существует"
            });
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
        public async Task<IdentityResult> Update(int articleId, UpdateArticleRequest updateArticleRequest, string login)
        {
            var initiator = await _userRepository.GetByLogin(login);
            var role = await _roleRepository.GetRoleById(initiator.RoleId);
            var entity = await _articleRepository.Get(articleId);

            if (entity != null)
            {
                if (role.RoleName == "Модератор")
                {
                    var article = _mapper.Map<Article>(updateArticleRequest);
                    await _articleRepository.Update(articleId, article);
                    return IdentityResult.Success;
                }

                if (role.RoleName == "Пользователь" && initiator.UserId == entity.UserId)
                {
                    var article = _mapper.Map<Article>(updateArticleRequest);
                    await _articleRepository.Update(articleId, article);
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed(new IdentityError
            {
                Description = "Статьи не существует"
            });
        }
    }
}
