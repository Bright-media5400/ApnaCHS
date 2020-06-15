using ApnaCHS.Common;
using ApnaCHS.Entities;
using ApnaCHS.Services;
using ApnaCHS.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ApnaCHS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/BillingTransaction")]
    public class BillingTransactionController : BaseController
    {
        private readonly IBillingTransactionService _billingTransactionService;

         public BillingTransactionController()
            : base(new UserService())
        {
            _billingTransactionService = new BillingTransactionService();
        }

         [HttpPost]
         [Route("Create")]
         public async Task<IHttpActionResult> Create([FromBody]BillingTransactionViewModel viewmodel)
         {
             var model = Mapper.Map<BillingTransaction>(viewmodel);
             var result = await _billingTransactionService.Create(model, viewmodel.receiptNo, CurrentUser);

             return Ok(Mapper.Map<BillingTransactionViewModel>(result));
         }

         [HttpPost]
         [Route("Read")]
         public async Task<IHttpActionResult> Read([FromBody] int id)
         {
             var result = await _billingTransactionService.Read(id);
             return Ok(Mapper.Map<BillingTransactionViewModel>(result));
         }

         [HttpPost]
         [Route("Delete")]
         public async Task<IHttpActionResult> Delete([FromBody] int id)
         {
             await _billingTransactionService.Delete(id);
             return Ok();
         }

         [HttpPost]
         [Route("List")]
         public async Task<IHttpActionResult> List([FromBody]BillingTransactionSearchParamViewModel searchParams)
         {
             var inParams = Mapper.Map<BillingTransactionSearchParams>(searchParams);
             var result = await _billingTransactionService.List(inParams);
             var viewmodels = new List<BillingTransactionViewModel>();

             foreach (var item in result)
             {
                 viewmodels.Add(Mapper.Map<BillingTransactionViewModel>(item));
             }

             return Ok(viewmodels);
         }

         [HttpPost]
         [Route("MyHistory")]
         public async Task<IHttpActionResult> MyHistory([FromBody]BillingTransactionSearchParamViewModel searchParams)
         {
             var inParams = Mapper.Map<BillingTransactionSearchParams>(searchParams);
             var result = await _billingTransactionService.MyHistory(inParams);
             var viewmodels = new List<BillingTransactionViewModel>();

             foreach (var item in result)
             {
                 viewmodels.Add(Mapper.Map<BillingTransactionViewModel>(item));
             }

             return Ok(viewmodels);
         }
    }
}