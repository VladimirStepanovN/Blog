using Blog.DAL.Entities;
using Blog.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для операций с объектами типа Article
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        private readonly string _connectionString;

        public ArticleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Создает новую статью
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public async Task Add(Article article)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                var entry = context.Entry(article);
                if (entry.State == EntityState.Detached)
                    await context.Articles.AddAsync(article);

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаляет существующую статью
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task Delete(int articleId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                var article = await context.Articles.FirstOrDefaultAsync(a => a.ArticleId == articleId);
                if (article != null)
                {
                    context.Articles.Remove(article);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Поиск статьи по Идентификатору
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<Article?> Get(int articleId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Articles.Where(a => a.ArticleId == articleId).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Получение списка статей
        /// </summary>
        /// <returns></returns>
        public async Task<Article[]> GetArticles()
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Articles.ToArrayAsync();
            }
        }

        /// <summary>
        /// Получение списка статей определенного автора по его Идентификатору
        /// </summary>
        /// <returns></returns>
        public async Task<Article[]> GetArticles(int userId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Articles.Where(a => a.UserId == userId).ToArrayAsync();
            }
        }

        /// <summary>
        /// Обновление существующей статьи
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public async Task Update(int articleId, Article article)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                await context.Articles.Where(a => a.ArticleId == articleId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(a => a.Title, a => article.Title)
                .SetProperty(a => a.Content, a => article.Content));

                await context.SaveChangesAsync();
            }      
        }
    }
}
