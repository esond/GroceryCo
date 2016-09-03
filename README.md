# GroceryCo
A console-based checkout system for customers of GroceryCo supermarkets (coding exercise).

####Third-Party Libraries Used:
  - NUnit 3.4.1 (http://www.nunit.org/)
  - Json.NET (http://www.newtonsoft.com/json)
  
The above packages are all available on NuGet.

####Assumptions
  - Sale "types" are not user defined. That is to say, new "types" of sales (eg.: percentage-based, such as 50% off) will require new development.
  - A grocery item cannot have more than one promotion associated with it at any given time.

####Behavioral Decisions
  - Promotions run for a number of days from the start date, with the start date being day 1. For example, a 7-day promotion started any time on Monday should end at 12:00 PM on Sunday that week. IT seems that the vast majority of real-world promotions with time constraints are held in this fashion.

