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
    public interface IBillingTransactionService
    {
        Task<BillingTransaction> Create(BillingTransaction billingTransaction, int receiptNo, ApplicationUser currentUser);

        Task<BillingTransaction> Read(int key);

        Task Delete(int id);

        Task<List<BillingTransaction>> List(BillingTransactionSearchParams searchParams);

        Task<List<BillingTransaction>> MyHistory(BillingTransactionSearchParams searchParams);
    }
    public class BillingTransactionService : IBillingTransactionService
    {
        IBillingTransactionRepository _billingTransactionRepository = null;

        public BillingTransactionService()
        {
            _billingTransactionRepository = new BillingTransactionRepository();
        }

        public Task<BillingTransaction> Create(BillingTransaction billingTransaction, int receiptNo, ApplicationUser currentUser)
        {
            return _billingTransactionRepository.Create(billingTransaction, receiptNo, currentUser);
        }

        public Task Delete(int id)
        {
            return _billingTransactionRepository.Delete(id);
        }

        public Task<List<BillingTransaction>> List(BillingTransactionSearchParams searchParams)
        {
            return _billingTransactionRepository.List(searchParams);
        }

        public Task<BillingTransaction> Read(int key)
        {
            return _billingTransactionRepository.Read(key);
        }

        public Task<List<BillingTransaction>> MyHistory(BillingTransactionSearchParams searchParams)
        {
            return _billingTransactionRepository.MyHistory(searchParams);
        }
    }
}
