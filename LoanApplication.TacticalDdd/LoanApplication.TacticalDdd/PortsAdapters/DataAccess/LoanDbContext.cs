using LoanApplication.TacticalDdd.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess
{
    public class LoanDbContext : DbContext
    {
        public DbSet<DomainModel.LoanApplication> LoanApplications { get; set; }
        
        public DbSet<Operator> Operators { get; set; }

        public LoanDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .Property(l => l.Number)
                .HasConversion(x => x.Number, x => new LoanApplicationNumber(x));
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .HasKey(l => l.Id);
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .Property(l => l.Id)
                .HasConversion(x=>x.Value, x=> new LoanApplicationId(x));
            
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .Property(l => l.Number);
            
            var converter = new EnumToStringConverter<LoanApplicationStatus>();
            var converterForScore = new EnumToStringConverter<ApplicationScore>();
            
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .Property(l => l.Status).HasConversion(converter);
            
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Score, s =>
                {
                    s.Property(x => x.Explanation);
                    s.Property(x => x.Score).HasConversion(converterForScore);
                });
            
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Customer)
                .OwnsOne(c =>c.Address, ca =>
                {
                    ca.Property(x => x.Country);
                    ca.Property(x => x.City);
                    ca.Property(x => x.ZipCode);
                    ca.Property(x => x.Street);
                });
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Customer)
                .OwnsOne(c =>c.NationalIdentifier, ni => ni.Property(x=>x.Value));
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Customer)
                .OwnsOne(c =>c.Name, cn =>
                {
                    cn.Property(x => x.First);
                    cn.Property(x => x.Last);
                });
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Customer, c => c.Property(x => x.Birthdate));
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Customer)
                .OwnsOne(c =>c.MonthlyIncome, ma => ma.Property(x=>x.Amount));
            
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Property)
                .OwnsOne(p =>p.Address, pa =>
                {
                    pa.Property(x => x.Country);
                    pa.Property(x => x.City);
                    pa.Property(x => x.ZipCode);
                    pa.Property(x => x.Street);
                });
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Property)
                .OwnsOne(p =>p.Value, pv => pv.Property(x=>x.Amount));
            
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Loan)
                .OwnsOne(l =>l.InterestRate, ir => ir.Property(x=>x.Value));
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Loan)
                .OwnsOne(l =>l.LoanAmount, la => { la.Property(x => x.Amount); });
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Loan, l => l.Property(x => x.LoanNumberOfYears));
                
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Decision, d =>
                {
                    d.OwnsOne(x => x.DecisionBy, db => db.Property(y => y.Value));
                    d.Property(x => x.DecisionDate);
                });
            
            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Registration, r =>
                {
                    r.OwnsOne(x => x.RegisteredBy, db => db.Property(y => y.Value));
                    r.Property(x => x.RegistrationDate);
                });
            
            modelBuilder.Entity<DomainModel.Operator>()
                .HasKey(l => l.Id);
            modelBuilder.Entity<DomainModel.Operator>()
                .Property(l => l.Id)
                .HasConversion(x=>x.Value, x=> new OperatorId(x));
            modelBuilder.Entity<DomainModel.Operator>()
                .OwnsOne(o => o.CompetenceLevel, cl => cl.Property(c=>c.Amount));

        }
    }
}