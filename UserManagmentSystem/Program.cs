using UserManagmentSystem.Data;
using UserManagmentSystem.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ConsoleApp10.SMTP;

DataContext _context = new DataContext();
EmailSender _sender = new EmailSender();


while (true)
{
    Console.WriteLine("1. Register \n 2. Login");
    var choice = Console.ReadLine();
    if (choice == "1")
    {
        registerUser();
    }
    else if (choice == "2")
    {
        var user = loginUser();
        if (user != null)
        {
            Console.WriteLine("1. Reset Password \n 2. Delete current User \n 3. Update Current User \n 4. Logout");
            var option = Console.ReadLine();
            if (option == "1")
            {
                resetPassword(user);
            }
            else if (option == "2")
            {
                deleteUser(user);
            }
            else if(option == "3")
            {
                Console.WriteLine("Enter New username to update");
                var newUsername = Console.ReadLine();
                Console.WriteLine("Enter New username to update");
                var newEmail = Console.ReadLine();
                updateUser(user.Id, newUsername, newEmail);
            }
            else if (option == "4")
            {
                Console.WriteLine("Goodbye!");
                break;
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid Choice!");
    }
}


    void registerUser()
{
    Console.WriteLine("Please Enter your Username");
    var username = Console.ReadLine();
    Console.WriteLine("Enter Your Email");
    var email = Console.ReadLine();
    Console.WriteLine("Enter Your Password Min-8 Char!");
    var unhashedPass = Console.ReadLine();
    if(unhashedPass.Count() < 8)
    {
        Console.WriteLine("Password Must be at least 8 Char!");
        return;
    }
    var hashedPass = BCrypt.Net.BCrypt.HashPassword(unhashedPass);
    
    var checkUnique = _context.Users.FirstOrDefault(x => x.Username == username || x.Email == email);

    if (checkUnique != null)
    {
        Console.WriteLine("Username or Email already exist!");
        return;
    }


    Console.WriteLine("Enter your Profile First Name");
    var firstName = Console.ReadLine();
    Console.WriteLine("Enter your Profile Last Name");
    var lastName = Console.ReadLine();
    Console.WriteLine("Enter your Phone Number");
    var phoneNumber = Console.ReadLine();
    Console.WriteLine("Enter your Adrress");
    var adrress = Console.ReadLine();

    var userProfile = new UserProfile
    {
        FirstName = firstName,
        LastName = lastName,
        PhoneNumber = phoneNumber,
        Adrress = adrress,
    };
    var user = new User
    {
        Username = username,
        Email = email,
        PasswordHash = hashedPass,
        CreateAt = DateTime.Now,
        UserProfile = userProfile
    };
    var isVerified = verifyCode(user.Email);
    Console.WriteLine($"Verification code is sent to {user.Email}, please enter the code to verify");
    var code = Console.ReadLine();
    if (code != isVerified)
    {
        Console.WriteLine("Verification code is incorrect!");
        return;
    }
    Console.WriteLine("Account verified succsessfuly!");
    _context.Users.Add(user);
    Console.WriteLine("Your Profile has been created!");
    _context.SaveChanges();

}


User loginUser()
{
    Console.WriteLine("Enter your Username");
    var username = Console.ReadLine();
    Console.WriteLine("Enter your Password");
    var password = Console.ReadLine();
    var user = _context.Users.FirstOrDefault(x => x.Username == username);
    if (user == null)
    {
        Console.WriteLine("User not found!");
        return null;
    }
    if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
    {
        Console.WriteLine("Password is incorrect!");
        return null;
    }
    var userWithProfile = _context.Users
    .Include(u => u.UserProfile) 
    .FirstOrDefault(u => u.Id == user.Id);
    Console.WriteLine("Welcome " + userWithProfile.UserProfile.FirstName + " " + userWithProfile.UserProfile.LastName);
    user.LastLoginAt = DateTime.Now;
    _context.SaveChanges();

    return userWithProfile;
}

void deleteUser(User user)
{
    _context.Users.Remove(user);
    _context.SaveChanges();
    Console.WriteLine("User Deleted successfully.");
}
void updateUser(int userId, string newUsername, string newEmail)
{
    var user = _context.Users.FirstOrDefault(u => u.Id == userId);

    if (user == null)
    {
        Console.WriteLine("User not found!");
        return;
    }

    user.Username = newUsername;
    user.Email = newEmail;


    _context.SaveChanges();
    Console.WriteLine("User information updated successfully.");
}


void resetPassword(User user)
{
    Console.WriteLine("Verification code is sent to Email!");
    var code = verifyCode(user.Email);
    Console.WriteLine("Enter the code to reset your password");
    var userCode = Console.ReadLine();
    if (userCode != code)
    {
        Console.WriteLine("Verification code is incorrect!");
        return;
    }
    Console.WriteLine("Enter your new password");
    var newPassword = Console.ReadLine();
    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
    _context.SaveChanges();
}


string verifyCode(string toMail)
{
    var random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    var randomCode = new string(Enumerable.Range(0, 6)
        .Select(_ => chars[random.Next(chars.Length)])
        .ToArray());
    _sender.sendMail(toMail, "Verify Email", $"your Verify Code is {randomCode}");
    return randomCode;
}



