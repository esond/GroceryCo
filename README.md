# GroceryCo (Coding Exercise)
A console-based checkout system for customers of GroceryCo supermarkets.

####Third-Party Libraries Used:
  - NUnit 3.4.1 (http://www.nunit.org/)
  - Json.NET (http://www.newtonsoft.com/json)
  
The above packages are all available on NuGet.

####Assumptions Made
  - Sale "types" are not user defined. That is to say, new "types" of sales (a "combo" deal for example; buy one Apple and save on Oranges) will require new development.
  - A grocery item cannot have more than one promotion associated with it at any given time.
  - On the functioning of the "Additional product discount": only one subsequent item will recieve a discount after the requisite number of items have been added to the checkout. This discount will be percentage-based. In the case of "buy one get one free", the discount is 100%.
  - Basket files (containing an unsorted list of item names) are assumed to be comma-separated text (.txt) files. See sample files in /data/Baskets.

####Behavioral Decisions
  - Promotions run for a number of days from the start date, with the start date being day 1. For example, a 7-day promotion started any time on Monday should end at 12:00 PM on Sunday that week. It seems that the vast majority of real-world promotions with time constraints are held in this fashion.

  - When in "Cashier" mode, an open file dialog is opened where a "basket" can be selected to simulate a customer starting a checkout. Console applications shouldn't really use UI components when possible, but remaining in the console isn't a strict requirement, so this was implemented for ease-of use.

####Design Decisions
  - Entities will be serialized and stored in files using JSON. One of the requirements of this project is that "the format describing regular prices and promotions [...] should be accessible for GroceryCo staff (product supply, marketing)". In other words, the layperson should be able to understand what they are looking at when they open one of the data files. In this developer's opinion, JSON strikes a good balance between functionality and plain-english readability.

  - A Repository Pattern was use for persistence. The reasoning behind this is the pattern's extensibility in the case new entity types are added to the system. However, the pattern isn't very unit testable (strictly speaking), but integration tests can be written to ensure test coverage.