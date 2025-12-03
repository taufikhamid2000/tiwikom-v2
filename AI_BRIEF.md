# 🤖 AI Assistant Brief - TIWIKOM Project

**Last Updated:** 2025-11-30  
**Project Status:** ✅ Consolidated and Running

---

## 📋 Quick Project Summary

**TIWIKOM** = "Things I Wish I Knew On My..." - An ASP.NET Core web application for sharing workplace tips and advice.

### Core Technology Stack
- **Framework:** ASP.NET Core 8.0
- **Frontend:** Razor Pages
- **Database:** SQL Server (LocalDB)
- **Authentication:** ASP.NET Core Identity
- **Logging:** Serilog
- **ORM:** Entity Framework Core

---

## 📁 Project Structure (FINAL - CONSOLIDATED)

```
C:\Users\TaufikHamid\Desktop\TIWIKOM\
├── TIWIKOM.sln                        ← Open this in Visual Studio
├── TIWIKOM.Entities/                  ← Data Models & Database Context
│   ├── TIWIKOM.Entities.csproj
│   ├── ApplicationUser.cs              (User model - inherits from IdentityUser)
│   ├── Tip.cs                          (Tip model - main content entity)
│   ├── Category.cs                     (Categories for organizing tips)
│   └── Contexts/
│       └── ApplicationDbContext.cs     (EF Core DbContext)
│
└── TIWIKOM.WebApp/                    ← Web Application
    ├── TIWIKOM.WebApp.csproj
    ├── Program.cs                      (Startup configuration - KEY FILE)
    ├── appsettings.json               (Database connection string, logging config)
    ├── appsettings.Development.json
    ├── Pages/
    │   ├── Index.cshtml               (Home page - list all tips)
    │   ├── Index.cshtml.cs
    │   └── Tips/
    │       ├── Create.cshtml          (Create new tip form)
    │       ├── Create.cshtml.cs
    │       ├── Detail.cshtml          (View single tip)
    │       └── Detail.cshtml.cs
    ├── Services/
    │   ├── TipService.cs              (Business logic for tips)
    │   ├── RoleDataInitializer.cs     (Seeds roles: Admin, Supervisor, Employee)
    │   └── TipDataInitializer.cs      (Seeds tip categories)
    ├── wwwroot/                        (Static files: CSS, JS, images)
    └── bin/                            (Built assemblies)
```

### ❌ OLD FILES (Safe to Delete When Ready)
```
C:\Users\TaufikHamid\Desktop\TIWIKOM.sln           ← Original (copy exists in TIWIKOM/)
C:\Users\TaufikHamid\Desktop\TIWIKOM.Entities\    ← Original (copy exists in TIWIKOM/)
C:\Users\TaufikHamid\Desktop\TIWIKOM.WebApp\      ← Original (copy exists in TIWIKOM/)
C:\Users\TaufikHamid\Desktop\TIWIKOM_New\         ← Intermediate (copy exists in TIWIKOM/)
```

---

## 🚀 How to Run the Application

### From Command Line
```powershell
cd C:\Users\TaufikHamid\Desktop\TIWIKOM
dotnet build TIWIKOM.sln
dotnet run --project TIWIKOM.WebApp
```

### From Visual Studio
1. Open `C:\Users\TaufikHamid\Desktop\TIWIKOM\TIWIKOM.sln`
2. Right-click `TIWIKOM.WebApp` → Set as Startup Project
3. Press `F5` to run

### Application URLs
- **HTTPS:** `https://localhost:5001`
- **HTTP:** `http://localhost:5000`

### Database Setup
The app automatically:
- ✅ Runs Entity Framework migrations
- ✅ Creates SQL Server (LocalDB) database named `TIWIKOM`
- ✅ Seeds roles: Admin, Supervisor, Employee
- ✅ Seeds tip categories: First Day, Technical, Culture, Career Growth, Communication

---

## 🔧 Key Configuration Files

### `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TIWIKOM;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AppSettings": {
    "ProductName": "TIWIKOM"
  }
}
```

### `Program.cs` (Startup Configuration)
- Configures Razor Pages & MVC
- Sets up Entity Framework with SQL Server
- Configures ASP.NET Core Identity with password requirements:
  - Minimum 8 characters
  - Must contain: uppercase, lowercase, digit, special character
- Registers services: TipService, RoleDataInitializer, TipDataInitializer
- Configures Serilog for logging
- Initializes database and seeds data on startup

---

## 👥 User Roles & Authentication

### Built-in Roles
1. **Admin** - Full access, can manage all tips and users
2. **Supervisor** - Can post and manage tips
3. **Employee** - Can view tips

### Password Requirements
```
✓ Minimum 8 characters
✓ At least 1 uppercase letter (A-Z)
✓ At least 1 lowercase letter (a-z)
✓ At least 1 digit (0-9)
✓ At least 1 special character (!@#$%^&*)
```

**Example valid password:** `Test@1234`

---

## 📊 Database Schema

### Users Table
- **UserId** (PK)
- **Email**
- **UserName**
- **PasswordHash**
- **Created Date**

### Tips Table
- **TipId** (PK)
- **Title**
- **Content**
- **CategoryId** (FK → Categories)
- **UserId** (FK → Users) - Author
- **CreatedDate**
- **ViewCount**
- **Abbreviation** (Short category name)

### Categories Table
- **CategoryId** (PK)
- **Name** (e.g., "First Day", "Technical")
- **Description**

### Roles & UserRoles Tables
- Managed by ASP.NET Core Identity
- Links users to roles

---

## 🐛 Common Issues & Solutions

### Issue: "Connection Refused" at localhost:5001
**Solution:** Ensure the app is running with `dotnet run --project TIWIKOM.WebApp`

### Issue: "Database does not exist"
**Solution:** Delete database and let migrations recreate it:
```powershell
dotnet ef database drop --project TIWIKOM.WebApp
dotnet run --project TIWIKOM.WebApp
```

### Issue: Port 5001 already in use
**Solution:** Change port in `launchSettings.json`:
```json
"applicationUrl": "https://localhost:5002;http://localhost:5000"
```

### Issue: Files locked during copy/build
**Solution:** Stop dotnet processes:
```powershell
Stop-Process -Name dotnet -Force
```

---

## 🔑 Important Code Entry Points

### Main Entry Point
- **File:** `TIWIKOM.WebApp/Program.cs`
- **What it does:** Configures DI, database, identity, logging, middleware
- **Key method:** `Main()` and `ConfigureLogging()`

### Database Context
- **File:** `TIWIKOM.Entities/Contexts/ApplicationDbContext.cs`
- **Contains:** DbSets for User, Tip, Category
- **Connection:** Uses appsettings.json connection string

### Business Logic
- **File:** `TIWIKOM.WebApp/Services/TipService.cs`
- **Contains:** Methods to create, read, update, delete tips

### Page Handlers
- **Home:** `TIWIKOM.WebApp/Pages/Index.cshtml.cs` - Lists all tips
- **Create Tip:** `TIWIKOM.WebApp/Pages/Tips/Create.cshtml.cs` - Create new tip
- **View Tip:** `TIWIKOM.WebApp/Pages/Tips/Detail.cshtml.cs` - Show single tip

---

## 📝 Development Notes

### To Add a New Feature
1. Create model in `TIWIKOM.Entities/`
2. Add DbSet to `ApplicationDbContext`
3. Create EF migration: `dotnet ef migrations add FeatureName --project TIWIKOM.WebApp`
4. Update database: `dotnet ef database update --project TIWIKOM.WebApp`
5. Create service in `TIWIKOM.WebApp/Services/`
6. Create Razor Pages in `TIWIKOM.WebApp/Pages/`
7. Register service in `Program.cs`

### To Change Database Connection
Edit `appsettings.json`:
```json
"DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DB;User Id=sa;Password=your_password;"
```

### To Add a New Role
Edit `TIWIKOM.WebApp/Services/RoleDataInitializer.cs`:
```csharp
await roleManager.CreateAsync(new IdentityRole("YourRoleName"));
```

---

## ✅ Latest Status

**Last Action Taken:** 2025-11-30 13:47
- ✅ Consolidated project into single parent folder: `C:\Users\TaufikHamid\Desktop\TIWIKOM\`
- ✅ Verified build succeeds from new location
- ✅ All project files copied and intact
- ✅ Solution file paths validated
- ⏳ Ready to delete old scattered files on Desktop (once confirmed working)

**Application Status:** Ready to run from new location

---

## 🎯 Next Steps (If Needed)

1. **Test the consolidated setup:**
   ```powershell
   cd C:\Users\TaufikHamid\Desktop\TIWIKOM
   dotnet run --project TIWIKOM.WebApp
   ```

2. **Once confirmed working, delete old files:**
   - `C:\Users\TaufikHamid\Desktop\TIWIKOM.sln`
   - `C:\Users\TaufikHamid\Desktop\TIWIKOM.Entities\`
   - `C:\Users\TaufikHamid\Desktop\TIWIKOM.WebApp\`
   - `C:\Users\TaufikHamid\Desktop\TIWIKOM_New\`

3. **Use `C:\Users\TaufikHamid\Desktop\TIWIKOM\` as your working directory for all future development**

---

## 📞 For Future AI Assistance

When asking for help with TIWIKOM:
- Always reference the new consolidated location: `C:\Users\TaufikHamid\Desktop\TIWIKOM\`
- Provide the file path relative to TIWIKOM/ folder
- Mention if changes needed in `Program.cs`, `appsettings.json`, services, or pages
- Include any error messages or stack traces
- Specify which role/user type should have access to new features

**Example good request:**
> "Add a new 'Feedback' feature to TIWIKOM. Users should be able to rate tips 1-5 stars. Only authenticated employees and above can rate."

---

**Good luck with TIWIKOM! 🚀**
