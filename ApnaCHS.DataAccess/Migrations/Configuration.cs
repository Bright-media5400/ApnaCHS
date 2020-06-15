namespace ApnaCHS.DataAccess.Migrations
{
    using ApnaCHS.AppCommon;
    using ApnaCHS.Entities;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApnaCHS.DataAccess.DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApnaCHS.DataAccess.DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //Roles
            context.Roles.AddOrUpdate(r => r.Id, new ApplicationRole() { Id = 1, Name = "Backend", DisplayName = "Backend", IsBack = true, IsDefault = true, CreatedBy = "Anonymous", ModifiedBy = "Anonymous", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Deleted = true, CanChange = false });
            context.Roles.AddOrUpdate(r => r.Id, new ApplicationRole() { Id = 2, Name = "Frontend", DisplayName = "Frontend", IsBack = true, IsDefault = true, CreatedBy = "Anonymous", ModifiedBy = "Anonymous", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Deleted = true, CanChange = false });
            context.Roles.AddOrUpdate(r => r.Id, new ApplicationRole() { Id = 3, Name = "Super Admin", DisplayName = "Super Admin", IsBack = null, IsDefault = true, CreatedBy = "Anonymous", ModifiedBy = "Anonymous", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Deleted = true, CanChange = false });
            context.Roles.AddOrUpdate(r => r.Id, new ApplicationRole() { Id = 4, Name = "Admin", DisplayName = "Admin", IsBack = null, IsDefault = true, CreatedBy = "Anonymous", ModifiedBy = "Anonymous", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Deleted = true, CanChange = false });
            context.Roles.AddOrUpdate(r => r.Id, new ApplicationRole() { Id = 5, Name = "Member", DisplayName = "Member", IsBack = false, IsDefault = true, CreatedBy = "Anonymous", ModifiedBy = "Anonymous", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Deleted = true, CanChange = false });
            context.Roles.AddOrUpdate(r => r.Id, new ApplicationRole() { Id = 6, Name = "Owner", DisplayName = "Owner", IsBack = false, IsDefault = true, CreatedBy = "Anonymous", ModifiedBy = "Anonymous", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Deleted = true, CanChange = false });
            context.Roles.AddOrUpdate(r => r.Id, new ApplicationRole() { Id = 7, Name = "Tenant", DisplayName = "Tenant", IsBack = false, IsDefault = true, CreatedBy = "Anonymous", ModifiedBy = "Anonymous", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Deleted = true, CanChange = false });
            context.Roles.AddOrUpdate(r => r.Id, new ApplicationRole() { Id = 8, Name = "Owner Family", DisplayName = "Owner Family", IsBack = false, IsDefault = true, CreatedBy = "Anonymous", ModifiedBy = "Anonymous", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Deleted = true, CanChange = false });
            context.Roles.AddOrUpdate(r => r.Id, new ApplicationRole() { Id = 9, Name = "Tenant Family", DisplayName = "Tenant Family", IsBack = false, IsDefault = true, CreatedBy = "Anonymous", ModifiedBy = "Anonymous", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, Deleted = true, CanChange = false });
            context.SaveChanges();

            var passwordHash = new PasswordHasher();
            var user = new ApplicationUser()
            {
                Id = 1,
                Name = "Admin",
                PasswordHash = passwordHash.HashPassword("admin2019"),
                Email = "admin@apnachs.com",
                UserName = "admin",
                SecurityStamp = Guid.NewGuid().ToString(),
                Deleted = false,
                MaxAttempts = ProgramCommon.MaxAttempts,
                IsBack = true,
                IsDefault = true
            };
            user.Roles.Add(new ApplicationUserRole() { RoleId = 1 });
            context.Users.AddOrUpdate(u => u.UserName, user);
            context.SaveChanges();
        }
    }
}
