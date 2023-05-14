Feature: Restaurant Serving Tests

You are testing a checkout system for a restaurant. There is a new endpoint that will calculate the total of the order, and add a 10% service charge on food.
The restaurant only serves starters, mains and drinks, and the set cost for each is
Background:
	Given the following set costs for the restaurant:
		| Item    | Cost  |
		| Starter | £4.00 |
		| Main    | £7.00 |
		| Drink   | £2.50 |
	And drinks have a 30% discount when ordered before 19:00
	And service charge on food is 10%

@TC_1
@TC_2
Scenario Outline: Order before 18:00
	When A group orders 4 starters, 4 mains and 4 drinks at <OrderTime>
	Then The order is sent to the endpoint the '<total>' is calculated correctly in the bill
Examples:
	| OrderTime | total |
	| 19:00     | 59.4  |
	| 22:00     | 59.4  |
	| 18:00     | 56.1  |

@TC_3
@TC_4
Scenario Outline: Order after 19:10 remove items
	When A group orders 4 starters, 4 mains and 4 drinks at <OrderTime>
	Then The order is sent to the endpoint the '<total1>' is calculated correctly in the bill
	When 1 starter, 1 main and 1 drink are removed from order
	Then The order is sent to the endpoint the '<total2>' is calculated correctly in the bill
Examples:
	| OrderTime | total1 | total2 |
	| 19:10     | 59.4   | 44.55  |
	| 18:00     | 56.1   | 42.07  |
		
