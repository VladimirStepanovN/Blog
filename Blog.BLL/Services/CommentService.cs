using AutoMapper;
using Blog.BLL.BlogConfiguration;
using Blog.BLL.BusinessModels.Requests.CommentRequests;
using Blog.BLL.BusinessModels.Responses.CommentResponses;
using Blog.BLL.Services.IServices;
using Blog.DAL.Entities;
using Blog.DAL.Repositories;
using Blog.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public CommentService(ConnectionSettings settings)
        {
            _commentRepository = new CommentRepository(settings.DefaultConnection);
            _userRepository = new UserRepository(settings.DefaultConnection);
            _articleRepository = new ArticleRepository(settings.DefaultConnection);
            _roleRepository = new RoleRepository(settings.DefaultConnection);
            var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new BusinessMappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        /// <summary>
        /// Логика сервиса срздания комментария
        /// </summary>
        /// <param name="addCommentRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Create(AddCommentRequest addCommentRequest)
        {
            var user = await _userRepository.Get(addCommentRequest.UserId);
            var article = await _articleRepository.Get(addCommentRequest.ArticleId);

            if(user == null || article == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Пользователя или статьи не существует"
                });
            }

            var comment = _mapper.Map<Comment>(addCommentRequest);
            await _commentRepository.Add(comment);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса удаления комментария
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Delete(int commentId, string login)
        {
            var initiator = await _userRepository.GetByLogin(login);
            var role = await _roleRepository.GetRoleById(initiator.RoleId);
            var entity = await _commentRepository.Get(commentId);

            if (entity != null)
            {
                if (role.RoleName == "Модератор")
                {
                    await _commentRepository.Delete(commentId);
                    return IdentityResult.Success;
                }

                if (role.RoleName == "Пользователь" && initiator.UserId == entity.UserId)
                {
                    await _commentRepository.Delete(commentId);
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed(new IdentityError
            {
                Description = $"Комментария не существует"
            });
        }

        /// <summary>
        /// Логика сервиса получения комментария по его Идентификатору
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<GetCommentResponse> Get(int commentId)
        {
            var comment = await _commentRepository.Get(commentId);
            var getCommentResponse = _mapper.Map<GetCommentResponse>(comment);
            return getCommentResponse;
        }

        /// <summary>
        /// Логика сервиса получения всех комментариев
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public async Task<GetCommentResponse[]> GetAll()
        {
            var comments = await _commentRepository.GetComments();
            var getCommentResponse = _mapper.Map<Comment[], GetCommentResponse[]>(comments);
            return getCommentResponse;
        }

        /// <summary>
        /// Логика сервиса обновления комментария
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="updateCommentRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Update(UpdateCommentRequest updateCommentRequest, string login)
        {
            var initiator = await _userRepository.GetByLogin(login);
            var role = await _roleRepository.GetRoleById(initiator.RoleId);
            var entity = await _commentRepository.Get(updateCommentRequest.CommentId);

            if (entity != null)
            {
                if (role.RoleName == "Модератор")
                {
                    var comment = _mapper.Map<Comment>(updateCommentRequest);
                    await _commentRepository.Update(updateCommentRequest.CommentId, comment);
                    return IdentityResult.Success;
                }

                if (role.RoleName == "Пользователь" && initiator.UserId == entity.UserId)
                {
                    var comment = _mapper.Map<Comment>(updateCommentRequest);
                    await _commentRepository.Update(updateCommentRequest.CommentId, comment);
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed(new IdentityError
            {
                Description = $"Комментария не существует"
            });
        }
    }
}
