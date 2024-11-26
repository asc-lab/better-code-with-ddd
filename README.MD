# Better code with DDD building blocks

[![.NET](https://github.com/asc-lab/better-code-with-ddd/actions/workflows/dotnet.yml/badge.svg)](https://github.com/asc-lab/better-code-with-ddd/actions/workflows/dotnet.yml)

This repository contains code that accompanies presentation ASC LAB team gave at meetup about “Creating better code with Domain Driven Design”.

Check out our article on [Altkom Software & Consulting blog](https://altkomsoftware.pl/en/blog/create-better-code-using-domain-driven-design/).

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


## Domain model
Domain model is pretty simple. There are two aggregates LoanApplication and Operator. LoanApplication represenst, as you can guess, a loan application submitted for processsing.
LoadApplication is composed of several value objects, which in turn are also composed from other value object.
Operator represents a bank employee responsible for processing loan application.

<center>

![Domain model - aggregates](https://raw.githubusercontent.com/asc-lab/better-code-with-ddd/ef_core/LoanApplication.TacticalDdd/Docs/class_model_aggregates.png)
Domain mode - aggregates

</center>


The core functionality of our service is loan application evaluation. Each rule is implemented as a separate class that implements IScoringRule interface. A domain service is created to encapsulate rules and score calculation.

<center>

![Domain model - rules](https://github.com/asc-lab/better-code-with-ddd/blob/ef_core/LoanApplication.TacticalDdd/Docs/class_model_scoring_rules.png?raw=true)
Domain mode - business rules

</center>

Application services are clients of domain model. We have three application services one that is responsible for loan application submission, second that evaluates applications and third one that lets users accept or reject application.
Diagram below presents dependencies between one sample application service and domain model parts. It also presents dependency between infrastructure layer and domain model, where infrastructure layer provides implementations for interfaces defined as part of the domain model (for example for repositories).  


<center>

![Application services interaction with the domain model](https://github.com/asc-lab/better-code-with-ddd/blob/ef_core/LoanApplication.TacticalDdd/Docs/class_model_decision_services.png?raw=true)
Application services interaction with the domain model

</center>
