using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.IRepositories
{
    public interface IArticleRepository
    {
        Task Add(Article article);
        Task<Article?> Get(Guid id);
        Task<Article[]> GetArticles();
        Task Update(Article article);
        Task Delete(Article article);
    }
}
