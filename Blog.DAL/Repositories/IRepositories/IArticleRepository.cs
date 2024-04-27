using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.IRepositories
{
    public interface IArticleRepository
    {
        Task Add(Article article);
        Task<Article?> Get(int articleId);
        Task<Article[]> GetArticles();
        Task<Article[]> GetArticles(int userId);
		Task Update(int articleId, Article article);
        Task Delete(int articleId);
    }
}
