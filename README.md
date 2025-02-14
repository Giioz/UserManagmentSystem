UPDATE YOUR SMTP SENDER WITH VALID CREDENTIALS TO SEND VERIFICATION EMAILS!!!!!!


User Management System
This is a simple User Management System built using Entity Framework and C#. It provides basic functionality for managing users in a database, showcasing essential features such as user registration, email verification, password reset, and user information updates.

Features
User Registration: Allows users to register by providing a username, email, and password.
Email Verification: Sends a verification code to the user's email (SMTP integration) to confirm registration.
Password Reset: Users can reset their password by verifying a code sent to their email.
User Authorization: Ensures secure login by verifying credentials.
Update User Information: Update a user's details, including username, email, and password.
Delete User: Provides functionality to delete a user from the system.
Technologies Used
C#: Programming language for building the application.
Entity Framework: Object-Relational Mapping (ORM) to interact with the database.
SMTP: Simple Mail Transfer Protocol to send email verification codes.
BCrypt: Password hashing for secure storage.
Setup and Installation
Clone this repository:

bash
Copy
git clone https://github.com/yourusername/usermanagement-system.git
cd usermanagement-system
Install the necessary packages:

bash
Copy
dotnet restore
Set up your SMTP email sender with valid credentials to send verification emails.

Configure your database connection string in appsettings.json.

Run the application:

bash
Copy
dotnet run
Usage
Register User:
Enter a valid username, email, and password.
A verification code will be sent to your email.
Enter the verification code to complete the registration.
Reset Password:
Enter your email to receive a verification code.
Input the received code and enter your new password.
Update User:
Update a user's information, including username, email, and password.
Delete User:
Delete a user from the system.
Contributing
Feel free to fork this repository and submit pull requests for any improvements or new features.
