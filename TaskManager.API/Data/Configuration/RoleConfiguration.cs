using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManager.API.Data.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {

            builder.HasData(
               new IdentityRole
               {
                   Id = "81492543 - e03b - 48a3 - ba85 - c6045f300d4d",
                   Name = "Admin",
                   NormalizedName = "ADMIN",
               },
               new IdentityRole
               {
                   Id = "bbe6ae2c-2716-4c8a-b3f2-b353c65888ae",
                   Name = "User",
                   NormalizedName = "USER"
               }
               );
        }
    }
}
