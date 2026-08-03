# Code Quality & Static Analysis — Questions & Answers

## 1. Which NFRs are affected by the code quality? What are the code quality metrics?

Code quality is not itself an NFR, but it directly influences how well several non-functional requirements are met:

- **Maintainability** - clean, well-structured code is cheaper and safer to change.
- **Reliability** - fewer bugs and edge cases slipping into production.
- **Security** - poor code quality (unchecked inputs, weak error handling) often correlates with vulnerabilities.
- **Testability** - well-decoupled code is easier to cover with unit tests.
- **Performance** - inefficient or overly complex code can introduce unnecessary overhead.
- **Scalability** - tightly coupled code is harder to extract into services or scale independently.

**Code quality metrics**
- **Cyclomatic complexity** - number of independent paths through a function. High values indicate hard-to-test, error-prone code.
- **Cognitive complexity** - similar to cyclomatic complexity but weighted toward how hard the code is for a human to understand.
- **Code coverage** - percentage of code covered by automated tests.
- **Code duplication** - percentage of copy-pasted code blocks.
- **Maintainability index** - estimated effort required to fix all maintainability issues relative to the effort to build the code from scratch.
- **Coupling** - coupling is the degree of interdependence between software modules, a measure of how closely connected two routines or modules are.
- **Code smells count** - number of style and design issues flagged by static analysis (long methods, god classes, magic numbers).
- **Bugs and vulnerabilities count** - issues detected by static analysis, often ranked by severity.
- **Documentation density** - proportion of documented public APIs.


## 2. What are the goals of static code analysis? How many code analyzers can be used on a project?

**Goals of static code analysis**
- Catch bugs and logic errors before runtime, without executing the code.
- Enforce a consistent coding style across the team.
- Detect security vulnerabilities and unsafe patterns.
- Identify code smells, dead code and duplication.
- Track and reduce technical debt over time.
- Provide objective, repeatable metrics for code reviews and release decisions.
- Reduce the manual burden on human reviewers, letting them focus on design and logic instead of formatting.

**How many analyzers can be used on a project**  
There is no hard limit, and in practice a project usually combines several analyzers, since each tool specializes in a different concern:

- **Linters** - style and syntax rules (StyleCop).
- **Formatters** - automatic code formatting (dotnet format).
- **Complexity/duplication analyzers** - cyclomatic complexity, copy-paste detection (SonarQube).
- **Security scanners** - vulnerability and unsafe-pattern detection (Snyk).
- **Dependency scanners** - known-vulnerability detection in third-party libraries (OWASP Dependency-Check, Snyk, SonarQube).

A typical pipeline runs 3-5 complementary tools. Running too many overlapping analyzers on the same concern tends to produce conflicting rules and excessive false-positive noise, so the tools are usually chosen to cover distinct concerns rather than duplicate each other.


## 3. How to ensure every team member will follow a style guide? How to apply style guide for the legacy projects?

**Ensuring team-wide adherence**
- Automate enforcement with linters and formatters running directly in the IDE, so violations are visible while typing.
- Add **pre-commit hooks** that block a commit if style checks fail.
- Add a **style-check step in CI/CD** that fails the build on violations, so nothing non-compliant reaches the main branch.
- Share a common IDE configuration (`.editorconfig`) so the rules are identical for everyone, not tool-dependent.
- Make style guide compliance part of the **code review checklist**.
- Cover the style guide in onboarding documentation so new members start compliant from day one.

**Applying a style guide to legacy projects**
- Avoid reformatting the entire codebase in one pass - it creates a lot of unnecessary noise.
- Start with all checks disabled. 
- Gradually enable most relevant checks.


## 4. Which are the pros and cons of the SonarQube analyzer? Think of a criterion to use it on a project.

**Pros**
- Combines multiple concerns: bugs, code smells, duplication, complexity, test coverage and security hotspots.
- Provides configurable **quality gates** that can block a merge or release.
- Historical dashboards showing quality trends over time.
- Integrates well with CI/CD pipelines.
- Has an open-source Community Edition.

**Cons**
- Self-hosting the server requires infrastructure and ongoing maintenance (database, upgrades, storage for history).
- Prone to false positives. Rule sets need tuning per project to stay useful instead of noisy.
- Some valuable features (multi-branch analysis, advanced security reports) are locked behind paid editions.
- Initial setup of quality profiles and gates has a learning curve.
- Analysis time can meaningfully slow down CI for very large codebases.
- It complements but does not replace human code review - it won't catch design or business-logic issues.

**Criterion for deciding whether to use it on a project**

| Criterion | Favors using SonarQube |
|---|---|
| Codebase size | Large or long-lived codebase where trend tracking pays off |
| Team size | Multiple contributors who need a consistent, automated bar |
| Language mix | Single dashboard for all |
| Compliance needs | Regulatory or security reporting is required |
| Infrastructure budget | Team can host and maintain the server |
| CI/CD maturity | Pipeline already exists and can gate merges on results |

For a small, short-lived, or single-developer project, a lightweight linter and formatter combo is often enough, and the overhead of running SonarQube may not be justified.


## 5. What are the quality gates?

A **quality gate** is a set of predefined pass/fail conditions that code must satisfy before it is allowed to move to the next stage of the pipeline - typically before a merge or a release.

Typical conditions included in a quality gate:
- Code coverage on new code above a threshold (> 80%).
- Zero new **blocker** or **critical** bugs.
- Zero new security vulnerabilities or unresolved security hotspots.
- Code duplication below a threshold (< 3%).
- No new code smells above a chosen severity level.
- Maintainability index within an acceptable range.
