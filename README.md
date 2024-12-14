# Udemy Clone - Microservices Architecture

This project is a Udemy-like platform built using a microservices architecture. The communication between services is handled via **gRPC** for efficiency and speed, while **REST controllers** are exposed for external clients to interact with the system. The system is designed with **Clean Architecture** and **Domain-Driven Design (DDD)** principles to ensure scalability, modularity, and maintainability. Additionally, extensive testing has been implemented, including **unit tests** and **integration tests** using **Testcontainers**.

---

## Architecture Overview

### Key Design Principles:
1. **gRPC for Inter-Service Communication**: 
   - All internal service-to-service communication is done using gRPC for faster and more efficient messaging.
2. **REST for External API Endpoints**:
   - RESTful APIs are exposed via controllers to provide easy access for external clients (e.g., frontend, mobile apps).
3. **Clean Architecture**:
   - Each service is built using Clean Architecture principles to separate concerns and keep business logic independent of frameworks and external dependencies.
4. **Domain-Driven Design (DDD)**:
   - Focused on creating rich domain models with entities, value objects, aggregates, and domain events.
   - The core domain logic is encapsulated in the domain layer, ensuring that the business logic is reusable and testable.

---

## Microservices Overview

### 1. **Auth Service**
- Manages user registration, login, and role-based access control.
- **gRPC Endpoints**: Authentication, user profile updates.
- **REST Controllers**: User registration, login.

### 2. **Course Management Service**
- Handles course creation, updates, and listings.
- **gRPC Endpoints**: Fetch course metadata.
- **REST Controllers**: Course search, filter, and view.

### 3. **Video Management Service**
- Manages video uploads, storage, and playback.
- **gRPC Endpoints**: Internal video metadata retrieval.
- **REST Controllers**: Upload videos, get playback links.

### 4. **Order and Payment Service**
- Handles course purchases and payment processing.
- **gRPC Endpoints**: Verify and process orders.
- **REST Controllers**: Initiate payments, view purchase history.

### 5. **Notification Service**
- Sends emails and real-time notifications.
- **gRPC Endpoints**: Trigger notifications for events.
- **REST Controllers**: Manage notification settings.

### 6. **Review and Rating Service**
- Allows users to leave reviews and rate courses.
- **gRPC Endpoints**: Fetch and aggregate review data.
- **REST Controllers**: Post comments, view ratings.

### 7. **Statistics and Reporting Service**
- Provides analytics and insights.
- **gRPC Endpoints**: Generate statistics for admins.
- **REST Controllers**: View reports and statistics.

### 8. **Category and Tag Management Service**
- Categorizes and tags courses.
- **gRPC Endpoints**: Fetch categories and tags.
- **REST Controllers**: Add new categories/tags, search by tag.

### 9. **API Gateway**
- Routes external REST requests to respective services.
- Handles load balancing and authentication.

---

## Communication Overview

### gRPC:
- **Purpose**: Used for internal communication between services to ensure low-latency and high-performance messaging.
- **Protocol**: Uses HTTP/2 and Protocol Buffers for serialization.
- **Example**: 
  - Video Management Service communicates with the Course Management Service to fetch course details via gRPC.

### REST:
- **Purpose**: Provides a client-friendly interface for external applications such as the frontend or mobile apps.
- **Example**:
  - A REST API endpoint in the User Management Service allows new users to register.

---

## Testing Strategy

### Unit Tests:
- Each layer of the application (domain, application, infrastructure) includes unit tests to ensure code correctness.
- Mocks and fakes are used to isolate dependencies.

### Integration Tests:
- Integration tests are implemented using **Testcontainers** to spin up real instances of dependent services (e.g., databases, message brokers) in isolated Docker containers.
- Example: Testing the Order Service with a live PostgreSQL container to validate data persistence and integrity.

### Tools and Frameworks:
- **Unit Tests**: xUnit, Moq
- **Integration Tests**: Testcontainers, Docker
- **Code Coverage**: Coverlet, ReportGenerator

---

## Technologies Used
- **Backend**: ASP.NET Core (.NET 8)
- **Architecture**: Clean Architecture, Domain-Driven Design
- **Communication**: gRPC (for internal), REST (for external)
- **Database**: PostgreSQL, MongoDB (for specific use cases), Redis (for caching), Elasticsearch (for search), InfluxDB (for metrics), ClickHouse (for analytics), RabbitMQ (for messaging), Amazon S3 (for file storage), Prometheus (for monitoring), Grafana (for visualization), Kibana (for logging), Consul (for service discovery), Vault (for secrets management)
- **Message Broker**: RabbitMQ (for async notifications)
- **Containerization**: Docker
- **Orchestration**: Docker-Compose
- **Testing**: xUnit, Moq, Testcontainers

---

## Setup Instructions

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-repo/udemy-clone.git
   cd udemy-clone

2. **Build and Run Services**:
   ```bash
	docker-compose up --build
	```

3. **Access the API Gateway**:
   - API Gateway: http://localhost:3000
