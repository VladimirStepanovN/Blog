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
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                var entry = _context.Entry(user);
                if (entry.State == EntityState.Detached)
                    await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаление существующего пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Delete(User user)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Поиск пользователя по Идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User?> Get(Guid id)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Users.Where(u => u.UserId == id).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<User[]> GetUsers()
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Users.ToArrayAsync();
            }
        }

        /// <summary>
        /// Поиск пользователя по Логину. 
        /// В качестве логина используется eMail
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<User?> GetByLogin(string login)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Users.Where(u => u.Login == login).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Update(User user)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                await _context.Users.Where(u => u.UserId == user.UserId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.FirstName, u => user.FirstName)
                .SetProperty(u => u.LastName, u => user.LastName)
                .SetProperty(u => u.Email, u => user.Email));

                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetByEMail(string email)
        {
            await using (BlogContext _context = new BlogContext(_connectionString))
            {
                return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            }
        }
    }
}
