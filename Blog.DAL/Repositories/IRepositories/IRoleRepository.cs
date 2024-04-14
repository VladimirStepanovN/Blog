using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.IRepositories
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleUser();
        Task<Role> GetRoleById(int roleId);
    }
}
