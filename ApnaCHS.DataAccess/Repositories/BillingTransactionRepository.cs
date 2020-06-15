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
    public interface IBillingTransactionRepository
    {
        Task<BillingTransaction> Create(BillingTransaction billingTransaction, int receiptNo, ApplicationUser currentUser);

        Task<BillingTransaction> Read(int key);

        Task Delete(int id);

        Task<List<BillingTransaction>> List(BillingTransactionSearchParams searchParams);

        Task<List<BillingTransaction>> MyHistory(BillingTransactionSearchParams searchParams);
    }
    public class BillingTransactionRepository : IBillingTransactionRepository
    {
        public Task<BillingTransaction> Create(BillingTransaction billingTransaction, int receiptNo, ApplicationUser currentUser)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (billingTransaction.Society != null)
                    {
                        billingTransaction.SocietyId = billingTransaction.Society.Id;
                        billingTransaction.Society = null;
                    }

                    var bill = context
                        .Bills
                        .FirstOrDefault(b => b.ReceiptNo == receiptNo && b.SocietyId == billingTransaction.SocietyId);

                    if (bill == null)
                    {
                        throw new Exception("No bill found for the receipt number");
                    }

                    var max = context.BillingTransactions.Count(b => b.SocietyId == billingTransaction.SocietyId) > 0
                        ? context.BillingTransactions.Where(b => b.SocietyId == billingTransaction.SocietyId).Max(b => b.TransactionNo)
                        : 0;

                    var transact = new BillingTransaction();

                    transact.TransactionNo = max + 1;
                    transact.Date = DateTime.Now;
                    transact.Amount = billingTransaction.Amount;
                    transact.Reference = billingTransaction.Reference;
                    transact.Mode = billingTransaction.Mode;
                    transact.Description = billingTransaction.Description;
                    transact.AuthorizedBy = currentUser.UserName;
                    transact.Bank = billingTransaction.Bank;
                    transact.Branch = billingTransaction.Branch;
                    transact.ChequeNo = billingTransaction.ChequeNo;
                    transact.ChequeDate = billingTransaction.ChequeDate;

                    transact.CreatedDate = billingTransaction.CreatedDate;

                    transact.FlatId = bill.FlatId;
                    transact.SocietyId = bill.SocietyId;
                    transact.Name = bill.Name;

                    context.BillingTransactions.Add(transact);
                    context.SaveChanges();

                    return transact;
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
                    var existingRecord = context.BillingTransactions.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Billing Transaction not found");
                    }

                    context.BillingTransactions.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<BillingTransaction> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .BillingTransactions
                        .Include(s => s.Flat)
                        .Include(s => s.Society)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Billing Transaction not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task<List<BillingTransaction>> List(BillingTransactionSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .BillingTransactions
                        .Include(s => s.Flat)
                        .Include(s => s.Society);

                    if (searchParams.SocietyId.HasValue)
                    {
                        ctx = ctx
                            .Where(t => t.SocietyId == searchParams.SocietyId.Value);
                    }

                    if (searchParams.Year.HasValue)
                    {
                        ctx = ctx
                            .Where(t => t.Date.Year == searchParams.Year.Value);
                    }

                    if (searchParams.Month.HasValue)
                    {
                        ctx = ctx
                            .Where(t => t.Date.Month == searchParams.Month.Value);
                    }

                    return ctx.ToList();
                }
            });
            return taskResult;
        }

        public Task<List<BillingTransaction>> MyHistory(BillingTransactionSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = (from msf in context.MapsSocietiesToFacilities
                               join f in context.Facilities on msf.FacilityId equals f.Id
                               join flr in context.Floors on f.Id equals flr.FacilityId
                               join fls in context.Flats on flr.Id equals fls.FloorId
                               join mffo in context.MapsFlatToFlatOwner on fls.Id equals mffo.FlatId
                               join fo in context.FlatOwners on mffo.FlatOwnerId equals fo.Id
                               join bt in context.BillingTransactions on fls.Id equals bt.FlatId
                               where msf.SocietyId == searchParams.SocietyId.Value && fo.MobileNo == searchParams.Username
                               select bt)
                                .Include(s => s.Flat)
                                .Include(s => s.Society);

                    return ctx.ToList();
                }
            });
            return taskResult;
        }  
    }
}
