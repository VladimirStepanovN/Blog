using Blog.DAL.Entities;
using Blog.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для операций с объектами типа User
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Add(User user)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                var entry = context.Entry(user);
                if (entry.State == EntityState.Detached)
                    await context.Users.AddAsync(user);

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаление существующего пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Delete(int userId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user != null)
                {
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Поиск пользователя по Идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User?> Get(int userId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<User[]> GetUsers()
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Users.ToArrayAsync();
            }
        }

        /// <summary>
        /// Поиск пользователя по Логину. 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<User?> GetByLogin(string login)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Users.Where(u => u.Login == login).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Update(int userId, User user)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                await context.Users.Where(u => u.UserId == userId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.FirstName, u => user.FirstName)
                .SetProperty(u => u.LastName, u => user.LastName)
                .SetProperty(u => u.Email, u => user.Email)
                .SetProperty(u => u.Password, u => user.Password));

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Поиск пользователя по Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User?> GetByEMail(string email)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                return await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            }
        }
    }
}
