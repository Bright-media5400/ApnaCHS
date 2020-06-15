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
    public interface IBillingService
    {
        Task<List<GenerateBillResult>> GenerateBill(long societyId, DateTime generationDate);

        Task<List<Bill>> List(BillingSearchParams searchParams);

        Task<List<PendingBill>> Pending(BillingSearchParams searchParams);

        Task<List<Bill>> MyCurrentDues(long societyId, string username);

        Task<Bill> FlatCurrentDues(long flatId);

        Task<List<Bill>> MyBills(long societyId, string username);

        Task<Bill> Read(int key);

        Task<List<UploadReply>> UploadOpeningBills(List<UploadBillDetail> bills, long facilityId, long societyId, DateTime generatedDate);
    }
    public class BillingService : IBillingService
    {
        IBillingRepository _billingRepository = null;

        public BillingService()
        {
            _billingRepository = new BillingRepository();
        }

        public Task<List<GenerateBillResult>> GenerateBill(long societyId, DateTime generationDate)
        {
            return _billingRepository.GenerateBill(societyId, generationDate);
        }

        public Task<List<Bill>> List(BillingSearchParams searchParams)
        {
            return _billingRepository.List(searchParams);
        }

        public Task<List<PendingBill>> Pending(BillingSearchParams searchParams)
        {
            return _billingRepository.Pending(searchParams);
        }

        public Task<List<Bill>> MyCurrentDues(long societyId, string username)
        {
            return _billingRepository.MyCurrentDues(societyId, username);
        }

        public Task<Bill> FlatCurrentDues(long flatId)
        {
            return _billingRepository.FlatCurrentDues(flatId);
        }

        public Task<List<Bill>> MyBills(long societyId, string username)
        {
            return _billingRepository.MyBills(societyId, username);
        }

        public Task<Bill> Read(int key)
        {
            return _billingRepository.Read(key);
        }

        public Task<List<UploadReply>> UploadOpeningBills(List<UploadBillDetail> bills, long facilityId, long societyId, DateTime generatedDate)
        {
            return _billingRepository.UploadOpeningBills(bills, facilityId, societyId, generatedDate);
        }
    }
}
