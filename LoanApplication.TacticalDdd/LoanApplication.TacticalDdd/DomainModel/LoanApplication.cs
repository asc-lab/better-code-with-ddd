using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.DomainModel;

public class LoanApplication : Entity<LoanApplicationId>
{
    public LoanApplicationNumber Number { get; }
    public LoanApplicationStatus Status { get; private set; }
    public ScoringResult Score { get; private set; }
    public Customer Customer { get; }
    public Property Property { get;  }
    public Loan Loan { get; }
        
    public Decision Decision { get; private set; }

    public Registration Registration { get; }

        
    public LoanApplication(LoanApplicationNumber number, Customer customer, Property property, Loan loan, Operator registeredBy)
        : this(number, LoanApplicationStatus.New, customer, property,loan, 
            null,new Registration(SysTime.Today(), registeredBy), null)
    {
    }

    // To satisfy EF Core
    protected LoanApplication()
    {
    }

    public void Evaluate(ScoringRules rules)
    {
        Score = rules.Evaluate(this);
        if (Score.IsRed())
        {
            Status = LoanApplicationStatus.Rejected;
        }
    }

    public void Accept(Operator decisionBy)
    {
        if (Status != LoanApplicationStatus.New)
        {
            throw new ApplicationException("Cannot accept application that is already accepted or rejected");
        }

        if (Score == null)
        {
            throw new ApplicationException("Cannot accept application before scoring");
        }
            
        if (!decisionBy.CanAccept(this.Loan.LoanAmount))
        {
            throw new ApplicationException("Operator does not have required competence level to accept application");
        }
            
        Status = LoanApplicationStatus.Accepted;
        Decision = new Decision(SysTime.Today(), decisionBy);
    }
        
    public void Reject(Operator decisionBy)
    {
        if (Status != LoanApplicationStatus.New)
        {
            throw new ApplicationException("Cannot reject application that is already accepted or rejected");
        }

        Status = LoanApplicationStatus.Rejected;
        Decision = new Decision(SysTime.Today(), decisionBy);
    }
        
    private LoanApplication(
        LoanApplicationNumber number, 
        LoanApplicationStatus status, 
        Customer customer, 
        Property property, 
        Loan loan, 
        ScoringResult score,
        Registration registration,
        Decision decision)
    {
        if (number==null)
            throw new ArgumentException("Number cannot be null");
        if (customer==null)
            throw new ArgumentException("Customer cannot be null");
        if (property==null)
            throw new ArgumentException("Property cannot be null");
        if (loan==null)
            throw new ArgumentException("Loan cannot be null");
        if (registration==null)
            throw new ArgumentException("Registration cannot be null");
           
        Id = new LoanApplicationId(Guid.NewGuid());
        Number = number;
        Status = status;
        Score = score;
        Customer = customer;
        Property = property;
        Loan = loan;
        Registration = registration;
        Decision = decision;
    }
}
    
public class LoanApplicationId : ValueObject<LoanApplicationId>
{
    public Guid Value { get; }

    public LoanApplicationId(Guid value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return Value;
    }

    protected LoanApplicationId()
    {
    }
}
    
public class LoanApplicationNumber : ValueObject<LoanApplicationNumber>
{
    public string Number { get; }
    public LoanApplicationNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Loan application number cannot be null or empty string");
        Number = number;
    }
        
        
    public static LoanApplicationNumber NewNumber() => new LoanApplicationNumber(Guid.NewGuid().ToString());
        
    public static LoanApplicationNumber Of(string number) => new LoanApplicationNumber(number);

    public static implicit operator string(LoanApplicationNumber number) => number.Number;
        
    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return Number;
    }

}