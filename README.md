# WGU Student Tracker App

**Developer:** Christopher Whaley  
**Project:** Western Governors University - D424 Software Engineering Capstone

## 📱 About the App
The Student Tracker App is a comprehensive, offline-first mobile application designed to help students efficiently manage their academic journey. Built with a focus on intuitive UI/UX and robust data integrity, the app allows users to seamlessly track their degree progress from the macro level (Semesters/Terms) down to the micro level (Specific Courses and Assessments). 

Whether you are planning future terms or checking upcoming exam dates, this application provides a centralized, secure, and highly responsive dashboard for all your academic needs.

## ✨ Key Features
* **Secure Authentication:** Local user registration and login system utilizing cryptographic password hashing.
* **Hierarchical Academic Tracking:** Full Create, Read, Update, and Delete (CRUD) functionality for Terms, Courses, and Assessments.
* **Interactive Reporting:** A dynamic dashboard that aggregates and visualizes the user's current academic standing (e.g., Courses Completed, In Progress, Dropped).
* **Robust Search:** A global search function capable of filtering through both Courses and Assessments simultaneously.
* **Strict Data Integrity:** Built-in Regular Expression (Regex) validation ensuring secure passwords, formatted emails, and proper phone numbers.
* **Device Integration:** Utilizes local device notifications to alert students of upcoming course start dates and assessment deadlines.

## 🛠️ Tech Stack
This application was engineered using modern mobile development frameworks and local database architecture:
* **Framework:** .NET MAUI (.NET 9.0)
* **Language:** C# / XAML
* **Database:** SQLite (`sqlite-net-pcl`)
* **Testing:** xUnit (Automated Unit Testing)
* **Additional Libraries:** `Plugin.LocalNotification`

## 🚀 Installation and Access
*(Note: This section will be updated shortly with a direct download link once the application is deployed to GitHub Pages!)*

### Download the App
1. Navigate to the **[Student Tracker App Release Page](#)** *(Link coming soon)*.
2. Download the latest `StudentTrackerApp.apk` file to your Android device or emulator.
3. If prompted, allow your device to "Install from unknown sources."
4. Open the app, create a secure account, and begin tracking your degree!

### Running the Source Code locally
1. Clone this repository to your local machine:
   `git clone https://github.com/YOUR-USERNAME/YOUR-REPO-NAME.git`
2. Open the solution file (`.sln`) in **Visual Studio 2022**.
3. Ensure you have the **.NET Multi-platform App UI development** workload installed.
4. Set your target device to an Android Emulator (API 30+ recommended).
5. Build and run the project!

## 🧪 Testing Overview
The backend logic, data validation, and database operations are strictly validated using an automated xUnit testing suite. The tests verify asynchronous database locking, isolated data generation, and deterministic password hashing to ensure a completely stable production environment.
