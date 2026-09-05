# TIWIKOM - Things I Wish I Knew On My...

A modern ASP.NET Core web application for sharing workplace wisdom and tips across your organization.

**Live demo:** https://tiwikom.vercel.app/

> **Status: Archived.** This project is no longer actively maintained. The live demo above may still be reachable but is not receiving updates. This repo is the more complete ASP.NET Core rewrite of the earlier Angular prototype, [tiwikom](https://github.com/taufikhamid2000/tiwikom).

## 📋 Overview

TIWIKOM is a full-featured tip-sharing platform where employees can post valuable insights, advice, and lessons learned. Built with **ASP.NET Core MVC**, **Entity Framework Core**, and **Bootstrap**, it provides a professional experience for knowledge sharing.

## ✨ Features

### Core Features
- ✅ **Complete Tip Management** - Create, read, update, delete tips with rich text editing
- ✅ **User Authentication** - Secure login/register with ASP.NET Core Identity
- ✅ **Role-Based Authorization** - Admin, Supervisor, and Employee roles
- ✅ **Category Organization** - Organize tips by custom categories

### Social Features
- ✅ **Comments** - Users can comment on tips and discuss insights
- ✅ **Likes** - Heart icon to like and show appreciation for tips
- ✅ **Share** - Native share or copy links to share tips
- ✅ **View Counting** - Track tip popularity with view counts

### Search & Discovery
- ✅ **Full-Text Search** - Search tips by title, content, or author
- ✅ **Category Filtering** - Filter tips by category
- ✅ **Multiple Sorting Options** - Sort by newest, most liked, most viewed, or most discussed
- ✅ **Advanced Pagination** - Smooth pagination with filter preservation

### Content Creation
- ✅ **Rich Text Editor** - TinyMCE editor with formatting, lists, code blocks
- ✅ **Professional Styling** - Beautiful rendering of formatted content
- ✅ **Draft/Publish** - Save as draft or publish immediately

### Admin Features
- ✅ **User Management** - View, manage, and delete users
- ✅ **Role Assignment** - Assign roles to users (Admin, Supervisor, Employee)
- ✅ **Admin Panel** - Comprehensive admin dashboard
- ✅ **Seeding** - Auto-create default admin and seed data

## 🛠️ Technology Stack

- **Backend**: ASP.NET Core 8 (MVC pattern)
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: Bootstrap 5, Bootstrap Icons
- **Rich Text Editor**: TinyMCE 6
- **Authentication**: ASP.NET Core Identity
- **Logging**: Serilog
- **API**: RESTful API for interactions (comments, likes)

## 📦 Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full version)
- Visual Studio 2022 or VS Code

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/taufikhamid2000/tiwikom-v2.git
cd tiwikom-v2
```

### 2. Restore dependencies
```bash
dotnet restore
```

### 3. Configure database connection
Update `appsettings.json` with your SQL Server connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TIWIKOM;Trusted_Connection=true;"
  }
}
```

### 4. Run migrations
```bash
dotnet ef database update --project TIWIKOM.Entities --startup-project TIWIKOM.WebApp
```

### 5. Run the application
```bash
cd TIWIKOM.WebApp
dotnet run
```

The application will be available at `https://localhost:5001`

## 👤 Default Admin Account

> ⚠️ **Security warning:** These are **local development seed credentials only**, hardcoded for convenience when running the app on your own machine. They must be changed or removed before deploying this application anywhere reachable by others — never ship this seed account (or reuse this password) to a real/production deployment.

After running the application locally, login with:
- **Email**: `admin@tiwikom.com`
- **Password**: `Admin@123`

Use the admin panel to manage users and assign roles.

## 📁 Project Structure

```
TIWIKOM/
├── TIWIKOM.Entities/          # Entity models and DbContext
│   ├── Models/                # Entity classes
│   ├── Contexts/              # ApplicationDbContext
│   └── Migrations/            # EF Core migrations
├── TIWIKOM.WebApp/            # Main ASP.NET Core application
│   ├── Controllers/           # MVC controllers
│   ├── Views/                 # Razor views
│   ├── Services/              # Business logic services
│   ├── Models/                # View models
│   ├── Helpers/               # Utility classes
│   ├── Areas/Identity/        # Identity UI pages
│   └── wwwroot/               # Static files (CSS, JS, images)
└── README.md
```

## 🔑 Key Endpoints

### Public Pages
- `GET /` - Home page with all tips
- `GET /Home/Search?q=term` - Search results
- `GET /Home/Privacy` - Privacy policy
- `GET /Tips/Detail/{id}` - View tip details

### Authenticated Pages
- `GET /Tips/Create` - Create new tip (Admin/Supervisor only)
- `POST /Tips/Create` - Submit new tip
- `GET /Tips/Edit/{id}` - Edit tip (Author/Admin only)
- `POST /Tips/Edit/{id}` - Update tip
- `GET /Tips/Delete/{id}` - Delete tip (Author/Admin only)
- `GET /Tips/MyTips` - View user's tips

### API Endpoints
- `POST /api/interactions/like/{tipId}` - Toggle like on tip
- `GET /api/interactions/like/{tipId}` - Get like info
- `POST /api/interactions/comment` - Add comment
- `GET /api/interactions/comments/{tipId}` - Get comments
- `DELETE /api/interactions/comment/{id}` - Delete comment

### Admin Pages
- `GET /Admin` - User management dashboard
- `GET /Admin/ManageRoles/{userId}` - Assign roles to user
- `GET /Admin/DeleteUser/{userId}` - Delete user

## 🔒 Authentication & Authorization

The application uses ASP.NET Core Identity with role-based authorization:

- **Admin**: Full access to all features and admin panel
- **Supervisor**: Can create, edit, and delete their own tips
- **Employee**: Can view tips and comment, but cannot create tips

## 📝 Usage Examples

### Creating a Tip
1. Login as Admin or Supervisor
2. Click "Create Tip" in navigation
3. Fill in title, abbreviation, category, and content
4. Use the rich text editor to format content
5. Click "Create Tip" to publish

### Searching Tips
1. Use the search box in navigation for quick search
2. Or use the advanced filters on the home page
3. Filter by category, sort by different criteria
4. Pagination preserves your filters

### Liking and Commenting
1. View a tip detail page
2. Click the heart icon to like (must be logged in)
3. Scroll to comments section and add a comment
4. Comments load in real-time without page refresh

## 🤝 Contributing

To contribute to this project:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Author

Created with ❤️ for knowledge sharing in organizations.

## 📞 Support

For issues, questions, or suggestions, please open an issue on GitHub.

## 🎉 Acknowledgments

- Built with ASP.NET Core 8
- UI powered by Bootstrap 5
- Rich text editing by TinyMCE
- Icons from Bootstrap Icons
- Logging with Serilog

---

Built by [Muhammad Taufik](https://taufik.vercel.app)
