using Blog.DAL.Entities;
using Blog.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Получение роли по идентификатору
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<Role> GetRoleById(int roleId)
        {
            await using (var context = new BlogContext(_connectionString))
            {
                var role = await context.Roles.Where(r => r.RoleId == roleId).FirstOrDefaultAsync();
                return role;
            }
        }

        /// <summary>
        /// Добавление роли к пользователю
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public async Task<Role> GetRoleUser()
        {
            await using (var context = new BlogContext(_connectionString))
            {
                var role = await context.Roles.Where(r => r.RoleName == "Пользователь").FirstOrDefaultAsync();
                Role userRole = (new Role
                {
                    RoleId = 1,
                    RoleName = "Пользователь"
                });

                if (role == null)
                {
                    await context.Roles.AddAsync(userRole);
                    await context.Roles.AddAsync(new Role
                    {
                        RoleId = 2,
                        RoleName = "Администратор"
                    });
                    await context.Roles.AddAsync(new Role
                    {
                        RoleId = 3,
                        RoleName = "Модератор"
                    });
                    await context.SaveChangesAsync();
                }
                else
                {
                    return role;
                }
                return userRole;
            }
        }
    }
}
