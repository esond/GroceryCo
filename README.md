# GroceryCo (Coding Exercise)
A console-based checkout system for customers of GroceryCo supermarkets.

####Third-Party Libraries Used:
  - NUnit 3.4.1 (http://www.nunit.org/)
  - Json.NET (http://www.newtonsoft.com/json)
  
The above packages are all available on NuGet.

####Assumptions Made
  - Sale "types" are not user defined. That is to say, new "types" of sales (eg.: percentage-based, such as 50% off) will require new development.
  - A grocery item cannot have more than one promotion associated with it at any given time.

####Behavioral Decisions
  - Promotions run for a number of days from the start date, with the start date being day 1. For example, a 7-day promotion started any time on Monday should end at 12:00 PM on Sunday that week. It seems that the vast majority of real-world promotions with time constraints are held in this fashion.

####Design Decisions
  - Entities will be serialized and stored in files using JSON. One of the requirements of this project is that "the format describing regular prices and promotions [...] should be accessible for GroceryCo staff (product supply, marketing)". In other words, the layperson should be able to understand what they are looking at when they open one of the data files. In this developer's opinion, JSON strikes a good balance between functionality and plain-english readability.

