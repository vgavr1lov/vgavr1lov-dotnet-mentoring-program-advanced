# Security. Authentication & Authorization — Questions & Answers

## 1. What is the difference between authentication and authorization?

**Authentication** is the process of verifying who a user or system is. It answers the question: "Are you who you claim to be?" Common mechanisms include username/password, biometrics, and tokens.  
**Authorization** is the process of determining what an authenticated identity is allowed to do. It answers: "What are you allowed to access or perform?" It defines permissions and access control policies.  

Key difference: authentication must happen before authorization. You first prove your identity, then the system determines your permitted actions.


## 2. What authorization approaches can you list? What is role-based access control?

Common authorization approaches include:
- **Role-Based Access Control** - permissions are assigned to roles, and roles are assigned to users.
- **Attribute-Based Access Control** - access is granted based on attributes of the user, resource and environment (department, time of day).
- **Policy-Based Access Control** - access decisions are driven by declarative policies, often combining **Role-Based Access Control** and **Attribute-Based Access Control**.
- **Access Control List** - a list that explicitly maps users or groups to specific permissions on a resource.
- **Discretionary Access Control** - resource owners define who can access their resources.

**Role-Based Access Control** is an authorization model where permissions are not assigned directly to individual users but to roles (Admin, Editor, Viewer). Users are then assigned one or more roles, inheriting all associated permissions.  
Example:
- **Admin role** - can create, read, update, delete
- **Editor role** - can create, read, update
- **Viewer role** - can only read

**Role-Based Access Control** simplifies permission management, especially in large systems with many users, because changing a role's permissions automatically affects all users holding that role.


## 3. What exactly is Identity Management (Identity and Access Management)?

**Identity and Access Management (IAM)** is a framework of policies, processes and technologies that ensures the right individuals have the appropriate access to the right resources at the right time.  
IAM covers three main areas:
- **Identity Management** - creating, maintaining and deactivating digital identities (user accounts, service accounts).
- **Authentication** - verifying identity via passwords, MFA, biometrics, certificates or SSO.
- **Authorization** - defining and enforcing what each identity is allowed to do (Role-Based Access Control, Attribute-Based Access Control, policies).

IAM systems typically provide:
- Centralized user directory (Active Directory)
- Single Sign-On (SSO) across services
- Multi-Factor Authentication (MFA)
- Audit logs and compliance reporting
- Lifecycle management: provisioning and deprovisioning

Examples of IAM providers: Okta, Microsoft Entra ID (Azure AD), AWS IAM, Keycloak.


## 4. What authentication/authorization protocols do you know? What is the difference between OAuth & OpenID?

**Common Protocols**  
- **OAuth 2.0** - an authorization framework that allows a third-party application to obtain limited access to a resource on behalf of a user.
- **OpenID Connect (OIDC)** - an authentication layer built on top of OAuth 2.0. Provides identity information (who the user is) via an ID token.
- **SAML 2.0** - an XML-based protocol for exchanging authentication and authorization data between an Identity Provider and a Service Provider. Widely used in enterprise SSO.
- **Kerberos** - a network authentication protocol using secret-key cryptography, common in enterprise Windows environments.
- **LDAP** - a protocol for accessing and maintaining distributed directory information; often used as a user store.

**OAuth vs OpenID**
OAuth 2.0 is an authorization protocol. Its purpose is to grant a client application limited access to a user's resources on another service ("Allow app X to read your Google Drive"). It issues an access token but does not define how to obtain information about the user's identity.
OpenID Connect (OIDC) is an authentication protocol built on top of OAuth 2.0. It adds an ID token (a JWT) that contains claims about the authenticated user (name, email, subject ID). 
OIDC answers "Who is this user?", while OAuth answers "What is this user allowed to do?"
In practice, OIDC is used for login/identity, OAuth 2.0 is used for API access delegation. Most modern identity systems use both together.


## 5. What is Authentication/Authorization Token. What is JWT token? What other approaches except authentication/authorization, can we use with security token?

A security token is a self-contained artifact issued by an authentication server that represents a claim about a user or client. It is used to prove identity or grant access without requiring repeated credential checks.

**Common token types**  
- **Access Token** - short-lived token granting access to a specific resource (used in OAuth 2.0).
- **Refresh Token** - long-lived token used to obtain new access tokens without re-authentication.
- **ID Token** - contains identity claims about the user (used in OIDC).

### JWT (JSON Web Token)
JWT is a compact, URL-safe token format defined in RFC 7519. It consists of three Base64URL-encoded parts separated by dots:
1.	**Header** - token type and signing algorithm (HS256, RS256).
2.	**Payload** - claims: standard (iss, sub, exp, iat) and custom (roles, userId).
3.	**Signature** - ensures the token has not been tampered with.

JWTs are stateless: the server can verify the token locally without a database lookup, making them ideal for distributed systems and microservices.

### Other Uses of Security Tokens 
Beyond authentication and authorization, security tokens can be used for:
- Email verification / password reset - time-limited tokens sent to a user's email to verify ownership.
- CSRF protection - tokens embedded in forms to prevent cross-site request forgery.
- API key identification - tokens that identify a calling application (not a user) for rate limiting and logging.
- Invitation links - one-time tokens to invite users to join a system or accept a transfer.
- Audit and traceability - tokens carrying correlation IDs to trace requests across distributed services.


## 6. What is Single Sign-On (SSO)? Name the steps to implement SSO. What are the benefits of SSO?

**Single Sign-On (SSO)** is an authentication mechanism that allows a user to log in once with a single set of credentials and gain access to multiple applications or services without re-authenticating for each one.

**Steps to Implement SSO**   
4.	Choose an Identity Provider (IdP) - Okta, Azure AD, Keycloak, Google Identity.
5.	Select a protocol - SAML 2.0 (enterprise), OIDC (modern web/mobile) or OAuth 2.0.
6.	Register Service Providers (SP) - register each application with the IdP, configuring callback/redirect URIs and client credentials.
7.	Implement the auth flow - redirect unauthenticated users to the IdP login page.
8.	Exchange tokens or assertions - after login, the IdP issues a token (OIDC) or SAML assertion that the SP validates.
9.	Establish a session - the SP creates a local session for the user based on the validated identity.
10.	Implement Single Logout (SLO) - ensure that logging out from one SP propagates logout to the IdP and all other SPs.

**Benefits of SSO**
- Improved user experience - one login for many systems
- Reduced password fatigue and fewer forgotten credentials
- Centralized access control and easier deprovisioning
- Reduced attack surface - fewer passwords to steal
- Simplified compliance auditing - all auth events in one place


## 7. What is the difference between Two-Factor Authentication and Multi-Factor Authentication?

Authentication factors fall into three categories:  
- Something you know - password, PIN, security question.
- Something you have - OTP app, hardware token, SMS code, smart card.
- Something you are - fingerprint, face scan, voice recognition.

**Two-Factor Authentication (2FA)** requires exactly two different factors to authenticate. For example, a password (know) + an OTP from an authenticator app (have). It is a specific subset of MFA.
**Multi-Factor Authentication (MFA)** is the broader concept requiring two or more factors. All 2FA is MFA, but MFA can also require three factors (password + OTP + fingerprint). High-security systems may enforce three-factor authentication.  
In practical usage the terms are often used interchangeably, but strictly speaking 2FA is a special case of MFA where exactly two factors are required.


## 8. Which of the OAuth flows can be used for user (customer) and which for client (server) authentication?

**Flows for User (Customer) Authentication**  
- **Authorization Code Flow** - the most secure and recommended flow for user-facing apps. The user authenticates at the IdP and the app receives an authorization code exchanged for tokens server-side. Used in web apps.
- **Authorization Code Flow with PKCE (Proof Key for Code Exchange)** - the recommended variant for public clients (SPAs, mobile apps) where a client secret cannot be kept confidential. PKCE prevents authorization code interception attacks.
- **Device Authorization Flow (Device Code)** - designed for input-constrained devices (smart TVs, CLIs). The user authenticates on a secondary device (phone/browser) using a code displayed on the primary device.

**Flows for Client (Server / Machine-to-Machine) Authentication**  
- **Client Credentials Flow** - used when no user is involved. A backend service authenticates directly with the IdP using its own client ID and secret, receiving an access token to call other services. The standard approach for service-to-service (M2M) communication.

**Deprecated / Discouraged Flows**  
- **Implicit Flow** - formerly used for SPAs, now deprecated in favor of Authorization Code + PKCE. Tokens were returned directly in the URL fragment, posing security risks.
- **Resource Owner Password Credentials (ROPC)** - the user's credentials are passed directly to the client, which exchanges them for tokens. Strongly discouraged because it exposes credentials to the client and bypasses IdP security features (MFA, consent).

| Flow | Use Case | Actor |
|--------|-------------|------|
| Authorization Code | Web apps with backend | User |
| Auth Code + PKCE | SPA / Mobile apps | User |
| Device Authorization | Smart TV, CLI | User |
| Client Credentials | Service-to-service (M2M) | Client / Server |
| Implicit (deprecated) | SPAs - deprecated | User |
| ROPC (discouraged) | Legacy - discouraged | User |