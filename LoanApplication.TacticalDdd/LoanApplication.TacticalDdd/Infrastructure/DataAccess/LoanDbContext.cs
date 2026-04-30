using LoanApplication.TacticalDdd.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanApplication.TacticalDdd.Infrastructure.DataAccess;

public class LoanDbContext : DbContext
{
    public DbSet<DomainModel.LoanApplication> LoanApplications => Set<DomainModel.LoanApplication>();

    public DbSet<Operator> Operators => Set<Operator>();

    public LoanDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OperatorMapping());
        modelBuilder.ApplyConfiguration(new LoanApplicationMapping());
    }
}

class OperatorMapping : IEntityTypeConfiguration<Operator>
{
    public void Configure(EntityTypeBuilder<Operator> builder)
    {
        builder.ToTable("Operators");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new OperatorId(x));

        builder.Property(x => x.Login)
            .HasConversion(x => x.Value, x => new Login(x))
            .IsRequired();

        builder.Property(x => x.Password)
            .HasConversion(x => x.Value, x => new Password(x))
            .IsRequired();

        builder.ComplexProperty(x => x.Name, opts =>
        {
            opts.Property(x => x.First).HasColumnName("FirstName").IsRequired();
            opts.Property(x => x.Last).HasColumnName("LastName").IsRequired();
        });

        builder.Property(x => x.CompetenceLevel)
            .HasConversion(x => x != null ? x.Amount : (decimal?)null,
                x => x.HasValue ? new MonetaryAmount(x.Value) : null)
            .HasColumnName("CompetenceLevel_Amount")
            .IsRequired();
    }
}

class LoanApplicationMapping : IEntityTypeConfiguration<DomainModel.LoanApplication>
{
    public void Configure(EntityTypeBuilder<DomainModel.LoanApplication> builder)
    {
        builder.ToTable("LoanApplications");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new LoanApplicationId(x));

        builder
            .Property(x => x.Number)
            .HasConversion(x => x.Number, x => new LoanApplicationNumber(x));

        builder
            .Property(x => x.Status)
            .HasConversion<string>();

        builder.ComplexProperty(x => x.Score, opts =>
        {
            opts.Property(x => x.Explanation);
            opts.Property(x => x.Score).HasConversion<string>().IsRequired();
        });

        builder.ComplexProperty(x => x.Customer, opts =>
        {
            opts
                .Property(x => x.NationalIdentifier)
                .HasConversion(x => x.Value, x => new NationalIdentifier(x))
                .HasColumnName("Customer_NationalIdentifier_Value");

            opts.ComplexProperty(x => x.Name, name =>
            {
                name.Property(x => x.First).IsRequired();
                name.Property(x => x.Last).IsRequired();
            });

            opts.Property(x => x.Birthdate).IsRequired();

            opts
                .Property(x => x.MonthlyIncome)
                .HasConversion(x => x.Amount, x => new MonetaryAmount(x))
                .HasColumnName("Customer_MonthlyIncome_Amount");

            opts.ComplexProperty(x => x.Address, addr =>
            {
                addr.Property(x => x.Country).IsRequired();
                addr.Property(x => x.ZipCode).IsRequired();
                addr.Property(x => x.City).IsRequired();
                addr.Property(x => x.Street).IsRequired();
            });
        });

        builder.ComplexProperty(x => x.Property, opts =>
        {
            opts.Property(x => x.Value)
                .HasConversion(x => x.Amount, x => new MonetaryAmount(x))
                .HasColumnName("Property_Value_Amount")
                .IsRequired();

            opts.ComplexProperty(x => x.Address, addr =>
            {
                addr.Property(x => x.Country).IsRequired();
                addr.Property(x => x.ZipCode).IsRequired();
                addr.Property(x => x.City).IsRequired();
                addr.Property(x => x.Street).IsRequired();
            });
        });

        builder.ComplexProperty(x => x.Loan, opts =>
        {
            opts.Property(x => x.InterestRate)
                .HasConversion(x => x.Value, x => new Percent(x))
                .HasColumnName("Loan_InterestRate_Value")
                .IsRequired();

            opts.Property(x => x.LoanAmount)
                .HasConversion(x => x.Amount, x => new MonetaryAmount(x))
                .HasColumnName("Loan_LoanAmount_Amount")
                .IsRequired();

            opts.Property(x => x.LoanNumberOfYears).IsRequired();
        });

        builder.ComplexProperty(x => x.Decision, opts =>
        {
            opts.Property(x => x.DecisionDate).IsRequired();
            opts.Property(x => x.DecisionBy)
                .HasConversion(x => x != null ? x.Value : (Guid?)null, x => x.HasValue ? new OperatorId(x.Value) : null)
                .HasColumnName("Decision_DecisionBy_Value");
        });

        builder.ComplexProperty(x => x.Registration, opts =>
        {
            opts.Property(x => x.RegistrationDate).IsRequired();
            opts.Property(x => x.RegisteredBy)
                .HasConversion(x => x != null ? x.Value : (Guid?)null, x => x.HasValue ? new OperatorId(x.Value) : null)
                .HasColumnName("Registration_RegisteredBy_Value");
        });
    }
}