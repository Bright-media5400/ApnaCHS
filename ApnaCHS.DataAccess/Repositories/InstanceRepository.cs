using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using ApnaCHS.Entities;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IInstanceRepository
    {
        Task<List<Instance>> Dropdown();
    }

    public class InstanceRepository : IInstanceRepository
    {
        public Task<List<Instance>> Dropdown()
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    return context
                        .Instances
                        //.Where(i => !i.bDeleted)
                        .ToList();
                }
            });
            return taskResult;
        }
    }
}
