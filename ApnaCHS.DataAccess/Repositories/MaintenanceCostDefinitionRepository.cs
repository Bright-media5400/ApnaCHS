using ApnaCHS.Common;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IMaintenanceCostDefinitionRepository
    {
        Task<MaintenanceCostDefinition> Create(MaintenanceCostDefinition maintenanceCostDefinition);

        Task<MaintenanceCostDefinition> Read(int key);

        Task Update(MaintenanceCostDefinition maintenanceCostDefinition);

        Task Delete(int id);

        Task<List<MaintenanceCostDefinition>> List(MaintenanceCostDefinitionSearchParams searchParams);

    }
    public class MaintenanceCostDefinitionRepository : IMaintenanceCostDefinitionRepository
    {
        public Task<MaintenanceCostDefinition> Create(MaintenanceCostDefinition maintenanceCostDefinition)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {

                    context.MaintenanceCostDefinition.Add(maintenanceCostDefinition);
                    context.SaveChanges();

                    return maintenanceCostDefinition;
                }
            });
            return taskResult;
        }

        public Task Delete(int id)
        {
            var taskresult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.MaintenanceCostDefinition.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Maintenance Cost Definition not found");
                    }

                    context.MaintenanceCostDefinition.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<MaintenanceCostDefinition> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .MaintenanceCostDefinition
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Maintenance Cost Definition not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(MaintenanceCostDefinition maintenanceCostDefinition)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.MaintenanceCostDefinition.FirstOrDefault(p => p.Id == maintenanceCostDefinition.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Maintenance Cost Definition detail not found");
                    }

                    existingRecord.Name = maintenanceCostDefinition.Name;
                    existingRecord.CalculationOnPerSftArea = maintenanceCostDefinition.CalculationOnPerSftArea;
                    existingRecord.For2Wheeler = maintenanceCostDefinition.For2Wheeler;
                    existingRecord.For4Wheeler = maintenanceCostDefinition.For4Wheeler;
                    existingRecord.FacilityType = maintenanceCostDefinition.FacilityType;
                    context.SaveChanges();
                }
            });
            return taskResult;
        }


        public Task<List<MaintenanceCostDefinition>> List(MaintenanceCostDefinitionSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .MaintenanceCostDefinition
                        .ToList();

                    return ctx;
                }
            });
            return taskResult;
        }
    }
}
