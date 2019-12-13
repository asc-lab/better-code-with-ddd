# Better code with DDD building blocks

This repository contains code that accompanies presentation ASC LAB team gave at meetup about “Creating better code with Domain Driven Design”.

## Business case

Domain Driven Design tactical patterns are presented here using mortgage loan application processing use case.  Business wants to increase efficiency of mortgage loan application processing for individual customers by: automating application score calculation, combining information from online available sources, auto rejecting applications with RED score, having ability to relatively easy add new rules for scoring.

The process: 
* operator submits loan application with property information, customer data, loan information and attached documents provided by customer
* system performs basic validation, required fields, correct formats
* operator commands the system to calculates score based on rules
* if score is RED application is rejected and explanation is provided
* if score is GREEN then operator validates attached documents and accepts application or rejects it due to discrepancies between provided data and documents. System validates operator’s competence level

The rules:
* property value must not exceed requested loan amount
* customer age at the day of last loan installment must not exceed 65 years
* customer must not be registered debtor in national debtors registry system
* loan monthly installment must not exceed 15% of customer monthly income


## Solutions

Repository contains two solutions. First solution presents typical layered application building approach with anemic model and business logic scattered in services that reside in application layer. This solution also presents usage of generic repository and mixing read and write concerns in the same application service class. First solution is located in the LoanApplication.EnterpriseCake folder.

Second solution presents usage of DDD tactical patterns like: value objects, entities, repositories, factories, domain services and application services to achieve better readability and expressiveness of the code. Applying DDD patterns together with ubiquitous language closes the gap between language spoken by experts and the team and language used in the code.
Solution with DDD building blocks applied is located in the LoanApplication.TacticalDdd folder.


## DDD Tactical Patterns Based Solution
This  solution has two variants. On master branch you can find a version that uses [Marten](https://martendb.io/) to persist and load domain entities as JSON documents. Read side uses Dapper that executes SQL statements directly against database views.
Second variant that lives on ef_cofe branch. This version uses Entity Framework Core to persist and load domain entities. Dapper is still used to handle query side of the application.

DDD solution conatins three projects:
* LoanApplication.TacticalDdd which contains all the domain logic, application layer which exposes domain logic to the external world, read model which handles queries and adapter that implement infrustructure needed to communicate with databases, message queues and external systems.
* LoanApplication.TacticalDdd.Tests which contains unit tests for domain model and application layer
* DebtorRegistryMock which contains mocked version of debtor registry system.
