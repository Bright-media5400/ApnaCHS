using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ApnaCHS.Entities;
using ApnaCHS.DataAccess.Migrations;

namespace ApnaCHS.DataAccess
{
    public interface IDbContext : IDbContextFactory<DbContext>
    {
    }

    public class DbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IDbContext
    {
        public DbContext()
            : base("DefaultConnection")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
        }

        public DbContext Create()
        {
            return new DbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbContext, Configuration>());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Ticket>()
            //    .HasMany<Transaction>(c => c.Transactions)
            //    .WithRequired(c => c.Ticket)
            //    .HasForeignKey(c => c.TicketId)
            //    .WillCascadeOnDelete(true);

            #region Users, Roles and Permissions related tables setup

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims");

            //modelBuilder.Entity<ApplicationRole>()
            //    .HasMany(c => c.Permissions)
            //    .WithMany(p => p.Roles)
            //    .Map(m =>
            //    {
            //        m.MapLeftKey("ApplicationRoleId");
            //        m.MapRightKey("AccessPermissionId");
            //        m.ToTable("MapRolePermission");
            //    });

            #endregion


            //modelBuilder.Entity<ApplicationUser>()
            //    .HasMany(c => c.UserGroups)
            //    .WithMany(p => p.Users)
            //    .Map(m =>
            //    {
            //        m.MapLeftKey("UserGroupId");
            //        m.MapRightKey("UserId");
            //        m.ToTable("MapUserWithUserGroup");
            //    });

            modelBuilder.Entity<MaintenanceCost>()
                .HasMany(c => c.Flats)
                .WithMany(p => p.MaintenanceCosts)
                .Map(m =>
                {
                    m.MapLeftKey("MaintenanceCostId");
                    m.MapRightKey("FlatId");
                    m.ToTable("MapMaintenanceCostsToFlats");
                });

            #region Cascade delete complex

            modelBuilder.Entity<Complex>()
                .HasMany<Society>(c => c.Societies)
                .WithRequired(c => c.Complex)
                .HasForeignKey(c => c.ComplexId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Complex>()
                .HasMany<Facility>(c => c.Facilities)
                .WithRequired(c => c.Complex)
                .HasForeignKey(c => c.ComplexId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Complex>()
                .HasMany<SocietyAsset>(c => c.SocietyAssets)
                .WithRequired(c => c.Complex)
                .HasForeignKey(c => c.ComplexId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Complex>()
                .HasMany<SocietyAsset>(c => c.SocietyAssets)
                .WithRequired(c => c.Complex)
                .HasForeignKey(c => c.ComplexId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Complex>()
                .HasMany<MapUserToComplex>(c => c.User)
                .WithRequired(c => c.Complex)
                .HasForeignKey(c => c.ComplexId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Society>()
                .HasMany<MapSocietiesToFacilities>(c => c.Facilities)
                .WithRequired(c => c.Society)
                .HasForeignKey(c => c.SocietyId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Society>()
                .HasMany<MapUserToSociety>(c => c.User)
                .WithRequired(c => c.Society)
                .HasForeignKey(c => c.SocietyId)
                .WillCascadeOnDelete(true);

            #endregion


            modelBuilder.Entity<FlatOwner>()
                .HasMany<MapFlatToFlatOwner>(c => c.Flats)
                .WithRequired(c => c.FlatOwner)
                .HasForeignKey(c => c.FlatOwnerId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<MaintenanceCost>()
                .HasMany<CommentMC>(c => c.Comments)
                .WithRequired(c => c.MaintenanceCost)
                .HasForeignKey(c => c.MaintenanceCostId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<MaintenanceCost>()
                .HasMany<DataApproval>(c => c.Approvals)
                .WithRequired(c => c.MaintenanceCost)
                .HasForeignKey(c => c.MaintenanceCostId)
                .WillCascadeOnDelete(true);

        }

        public override int SaveChanges()
        {
            try
            {
                UpdateAuditProperties();
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UpdateAuditProperties()
        {
            var entities =
                ChangeTracker.Entries()
                    .Where(
                        x => x.Entity is IAuditProperties && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = !string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name)
                ? Thread.CurrentPrincipal.Identity.Name
                : "Anonymous";

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((IAuditProperties)entity.Entity).CreatedDate = DateTime.Now;
                    ((IAuditProperties)entity.Entity).CreatedBy = currentUsername;
                    ((IAuditProperties)entity.Entity).Deleted = false;
                }

                ((IAuditProperties)entity.Entity).ModifiedDate = DateTime.Now;
                ((IAuditProperties)entity.Entity).ModifiedBy = currentUsername;
            }
        }

        public DbSet<Instance> Instances { get; set; }
        public DbSet<Society> Societies { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<MapSocietiesToFacilities> MapsSocietiesToFacilities { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<MasterValue> MasterValues { get; set; }

        public DbSet<Flat> Flats { get; set; }
        public DbSet<FlatOwner> FlatOwners { get; set; }
        public DbSet<FlatOwnerFamily> FlatOwnerFamilies { get; set; }

        public DbSet<MapFlatToFlatOwner> MapsFlatToFlatOwner { get; set; }

        public DbSet<SocietyStaff> SocietyStaffList { get; set; }
        public DbSet<SecurityStaff> SecurityStaffList { get; set; }
        public DbSet<SocietyDocument> SocietyDocuments { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<FlatParking> FlatParkings { get; set; }
        public DbSet<Attendance> AttendanceList { get; set; }

        public DbSet<MaintenanceCost> MaintCostList { get; set; }
        public DbSet<MaintenanceCostDefinition> MaintenanceCostDefinition { get; set; }

        public DbSet<Complex> ComplexList { get; set; }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillingLine> BillingLines { get; set; }
        public DbSet<BillingTransaction> BillingTransactions { get; set; }

        public DbSet<MapUserToComplex> MapUserToComplexes { get; set; }
        public DbSet<MapUserToSociety> MapUserToSocieties { get; set; }

        public DbSet<SocietyAsset> SocietyAssets { get; set; }
        public DbSet<DataApproval> DataApprovals { get; set; }

        public DbSet<CommentMC> CommentMCs { get; set; }

        public DbSet<CommentFlat> CommentFlats { get; set; }

        public DbSet<CommentFlatOwner> CommentFlatOwners { get; set; }

        public DbSet<AllIndiaPincode> AllIndiaPincodes { get; set; }

        public DbSet<CommentFlatOwnerFamily> CommentFlatOwnerFamilies { get; set; }

        public DbSet<CommentVehicle> CommentVehicles { get; set; }
    }
}
