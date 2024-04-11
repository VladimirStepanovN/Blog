using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User?> Get(int userId);
        Task<User[]> GetUsers();
        Task<User?> GetByLogin(string login);
        Task<User?> GetByEMail(string email);
        Task Update(int userId, User user);
        Task Delete(int userId);
    }
}
