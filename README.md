# MeetingSchedule
Address book, to add, edit, search contacts. Sort them by tags or categories. The idea is to use the Mediator design pattern, the idea is to start simple and add functionality little by little, and learn how MediatR scales in growing applications.

Design patterns:

Factory →	Create API response objects in a consistent and centralized manner.
Unit of Work →	Groups transactions into a single atomic operation.
Repository →	Encapsulates data access logic per entity.
MediatR →	Orchestrates communication between objects without direct coupling.

Remember: these design patterns are not mandatory, but they are highly recommended when:

- Domains with complex or growing logic.
- Medium to large applications with many modules (users, contacts, tags, specialties, contacttags , etc.).
- Multiple data sources or repositories.
- You need to test parts of the business logic easily.
- You are looking for scalability and long-term maintenance.
- When there are large teams or multiple developers.

The code explained here is for example and visualization purposes.

