using Blog.BLL.BusinessModels.Requests.TagRequests;
using Blog.BLL.BusinessModels.Responses.TagResponses;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.IServices
{
    public interface ITagService
    {
        Task<IdentityResult> Create(AddTagRequest addTagRequest);
        Task<GetTagResponse> Get(int tagId);
		Task<GetTagFullResponse> GetAllInfo(int tagId);
		Task<GetTagResponse[]> GetAll();
        Task<IdentityResult> Update(UpdateTagRequest updateTagRequest);
        Task<IdentityResult> Delete(int tagId);
    }
}
