using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.DataAccess.Repository
{
    public class ProjectRepository(DbkanbanContext context) : RepositoryBase<Project>(context), IProjectRepository
    {
        public async Task AddUserProject(UserProject userProject)
        {
            await _context.UserProjects.AddAsync(userProject);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
        {
            var projectIds = await _context.UserProjects
                .Where(up => up.IdUser == userId)
                .Select(up => up.IdProject)
                .ToListAsync();

            var projects = await _context.Projects
                .Where(p => projectIds.Contains(p.IdProject))
                .ToListAsync();

            return projects;
        }

        public int CountUsers(int projectID) => _context.UserProjects.Where(p => p.IdProject == projectID).Select(x => x.IdUser).Count();

        public async Task<bool> IsUserInProject(int userId, int projectId) => await _context.UserProjects.AnyAsync(up => up.IdUser == userId && up.IdProject == projectId);
    }
}
