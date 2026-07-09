using SimpleKanbanBoards.Business.Models.Role;
using SimpleKanbanBoards.Business.Service.IService;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IReadOnlyCollection<RoleModel>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAll(r => r.RolName.ToLower() != "admin");
            return roles.Select(r => new RoleModel
            {
                Id = r.IdRol,
                Name = r.RolName
            }).ToList();
        }
    }
}
