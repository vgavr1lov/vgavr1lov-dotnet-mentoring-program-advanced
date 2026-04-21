# .NET Mentoring Program Advanced

## Architectural Styles and Patterns

### Questions with answers

**1.	What are the cons and pros of the Monolith architectural style?**

  Pros:
  * Easy to test
  * Easy to debug
  *	Less boilerplate code and a simpler structure
  *	Easy to deploy
    
  Cons: 
  *	Deployment takes longer, as the full application has to be deployed each time
  *	Each change may require extensive regression testing
  *	The monolithic application may become too complex and unmanageable over time
  *	Scaling is not flexible
  *	A minor bug in any functionality may shut down the whole application  
 
**2.	What are the cons and pros of the Microservices architectural style?**
   
  Pros:
  *	Flexible horizontal scaling
  *	Independent deployment
  *	Faster time to market, as this architecture supports large teams
  *	Good scalability
  *	Reduced need for regression testing
     
  Cons: 
  *	More boilerplate code and increased structural complexity
  *	Harder to ensure transactional integrity due to the distributed nature
  *	Requires robust logging and monitoring
  *	More complex CI/CD processes
  *	Harder to test
  *	Latency overhead
  
**3.	What is the difference between SOA and Microservices?**

  *	In SOA, communication happens via an ESB, whereas microservices communicate directly with each other
  *	In SOA, databases can be shared, whereas in microservices each service typically has its own database
  * Different scope: SOA often operates at the enterprise level, enabling communication between different applications, while microservices operate at the application level

**4.	[Open question] What does hybrid architectural style mean? Think of your current and previous projects and try to describe which architectural styles they most likely followed.**

  Hybrid architecture may combine different architectural styles and patterns. In my last project, the main architectural style was microservices. At the same time, the architecture of each module was layered with separate database, business logic and API layers. Another scenario is the migration from a monolithic architecture to a microservices architecture. During this process, part of the application remains monolithic, while another part is implemented as microservices.

**6.	Name several examples of the distributed architectures. What do ACID and BASE terms mean.**

Distributed architectures:
*	SOA
*	Serverless
*	Microservices
  
**ACID** is a principle that describes a guaranteed database transactional model.  
  **A** - Atomicity -  all steps are succeeded or none.  
  **C** -  Consistency -  keeps database valid.  
  **I** -  Isolation -  transactions are isolated.  
  **D** - Durability – committed data remains save.  

**BASE** is a principle, which suits more to the distributed systems. It favours availability over strict consistency.  
  **BA** - Basically Available – the system usually responds.  
  **S** – Soft State – not the final state, will change over time.  
  **E** - Eventual Consistency – state become consistent later.  

**6.	Name several use cases where Serverless architecture would be beneficial.**

  *	Event-driven or Asynchronous processing where low latency is not critical.
  *	Highly variable workloads – serverless architecture auto-scales during high demand and does not waste resources during low or no demand.
  *	Suitable for MVPs or rapid prototyping, it reduces the effort required for infrastructure and partially for backend development.

### Task 1:  

The architecture of the described solution is hybrid: all applications share the same database, so it can be described as a shared database architecture. On the other hand, each application has a layered architecture.
As the company’s declared main goal is to enter new markets in different countries, I would suggest adopting a Serverless architecture, specifically Function-as-a-Service (FaaS). There is an industry consensus that, among all possible options, serverless architecture provides the fastest time-to-market, which can be a significant competitive advantage. Another benefit is that, since it is difficult to predict demand in new markets, a Serverless architecture, with its auto-scaling capabilities, can help save infrastructure resources when demand is low and automatically scale up in case of high demand.
 
### Task 2:  

Another option is a microservice architecture. This involves transforming different applications into domains and building a new application based on a microservice architecture. It can also be a desirable approach.

Pros:
-	Multiple teams can work on the project simultaneously, which can result in a fast time-to-market.
-	Microservice architecture allows the adoption of new technologies and the phasing out of outdated technologies, such as WinForms and MVC.
-	Microservices can be scaled independently, for instance, the Online Shop (Order Placement) service can be scaled up during periods of high demand.

Cons:
-	Microservice architecture introduces typical challenges, including higher latency and added complexity in deployment, internal communication and testing.
-	Rebuilding the system from scratch as a microservice architecture will require more time and resources.  

Comparison with the proposed Serverless architecture (Function-as-a-Service):  

Pros:
-	Fastest time-to-market.
-	Requires less effort and fewer resources, as it allows parts of existing applications to be preserved.
-	Auto-scaling in response to volatile demand.
-	Potential cost savings if market expansion fails or demand is low, since payment is based on actual computation.

Cons:
-	A typical drawback of serverless architecture is the “cold start” problem, which needs to be addressed.
-	Serverless often involves vendor lock-in, which can be risky.
-	Potentially higher costs, so a thorough cost analysis is required.

Comparison table:  

| Criteria | Microservice Architecture | Serverless Architecture (FaaS) |
|----------|---------------------------|---------------------------------|
| Time-to-Market | Fast (parallel development by multiple teams) | Fastest (minimal infrastructure setup) |
| Development Effort | High (requires redesign and rebuilding from scratch) | Lower (can reuse parts of existing applications) |
| Scalability | Independent scaling of services (for example, Online Shop) | Automatic, built-in auto-scaling |
| Technology Stack | Flexible: enables migration away from legacy tech (WinForms, MVC) | Limited by vendor ecosystem |
| Cost Efficiency | Higher upfront and operational costs | Pay-per-use model, cost-efficient for unpredictable demand |
| Performance | More predictable latency | May suffer from cold start latency |
| Complexity | High (deployment, communication, testing, orchestration) | Lower operational complexity (managed by provider) |
| Infrastructure | Requires infrastructure management | No infrastructure management required |
| Vendor Lock-in | Low | High (depends heavily on cloud provider) |
| Best Use Case | Large, complex systems requiring flexibility and control | Rapid market entry with uncertain or volatile demand |
| Risk Level | Higher implementation risk due to complexity and effort | Cost risk if not optimized, dependency on provider |

**Architecture diagram**  

![Self-editing Diagram](https://github.com/vgavr1lov/vgavr1lov-dotnet-mentoring-program-advanced/blob/main/Home%20task%20-%20Serverless%20architecture%20diagram.drawio.svg)
