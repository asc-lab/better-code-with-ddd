CREATE TABLE public."Operators"
(
  "Password" character varying(50),
  "FirstName" character varying(50),
  "LastName" character varying(75),
  "CompetenceLevel_Amount" numeric(19,2),
  "Id" uuid NOT NULL,
  "Login" character varying(50),
  CONSTRAINT operators_pk PRIMARY KEY ("Id")
);

CREATE TABLE public."LoanApplications"
(
  "Number" character varying(50),
  "Status" character varying(50),
  "Score_Score" character varying(50),
  "Score_Explanation" character varying(250),
  "Customer_NationalIdentifier_Value" character varying(50),
  "Customer_Name_First" character varying(50),
  "Customer_Name_Last" character varying(50),
  "Customer_Birthdate" date,
  "Customer_MonthlyIncome_Amount" numeric(19,2),
  "Customer_Address_Country" character varying(50),
  "Customer_Address_ZipCode" character varying(50),
  "Customer_Address_City" character varying(50),
  "Customer_Address_Street" character varying(50),
  "Property_Value_Amount" numeric(19,2),
  "Property_Address_Country" character varying(50),
  "Property_Address_ZipCode" character varying(50),
  "Property_Address_City" character varying(50),
  "Property_Address_Street" character varying(50),
  "Loan_LoanAmount_Amount" numeric(19,2),
  "Loan_LoanNumberOfYears" integer,
  "Loan_InterestRate_Value" numeric(19,2),
  "Decision_DecisionDate" date,
  "Decision_DecisionBy_Value" uuid,
  "Registration_RegistrationDate" date,
  "Registration_RegisteredBy_Value" uuid,
  "Id" uuid NOT NULL,
  CONSTRAINT loan_applications_pk PRIMARY KEY ("Id")
);




CREATE OR REPLACE VIEW loan_details_view AS 
  SELECT 
    loan."Id" id,
    loan."Number" AS number,
    loan."Status" AS status,
    loan."Score_Score" AS score,
    loan."Customer_NationalIdentifier_Value" AS customernationalidentifier,
    loan."Customer_Name_First" AS customerfirstname,
    loan."Customer_Name_Last" AS customerlastname,
    loan."Customer_Birthdate" AS customerbirthdate,
    loan."Customer_MonthlyIncome_Amount" AS customermonthlyincome,
    loan."Customer_Address_Country" AS customeraddress_country,
    loan."Customer_Address_ZipCode" AS customeraddress_zipcode,
    loan."Customer_Address_City" AS customeraddress_city,
    loan."Customer_Address_Street" AS customeraddress_street,
    loan."Property_Value_Amount" AS propertyvalue,
    loan."Property_Address_Country" AS propertyaddress_country,
    loan."Property_Address_ZipCode" AS propertyaddress_zipcode,
    loan."Property_Address_City" AS propertyaddress_city,
    loan."Property_Address_Street" AS propertyaddress_street,
    loan."Loan_LoanAmount_Amount" AS loanamount,
    loan."Loan_LoanNumberOfYears" AS loannumberofyears,
    loan."Loan_InterestRate_Value" AS interestrate,
    loan."Decision_DecisionDate" AS decisiondate,
    opdec."Login" AS decisionby,
    opreg."Login" AS registeredby,
    loan."Registration_RegistrationDate" AS registrationdate
   FROM "LoanApplications" loan
     LEFT JOIN "Operators" opreg ON loan."Registration_RegisteredBy_Value" = opreg."Id"
     LEFT JOIN "Operators" opdec ON loan."Decision_DecisionBy_Value" = opreg."Id";