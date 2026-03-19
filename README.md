# 🏗️ ASP.NET Core Clean Architecture Demo

ตัวอย่างโปรเจค ASP.NET Core API ที่ใช้ **Clean Architecture** สำหรับ **Product Management System**

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

## 📁 โครงสร้างโปรเจค

```
CleanArchitectureDemo/
├── src/
│   ├── CleanArchitectureDemo.Domain/          ← Layer 1 (innermost)
│   │   ├── Entities/                          Product, Category
│   │   ├── Enums/                             ProductStatus
│   │   ├── Exceptions/                        DomainException
│   │   └── Interfaces/                        IProductRepository, ICategoryRepository
│   │
│   ├── CleanArchitectureDemo.Application/     ← Layer 2
│   │   ├── Common/                            Result<T> wrapper
│   │   ├── DTOs/                              ProductDto, CategoryDto
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
| **Entities** | Business objects ที่มี logic ภายใน (Rich Domain Model) เช่น Product มี method `Activate()`, `SetPrice()` ที่มี validation |
| **Enums** | ค่าคงที่ทาง business เช่น `ProductStatus` (Draft, Active, Inactive) |
| **Exceptions** | Custom exceptions สำหรับ business rule violations |
| **Interfaces** | Repository interfaces — **ประกาศใน Domain** แต่ implement ใน Infrastructure (Dependency Inversion) |

### 🟢 Phase 2: Application Layer (Use Cases)
**depends on Domain เท่านั้น**

| ส่วนประกอบ | หน้าที่ |
|---|---|
| **DTOs** | Data objects สำหรับรับ-ส่งข้อมูลกับ client — ไม่ expose Entity ตรงๆ  |
| **Interfaces** | Service interfaces กำหนด use cases ที่ app ทำได้ |
| **Services** | ประสาน workflow: รับ DTO → เรียก Entity logic → เรียก Repository → คืน DTO |
| **Common** | `Result<T>` wrapper สำหรับส่งสถานะ success/failure แทนการ throw exception |

### 🟡 Phase 3: Infrastructure Layer (Implementation Details)
**depends on Application (และ Domain ผ่าน transitive)**

| ส่วนประกอบ | หน้าที่ |
|---|---|
| **DbContext** | EF Core database context — กำหนดการเชื่อมต่อ DB |
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

```bash
# Build
dotnet build CleanArchitectureDemo.slnx

# Run API
dotnet run --project src/CleanArchitectureDemo.API

# เปิด Swagger UI
# http://localhost:5228
```

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

## 💡 หลักการสำคัญที่ใช้

1. **Dependency Inversion Principle (DIP)** — Repository interface อยู่ใน Domain, implementation อยู่ใน Infrastructure
2. **Separation of Concerns** — แต่ละ layer มีหน้าที่ชัดเจน ไม่ปนกัน
3. **Rich Domain Model** — Entity มี business logic ภายใน ไม่ใช่แค่ data container
4. **DTO Pattern** — ไม่ expose Domain Entity ให้ client โดยตรง
5. **Result Pattern** — ใช้ `Result<T>` แทนการ throw exception สำหรับ expected failures
