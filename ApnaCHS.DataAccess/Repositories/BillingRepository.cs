using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ApnaCHS.Common;
using ApnaCHS.AppCommon;
using System.Data.SqlClient;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IBillingRepository
    {
        Task<List<GenerateBillResult>> GenerateBill(long societyId, DateTime generationDate);

        Task<List<Bill>> List(BillingSearchParams searchParams);

        Task<List<PendingBill>> Pending(BillingSearchParams searchParams);

        Task<List<Bill>> MyCurrentDues(long societyId, string username);

        Task<Bill> FlatCurrentDues(long flatId);

        Task<List<Bill>> MyBills(long societyId, string username);

        Task<Bill> LatestBill(long flatId);

        Task<Bill> Read(int key);

        Task<List<UploadReply>> UploadOpeningBills(List<UploadBillDetail> bills, long facilityId, long societyId, DateTime generatedDate);
    }

    public class BillingRepository : IBillingRepository
    {
        IMaintenanceCostRepository _maintenanceCostRepository = null;
        IFlatOwnerRepository _flatOwnerRepository = null;
        IDataApprovalRepository _dataApprovalRepository = null;

        public BillingRepository()
        {
            _maintenanceCostRepository = new MaintenanceCostRepository();
            _flatOwnerRepository = new FlatOwnerRepository();
            _dataApprovalRepository = new DataApprovalRepository();
        }

        public Task<List<GenerateBillResult>> GenerateBill(long societyId, DateTime generationDate)
        {
            var taskresult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var result = new List<GenerateBillResult>();

                    var flats = (from msf in context.MapsSocietiesToFacilities
                                 join fl in context.Floors on msf.FacilityId equals fl.FacilityId
                                 join ft in context.Flats on fl.Id equals ft.FloorId
                                 where msf.SocietyId == societyId
                                 select new
                                 {
                                     flat = ft,
                                     society = msf.Society
                                 })
                                .ToList();

                    foreach (var item in flats)
                    {
                        //check if bill is already generated for the flat
                        if (context.Bills.Any(b => b.BillType == (byte)EnBillType.Monthly && b.Month == generationDate.Month && b.Year == generationDate.Year && b.FlatId == item.flat.Id))
                        {
                            continue;
                        }

                        var flatowner = await _flatOwnerRepository.CurrentOwner(item.flat.Id);
                        if (flatowner != null && flatowner.IsApproved)
                        {
                            var mclines = await _maintenanceCostRepository.GetCostLineForBillGeneration(item.flat.Id, societyId, generationDate);
                            var billinglines = new List<BillingLine>();

                            foreach (var mc in mclines)
                            {
                                var bline = new BillingLine();
                                bline.Definition = mc.Definition.Name;

                                if (mc.Definition.For2Wheeler)
                                {
                                    var total2Wheeler = item.flat.Vehicles.Count(f => f.Type == (byte)EnParkingType.TwoWheeler);
                                    if (total2Wheeler > 0)
                                    {
                                        var fromSecond = item.society.Second2Wheeler;

                                        bline.Amount = !fromSecond.HasValue ? total2Wheeler * mc.Amount : mc.Amount + ((total2Wheeler - 1) * fromSecond.Value);
                                        bline.BaseAmount = mc.Amount;
                                        bline.OtherAmount = fromSecond;
                                        bline.OnArea = mc.Definition.CalculationOnPerSftArea ? item.flat.TotalArea : new Nullable<int>();
                                        billinglines.Add(bline);
                                    }
                                }
                                else if (mc.Definition.For4Wheeler)
                                {
                                    var total4Wheeler = item.flat.Vehicles.Count(f => f.Type == (byte)EnParkingType.FourWheeler);
                                    if (total4Wheeler > 0)
                                    {
                                        var fromSecond = item.society.Second4Wheeler;

                                        bline.Amount = !fromSecond.HasValue ? total4Wheeler * mc.Amount : mc.Amount + ((total4Wheeler - 1) * fromSecond.Value);
                                        bline.BaseAmount = mc.Amount;
                                        bline.OtherAmount = fromSecond;
                                        bline.OnArea = mc.Definition.CalculationOnPerSftArea ? item.flat.TotalArea : new Nullable<int>();
                                        billinglines.Add(bline);
                                    }
                                }
                                else
                                {
                                    bline.Amount = mc.Amount * (mc.Definition.CalculationOnPerSftArea ? item.flat.TotalArea : 1);
                                    bline.BaseAmount = mc.Amount;
                                    bline.OnArea = mc.Definition.CalculationOnPerSftArea ? item.flat.TotalArea : new Nullable<int>();
                                    billinglines.Add(bline);
                                }
                            }

                            //get pending amount
                            decimal pendingAmount = await GetPendingAmountForFlat(item.flat.Id);
                            var latest = await LatestBill(item.flat.Id);

                            if (pendingAmount != 0)
                            {
                                var arrears = new BillingLine();
                                arrears.Definition = "Arrears";
                                arrears.Amount = pendingAmount;
                                arrears.BaseAmount = pendingAmount;
                                arrears.OnArea = null;

                                billinglines.Add(arrears);
                            }

                            //calculate interest
                            if (latest != null
                                && ((latest.BillType == (byte)EnBillType.Opening && item.society.OpeningInterest) ||  latest.BillType == (byte)EnBillType.Monthly)
                                && item.society.InterestPercent.HasValue
                                && pendingAmount > 0)
                            {
                                decimal amt = Math.Round(pendingAmount * (item.society.InterestPercent.Value / 100), 2);

                                var bline = new BillingLine();
                                bline.Definition = "Interest";
                                bline.Amount = amt;
                                bline.BaseAmount = amt;
                                bline.OnArea = null;

                                billinglines.Add(bline);
                            }

                            var max = context.Bills.Count(b => b.SocietyId == societyId) > 0 ? context.Bills.Where(b => b.SocietyId == societyId).Max(b => b.ReceiptNo) : 0;
                            var bill = new Bill();
                            bill.Name = flatowner.Name;
                            bill.ReceiptNo = max + 1;
                            bill.Date = generationDate;
                            bill.Amount = billinglines.Sum(b => b.Amount);
                            bill.Month = bill.Date.Month;
                            bill.Year = bill.Date.Year;
                            bill.FlatId = item.flat.Id;
                            bill.SocietyId = societyId;
                            bill.BillingLines = billinglines;
                            bill.Pending = pendingAmount;
                            bill.MonthlyAmount = bill.Amount - bill.Pending;
                            bill.BillType = (byte)EnBillType.Monthly;

                            //add new bill
                            context.Bills.Add(bill);
                            context.SaveChanges();

                            var reply = new GenerateBillResult();

                            reply.Name = flatowner.Name;
                            reply.Email = flatowner.EmailId;
                            reply.Amount = bill.Amount;
                            reply.Month =  bill.Month;
                            reply.Year = bill.Year;

                            result.Add(reply);
                        }
                    }
                    return result;
                }
            });
            return taskresult;
        }

        public Task<List<Bill>> List(BillingSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .Bills
                        .Where(b => b.SocietyId == searchParams.SocietyId)
                        .Include(c => c.Society)
                        .Include(c => c.Flat)
                        .AsQueryable();

                    if (searchParams.Year.HasValue)
                    {
                        ctx = ctx
                            .Where(t => t.Year == searchParams.Year.Value);
                    }

                    if (searchParams.Month.HasValue)
                    {
                        ctx = ctx
                            .Where(t => t.Month == searchParams.Month.Value);
                    }

                    if (searchParams.Facility.HasValue)
                    {
                        ctx = ctx
                            .Where(t => t.Flat.Floor.FacilityId == searchParams.Facility.Value);
                    }

                    if (searchParams.Floor.HasValue)
                    {
                        ctx = ctx
                            .Where(t => t.Flat.FloorId == searchParams.Floor.Value);
                    }

                    return ctx.ToList();
                }
            });
            return taskResult;
        }

        public Task<List<PendingBill>> Pending(BillingSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var societyIdParameter = new SqlParameter("@SocietyId", searchParams.SocietyId);

                    var result = context.Database
                        .SqlQuery<PendingBill>("GetPendingBills @SocietyId", societyIdParameter)
                        .ToList();

                    return result;
                }
            });
            return taskResult;
        }

        public Task<List<Bill>> MyCurrentDues(long societyId, string username)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    //throw new Exception();
                    var qry = (from msf in context.MapsSocietiesToFacilities
                               join f in context.Facilities on msf.FacilityId equals f.Id
                               join flr in context.Floors on f.Id equals flr.FacilityId
                               join fls in context.Flats on flr.Id equals fls.FloorId
                               join mffo in context.MapsFlatToFlatOwner on fls.Id equals mffo.FlatId
                               join fo in context.FlatOwners on mffo.FlatOwnerId equals fo.Id
                               where msf.SocietyId == societyId && fo.MobileNo == username
                               select new
                               {
                                   Flat = fls
                               })
                               .ToList();

                    var result = new List<Bill>();

                    foreach (var item in qry)
                    {
                        var pending = await GetPendingAmountForFlat(item.Flat.Id);
                        if (pending != 0)
                        {
                            var latest = await LatestBill(item.Flat.Id);
                            if (latest != null)
                            {
                                latest.Amount = pending;
                                result.Add(latest);
                            }
                        }
                    }

                    return result;
                }
            });
            return taskResult;
        }

        public Task<Bill> FlatCurrentDues(long flatId)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {

                    var pending = await GetPendingAmountForFlat(flatId);
                    if (pending != 0)
                    {
                        var latest = await LatestBill(flatId);
                        if (latest != null)
                        {
                            latest.Amount = pending;
                            return latest;
                        }
                    }

                    return null;
                }
            });
            return taskResult;
        }

        public Task<List<Bill>> MyBills(long societyId, string username)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var qry = (from msf in context.MapsSocietiesToFacilities
                               join f in context.Facilities on msf.FacilityId equals f.Id
                               join flr in context.Floors on f.Id equals flr.FacilityId
                               join fls in context.Flats on flr.Id equals fls.FloorId
                               join mffo in context.MapsFlatToFlatOwner on fls.Id equals mffo.FlatId
                               join fo in context.FlatOwners on mffo.FlatOwnerId equals fo.Id
                               join bl in context.Bills on fls.Id equals bl.FlatId
                               where msf.SocietyId == societyId && fo.MobileNo == username
                               select bl)
                                .Include(c => c.Society)
                                .Include(c => c.Flat)
                                .ToList();
                    return qry;
                }
            });
            return taskResult;
        }

        public Task<Bill> LatestBill(long flatId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .Bills
                        .Where(b => b.FlatId == flatId)
                        .OrderByDescending(b => b.Date)
                        .Take(1)
                        .Include(c => c.Society)
                        .Include(c => c.Flat)
                        .FirstOrDefault();

                    return ctx;
                }
            });
            return taskResult;
        }

        private Task<decimal> GetPendingAmountForFlat(long flatId)
        {
            var taskResult = Task.Run(() =>
           {
               decimal pending = 0;

               using (var context = new DbContext())
               {
                   decimal billtot = 0;
                   decimal payment = 0;

                   var bills = context.Bills.Where(b => b.FlatId == flatId);
                   if (bills.Count() > 0)
                   {
                       billtot = bills.Sum(b => b.MonthlyAmount);
                   }

                   //var latest = await LatestBill(flatId);
                   //if (latest == null)
                   //{
                   //    billtot = 0;
                   var payments = context.BillingTransactions.Where(b => b.FlatId == flatId);
                   if (payments.Count() > 0)
                   {
                       payment = payments.Sum(b => b.Amount);
                   }
                   //}
                   //else
                   //{
                   //    billtot = latest.Amount;

                   //    var payments = context.BillingTransactions.Where(b => b.FlatId == flatId && b.Date > latest.Date);
                   //    if (payments.Count() > 0)
                   //    {
                   //        payment = payments.Sum(b => b.Amount);
                   //    }
                   //}

                   pending = billtot - payment;
               }
               return pending;
           });
            return taskResult;
        }

        public Task<Bill> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .Bills
                        .Include(s => s.Society)
                        .Include(f => f.Flat)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Bill details not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task<List<UploadReply>> UploadOpeningBills(List<UploadBillDetail> bills, long facilityId, long societyId, DateTime generatedDate)
        {
            var taskResult = Task.Run(async() =>
            {
                using (var context = new DbContext())
                {
                    var result = new List<UploadReply>();

                    foreach (var bill in bills)
                    {
                        var reply = new UploadReply();
                        reply.Id = bill.SrNo;

                        var sb = new StringBuilder();

                        if (string.IsNullOrEmpty(bill.FlatName))
                        {
                            sb.Append(",FlatName");
                        }

                        if (bill.Amount == 0)
                        {
                            sb.Append(",Amount");
                        }

                        if (sb.Length > 0)
                        {
                            reply.IsSuccess = false;
                            reply.Message = sb.ToString().Trim(',');
                            result.Add(reply);
                            continue;
                        }

                        var flat = context
                            .Flats
                            .FirstOrDefault(f => f.Name.Equals(bill.FlatName, StringComparison.InvariantCultureIgnoreCase) && f.Floor.FacilityId == facilityId);

                        if (flat == null)
                        {
                            reply.IsSuccess = false;
                            reply.Message = "Flat not found";
                            result.Add(reply);
                            continue;
                        }

                        var flatowner = await _flatOwnerRepository.CurrentOwner(flat.Id);
                        if (flatowner == null)
                        {
                            reply.IsSuccess = false;
                            reply.Message = "Flat owner not found";
                            result.Add(reply);
                            continue;
                        }
                        //---------------------------------------------------------------------------------//

                        var billinglines = new List<BillingLine>();

                        var opening = new BillingLine();
                        opening.Definition = "Opening";
                        opening.Amount = bill.Amount;
                        opening.BaseAmount = bill.Amount;
                        opening.OnArea = null;

                        billinglines.Add(opening);


                        //get pending amount
                        decimal pendingAmount = await GetPendingAmountForFlat(flat.Id);

                        if (pendingAmount != 0)
                        {
                            var arrears = new BillingLine();
                            arrears.Definition = "Arrears";
                            arrears.Amount = pendingAmount;
                            arrears.BaseAmount = pendingAmount;
                            arrears.OnArea = null;

                            billinglines.Add(arrears);
                        }


                        //---------------------------------------------------------------------------------//

                        var max = context.Bills.Count(b => b.SocietyId == societyId) > 0 ?
                            context.Bills.Where(b => b.SocietyId == societyId).Max(b => b.ReceiptNo) :
                            0;

                        var item = new Bill();
                        item.Name = flatowner.Name;
                        item.ReceiptNo = max + 1;
                        item.Date = generatedDate;
                        item.Amount = billinglines.Sum(b => b.Amount);
                        item.Month = generatedDate.Month;
                        item.Year = generatedDate.Year;
                        item.FlatId = flat.Id;
                        item.SocietyId = societyId;
                        item.BillingLines = billinglines;
                        item.Pending = pendingAmount;
                        item.MonthlyAmount = item.Amount - item.Pending;
                        item.BillType = (byte)EnBillType.Opening;
                        item.Note = bill.Note;

                        //add bill
                        context.Bills.Add(item);
                        context.SaveChanges();

                        reply.IsSuccess = true;
                        reply.Message = "Done";
                        result.Add(reply);
                    }
                    return result;
                }
            });
            return taskResult;
        }
    }
}
