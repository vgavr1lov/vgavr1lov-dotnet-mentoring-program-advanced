# RESTful Web APIs — Questions & Answers

## 1. Explain the difference between terms: REST and RESTful. What are the six constraints?

REST (Representational State Transfer) is an architectural style defined by Roy Fielding. RESTful refers to a web service that implements and complies with the REST constraints. In other words, REST is the specification, and RESTful is the implementation.

The six constraints are:  
- **Client-Server** - the client and server are separated, each can evolve independently.
- **Statelessness** - each request contains all the information needed to process it; no session state is stored on the server.
- **Cacheability** - responses must define themselves as cacheable or non-cacheable to improve performance.
- **Uniform Interface** - a standardised way of communicating between client and server, including resource identification via URIs.
- **Layered System** - the client does not need to know whether it is communicating directly with the server or through intermediaries such as load balancers or proxies.
- **Code on Demand** - the server can extend client functionality by transferring executable code, such as JavaScript.

## 2. HTTP Request Methods (the difference) and HTTP Response codes. What is idempotency? Is HTTP the only protocol supported by REST?

The main HTTP methods are:

| Method | Description | Safe | Idempotent |
|--------|-------------|------|------------|
| GET | Retrieves a resource | yes | no |
| POST | Creates a new resource | no | no |
| PUT | Replaces a resource entirely | no | yes |
| PATCH | Partially updates a resource | no | no |
| DELETE | Removes a resource | no | yes |

Common HTTP response code groups:  
- **2xx** - success (200 OK, 201 Created, 204 No Content)
- **4xx** - client errors (400 Bad Request, 401 Unauthorized, 403 Forbidden, 404 Not Found)
- **5xx** - server errors (500 Internal Server Error, 503 Service Unavailable)

**Idempotency** means that making the same request multiple times produces the same result as making it once. For example, calling DELETE on the same resource repeatedly always results in the resource being absent, regardless of how many times the call is made.

HTTP is not the only protocol supported by REST. Since REST is an architectural style rather than a protocol specification, it can theoretically be applied over other protocols. However, in practice, HTTP/HTTPS is the standard and most widely used protocol for RESTful APIs.

## 3. What are the advantages of statelessness in RESTful services?

- **Scalability** - since no session state is stored on the server, requests can be distributed across multiple server instances without requiring to know session state.
- **Reliability** - if one server instance fails, another can handle the next request without any session loss.
- **Simplicity** - the server does not need to manage session state, which reduces complexity.
- **Visibility** - each request is self-contained and can be understood in isolation, which simplifies monitoring and debugging.

The trade-off is that statelessness may increase the size of requests, since the client must include all necessary context each time.

## 4. How can caching be organized in RESTful services?

Caching in RESTful services can be organized at multiple levels:  
- **HTTP cache headers** - the server can include headers such as Cache-Control, ETag and Last-Modified in responses. The client or intermediary proxies can then cache responses and avoid redundant requests.
  ETags - the server returns an ETag (entity tag) representing the current version of a resource. The client sends this value in subsequent requests via If-None-Match. If the resource has not changed, the server returns 304 Not Modified without a body.
- **CDN or reverse proxy caching** - intermediary layers such as CDN nodes or reverse proxies (Varnish, NGINX) can cache responses and serve them without hitting the application server.
- **In-memory caching** - within the application, tools such as Redis or in-process memory caches (IMemoryCache in .NET) can store frequently accessed data and reduce database load.

## 5. How can versioning be organized in RESTful services?

There are several common approaches to API versioning:  
- **URI versioning** - the version is included in the URL path, for example /api/v1/orders. This is the most explicit and widely used approach.
- **Query string versioning** - the version is passed as a query parameter, for example /api/orders?version=1. Less common and considered less clean.
- **Header versioning** - the version is specified in a custom HTTP header, for example API-Version: 1. Keeps the URI clean but is less discoverable.
- **Media type versioning (content negotiation)** - the version is embedded in the Accept header, for example Accept: application/json;version=1. The most RESTful approach according to purists, but the most complex to implement and consume.

In practice, URI versioning is the most straightforward and most commonly adopted approach, as it is transparent and easy to test and document.

## 6. What are the best practices of resource naming?

- Use **nouns, not verbs** to represent resources: /orders not /getOrders.
- Use **plural nouns** for collections: /products, /customers.
- Use **hierarchical structure** to represent relationships: /customers/{id}/orders.
- Use **lowercase letters and hyphens** for readability: /order-items not /orderItems or /order_items.
- Avoid deep nesting beyond two or three levels, as it becomes difficult to maintain.
- Keep URIs **consistent and predictable** across the API.
- Do not include the file extension or format in the URI.
- Do not end URI witg slash / .

## 7. What are OpenAPI and Swagger? What implementations/libraries for .NET exist? When would you prefer to generate API docs automatically and when manually?

**OpenAPI** is a specification for describing RESTful APIs in a machine-readable format (JSON or YAML). It defines endpoints, request/response schemas, authentication and other contract details.  
**Swagger** is a set of tools built around the OpenAPI specification, including a UI for interactive API exploration (Swagger UI) and tooling for generating clients and documentation.

In .NET, the most commonly used libraries are:  
- **Swashbuckle** – integrates with ASP.NET Core and automatically generates OpenAPI documentation from code.
- **NSwag** – similar to Swashbuckle but also supports client code generation.
- **Scalar** – a more modern alternative to Swagger UI for rendering OpenAPI docs.
- **Microsoft.AspNetCore.OpenApi** – a built-in option introduced in newer versions of .NET.

**Automatic generation** is preferable when the API is code-first and the contract is derived directly from the implementation. It reduces maintenance effort and keeps documentation in sync with the actual code.  
**Manual documentation** is preferable when the API is design-first, when the contract needs to be agreed upon before implementation begins, or when the generated output does not accurately reflect the intended public contract. It gives more control over what is exposed and how it is described.

## 8. What is OData? When will you choose to follow it and when not?

OData (Open Data Protocol) is a standardised protocol for building and consuming RESTful APIs that provides rich querying capabilities, such as filtering, sorting, pagination and field selection directly in the URL:

```
/products?$filter=price gt 100&$orderby=name&$top=10
```

**Choose OData when:**  
- The consumer needs flexible, ad-hoc querying without requiring new endpoints for each use case.
- The API is primarily data-oriented and targets internal consumers or tools that support OData natively (SAP HANA).
- Rapid exposure of data with minimal backend logic is required.

**Avoid OData when:**  
- The API is public-facing and needs to be simple and predictable.
- The domain logic is complex and should not be tightly coupled to the query model.
- Performance is critical, since OData queries can be difficult to optimise and may generate inefficient database queries.
- The team is not familiar with OData and the added complexity is not justified.

## 9. What is the Richardson Maturity Model? Is it always a good idea to reach the 3rd level of maturity?

The Richardson Maturity Model describes the levels of REST compliance:  

| Level | Description |
|-------|-------------|
| **Level 0** | A single URI used for all operations via POST (RPC-style). |
| **Level 1** | Resources are introduced. Separate URIs for different resources. |
| **Level 2** | HTTP methods and status codes are used correctly. |
| **Level 3** | HATEOAS (Hypermedia as the Engine of Application State). Responses include links to related actions and resources. |

Reaching Level 3 is not always a good idea. HATEOAS adds significant complexity to both the server implementation and the client. In practice, most APIs operate successfully at Level 2 and it is widely considered a pragmatic standard. Level 3 may bring value in scenarios where the API is highly dynamic and the client must discover available actions at runtime. However, for most business applications, the added overhead of HATEOAS does not justify the benefits.

## 10. What pros and cons does REST have in comparison with other web API types?

**REST vs SOAP**  
- REST is simpler, more lightweight and easier to consume.
- REST does not enforce a strict contract by default, whereas SOAP uses WSDL.
- SOAP has built-in support for WS-Security and transactions, which can be an advantage in enterprise contexts.

**REST vs GraphQL**  
- REST is simpler to implement and understand.
- GraphQL allows the client to request exactly the fields it needs, which eliminates over-fetching and under-fetching.
- REST can suffer from over-fetching or the need for multiple requests to aggregate data from different resources.
- GraphQL is better suited for complex, client-driven data requirements.

**REST vs gRPC**  
- REST uses human-readable formats (JSON/XML), whereas gRPC uses binary serialisation (Protocol Buffers), making gRPC significantly faster.
- gRPC is better suited for internal service-to-service communication where performance is critical.
- REST has broader support and is easier to consume from browsers and external clients.
- gRPC requires less boiler-plate code when implemented.

In general, REST remains the most widely adopted standard for public-facing APIs due to its simplicity, broad tooling support and compatibility with web standards. However, it is not always the optimal choice, and the decision should be based on the specific requirements of the project.