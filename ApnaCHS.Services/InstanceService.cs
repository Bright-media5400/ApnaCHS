using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;

namespace ApnaCHS.Services
{
    public interface IInstanceService
    {
        Task<List<Instance>> Dropdown();
    }

    public class InstanceService : IInstanceService
    {
        IInstanceRepository _instanceRepository = null;

        public InstanceService()
        {
            _instanceRepository = new InstanceRepository();
        }

        public Task<List<Instance>> Dropdown()
        {
            return _instanceRepository.Dropdown();
        }
    }
}
