using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User?> Get(Guid id);
        Task<User[]> GetUsers();
        Task<User?> GetByLogin(string login);
        Task<User?> GetByEMail(string email);
        Task Update(User user);
        Task Delete(User user);
    }
}
