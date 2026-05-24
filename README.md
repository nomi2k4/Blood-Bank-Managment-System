# Blood Bank Management System

A simple Windows Forms desktop application for managing blood bank operations — donor records, patient management, blood inventory tracking, and blood transfers.

Built using **C# WinForms (.NET Framework 4.8)** and **SQL Server**.

---

## Features

### Authentication
- Secure employee login system
- SQL Server based authentication

### Dashboard
- Total donors count
- Total patients count
- Available blood stock
- Blood transfer statistics
- Monthly HTML report generation

### Donor Management
- Add donor records
- Update donor details
- Delete donor records
- Search donors instantly
- Blood stock auto increment

### Patient Management
- Add patient records
- Update patient details
- Delete patient records
- Live search functionality

### Blood Inventory
- Real-time blood stock tracking
- Supports all 8 blood groups:
  - A+
  - A-
  - B+
  - B-
  - O+
  - O-
  - AB+
  - AB-

### Blood Transfer
- Transfer blood to patients
- Automatic stock deduction
- Transfer history storage

### UI Features
- Animated splash screen
- Sidebar navigation
- Guna UI2 modern controls
- Responsive dashboard cards

---

# Technologies Used

| Technology | Purpose |
|---|---|
| C# | Application Logic |
| WinForms (.NET Framework 4.8) | Desktop UI |
| SQL Server | Database |
| ADO.NET | Database Connectivity |
| Guna.UI2 | Modern UI Controls |
| HTML/CSS | Monthly Reports |

---

# Project Structure

```bash
Blood-Bank-Managment-System/
│
├── Blood Bank.sln
├── App.config
├── Program.cs
│
├── Forms/
│   ├── Splash.cs
│   ├── Login.cs
│   ├── Mainform.cs
│   ├── Dashboard.cs
│   ├── Donor.cs
│   ├── ViewDonor.cs
│   ├── Patient.cs
│   ├── ViewPatients.cs
│   ├── BloodStock.cs
│   └── BloodTransfer.cs
│
├── Database/
│   ├── DbConnection.cs
│   └── DatabaseSchema.sql
│
├── Assets/
│   ├── Images
│   └── Icons
│
└── docs/
