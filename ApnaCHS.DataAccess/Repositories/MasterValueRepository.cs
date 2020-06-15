using ApnaCHS.AppCommon;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IMasterValueRepository
    {
        Task<List<MasterValue>> GetListActive(EnMasterValueType type);

        Task<List<MasterValue>> GetListAll(EnMasterValueType type);

        Task<List<MasterValue>> DropDown(EnMasterValueType type);

        Task<MasterValue> New(MasterValue model);

        Task Delete(MasterValue model);

        Task Update(MasterValue model);

        Task Active(long id);
    }

    public class MasterValueRepository : IMasterValueRepository
    {

        public Task<List<MasterValue>> GetListActive(EnMasterValueType type)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    return context
                        .MasterValues
                        .Where(g => g.Type == (byte)type && !g.Deleted)
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<List<MasterValue>> GetListAll(EnMasterValueType type)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    return context
                        .MasterValues
                        .Where(g => g.Type == (byte)type)
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<MasterValue> New(MasterValue model)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var row = context.MasterValues.FirstOrDefault(s => s.Text.Trim().ToLower() == model.Text.Trim().ToLower() && s.Type == model.Type);
                    if (row != null)
                        throw new Exception("Text already exists for this type");

                    context.MasterValues.Add(model);
                    context.SaveChanges();
                    return model;
                }
            });
            return taskResult;
        }

        public Task Update(MasterValue model)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (context.MasterValues.Any(s => s.Id != model.Id && s.Text.Trim().ToLower() == model.Text.Trim().ToLower() && s.Type == model.Type))
                        throw new Exception("Text already exists for this type");

                    var row = context.MasterValues.FirstOrDefault(s => s.Id == model.Id);
                    if (row != null)
                    {

                        row.Text = model.Text;
                        row.Description = model.Description;
                        row.CustomFields = model.CustomFields;
                        context.SaveChanges();
                    }                    
                }
            });
            return taskResult;
        }

        public Task Delete(MasterValue model)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var row = context
                        .MasterValues
                        .FirstOrDefault(s => s.Id == model.Id);

                    if (row != null)
                    {
                        row.Deleted = true;
                        context.SaveChanges();
                    }
                }
            });
            return taskResult;

        }

        public Task Active(long id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var row = context.MasterValues.FirstOrDefault(s => s.Id == id);
                    if (row != null)
                    {
                        row.Deleted = false;
                        context.SaveChanges();
                    }
                }
            });

            return taskResult;
        }

        public Task<List<MasterValue>> DropDown(EnMasterValueType type)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    return context
                        .MasterValues
                        .Where(g => g.Type == (byte)type)
                        .ToList();
                }
            });
            return taskResult;
        }
    }
}
