using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SupportApp.Entities;

namespace SupportApp.DataLayer.Mappings
{
    public static class EntitiyMappings
    {
        public static void AddCustomEntityMappings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SoftwareVersion>(build =>
            {
                build.Property(p => p.Name).HasMaxLength(450).IsRequired();
            });

            modelBuilder.Entity<RequestType>(build =>
            {
                build.Property(p => p.Name).HasMaxLength(450).IsRequired();
            });

            modelBuilder.Entity<Customer>(build =>
            {
                build.Property(p => p.Number).HasMaxLength(450).IsRequired();
                build.Property(p => p.Name).HasMaxLength(450).IsRequired();
                build.Property(p => p.Family).HasMaxLength(450).IsRequired();
                build.Ignore(p => p.FullName);
                build.HasOne(p => p.SoftwareVersion)
                    .WithMany(p => p.Customers);
                build.Property(p => p.LockNumber).HasMaxLength(450).IsRequired();
                build.Property(p => p.LockVersion).HasMaxLength(450).IsRequired();
                build.Property(p => p.AccountCount).HasMaxLength(450).IsRequired();
                build.Property(p => p.CompanyCount).HasMaxLength(450).IsRequired();
                build.Property(p => p.Address).IsRequired();
                build.Property(p => p.Tell).HasMaxLength(450).IsRequired();
            });
        }
    }
}
