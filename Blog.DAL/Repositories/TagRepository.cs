using Blog.DAL.Entities;
using Blog.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с объектами типа Tag
    /// </summary>
    public class TagRepository : ITagRepository
    {
        private readonly string _connectionString;

        public TagRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет тег к статье
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task Add(Tag tag)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                var entry = _context.Entry(tag);
                if (entry.State == EntityState.Detached)
                    await _context.Tags.AddAsync(tag);

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаялет сущестующий тег
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task Delete(Tag tag)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Поиск тега по Идентификкатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Tag?> Get(Guid id)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Tags.Where(t => t.TagId == id).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Получить массивы всех тегов
        /// </summary>
        /// <returns></returns>
        public async Task<Tag[]> GetTags()
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Tags.ToArrayAsync();
            }
        }

        /// <summary>
        /// Обновить существующий тег
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task Update(Tag tag)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                await _context.Tags.Where(t => t.TagId == tag.TagId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Name, t => tag.Name));

                await _context.SaveChangesAsync();
            }
        }
    }
}
