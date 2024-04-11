using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.IRepositories
{
    public interface ITagRepository
    {
        Task Add(Tag tag);
        Task<Tag?> Get(int tagId);
        Task<Tag[]> GetTags();
        Task Update(int tagId, Tag tag);
        Task Delete(int tagId);
    }
}
