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
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                var entry = _context.Entry(comment);
                if (entry.State == EntityState.Detached)
                    await _context.Comments.AddAsync(comment);

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаляет существующий комментарий
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task Delete(Comment comment)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Поиск комментария по Идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Comment?> Get(Guid id)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Comments.Where(c => c.CommentId == id).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Получение всех комментариев
        /// </summary>
        /// <returns></returns>
        public async Task<Comment[]> GetComments()
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Comments.ToArrayAsync();
            }
        }

        /// <summary>
        /// Редактирование существующего комментария
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task Update(Comment comment)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                await _context.Comments.Where(c => c.CommentId == comment.CommentId)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.Content, a => comment.Content));

                await _context.SaveChangesAsync();
            }
        }
    }
}
