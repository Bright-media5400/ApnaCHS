using ApnaCHS.AppCommon;
using ApnaCHS.Common;
using ApnaCHS.Services;
using ApnaCHS.Web.Models;
using ApnaCHS.Web.Providers;
using AutoMapper;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ApnaCHS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Billing")]
    public class BillingController : BaseController
    {
        private readonly IBillingService _billingService;

        public BillingController()
            : base(new UserService())
        {
            _billingService = new BillingService();
        }

        [HttpPost]
        [Route("Generate")]
        public async Task<IHttpActionResult> Generate([FromBody]dynamic inparams)
        {
            long id = inparams.id;
            DateTime generation = inparams.generation;

            var result = await _billingService.GenerateBill(id, generation);

            foreach (var item in result)
            {
                try
                {
                    var emailProvider = new EmailServiceProvider();
                    string monthName = ProgramCommon.GetMonthName(item.Month);
                    await emailProvider.GenerateBills(item.Name, item.Email, item.Amount, monthName, item.Year);
                }
                catch (Exception) { }

            }

            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public async Task<IHttpActionResult> List([FromBody]BillingSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<BillingSearchParams>(searchParams);
            var result = await _billingService.List(inParams);
            var viewmodels = new List<BillingViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<BillingViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Pending")]
        public async Task<IHttpActionResult> Pending([FromBody]BillingSearchParamViewModel searchParams)
        {
            var inParams = Mapper.Map<BillingSearchParams>(searchParams);
            var result = await _billingService.Pending(inParams);
            var viewmodels = new List<PendingBillViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<PendingBillViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("MyCurrentDues")]
        public async Task<IHttpActionResult> MyCurrentDues([FromBody]dynamic searchParams)
        {
            long societyId = searchParams.societyId;
            string username = searchParams.username;

            var result = await _billingService.MyCurrentDues(societyId, username);
            var viewmodels = new List<BillingViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<BillingViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("FlatCurrentDues")]
        public async Task<IHttpActionResult> FlatCurrentDues([FromBody] long flatId)
        {
            var result = await _billingService.FlatCurrentDues(flatId);
            return Ok(Mapper.Map<BillingViewModel>(result));
        }

        [HttpPost]
        [Route("MyBills")]
        public async Task<IHttpActionResult> MyBills([FromBody]dynamic searchParams)
        {
            long societyId = searchParams.societyId;
            string username = searchParams.username;

            var result = await _billingService.MyBills(societyId, username);
            var viewmodels = new List<BillingViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<BillingViewModel>(item));
            }

            return Ok(viewmodels);
        }

        [HttpGet]
        [Route("Print")]
        public HttpResponseMessage Print([FromUri]long id, [FromUri]int month, [FromUri]int year, [FromUri]long socid)
        {
            try
            {
                Stream repStream = null;
                var documentCreator = new DocumentCreator();

                //long id = inParams.id;
                //int month = inParams.month;
                //int year = inParams.year;

                try
                {
                    var report = documentCreator.GetBillReportDocument("GetBillDetails",
                        "Bill,BillingLines,FlatOwner,Society",
                        System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/CR/MaintenanceReceipt.rpt"),
                        socid,
                        id,
                        month,
                        year);

                    repStream = report.ExportToStream(ExportFormatType.PortableDocFormat);
                    report.Close();
                    report.Dispose();
                }
                catch (Exception ex)
                {
                    throw new Exception("Crystal report - " + string.Format("{0}{1}", ex.Message, ex.InnerException != null ? (Environment.NewLine + ex.InnerException.Message) : string.Empty));
                }

                var pdfname = string.Format("{0}-{1}{2}.pdf", "Receipt", CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month), year);
                var filename = "~/Uploads/MaintenanceReceipts/" + pdfname;
                FileStream fileStream = File.Create(System.Web.Hosting.HostingEnvironment.MapPath(filename), (int)repStream.Length);
                // Initialize the bytes array with the stream length and then fill it with data
                byte[] bytesInStream = new byte[repStream.Length];
                repStream.Read(bytesInStream, 0, bytesInStream.Length);
                // Use write method to write to the file specified above
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                fileStream.Close();
                fileStream.Dispose();

                var filePath = System.Web.Hosting.HostingEnvironment.MapPath(filename);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        ms.Write(bytes, 0, (int)file.Length);

                        HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                        httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
                        httpResponseMessage.Content.Headers.Add("x-filename", pdfname);

                        string mimeType = System.Web.MimeMapping.GetMimeMapping(pdfname);
                        httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                        httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        httpResponseMessage.Content.Headers.ContentDisposition.FileName = pdfname;
                        httpResponseMessage.StatusCode = HttpStatusCode.OK;

                        file.Close();
                        file.Dispose();

                        return httpResponseMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("Read")]
        public async Task<IHttpActionResult> Read([FromBody] int id)
        {
            var result = await _billingService.Read(id);
            return Ok(Mapper.Map<BillingViewModel>(result));
        }

        [HttpPost]
        [Route("UploadOpeningBills")]
        public async Task<IHttpActionResult> UploadOpeningBills([FromBody]dynamic searchParams)
        {
            long societyId = searchParams.societyId;
            List<UploadBillDetailViewModel> bills = searchParams.bills.ToObject<List<UploadBillDetailViewModel>>();
            long facilityId = searchParams.facilityId;
            DateTime generatedDate = searchParams.generatedDate;

            var model = new List<UploadBillDetail>();
            foreach (var item in bills)
            {
                model.Add(Mapper.Map<UploadBillDetail>(item));
            }

            var result = await _billingService.UploadOpeningBills(model, facilityId, societyId, generatedDate);
            var viewmodels = new List<UploadReplyViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<UploadReplyViewModel>(item));
            }

            return Ok(viewmodels);
        }

    }
}