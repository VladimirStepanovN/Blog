using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        Task Add(Comment comment);
        Task<Comment?> Get(int commentId);
        Task<Comment[]> GetComments();
        Task Update(int commentId, Comment comment);
        Task Delete(int commentId);
    }
}
