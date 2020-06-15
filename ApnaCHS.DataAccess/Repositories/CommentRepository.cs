using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface ICommentRepository
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

    public class CommentRepository : ICommentRepository
    {
        public Task<CommentFlat> New(CommentFlat comment, ApplicationUser currentuser)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    comment.CommentBy = currentuser.Name;

                    context.CommentFlats.Add(comment);
                    context.SaveChanges();

                    return comment;
                }
            });

            return taskResult;
        }

        public Task Update(CommentFlat comment)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var cd = context.CommentFlats.FirstOrDefault(c => c.Id == comment.Id);

                    if (cd != null && cd.CreatedBy.Equals(Thread.CurrentPrincipal.Identity.Name))
                    {
                        cd.Text = comment.Text;
                        context.SaveChanges();
                    }
                }
            });

            return taskResult;
        }

        public Task<CommentFlatOwner> New(CommentFlatOwner comment, ApplicationUser currentuser)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    comment.CommentBy = currentuser.Name;

                    context.CommentFlatOwners.Add(comment);
                    context.SaveChanges();

                    return comment;
                }
            });

            return taskResult;
        }

        public Task Update(CommentFlatOwner comment)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var cd = context.CommentFlatOwners.FirstOrDefault(c => c.Id == comment.Id);

                    if (cd != null && cd.CreatedBy.Equals(Thread.CurrentPrincipal.Identity.Name))
                    {
                        cd.Text = comment.Text;
                        context.SaveChanges();
                    }
                }
            });

            return taskResult;
        }

        public Task<CommentMC> New(CommentMC comment, ApplicationUser currentuser)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    comment.CommentBy = currentuser.Name;

                    context.CommentMCs.Add(comment);
                    context.SaveChanges();

                    return comment;
                }
            });

            return taskResult;
        }

        public Task Update(CommentMC comment)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var cd = context.CommentMCs.FirstOrDefault(c => c.Id == comment.Id);

                    if (cd != null && cd.CreatedBy.Equals(Thread.CurrentPrincipal.Identity.Name))
                    {
                        cd.Text = comment.Text;
                        context.SaveChanges();
                    }
                }
            });

            return taskResult;
        }

        public Task<CommentFlatOwnerFamily> New(CommentFlatOwnerFamily comment, ApplicationUser currentuser)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    comment.CommentBy = currentuser.Name;

                    context.CommentFlatOwnerFamilies.Add(comment);
                    context.SaveChanges();

                    return comment;
                }
            });

            return taskResult;
        }

        public Task Update(CommentFlatOwnerFamily comment)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var cd = context.CommentFlatOwnerFamilies.FirstOrDefault(c => c.Id == comment.Id);

                    if (cd != null && cd.CreatedBy.Equals(Thread.CurrentPrincipal.Identity.Name))
                    {
                        cd.Text = comment.Text;
                        context.SaveChanges();
                    }
                }
            });

            return taskResult;
        }

        public Task<CommentVehicle> New(CommentVehicle comment, ApplicationUser currentuser)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    comment.CommentBy = currentuser.Name;

                    context.CommentVehicles.Add(comment);
                    context.SaveChanges();

                    return comment;
                }
            });

            return taskResult;
        }

        public Task Update(CommentVehicle comment)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var cd = context.CommentVehicles.FirstOrDefault(c => c.Id == comment.Id);

                    if (cd != null && cd.CreatedBy.Equals(Thread.CurrentPrincipal.Identity.Name))
                    {
                        cd.Text = comment.Text;
                        context.SaveChanges();
                    }
                }
            });

            return taskResult;
        }

    }
}
