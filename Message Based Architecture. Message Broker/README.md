# Message Based Architecture. Message Broker — Questions & Answers

## 1. What is Message Based Architecture? What is the difference between Message Based Architecture and Event Based Architecture?

**Message-Based Architecture** is a style where services communicate asynchronously by sending messages through a message broker. A sender sends a message to a specific address or recipient, which implies some form of orchestration: the sender is aware of the target service. Services are decoupled but know about each-other.  
**Event-Based Architecture** is a related but distinct style where services publish events describing something that already happened. The publisher has no knowledge of who will consume the event. Consumers independently subscribe to the events they care about. This is the classic pub-sub model, where publishers and subscribers are fully decoupled from each other.

Key difference is that, in Message-Based Architecture, the sender knows and targets a specific recipient (point-to-point, orchestrated). In EBA, the publisher is unaware of its consumers. Receivers decide to what event to subscribe by them-self (choreography).

## 2. What is Message Broker? How do message brokers work?

A **message broker** is middleware that enables distributed services to communicate asynchronously by sending messages from producers to consumers.

**Message brokers** work the following:
1. Producer sends a message to the broker
2. Broker stores and routes it
3. Consumer receives the message
4. Consumer processes the message and acknowledges it

The broker can also handle retries, persistence and load balancing.

Examples:
- RabbitMQ
- Apache Kafka
- Azure Service Bus

## 3. When should you use message brokers?

Message brokers are used when:
- asynchronous communication is acceptable 
- loose coupling between services is desired
- scalability of the system is required
- the communication between distributed services has to be reliable and prone to failures.

Message brokers should be avoided when fast synchronous calls are required and eventual state consistency between distributed components is not acceptable. For example, online banking.

## 4. Name and describe any distribution pattern.

**Delivery patterns**  
Point-to-Point delivery, when a producer sends a messgae to a specific consumer.  
Pub/Sub, it is when a producer publishes an event and consumer subsribe independently to recieve the event.

**Routing patterns**  
Fanout, it is when a message is broadcasted to all bound consumers. Every subscriber gets every message.  
Direct routing, it is when a message has a routing key and only matching queues receive it.  
Topic routing, it is when consumers subscribe to a specific topic and gets only desired messages from the queue.

## 5. What are the advantages and disadvantages of using message broker?

### Advantages
- loose coupling between services
- better scalability
- asynchronous processing
- improved fault tolerance
- traffic buffering during spikes

### Disadvantages
- increased system complexity
- harder debugging and tracing
- eventual consistency
- duplicate message handling required
- additional infrastructure overhead

## 6. What is the difference between Queue and Topic?

A **queue** guarantees that a message in the queue will be processed only once. So, if there are multiple subscribers to the same queue, they start competing for messages, and only one will receive a message.  
The **topic** concept was introduced for the opposite scenario, where all consumers subscribed to a specific topic will receive the message. There is no competition between subscribers.

## 7. What are the typical failures in MBA? How can you address them? What is Saga pattern?

In a typical message-based architecture, failures can happen on the publisher side, the message broker side and the consumer side.

### Common failures  
- **Message loss** - use acknowledgements and persistence if possible
- **Duplicate messages** - make sure that a consumer ensure idempotency
- **Poison messages** - retries, if didn't work use dead-letter queue
- **Publisher failure** - retries, circuit breaker and Outbox pattern
- **Consumer failure** - retries, circuit breaker and Inbox pattern
- **Broker overload** - horizontal and vertical scaling, rate limiting

### Saga pattern

The **Saga pattern** is a way to manage distributed transactions across multiple services to ensure system integrity.  
If any server fails the system performs compensating actions to undo previous steps.

When to use saga pattern:
- multiple microservices 
- each service owns its own DB
- long-running workflows
- failures must be recoverable