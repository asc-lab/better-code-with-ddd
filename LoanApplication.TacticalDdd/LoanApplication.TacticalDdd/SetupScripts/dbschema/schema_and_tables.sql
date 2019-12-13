CREATE SCHEMA ddd_loan;

CREATE TABLE ddd_loan.mt_doc_loanapplication
(
  id uuid NOT NULL,
  data jsonb NOT NULL,
  mt_last_modified timestamp with time zone DEFAULT transaction_timestamp(),
  mt_version uuid NOT NULL DEFAULT (md5(((random())::text || (clock_timestamp())::text)))::uuid,
  mt_dotnet_type character varying,
  "number" character varying(50),
  CONSTRAINT pk_mt_doc_loanapplication PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);

COMMENT ON TABLE ddd_loan.mt_doc_loanapplication
  IS 'origin:Marten.IDocumentStore, Marten, Version=3.10.0.0, Culture=neutral, PublicKeyToken=null';

-- Index: ddd_loan.mt_doc_loanapplication_idx_number

-- DROP INDEX ddd_loan.mt_doc_loanapplication_idx_number;

CREATE UNIQUE INDEX mt_doc_loanapplication_idx_number
  ON ddd_loan.mt_doc_loanapplication
  USING btree
  (number COLLATE pg_catalog."default");


CREATE TABLE ddd_loan.mt_doc_operator
(
  id uuid NOT NULL,
  data jsonb NOT NULL,
  mt_last_modified timestamp with time zone DEFAULT transaction_timestamp(),
  mt_version uuid NOT NULL DEFAULT (md5(((random())::text || (clock_timestamp())::text)))::uuid,
  mt_dotnet_type character varying,
  CONSTRAINT pk_mt_doc_operator PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);

COMMENT ON TABLE ddd_loan.mt_doc_operator
  IS 'origin:Marten.IDocumentStore, Marten, Version=3.10.0.0, Culture=neutral, PublicKeyToken=null';


CREATE OR REPLACE VIEW ddd_loan.loan_details_view AS 
 SELECT loan.data ->> 'Id'::text AS id,
    loan.data ->> 'Number'::text AS number,
    loan.data ->> 'Status'::text AS status,
    (loan.data -> 'Score'::text) ->> 'Score'::text AS score,
    ((loan.data -> 'Customer'::text) -> 'NationalIdentifier'::text) ->> 'Value'::text AS customernationalidentifier,
    ((loan.data -> 'Customer'::text) -> 'Name'::text) ->> 'First'::text AS customerfirstname,
    ((loan.data -> 'Customer'::text) -> 'Name'::text) ->> 'Last'::text AS customerlastname,
    to_date((loan.data -> 'Customer'::text) ->> 'Birthdate'::text, 'YYYY-MM-DD'::text) AS customerbirthdate,
    ((((loan.data -> 'Customer'::text) -> 'MonthlyIncome'::text) ->> 'Amount'::text))::numeric(19,2) AS customermonthlyincome,
    ((loan.data -> 'Customer'::text) -> 'Address'::text) ->> 'Country'::text AS customeraddress_country,
    ((loan.data -> 'Customer'::text) -> 'Address'::text) ->> 'ZipCode'::text AS customeraddress_zipcode,
    ((loan.data -> 'Customer'::text) -> 'Address'::text) ->> 'City'::text AS customeraddress_city,
    ((loan.data -> 'Customer'::text) -> 'Address'::text) ->> 'Street'::text AS customeraddress_street,
    ((((loan.data -> 'Property'::text) -> 'Value'::text) ->> 'Amount'::text))::numeric(19,2) AS propertyvalue,
    ((loan.data -> 'Property'::text) -> 'Address'::text) ->> 'Country'::text AS propertyaddress_country,
    ((loan.data -> 'Property'::text) -> 'Address'::text) ->> 'ZipCode'::text AS propertyaddress_zipcode,
    ((loan.data -> 'Property'::text) -> 'Address'::text) ->> 'City'::text AS propertyaddress_city,
    ((loan.data -> 'Property'::text) -> 'Address'::text) ->> 'Street'::text AS propertyaddress_street,
    ((((loan.data -> 'Loan'::text) -> 'LoanAmount'::text) ->> 'Amount'::text))::numeric(19,2) AS loanamount,
    ((loan.data -> 'Loan'::text) ->> 'LoanNumberOfYears'::text)::integer AS loannumberofyears,
    ((((loan.data -> 'Loan'::text) -> 'InterestRate'::text) ->> 'Value'::text))::numeric(19,2) AS interestrate,
    to_date((loan.data -> 'Decision'::text) ->> 'DecisionDate'::text, 'YYYY-MM-DD'::text) AS decisiondate,
    opdec.data ->> 'Login'::text AS decisionby,
    opreg.data ->> 'Login'::text AS registeredby,
    to_date((loan.data -> 'Registration'::text) ->> 'RegistrationDate'::text, 'YYYY-MM-DD'::text) AS registrationdate
   FROM ddd_loan.mt_doc_loanapplication loan
     LEFT JOIN ddd_loan.mt_doc_operator opreg ON (((loan.data -> 'Registration'::text) ->> 'RegisteredBy'::text)::uuid) = opreg.id
     LEFT JOIN ddd_loan.mt_doc_operator opdec ON (((loan.data -> 'Decision'::text) ->> 'DecisionBy'::text)::uuid) = opdec.id;