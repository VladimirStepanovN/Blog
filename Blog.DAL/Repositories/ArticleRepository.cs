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
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task Add(Article article)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                var entry = _context.Entry(article);
                if (entry.State == EntityState.Detached)
                    await _context.Articles.AddAsync(article);

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаляет существующую статью
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task Delete(Article article)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Поиск статьи по Идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Article?> Get(Guid id)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Articles.Where(a => a.ArticleId == id).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Получение списка статей
        /// </summary>
        /// <returns></returns>
        public async Task<Article[]> GetArticles()
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Articles.ToArrayAsync();
            }
        }

        /// <summary>
        /// Обновление существующей статьи
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public async Task Update(Article article)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                await _context.Articles.Where(a => a.ArticleId == article.ArticleId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(a => a.Title, a => article.Title)
                .SetProperty(a => a.Content, a => article.Content));

                await _context.SaveChangesAsync();
            }      
        }
    }
}
