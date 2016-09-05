# GroceryCo (Coding Exercise)
A console-based checkout system for customers of GroceryCo supermarkets.

####Third-Party Libraries Used:
  - NUnit 3.4.1 for unit testing (http://www.nunit.org/)
  - Json.NET for serialization (http://www.newtonsoft.com/json)
  
The above packages are all available on NuGet.

I also used a code snippet from a stackoverflow answer to generate tabular console output (http://stackoverflow.com/a/19353995/1672990).

####Assumptions Made
  - Sale types are not user defined. That is to say, new "types" of sales (a "combo" deal for example; buy one Apple and save on Oranges) will require new development.

  - A grocery item cannot have more than one promotion associated with it at any given time.

  - On the functioning of the "Additional product discount": only one subsequent item will recieve a discount after the requisite number of items have been added to the checkout. This discount will be entered as a percentage. In the case of "buy one get one free", the discount is 100%.

  - Basket files (containing an unsorted list of item names) are assumed to be comma-separated .txt files. Sample files have been included in /data/baskets.

####Behavioral Decisions
  - When in "Cashier" mode, an open file dialog is opened where a "basket" can be selected to simulate a customer starting a checkout. Console applications shouldn't really use UI components when possible, but remaining in the console isn't a strict requirement, so this was implemented for ease-of use.

  - Reciepts will be output as a plain text file.

####Design Decisions
  - Entities will be serialized and stored in files using JSON. One of the requirements of this project is that "the format describing regular prices and promotions [...] should be accessible for GroceryCo staff (product supply, marketing)". In other words, the layperson should be able to understand what they are looking at when they open one of the data files. In this developer's opinion, JSON strikes a good balance between functionality and plain-english readability.

  - A Repository Pattern was use for persistence. The reasoning behind this is the pattern's extensibility in the case new entity types are added to the system. However, the pattern isn't very unit testable (strictly speaking), but integration tests can be (and have been) written to ensure test coverage.