using Blog.DAL.Entities;
using Blog.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для операций с объектами Comment
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        private readonly string _connectionString;

        public CommentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Создаёт новый комментарий
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task Add(Comment comment)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                var entry = context.Entry(comment);
                if (entry.State == EntityState.Detached)
                    await context.Comments.AddAsync(comment);

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаляет существующий комментарий
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task Delete(int commentId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                var comment = await context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);
                if (comment != null)
                {
                    context.Comments.Remove(comment);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Поиск комментария по Идентификатору
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<Comment?> Get(int commentId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Comments.Where(c => c.CommentId == commentId).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Получение всех комментариев
        /// </summary>
        /// <returns></returns>
        public async Task<Comment[]> GetComments()
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Comments
                                    .Include(c => c.Article)
                                    .Include(c => c.Author)
                                    .ToArrayAsync();
            }
        }

        /// <summary>
        /// Редактирование существующего комментария
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task Update(int commentId, Comment comment)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                await context.Comments.Where(c => c.CommentId == commentId)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.Content, a => comment.Content));

                await context.SaveChangesAsync();
            }
        }
    }
}
