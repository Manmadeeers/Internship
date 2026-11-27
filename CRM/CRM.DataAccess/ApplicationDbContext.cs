using CRM.Model.Companies;
using CRM.Model.Customers;
using CRM.Model.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.DataAccess;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<CustomerModel>Customers=> Set<CustomerModel>();
    public DbSet<CompanyModel>Companies => Set<CompanyModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CustomerModel>(entity =>
        {
            entity.ToTable("Customer");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e=>e.Email).IsUnique();
            entity.HasIndex(e=>e.PhoneNumber).IsUnique();
            entity.HasIndex(e => e.LastName);

            entity.HasOne(customer => customer.Company)
                .WithMany(company => company.Customers)
                .HasForeignKey(customer => customer.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<CompanyModel>(entity =>
        {
            entity.ToTable("Company");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}