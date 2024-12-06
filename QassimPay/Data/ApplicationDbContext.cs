using Microsoft.EntityFrameworkCore;
using QassimPay.Models;
using System.Security.Cryptography.Xml;

namespace QassimPay.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> User { get; set; }
        public DbSet<WalletModel> Wallet { get; set; }
        public DbSet<AddressModel> Address { get; set; }
        public DbSet<BillingModel> Billing { get; set; }
        public DbSet<TransferModel> Transfer { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly map the UserModel to the "USER" table
           // modelBuilder.Entity<UserModel>()
                //.ToTable("USER");

            modelBuilder.Entity<WalletModel>()
            .HasKey(w => w.Wallet_ID);

            modelBuilder.Entity<WalletModel>()
                .Property(w => w.Wallet_ID)
                .ValueGeneratedOnAdd();  // Ensure Wallet_ID is auto-generated

            modelBuilder.Entity<WalletModel>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wallets)
                .HasForeignKey(w => w.User_ID);

            modelBuilder.Entity<AddressModel>()
                .HasOne(a => a.User)
                .WithMany(u => u.Adresses)
                .HasForeignKey(a => a.U_ID);

            modelBuilder.Entity<TransferModel>()
                .HasOne(t => t.Wallet)
                .WithMany(w => w.Transfers)
                .HasForeignKey(t => t.Sender_ID);

            modelBuilder.Entity<BillingModel>()
                .HasOne(b => b.Wallet)
                .WithMany(w => w.Billings)
                .HasForeignKey(b => b.W_ID);
        }



    }
}