using ApnaCHS.Common;
using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Services
{
    public interface IMaintenanceCostDefinitionService
    {
        Task<MaintenanceCostDefinition> Create(MaintenanceCostDefinition maintenanceCostDefinition);

        Task<MaintenanceCostDefinition> Read(int key);

        Task Update(MaintenanceCostDefinition maintenanceCostDefinition);

        Task Delete(int id);

        Task<List<MaintenanceCostDefinition>> List(MaintenanceCostDefinitionSearchParams searchParams);

    }
    public class MaintenanceCostDefinitionService : IMaintenanceCostDefinitionService
    {
        IMaintenanceCostDefinitionRepository _maintenanceCostDefinitionRepository = null;

        public MaintenanceCostDefinitionService()
        {
            _maintenanceCostDefinitionRepository = new MaintenanceCostDefinitionRepository();
        }

        public Task<MaintenanceCostDefinition> Create(MaintenanceCostDefinition maintenanceCostDefinition)
       {
           return _maintenanceCostDefinitionRepository.Create(maintenanceCostDefinition);
       }

        public Task Delete(int id)
        {
            return _maintenanceCostDefinitionRepository.Delete(id);
        }

        public Task<List<MaintenanceCostDefinition>> List(MaintenanceCostDefinitionSearchParams searchParams)
        {
            return _maintenanceCostDefinitionRepository.List(searchParams);
        }

        public Task<MaintenanceCostDefinition> Read(int key)
        {
            return _maintenanceCostDefinitionRepository.Read(key);
        }

        public Task Update(MaintenanceCostDefinition maintenanceCostDefinition)
        {
            return _maintenanceCostDefinitionRepository.Update(maintenanceCostDefinition);
        }
    }
}
