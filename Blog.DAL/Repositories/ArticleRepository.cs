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
                if (entry.State == EntityState.Detached) context.Articles.Attach(article);

                foreach (var tag in article.Tags)
                {
                    var existingTag = await context.Tags.FindAsync(tag.TagId);
                    if (existingTag != null)
                    {
                        existingTag.Articles.Add(article);
                    }
                    else
                    {
                        context.Tags.Add(tag);
                    }
                }

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
                return await context.Articles.Where(a => a.ArticleId == articleId)
                                            .Include(a => a.Tags)
                                            .Include(a => a.Author)
                                            .Include(a => a.Comments)
                                            .FirstOrDefaultAsync();
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
                var articles = await context.Articles
                                             .Include(a => a.Tags)
                                             .Include(a => a.Author)
                                             .ToListAsync();
                return [.. articles];
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
                var existingArticle = await context.Articles
                                                    .Include(a => a.Tags)
                                                    .Include(a => a.Author)
                                                    .Include(a => a.Comments)
                                                    .SingleOrDefaultAsync(a => a.ArticleId == articleId);

                var tagsArticle = await context.Tags.Where(t => t.Articles.Contains(existingArticle)).ToListAsync();

                foreach (var oldTag in tagsArticle)
                {
                    oldTag.Articles.Remove(existingArticle);
                    context.Entry(oldTag).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();

                foreach (var newTag in article.Tags)
                {
                    var existingTag = await context.Tags
                                                   .Include(t => t.Articles)
                                                   .SingleOrDefaultAsync(t => t.TagId == newTag.TagId);
                    existingTag.Articles.Add(existingArticle);
                    context.Entry(existingTag).State = EntityState.Modified;
                }

                existingArticle.Title = article.Title;
                existingArticle.Content = article.Content;
                context.Entry(existingArticle).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }
    }
}
