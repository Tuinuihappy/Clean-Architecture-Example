# 🏗️ ASP.NET Core Clean Architecture + DDD + CQRS + EDA + Modular Monolith (Demo)

นี่คือโปรเจคสาธิตที่ออกแบบด้วยสถาปัตยกรรมระดับองค์กรผสานรวมหลักการ: **Clean Architecture**, **Modular Monolith**, **Domain-Driven Design (DDD)**, **CQRS (Command Query Responsibility Segregation)**, และ **Event-Driven Architecture (EDA)** 🚀 **Product Management System**

---

## 📐 Clean Architecture คืออะไร?

Clean Architecture (โดย Robert C. Martin / Uncle Bob) คือรูปแบบการออกแบบซอฟต์แวร์ที่แบ่ง code ออกเป็นชั้น (layer) โดยมีกฎสำคัญคือ:

> **Dependency Rule**: layer ด้านในไม่รู้จัก layer ด้านนอก — dependencies ชี้เข้าข้างในเสมอ

```
┌──────────────────────────────────────────────────┐
│  API / Presentation Layer   (Controllers, UI)    │
│  ┌──────────────────────────────────────────────┐│
│  │  Infrastructure Layer    (DB, External APIs) ││
│  │  ┌──────────────────────────────────────────┐││
│  │  │  Application Layer    (Use Cases, DTOs)  │││
│  │  │  ┌──────────────────────────────────────┐│││
│  │  │  │  Domain Layer      (Entities, Rules) ││││
│  │  │  └──────────────────────────────────────┘│││
│  │  └──────────────────────────────────────────┘││
│  └──────────────────────────────────────────────┘│
└──────────────────────────────────────────────────┘
```

---

│   │   ├── Interfaces/                        IProductService, ICategoryService
│   │   ├── Mappings/                          Entity ↔ DTO mapping
│   │   └── Services/                          ProductService, CategoryService
│   │
│   ├── CleanArchitectureDemo.Infrastructure/  ← Layer 3
│   │   ├── Data/                              AppDbContext, EF Configurations
│   │   ├── Repositories/                      Repository implementations
│   │   └── DependencyInjection.cs             DI registration
│   │
│   └── CleanArchitectureDemo.API/             ← Layer 4 (outermost)
│       ├── Controllers/                       ProductsController, CategoriesController
│       ├── Middleware/                         ExceptionHandlingMiddleware
│       └── Program.cs                         Entry point
│
└── CleanArchitectureDemo.slnx
```

---

## 🧩 อธิบายแต่ละ Layer

### 🔵 Phase 1: Domain Layer (แกนกลาง)
**ไม่มี dependency กับ layer อื่นเลย**

| ส่วนประกอบ | หน้าที่ |
|---|---|
| **Entities & Aggregate Root**| `Entity` จะมี Id ที่ใช้ยืนยันตัวตน ส่วน `AggregateRoot` จะเป็น Entity หลักของกลุ่มที่ใช้สำหรับดูแล Transaction |
| **Value Objects** | คลาสที่ไม่มี Identity เช่น ชื่อ, ที่อยู่ เท่ากันวัดที่มูลค่าข้างใน |
| **Domain Events** | `IDomainEvent` ใช้ยืนยันว่ามีสิ่งที่เกิดขึ้นจริงในธุรกิจแล้ว เช่น `ProductCreatedEvent` |
| **Enums** | ค่าคงที่ทาง business เช่น `ProductStatus` (Draft, Active, Inactive) |
| **Exceptions** | Custom exceptions สำหรับ business rule violations |
| **Interfaces** | Repository interfaces — **ประกาศใน Domain** แต่ implement ใน Infrastructure (Dependency Inversion) |

### 🟢 Application Layer (Use Cases, CQRS & Event Handlers)
**depends on Domain เท่านั้น**

| ส่วนประกอบ | หน้าที่ |
|---|---|
| **DTOs** | Data objects สำหรับรับ-ส่งข้อมูลกับ client — ไม่ expose Entity ตรงๆ  |
| **Commands / Queries** | ยึดหลัก CQRS แบ่ง Use Case ออกเป็นคำสั่งเปลี่ยนแปลงข้อมูล (Command) และการอ่านข้อมูล (Query) อย่างชัดเจน |
| **Handlers** | ประสาน workflow: รับ Command/Query ส่งผ่าน `MediatR` → เรียก Repository / Entity logic → คืนผลลัพธ์ |
| **Event Handlers** | รอรับ Domain Events ผ่าน `MediatR` แล้วค่อยเอาไปประมวลผลต่อ (เช่น ส่ง Email แจ้งเตือน) อย่างเป็นอิสระ |

### 🟡 Phase 3: Infrastructure Layer (Implementation Details & Event Dispatcher)
**depends on Application (และ Domain ผ่าน transitive)**

| ส่วนประกอบ | หน้าที่ |
|---|---|
| **DbContext** | EF Core database context (ใช้ PostgreSQL) — Override `SaveChangesAsync()` เพื่อเอา Events ทั้งหมดไปเข้าตาราง Dispatch ผ่าน MediatR ก่อน Save จริง |
| **Configurations** | EF entity configuration (constraints, indexes, relationships) |
| **Repositories** | Implement `IProductRepository` / `ICategoryRepository` ที่ประกาศใน Domain |
| **DI Extension** | `AddInfrastructure()` method ลงทะเบียน services ทั้งหมดไว้ที่เดียว |

### 🔴 Phase 4: API / Presentation Layer (Entry Point)
**depends on Infrastructure + Application**

| ส่วนประกอบ | หน้าที่ |
|---|---|
| **Controllers** | รับ HTTP request → เรียก Service → คืน HTTP response (thin layer, ไม่มี business logic) |
| **Middleware** | Global exception handling |
| **Program.cs** | Configuration, DI setup, middleware pipeline |

---

## 🚀 วิธีรัน

### สิ่งที่ต้องมีเบื้องต้น (Prerequisites)
- **PostgreSQL**: ต้องมี PostgreSQL server รันอยู่ (ค่าเริ่มต้นตอนนี้ตั้งไว้ที่ `localhost:5433`) หรือแก้ไข Connection String ใน `src/CleanArchitectureDemo.API/appsettings.json` ให้ตรงกับเครื่องของคุณ (Username/Password เริ่มต้นคือ `postgres`/`postgres`)
- **.NET SDK** 

```bash
# Build
dotnet build CleanArchitectureDemo.slnx

# Run API
dotnet run --project src/CleanArchitectureDemo.API

# เปิด Swagger UI
# http://localhost:5228
```

> **หมายเหตุ**: เมื่อรัน API ครั้งแรก ระบบจะทำการสร้างฐานข้อมูลและ Seed ข้อมูลตัวอย่างให้อัตโนมัติผ่าน `EnsureCreated()`

## 📡 API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/products` | ดึงสินค้าทั้งหมด |
| `GET` | `/api/products/{id}` | ดึงสินค้าตาม ID |
| `GET` | `/api/products/category/{id}` | ดึงสินค้าตามหมวดหมู่ |
| `POST` | `/api/products` | สร้างสินค้าใหม่ |
| `PUT` | `/api/products/{id}` | อัปเดตสินค้า |
| `DELETE` | `/api/products/{id}` | ลบสินค้า |
| `PATCH` | `/api/products/{id}/activate` | เปิดใช้งานสินค้า |
| `PATCH` | `/api/products/{id}/deactivate` | ปิดใช้งานสินค้า |
| `GET` | `/api/categories` | ดึงหมวดหมู่ทั้งหมด |
| `POST` | `/api/categories` | สร้างหมวดหมู่ |
| `PUT` | `/api/categories/{id}` | อัปเดตหมวดหมู่ |
| `DELETE` | `/api/categories/{id}` | ลบหมวดหมู่ |

## 💡 หลักการสำคัญที่เน้นในโปรเจคนี้

1. **CQRS Pattern (ใหม่)** — การแยกส่วนการอ่าน (Query) และการเขียน (Command) ออกจากกันอย่างเด็ดขาด ช่วยให้อ่านโค้ดเข้าใจง่าย บำรุงรักษาได้ดีขึ้น และรองรับสเกลประสิทธิภาพการอ่าน/เขียนแยกกันได้
2. **Rich Domain Model & Aggregate Root** — Entity มี business logic ภายใน ไม่ใช่แค่ data container และมีตัวแทนหลักทำหน้าที่คุม Transaction
3. **Domain Events** — สร้าง Event ขึ้นมาเมื่อมีคำสั่งจาก Business Requirement เปลี่ยนที่ Aggregate 
4. **Event Dispatching** — ก่อน Save DB (EF Core) ตัว AppDbContext จะลากกรอง Event แล้วสาด (Dispatch) ไปให้ Event Handlers
5. **Loosely Coupled via MediatR** — ใช้เป็นตัวกลางส่งทั้ง CQRS (IRequest/IRequestHandler) และ Events (INotification/INotificationHandler) ทำให้โค้ดไม่ผูกติดกันรบกวนกัน
