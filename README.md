Choreography based SAGA Pattern & Outbox Pattern

Explanation : 
A microservices architecture is built with Order Service -> Stock Service -> Payment Service  distributed services. 
Async communication over metwork between these services have challenging such as data inconsistency. To solve inconsistency data problem, implemented Choreography based Saga pattern. 
In this approach,  each service handles its responsibility, when its transaction completed  it publishes event to execute for the next local transaction of another service. 

We handle data inconsistency problem but it is not atomic at all.  What if a service can not publish ? Message broker can be down etc. In this case we may lose the events. 
To solve this problem,  implemented Outbox Pattern. 
In this approach,  instead of publishing events directly, save the events into an outbox collection (or table) in the database. Another process (background worker service) always poll the table and publishes unpublished events to the message broker. 
This makes the operations atomic. It is reliable, durable way. 

Note : 
Outbox Pattern works with "at-least-once"  delivery approach.Therefore we need to prevent duplication.  

To prevent duplication, implement the project with Inbox pattern : 
1-  We need to include an identifier inside events. (a unique number)
2- idempotent consumers (make sure that the event has not proceed before.)
With this approach, we guarantee exactly-once processing. 


