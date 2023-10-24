using Basic.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Basic.Models;
using System.Reflection.Emit;

namespace AuthSystem.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {

    }
    public DbSet<Merchant> Merchants { get; set; } 

    public DbSet<TransactionHistory> TransactionHistories { get; set; }

    public DbSet<Invoice> Invoices { get; set; }

    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    public DbSet<CardPayment> CardPayments { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {        
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<InvoiceItem>()
        .HasOne(i => i.Invoice)
        .WithMany(i => i.InvoiceItems)
        .HasForeignKey(ii => ii.InvoiceId);
    }
}
