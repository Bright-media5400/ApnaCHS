using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ApnaCHS.Services;
using ApnaCHS.Web.Controllers;
using ApnaCHS.Web.Models;

namespace ApnaCHS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Instance")]
    public class InstanceController : BaseController
    {
        private readonly IInstanceService _instanceService;

        public InstanceController()
            : base(new UserService())
        {
            _instanceService = new InstanceService();
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Dropdown")]
        public async Task<IHttpActionResult> Dropdown()
        {
            var result = await _instanceService.Dropdown();
            var viewmodels = new List<InstanceViewModel>();

            foreach (var item in result)
            {
                viewmodels.Add(Mapper.Map<InstanceViewModel>(item));
            }

            return Ok(viewmodels);
        }
    }
}