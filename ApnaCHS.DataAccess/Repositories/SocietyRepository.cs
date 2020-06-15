using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApnaCHS.Common;
using System.Data.Entity;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface ISocietyRepository
    {
        Task<Society> Create(Society society);

        Task<Society> Read(int key);

        Task Update(Society society);

        Task UpdateSetting(Society society);

        Task Delete(int id);

        Task<List<Society>> List(SocietySearchParams searchParams);

        Task<List<SocietyImportResult>> Import(List<Society> societies);

        Task<int> FlatCount(long societyId);

        Task UpdateLoginDetails(Society society);

    }
    public class SocietyRepository : ISocietyRepository
    {
        public Task<Society> Create(Society society)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (society.Complex != null)
                    {
                        society.ComplexId = society.Complex.Id;
                        society.Complex = null;
                    }
                    if (context.Societies.Count(s => s.Name == society.Name) > 0)
                    {
                        throw new Exception("Duplicate society name found");
                    }

                    context.Societies.Add(society);
                    context.SaveChanges();

                    return society;
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
                    var existingRecord = context.Societies.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Society not found");
                    }

                    context.Societies.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<Society> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .Societies
                        .Include(c => c.Complex)
                        .Include(c => c.Complex.State)
                        .Include(c => c.Complex.City)
                        .Include(c => c.SocietyAssets)
                        .Include(c => c.SocietyAssets.Select(s => s.Complex))
                        .Include(c => c.SocietyAssets.Select(s => s.Facility))
                        .Include(c => c.SocietyAssets.Select(s => s.Floor))
                        .Include(c => c.SocietyAssets.Select(s => s.Society))
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Society not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(Society society)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.Societies.FirstOrDefault(p => p.Id == society.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Society detail not found");
                    }
                    if (society.Complex != null)
                    {
                        society.ComplexId = society.Complex.Id;
                        society.Complex = null;
                    }

                    existingRecord.Name = society.Name;
                    existingRecord.RegistrationNo = society.RegistrationNo;
                    existingRecord.DateOfIncorporation = society.DateOfIncorporation;
                    existingRecord.DateOfRegistration = society.DateOfRegistration;
                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task UpdateSetting(Society society)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.Societies.FirstOrDefault(p => p.Id == society.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Society detail not found");
                    }
                    if (society.Complex != null)
                    {
                        society.ComplexId = society.Complex.Id;
                        society.Complex = null;
                    }

                    existingRecord.BillingCycle = society.BillingCycle;
                    existingRecord.DueDays = society.DueDays;
                    existingRecord.Second2Wheeler = society.Second2Wheeler;
                    existingRecord.Second4Wheeler = society.Second4Wheeler;
                    existingRecord.InterestPercent = society.InterestPercent;
                    existingRecord.ApprovalsCount = society.ApprovalsCount;
                    existingRecord.OpeningInterest = society.OpeningInterest;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<Society>> List(SocietySearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .Societies
                        .Include(c => c.Complex)
                        .Include(c => c.Complex.City)
                        .Include(c => c.Complex.State)
                        .Include(c => c.SocietyAssets);

                    if (searchParams.ComplexId.HasValue)
                    {
                        ctx = ctx.Where(c => c.ComplexId == searchParams.ComplexId.Value);
                    }

                    return ctx.ToList();
                }
            });
            return taskResult;
        }

        public Task<List<SocietyImportResult>> Import(List<Society> societies)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var returnList = new List<SocietyImportResult>();

                    foreach (var item in societies)
                    {
                        long id = item.Id;
                        item.Id = 0;

                        try
                        {
                            await Create(item);
                            returnList.Add(new SocietyImportResult() { Id = id, Result = "Society Registered", IsSuccess = true });
                        }
                        catch (Exception ex)
                        {
                            returnList.Add(new SocietyImportResult() { Id = id, Result = ex.Message, IsSuccess = false });
                        }
                    }

                    return returnList;
                }
            });
            return taskResult;
        }

        public Task<int> FlatCount(long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {

                    var flats = (from msf in context.MapsSocietiesToFacilities
                                 join f in context.Facilities on msf.FacilityId equals f.Id
                                 join flr in context.Floors on f.Id equals flr.FacilityId
                                 join fls in context.Flats on flr.Id equals fls.FloorId
                                 where msf.SocietyId == societyId
                                 select fls)
                                 .Count();

                    return flats;
                }
            });
            return taskResult;
        }

        public Task UpdateLoginDetails(Society society)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.Societies.FirstOrDefault(p => p.Id == society.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Society detail not found");
                    }
                    if (society.Complex != null)
                    {
                        society.ComplexId = society.Complex.Id;
                        society.Complex = null;
                    }

                    existingRecord.Email = society.Email;
                    existingRecord.PhoneNo = society.PhoneNo;
                    existingRecord.ContactPerson = society.ContactPerson;
                    context.SaveChanges();
                }
            });
            return taskResult;
        }
    }
}
