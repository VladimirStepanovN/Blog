using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        Task Add(Comment comment);
        Task<Comment?> Get(Guid id);
        Task<Comment[]> GetComments();
        Task Update(Comment comment);
        Task Delete(Comment comment);
    }
}
