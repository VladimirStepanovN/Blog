using AutoMapper;
using Blog.BLL.BlogConfiguration;
using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.BusinessModels.Responses.TagResponses;
using Blog.BLL.Services.IServices;
using Blog.DAL.Repositories.IRepositories;
using Blog.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Blog.DAL.Entities;

namespace Blog.BLL.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(ConnectionSettings settings)
        {
            _tagRepository = new TagRepository(settings.DefaultConnection);
            var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new BusinessMappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        /// <summary>
        /// Логика сервиса добавления тега
        /// </summary>
        /// <param name="addTagRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Create(AddTagRequest addTagRequest)
        {
            var tag = _mapper.Map<Tag>(addTagRequest);
            await _tagRepository.Add(tag);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса удаления тега
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Delete(int tagId)
        {
            var tag = await _tagRepository.Get(tagId);
            if (tag == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Тега не существует"
                });
            }

            await _tagRepository.Delete(tagId);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Логика сервиса получения тега по Идентификатору
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task<GetTagResponse> Get(int tagId)
        {
            var tag = await _tagRepository.Get(tagId);
            var getTagResponse = _mapper.Map<GetTagResponse>(tag);
            return getTagResponse;
        }

        /// <summary>
        /// Логика сервиса получения всех тегов
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public async Task<GetTagResponse[]> GetAll()
        {
            var tags = await _tagRepository.GetTags();
            var getTagsResponse = _mapper.Map<Tag[], GetTagResponse[]>(tags);
            return getTagsResponse;
        }

        /// <summary>
        /// Логика сервиса обновления тега
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="updateTagRequest"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Update(int tagId, UpdateTagRequest updateTagRequest)
        {
            var tag = _mapper.Map<Tag>(updateTagRequest);
            await _tagRepository.Update(tagId, tag);
            return IdentityResult.Success;
        }
    }
}
