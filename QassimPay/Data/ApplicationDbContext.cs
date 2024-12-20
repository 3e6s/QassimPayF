using Microsoft.EntityFrameworkCore;
using QassimPay.Models;

namespace QassimPay.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Define DbSet properties for your models
        public DbSet<UserModel> User { get; set; }
        public DbSet<WalletModel> Wallet { get; set; }
        public DbSet<AddressModel> Address { get; set; }
        public DbSet<BillingModel> Billing { get; set; }
        public DbSet<TransferModel> Transfer { get; set; }
        public DbSet<CurrencyExchangeModel> CurrencyExchange { get; set; }

        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // OnModelCreating method for custom configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserModel configuration
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.ToTable("User"); // Map to PostgreSQL "USER" table
                entity.Property(e => e.ID).HasColumnName("id"); // Match column names
                entity.Property(e => e.First_name).HasColumnName("first_name");
                entity.Property(e => e.Last_name).HasColumnName("last_name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Username).HasColumnName("username");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Monthly_income).HasColumnName("monthly_income");
            });

            // Wallet model configuration
            modelBuilder.Entity<WalletModel>(entity =>
            {
                entity.HasKey(w => w.Wallet_ID); // Define primary key
                entity.Property(w => w.Wallet_ID).ValueGeneratedOnAdd();
                entity.HasOne(w => w.User)
                      .WithMany(u => u.Wallets)
                      .HasForeignKey(w => w.User_ID);
            });

            // Address model configuration
           
            modelBuilder.Entity<AddressModel>(entity =>
            {
                entity.HasKey(a => a.ADD_ID); // Correct primary key
                entity.Property(a => a.ADD_ID).ValueGeneratedOnAdd(); // Auto-generate ID
                entity.Property(a => a.Street_Adress).IsRequired();
                entity.Property(a => a.City).IsRequired();
                entity.Property(a => a.State).IsRequired();
                entity.Property(a => a.Postal_Code).IsRequired();
                entity.Property(a => a.Country).IsRequired();

                entity.HasOne(a => a.User)
                      .WithMany(u => u.Adresses) // Ensure UserModel has a navigation property
                      .HasForeignKey(a => a.U_ID);
            });


            // Transfer model configuration
            modelBuilder.Entity<TransferModel>(entity =>
            {
                entity.HasKey(t => t.Receipt_ID);
                entity.HasOne(t => t.Wallet)
                      .WithMany(w => w.Transfers)
                      .HasForeignKey(t => t.Sender_ID);
            });

            // Billing model configuration
            modelBuilder.Entity<BillingModel>(entity =>
            {
                entity.HasKey(b => b.Billing_ID);
                entity.HasOne(b => b.Wallet)
                      .WithMany(w => w.Billings)
                      .HasForeignKey(b => b.W_ID);
            });

            // CurrencyExchangeModel configuration
            modelBuilder.Entity<CurrencyExchangeModel>(entity =>
            {
                entity.HasKey(c => c.ID);
                entity.HasData( // Seed data
                    new CurrencyExchangeModel { ID = 1, CurrencyFrom = "USD", CurrencyTo = "SAR", ExchangeRate = 3.75M },
                    new CurrencyExchangeModel { ID = 2, CurrencyFrom = "SAR", CurrencyTo = "USD", ExchangeRate = 0.27M },
                    new CurrencyExchangeModel { ID = 3, CurrencyFrom = "USD", CurrencyTo = "AED", ExchangeRate = 3.67M },
                    new CurrencyExchangeModel { ID = 4, CurrencyFrom = "AED", CurrencyTo = "USD", ExchangeRate = 0.27M },
                    new CurrencyExchangeModel { ID = 5, CurrencyFrom = "USD", CurrencyTo = "EGP", ExchangeRate = 30.96M },
                    new CurrencyExchangeModel { ID = 6, CurrencyFrom = "EGP", CurrencyTo = "USD", ExchangeRate = 0.032M },
                    new CurrencyExchangeModel { ID = 7, CurrencyFrom = "USD", CurrencyTo = "QAR", ExchangeRate = 3.64M },
                    new CurrencyExchangeModel { ID = 8, CurrencyFrom = "QAR", CurrencyTo = "USD", ExchangeRate = 0.27M },
                    new CurrencyExchangeModel { ID = 9, CurrencyFrom = "USD", CurrencyTo = "KWD", ExchangeRate = 0.31M },
                    new CurrencyExchangeModel { ID = 10, CurrencyFrom = "KWD", CurrencyTo = "USD", ExchangeRate = 3.24M }
                );
            });
        }    }
}