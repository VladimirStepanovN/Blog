using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.IRepositories
{
    public interface ITagRepository
    {
        Task Add(Tag tag);
        Task<Tag?> Get(Guid id);
        Task<Tag[]> GetTags();
        Task Update(Tag tag);
        Task Delete(Tag tag);
    }
}
