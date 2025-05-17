# Koperasi TenterApp - ASP.NET Core 8 Web API

This project is a secure user authentication API for mobile onboarding and login using ASP.NET Core 8 with Entity Framework (Code First). It includes:

- User registration by IC number
- OTP verification via mobile number
- Privacy policy acceptance
- 6-digit PIN setup
- Optional biometric login preference
- JWT-based authentication and authorization
- Swagger UI for API testing

## Project Features

- IC number-based registration
- OTP sent to mobile number (simulated)
- OTP validation with expiry
- Agreement to privacy policy tracking
- 6-digit PIN and biometric login setting
- JWT token issuance after PIN login
- Protected API endpoints requiring valid token

## Technologies Used

- ASP.NET Core 8
- Entity Framework Core (Code First)
- SQL Server
- JWT Bearer Authentication
- Swashbuckle for Swagger UI

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/zubayer01913/KoperasiTenterApp.git
cd KoperasiTenterApp
