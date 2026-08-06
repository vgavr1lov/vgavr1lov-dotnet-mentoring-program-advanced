# Containerization and Orchestration — Questions & Answers

## 1. What is orchestration?

**Orchestration** is the automated management of containers or other distributed application components. It is responsible for deploying, starting, stopping, scaling and monitoring application instances. The main purpose of orchestration is to make sure that the required number of application instances are running and that the system can recover from failures.

**Orchestration** provides:
- **Deployment** - starts application containers according to the desired configuration.
- **Scaling** - increases or decreases the number of running instances.
- **Load balancing** - distributes traffic between application instances.
- **Self-healing** - restarts failed containers or replaces unhealthy instances.
- **Rolling updates** - deploys a new application version without stopping the whole system.
- **Service discovery** - allows services to find and communicate with each other.

The main benefit is that instead of manually managing many containers, the desired state of the system is described and the orchestration platform continuously tries to maintain it.

Kubernetes is the most widely used container orchestration platform.


## 2. What is containerization and the pros and cons of using it?

**Containerization** is a way of packaging an application together with its dependencies and configuration into an isolated, portable container. The container shares the host operating system kernel but has its own filesystem, processes and networking environment.
A container image contains everything required to run the application, which helps ensure that it behaves consistently across different environments.

**Docker** is one of the most commonly used technologies for creating and running containers.

**Advantages**
- **Portability** - the same container can run on a developer machine, test environment or production server.
- **Consistency** - application dependencies and configuration can be packaged together.
- **Fast startup** - containers usually start much faster than virtual machines.
- **Efficient** resource usage - containers share the host OS kernel instead of requiring a separate guest OS.
- **Isolation** - applications and their dependencies are separated from each other.
- **Scalability** - many container instances can be created relatively easily.

**Disadvantages**
- **Security isolation is weaker than VMs** - containers share the host kernel.
- **Operational complexity** - managing many containers manually becomes difficult.
- **Persistent storage is more complicated** - containers are normally treated as temporary or replaceable.
- **Image management overhead** - images need to be built, stored, scanned and updated.
- **Orchestration may be required** for large systems, adding technologies such as Kubernetes.


## 3. What is the difference between containerization and virtualization?

**Virtualization** creates virtual machines where each VM normally contains a complete guest operating system. A hypervisor manages these virtual machines and provides hardware virtualization.

**Containerization** runs isolated processes as containers while sharing the host operating system kernel.

For example, if a server runs three virtual machines, each VM may have its own operating system. With containers, three applications can share the same host kernel while remaining isolated from each other.

Virtualization is useful when strong OS-level isolation or different operating systems are required. Containerization is useful when lightweight, portable and rapidly scalable application environments are needed.

| | Virtualization | Containerization |
|---|---|---|
| Isolation | Virtual machine level | Process level |
| Operating system | Each VM has its own OS | Containers share host kernel |
| Resource usage | Higher | Lower |
| Startup time | Slower | Faster |
| Size | Larger | Smaller |
| Isolation | Stronger | Weaker |
| Scaling | Heavy | Light and fast |

## 4. Explain the usage flow of Docker & Kubernetes.

Docker usage flow is:
1. Developer creates application code.
2. A Dockerfile describes how to build the application image.
3. Docker builds the image from the Dockerfile.
4. The image is stored locally or pushed to a container registry.
5. A container is started from the image.
6. The container runs the application in an isolated environment.

Kubernetes is normally used when there are multiple containers that need to be deployed, scaled and managed.

Kubernetes flow is:
1. Developer builds a container image.
2. The image is pushed to a container registry.
3. A Kubernetes Deployment describes the desired application state.
4. Kubernetes schedules Pods containing the containers on available nodes.
5. A Kubernetes Service provides stable network access to the Pods.
6. Kubernetes monitors the application and maintains the desired number of replicas.
7. If a container or Pod fails, Kubernetes can replace it.
8. When the application needs more capacity, Kubernetes can scale the number of replicas.


## 5. What are the best practices for containerization?

- **Use minimal base images** to reduce image size and attack surface.
- **Use multi-stage builds** when possible to avoid putting build dependencies into the final image.
- **Use a .dockerignore file** to avoid copying unnecessary files into the image.
- **Regularly scan images** for vulnerabilities.
- **Run containers as a non-root user** to lower security privilege risks.
- **Keep containers stateless** to ensure data persistency via mounted external volumes or cloud storage services.


## 6. How is Docker CI different from classic CI pipeline?

A **classic CI pipeline** usually builds the application directly on the CI server. The pipeline installs the required dependencies, builds the application, runs tests and produces an artifact.

A **Docker CI pipeline** performs these steps inside containers or produces a container image as the application artifact.

Main differences:
- **Environment consistency** - Docker provides a reproducible environment for building and testing.
- **Artifact** - instead of only producing a package such as a DLL, the pipeline can produce a complete container image.
- **Deployment consistency** - the same image tested in CI can be deployed to staging and production.
- **Isolation** - build and test dependencies can be isolated inside containers.
- **Image security** - container images can be scanned for known vulnerabilities before deployment.
- **Deployment model** - the final artifact can be directly deployed to a container platform such as Kubernetes.
