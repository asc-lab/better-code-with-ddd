using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LoanApplication.Contracts;
using LoanApplication.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication.BusinessLogic
{
    public class LoanApplicationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<LoanApplication> loanApplicationRepository;
        private readonly IGenericRepository<Operator> operatorsRepository;
        private readonly ValidationService validationService;
        private readonly ScoringService scoringService;
        
        public LoanApplicationService
        (
            IGenericRepository<LoanApplication> loanApplicationRepository, 
            ValidationService validationService, 
            ScoringService scoringService, 
            IUnitOfWork unitOfWork, IGenericRepository<Operator> operatorsRepository)
        {
            this.loanApplicationRepository = loanApplicationRepository;
            this.validationService = validationService;
            this.scoringService = scoringService;
            this.unitOfWork = unitOfWork;
            this.operatorsRepository = operatorsRepository;
        }

        public async Task<string> CreateLoanApplication(LoanApplicationDto loanApplicationDto, ClaimsPrincipal principal)
        {
            var user = await GetOperatorByLogin(principal.Identity.Name);
            
            var application = new LoanApplication
            {
                Number = GenerateLoanApplicationNumber(),
                Status = LoanApplicationStatus.New,
                Customer = new Customer
                {
                    NationalIdentifier = loanApplicationDto.CustomerNationalIdentifier,
                    FirstName = loanApplicationDto.CustomerFirstName,
                    LastName = loanApplicationDto.CustomerLastName,
                    Birthdate = loanApplicationDto.CustomerBirthdate,
                    MonthlyIncome = loanApplicationDto.CustomerMonthlyIncome,
                    Address = new Address
                    {
                        Country = loanApplicationDto.CustomerAddress.Country,
                        City = loanApplicationDto.CustomerAddress.City,
                        ZipCode = loanApplicationDto.CustomerAddress.ZipCode,
                        Street = loanApplicationDto.CustomerAddress.Street
                    }
                },
                Property = new Property
                {
                    Value = loanApplicationDto.PropertyValue,
                    Address = new Address
                    {
                        Country = loanApplicationDto.PropertyAddress.Country,
                        City = loanApplicationDto.PropertyAddress.City,
                        ZipCode = loanApplicationDto.PropertyAddress.ZipCode,
                        Street = loanApplicationDto.PropertyAddress.Street
                    }
                },
                LoanAmount = loanApplicationDto.LoanAmount,
                LoanNumberOfYears = loanApplicationDto.LoanNumberOfYears,
                InterestRate = loanApplicationDto.InterestRate,
                RegisteredBy = user,
                RegistrationDate = DateTime.Now
            };
            
            validationService.ValidateLoanApplication(application);
            
            await loanApplicationRepository.Create(application);

            await unitOfWork.CommitChanges();

            return application.Number;
        }

        private string GenerateLoanApplicationNumber()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task EvaluateLoanApplication(string applicationNumber)
        {
            var application = await GetLoanApplicationByNumber(applicationNumber);
            
            if (application.Status!=LoanApplicationStatus.New)
                throw new ApplicationException("Cannot evaluate application that is already accepted or rejected");

            await scoringService.EvaluateApplication(application);

            if (application.Score == ApplicationScore.Red)
                application.Status = LoanApplicationStatus.Rejected;
            
            await unitOfWork.CommitChanges();
        }

        public async Task<IList<LoanApplicationInfoDto>> FindLoanApplication(LoanApplicationSearchCriteriaDto criteria)
        {
            var queryAll = loanApplicationRepository.Query();

            if (!string.IsNullOrWhiteSpace(criteria.ApplicationNumber))
            {
                queryAll = queryAll.Where(a => a.Number == criteria.ApplicationNumber);
            }
            
            if (!string.IsNullOrWhiteSpace(criteria.CustomerNationalIdentifier))
            {
                queryAll = queryAll.Where(a => a.Customer.NationalIdentifier == criteria.CustomerNationalIdentifier);
            }
            
            if (!string.IsNullOrWhiteSpace(criteria.DecisionBy))
            {
                queryAll = queryAll.Where(a => a.DecisionBy.Login == criteria.DecisionBy);
            }
            
            if (!string.IsNullOrWhiteSpace(criteria.RegisteredBy))
            {
                queryAll = queryAll.Where(a => a.RegisteredBy.Login == criteria.RegisteredBy);
            }
            
            return await queryAll.Select(a => new LoanApplicationInfoDto
                {
                    Number = a.Number,
                    Status = a.Status.ToString(),
                    CustomerName = $"{a.Customer.FirstName} {a.Customer.LastName}",
                    LoanAmount = a.LoanAmount,
                    DecisionBy = a.DecisionBy!=null ? a.DecisionBy.Login : null,
                    DecisionDate = a.DecisionDate
                })
                .ToListAsync();
        }

        public async Task<LoanApplicationDto> GetLoanApplication(string applicationNumber)
        {
            var loanApplication = await GetLoanApplicationByNumber(applicationNumber);

            return new LoanApplicationDto
            {
                Number = loanApplication.Number,
                Status = loanApplication.Status.ToString(),
                CustomerAddress = new AddressDto
                {
                    City = loanApplication.Customer.Address.City,
                    Country = loanApplication.Customer.Address.Country,
                    Street = loanApplication.Customer.Address.Street,
                    ZipCode = loanApplication.Customer.Address.ZipCode
                },
                CustomerBirthdate = loanApplication.Customer.Birthdate,
                CustomerFirstName = loanApplication.Customer.FirstName,
                CustomerLastName = loanApplication.Customer.LastName,
                CustomerMonthlyIncome = loanApplication.Customer.MonthlyIncome,
                CustomerNationalIdentifier = loanApplication.Customer.NationalIdentifier,
                DecisionBy = loanApplication.DecisionBy?.Login,
                DecisionDate = loanApplication.DecisionDate,
                Score = loanApplication.Score?.ToString(),
                InterestRate = loanApplication.InterestRate,
                LoanAmount = loanApplication.LoanAmount,
                PropertyAddress = new AddressDto
                {
                    City = loanApplication.Property.Address.City,
                    Country = loanApplication.Property.Address.Country,
                    Street = loanApplication.Property.Address.Street,
                    ZipCode = loanApplication.Property.Address.ZipCode
                },
                PropertyValue = loanApplication.Property.Value,
                RegisteredBy = loanApplication.RegisteredBy.Login,
                LoanNumberOfYears = loanApplication.LoanNumberOfYears
            };
        }

        public async Task RejectApplication(string applicationNumber, ClaimsPrincipal principal, string rejectionReason)
        {
            var loanApplication = await GetLoanApplicationByNumber(applicationNumber);
            var user = await GetOperatorByLogin(principal.Identity.Name);
            
            if (loanApplication.Status!=LoanApplicationStatus.New)
                throw new ApplicationException("Cannot reject application that is already accepted or rejected");

            loanApplication.Status = LoanApplicationStatus.Rejected;
            loanApplication.DecisionBy = user;
            loanApplication.DecisionDate = DateTime.Now;
            
            await unitOfWork.CommitChanges();
        }

        public async Task AcceptApplication(string applicationNumber, ClaimsPrincipal principal)
        {
            var loanApplication = await GetLoanApplicationByNumber(applicationNumber);
            var user = await GetOperatorByLogin(principal.Identity.Name);
            
            if (loanApplication.Status!=LoanApplicationStatus.New)
                throw new ApplicationException("Cannot accept application that is already accepted or rejected");

            loanApplication.Status = LoanApplicationStatus.Accepted;
            loanApplication.DecisionBy = user;
            loanApplication.DecisionDate = DateTime.Now;
            
            await unitOfWork.CommitChanges();
        }

        private async Task<LoanApplication> GetLoanApplicationByNumber(string applicationNumber)
        {
            return await loanApplicationRepository.Query()
                .FirstOrDefaultAsync(a => a.Number == applicationNumber);
        }

        private async Task<Operator> GetOperatorByLogin(string login)
        {
            return await operatorsRepository.Query()
                .FirstOrDefaultAsync(op => op.Login == login);

        }
    }
}