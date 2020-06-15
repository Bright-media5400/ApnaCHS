using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Services
{
    public interface ICommentService
    {
        Task<CommentFlat> New(CommentFlat comment, ApplicationUser currentuser);

        Task Update(CommentFlat comment);

        Task<CommentFlatOwner> New(CommentFlatOwner comment, ApplicationUser currentuser);

        Task Update(CommentFlatOwner comment);

        Task<CommentMC> New(CommentMC comment, ApplicationUser currentuser);

        Task Update(CommentMC comment);

        Task<CommentFlatOwnerFamily> New(CommentFlatOwnerFamily comment, ApplicationUser currentuser);

        Task Update(CommentFlatOwnerFamily comment);

        Task<CommentVehicle> New(CommentVehicle comment, ApplicationUser currentuser);

        Task Update(CommentVehicle comment);
    }

    public class CommentService : ICommentService
    {
        ICommentRepository _commentRepository = null;

        public CommentService()
        {
            _commentRepository = new CommentRepository();
        }

        public Task<CommentFlat> New(CommentFlat comment, ApplicationUser currentuser)
        {
            return _commentRepository.New(comment, currentuser);
        }

        public Task Update(CommentFlat comment)
        {
            return _commentRepository.Update(comment);
        }

        public Task<CommentFlatOwner> New(CommentFlatOwner comment, ApplicationUser currentuser)
        {
            return _commentRepository.New(comment, currentuser);
        }

        public Task Update(CommentFlatOwner comment)
        {
            return _commentRepository.Update(comment);
        }

        public Task<CommentMC> New(CommentMC comment, ApplicationUser currentuser)
        {
            return _commentRepository.New(comment, currentuser);
        }

        public Task Update(CommentMC comment)
        {
            return _commentRepository.Update(comment);
        }

        public Task<CommentFlatOwnerFamily> New(CommentFlatOwnerFamily comment, ApplicationUser currentuser)
        {
            return _commentRepository.New(comment, currentuser);
        }

        public Task Update(CommentFlatOwnerFamily comment)
        {
            return _commentRepository.Update(comment);
        }

        public Task<CommentVehicle> New(CommentVehicle comment, ApplicationUser currentuser)
        {
            return _commentRepository.New(comment, currentuser);
        }

        public Task Update(CommentVehicle comment)
        {
            return _commentRepository.Update(comment);
        }

    }
}
