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
    public interface IFlatService
    {
        Task<Flat> Create(Flat flat, int? count);

        Task<Flat> Read(int key);

        Task Update(Flat flat);

        Task Delete(int id);

        Task<List<Flat>> List(FlatSearchParams searchParams);

        Task<List<FlatReportResult>> Report(FlatSearchParams searchParams);

        Task<List<UploadFlat>> ExportFloors(long societyId);

        Task<List<UploadFlat>> UploadFlats(List<UploadFlat> flats);

        Task<Flat> Approve(int id, string note, ApplicationUser currentUser);

        Task<List<ApprovalReply>> BulkApprove(int[] ids, string note, ApplicationUser currentUser);

        Task Reject(int id, string note, ApplicationUser currentUser);
    }
    public class FlatService : IFlatService
    {
        IFlatRepository _flatRepository = null;

        public FlatService()
        {
            _flatRepository = new FlatRepository();
        }

        public Task<Flat> Create(Flat flat, int? count)
        {
            return _flatRepository.Create(flat, count);
        }

        public Task Delete(int id)
        {
            return _flatRepository.Delete(id);
        }

        public Task<List<Flat>> List(FlatSearchParams searchParams)
        {
            return _flatRepository.List(searchParams);
        }

        public Task<Flat> Read(int key)
        {
            return _flatRepository.Read(key);
        }

        public Task Update(Flat flat)
        {
            return _flatRepository.Update(flat);
        }

        public Task<List<FlatReportResult>> Report(FlatSearchParams searchParams)
        {
            return _flatRepository.Report(searchParams);
        }

        public Task<List<UploadFlat>> ExportFloors(long societyId)
        {
            return _flatRepository.ExportFloors(societyId);
        }

        public Task<List<UploadFlat>> UploadFlats(List<UploadFlat> flats)
        {
            return _flatRepository.UploadFlats(flats);
        }

        public Task<Flat> Approve(int id, string note, ApplicationUser currentUser)
        {
            return _flatRepository.Approve(id, note, currentUser);
        }

        public Task<List<ApprovalReply>> BulkApprove(int[] ids, string note, ApplicationUser currentUser)
        {
            return _flatRepository.BulkApprove(ids, note, currentUser);
        }

        public Task Reject(int id, string note, ApplicationUser currentUser)
        {
            return _flatRepository.Reject(id, note, currentUser);
        }
    }
}

    