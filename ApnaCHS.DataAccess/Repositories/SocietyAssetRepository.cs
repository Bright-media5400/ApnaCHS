using ApnaCHS.Common;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface ISocietyAssetRepository
    {
        Task<SocietyAsset> Create(SocietyAsset societyAsset);

        Task<SocietyAsset> Read(int key);

        Task Update(SocietyAsset societyAsset);

        Task Delete(int id);

        Task<List<SocietyAsset>> List(SocietyAssetSearchParams searchParams);
    }
    public class SocietyAssetRepository : ISocietyAssetRepository
    {
        public Task<SocietyAsset> Create(SocietyAsset societyAsset)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (societyAsset.Facility != null)
                    {
                        societyAsset.FacilityId = societyAsset.Facility.Id;
                        societyAsset.Facility = null;
                    }
                    if (societyAsset.Society != null)
                    {
                        societyAsset.SocietyId = societyAsset.Society.Id;
                        societyAsset.Society = null;
                    }
                    if (societyAsset.Floor != null)
                    {
                        societyAsset.FloorId = societyAsset.Floor.Id;
                        societyAsset.Floor = null;
                    }
                    if (societyAsset.Complex != null)
                    {
                        societyAsset.ComplexId = societyAsset.Complex.Id;
                        societyAsset.Complex = null;
                    }
                    context.SocietyAssets.Add(societyAsset);
                    context.SaveChanges();

                    return societyAsset;
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
                    var existingRecord = context.SocietyAssets.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Society Asset not found");
                    }

                    context.SocietyAssets.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<SocietyAsset> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .SocietyAssets
                        .Include(c => c.Complex)
                        .Include(c => c.Complex.City)
                        .Include(c => c.Complex.State)
                        .Include(s => s.Society)
                        .Include(f => f.Facility)
                        .Include(s => s.Floor)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Society Asset not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(SocietyAsset societyAsset)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.SocietyAssets.FirstOrDefault(p => p.Id == societyAsset.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Society Asset detail not found");
                    }
                    if (societyAsset.Facility != null)
                    {
                        societyAsset.FacilityId = societyAsset.Facility.Id;
                        societyAsset.Facility = null;
                    }
                    if (societyAsset.Society != null)
                    {
                        societyAsset.SocietyId = societyAsset.Society.Id;
                        societyAsset.Society = null;
                    }
                    if (societyAsset.Floor != null)
                    {
                        societyAsset.FloorId = societyAsset.Floor.Id;
                        societyAsset.Floor = null;
                    }
                    if (societyAsset.Complex != null)
                    {
                        societyAsset.ComplexId = societyAsset.Complex.Id;
                        societyAsset.Complex = null;
                    }
                    existingRecord.Name = societyAsset.Name;
                    existingRecord.IsUsable = societyAsset.IsUsable;
                    existingRecord.IsOperational = societyAsset.IsOperational;
                    existingRecord.Quantity = societyAsset.Quantity;
                    existingRecord.CompanyName = societyAsset.CompanyName;
                    existingRecord.Brand = societyAsset.Brand;
                    existingRecord.PurchaseDate = societyAsset.PurchaseDate;
                    existingRecord.ModelNo = societyAsset.ModelNo;
                    existingRecord.SrNo = societyAsset.SrNo;
                    existingRecord.ContactPerson = societyAsset.ContactPerson;
                    existingRecord.CustomerCareNo = societyAsset.CustomerCareNo;
                    
                    context.SaveChanges();
                }
            });
            return taskResult;
        }
        
        public Task<List<SocietyAsset>> List(SocietyAssetSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .SocietyAssets
                        .Include(c => c.Complex)
                        .Include(c => c.Complex.City)
                        .Include(c => c.Complex.State)
                        .Include(s => s.Society)
                        .Include(f => f.Facility)
                        .Include(s => s.Floor);

                    if (searchParams.FacilityId.HasValue)
                    {
                        ctx = ctx.Where(c => c.FacilityId == searchParams.FacilityId.Value);
                    }

                    if (searchParams.SocietyId.HasValue)
                    {
                        ctx = ctx.Where(c => c.SocietyId == searchParams.SocietyId.Value);
                    }

                    return ctx.ToList();
                }
            });
            return taskResult;
        }
    }
}
