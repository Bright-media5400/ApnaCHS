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
    [RoutePrefix("api/Comment")]
    public class CommentController : BaseController
    {

        private readonly ICommentService _commentService;

        public CommentController()
            : base(new UserService())
        {
            _commentService = new CommentService();
        }

        [HttpPost]
        [Route("CommentMCUpdate")]
        public async Task<IHttpActionResult> CommentMCUpdate([FromBody]CommentMCViewModel viewmodel)
        {
            var model = Mapper.Map<CommentMC>(viewmodel);
            await _commentService.Update(model);
            return Ok();
        }

        [HttpPost]
        [Route("CommentMCAdd")]
        public async Task<IHttpActionResult> CommentMCAdd([FromBody]CommentMCViewModel viewmodel)
        {
            var model = Mapper.Map<CommentMC>(viewmodel);
            return Ok(Mapper.Map<CommentMCViewModel>(await _commentService.New(model, CurrentUser)));
        }

        [HttpPost]
        [Route("CommentFlatOwnerUpdate")]
        public async Task<IHttpActionResult> CommentFlatOwnerUpdate([FromBody]CommentFlatOwnerViewModel viewmodel)
        {
            var model = Mapper.Map<CommentFlatOwner>(viewmodel);
            await _commentService.Update(model);
            return Ok();
        }

        [HttpPost]
        [Route("CommentFlatOwnerAdd")]
        public async Task<IHttpActionResult> CommentFlatOwnerAdd([FromBody]CommentFlatOwnerViewModel viewmodel)
        {
            var model = Mapper.Map<CommentFlatOwner>(viewmodel);
            return Ok(Mapper.Map<CommentFlatOwnerViewModel>(await _commentService.New(model, CurrentUser)));
        }

        [HttpPost]
        [Route("CommentFlatUpdate")]
        public async Task<IHttpActionResult> CommentFlatUpdate([FromBody]CommentFlatViewModel viewmodel)
        {
            var model = Mapper.Map<CommentFlat>(viewmodel);
            await _commentService.Update(model);
            return Ok();
        }

        [HttpPost]
        [Route("CommentFlatAdd")]
        public async Task<IHttpActionResult> CommentFlatAdd([FromBody]CommentFlatViewModel viewmodel)
        {
            var model = Mapper.Map<CommentFlat>(viewmodel);
            return Ok(Mapper.Map<CommentFlatViewModel>(await _commentService.New(model, CurrentUser)));
        }

        [HttpPost]
        [Route("CommentFlatOwnerFamilyUpdate")]
        public async Task<IHttpActionResult> CommentFlatOwnerFamilyUpdate([FromBody]CommentFlatOwnerFamilyViewModel viewmodel)
        {
            var model = Mapper.Map<CommentFlatOwnerFamily>(viewmodel);
            await _commentService.Update(model);
            return Ok();
        }

        [HttpPost]
        [Route("CommentFlatOwnerFamilyAdd")]
        public async Task<IHttpActionResult> CommentFlatOwnerFamilyAdd([FromBody]CommentFlatOwnerFamilyViewModel viewmodel)
        {
            var model = Mapper.Map<CommentFlatOwnerFamily>(viewmodel);

            var result = await _commentService.New(model, CurrentUser);
            return Ok(Mapper.Map<CommentFlatOwnerFamilyViewModel>(result));
        }

        [HttpPost]
        [Route("CommentVehicleUpdate")]
        public async Task<IHttpActionResult> CommentVehicleUpdate([FromBody]CommentVehicleViewModel viewmodel)
        {
            var model = Mapper.Map<CommentVehicle>(viewmodel);
            await _commentService.Update(model);
            return Ok();
        }

        [HttpPost]
        [Route("CommentVehicleAdd")]
        public async Task<IHttpActionResult> CommentVehicleAdd([FromBody]CommentVehicleViewModel viewmodel)
        {
            var model = Mapper.Map<CommentVehicle>(viewmodel);
            return Ok(Mapper.Map<CommentVehicleViewModel>(await _commentService.New(model, CurrentUser)));
        }

    }
}