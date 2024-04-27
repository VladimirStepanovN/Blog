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
            await using (var context = new BlogContext(_connectionString))
            {
                var entry = context.Entry(tag);
                if (entry.State == EntityState.Detached)
                    await context.Tags.AddAsync(tag);

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаялет сущестующий тег
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task Delete(int tagId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                var tag = await context.Tags.FirstOrDefaultAsync(t => t.TagId ==  tagId);
                if (tag != null)
                {
                    context.Tags.Remove(tag);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Поиск тега по Идентификкатору
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task<Tag?> Get(int tagId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Tags.Where(t => t.TagId == tagId)
                                        .FirstOrDefaultAsync();
            }
        }

		/// <summary>
		/// Поиск тега по Идентификкатору
		/// </summary>
		/// <param name="tagId"></param>
		/// <returns></returns>
		public async Task<Tag?> GetAllInfo(int tagId)
		{
			await using (var context = new BlogContext(_connectionString))
			{
				var tag =  await context.Tags.Include(t => t.Articles)
										.SingleOrDefaultAsync(t => t.TagId == tagId);
                return tag;
			}
		}

		/// <summary>
		/// Получить массивы всех тегов
		/// </summary>
		/// <returns></returns>
		public async Task<Tag[]> GetTags()
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Tags.ToArrayAsync();
            }
        }

        /// <summary>
        /// Обновить существующий тег
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task Update(int tagId, Tag tag)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                await context.Tags.Where(t => t.TagId == tagId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Name, t => tag.Name));

                await context.SaveChangesAsync();
            }
        }
    }
}
