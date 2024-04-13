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
        private readonly IMapper _mapper;

        public CommentService(ConnectionSettings settings)
        {
            _commentRepository = new CommentRepository(settings.DefaultConnection);
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
            var comment = _mapper.Map<Comment>(addCommentRequest);
            await _commentRepository.Add(comment);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса удаления комментария
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Delete(int commentId)
        {
            var comment = await _commentRepository.Get(commentId);
            if (comment == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Комментария не существует"
                });
            }

            await _commentRepository.Delete(commentId);
            return IdentityResult.Success;
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
        public async Task<IdentityResult> Update(int commentId, UpdateCommentRequest updateCommentRequest)
        {
            var comment = _mapper.Map<Comment>(updateCommentRequest);
            await _commentRepository.Update(commentId, comment);
            return IdentityResult.Success;
        }
    }
}
