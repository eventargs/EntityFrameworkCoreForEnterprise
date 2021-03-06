using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Core.DataLayer.Contracts;
using Store.Core.EntityLayer.HumanResources;

namespace Store.Core.DataLayer.Repositories
{
    public class HumanResourcesRepository : Repository, IHumanResourcesRepository
    {
        public HumanResourcesRepository(IUserInfo userInfo, StoreDbContext dbContext)
            : base(userInfo, dbContext)
        {
        }

        public IQueryable<Employee> GetEmployees(Int32 pageSize = 10, Int32 pageNumber = 1)
            => DbContext.Paging<Employee>(pageSize, pageNumber);

        public async Task<Employee> GetEmployeeAsync(Employee entity)
            => await DbContext.Set<Employee>().FirstOrDefaultAsync(item => item.EmployeeID == entity.EmployeeID);

        public async Task<Int32> AddEmployeeAsync(Employee entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<Int32> UpdateEmployeeAsync(Employee changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<Int32> DeleteEmployeeAsync(Employee entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }
    }
}
