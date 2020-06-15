using ApnaCHS.Common;
using ApnaCHS.Entities;
using ApnaCHS.Services;
using ApnaCHS.Web.Models;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ApnaCHS.Web.Controllers
{
   
        [Authorize]
        [RoutePrefix("api/FlatParking")]
        public class FlatParkingController : BaseController
        {
            private readonly IFlatParkingService _flatParkingService;

            public FlatParkingController()
                : base(new UserService())
            {
                _flatParkingService = new FlatParkingService();
            }

            [HttpPost]
            [Route("Create")]
            public async Task<IHttpActionResult> Create([FromBody]dynamic viewmodel)
            {
                string flatParking = viewmodel.flatParking;
                int? count = viewmodel.count;
                var flatParkingViewModel = JsonConvert.DeserializeObject<FlatParkingViewModel>(flatParking);

                var model = Mapper.Map<FlatParking>(flatParkingViewModel);
                var result = await _flatParkingService.Create(model, count);

                return Ok(Mapper.Map<FlatParkingViewModel>(result));
            }

            [HttpPost]
            [Route("CreateMultiple")]
            public async Task<IHttpActionResult> CreateMultiple([FromBody]dynamic viewmodel)
            {
                int totalParkings = viewmodel.totalParkings;
                long? floorId = viewmodel.floorId;
                long? facilityId = viewmodel.facilityId;

                await _flatParkingService.Create(totalParkings, floorId, facilityId);
                return Ok();
            }

            [HttpPost]
            [Route("Read")]
            public async Task<IHttpActionResult> Read([FromBody] int id)
            {
                var result = await _flatParkingService.Read(id);
                return Ok(Mapper.Map<FlatParkingViewModel>(result));
            }

            [HttpPost]
            [Route("Update")]
            public async Task<IHttpActionResult> Update([FromBody]FlatParkingViewModel viewmodel)
            {
                var model = Mapper.Map<FlatParking>(viewmodel);
                await _flatParkingService.Update(model);

                return Ok();
            }
            
            [HttpPost]
            [Route("Delete")]
            public async Task<IHttpActionResult> Delete([FromBody] int id)
            {
                await _flatParkingService.Delete(id);
                return Ok();
            }
            
            [HttpPost]
            [Route("List")]
            public async Task<IHttpActionResult> List([FromBody]FlatParkingSearchParamViewModel searchParams)
            {
                var inParams = Mapper.Map<FlatParkingSearchParams>(searchParams);
                var result = await _flatParkingService.List(inParams);
                var viewmodels = new List<FlatParkingViewModel>();

                foreach (var item in result)
                {
                    viewmodels.Add(Mapper.Map<FlatParkingViewModel>(item));
                }

                return Ok(viewmodels);
            }


            [HttpPost]
            [Route("Assign")]
            public async Task<IHttpActionResult> Assign([FromBody]dynamic inparams)
            {
                long flatid = inparams.flatid;
                long[] parkingids = inparams.parkingids.ToObject<long[]>();

                await _flatParkingService.Assign(flatid, parkingids);
                return Ok();
            }
        }
    }
