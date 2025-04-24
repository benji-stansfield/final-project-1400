/*Benji Stansfield, 04-09-25, Final Project*/
using System.Diagnostics;

Console.Clear();

/*Debug Tests*/
Debug.Assert(ValidateUsername("benji stansfield") == false, "User shouldn't be allowed to use a space in username");
Debug.Assert(ValidateUsername("14b") == false, "username should be over 4 characters");
Debug.Assert(ValidateUsername("bms17") == true);
Debug.Assert(ValidateUsername("bstansfield123456789") == true);

int attempts = 3; //tracks login attempts
string usernameInput; 
string pinInput;
bool loggedIn = false; //used to log in a user
bool userFound = false;
bool usernameValidated = false; //used to validate a username while creating account
bool pinValidated = false; //used to validate a pin while creating account

Console.WriteLine(@"------------------------
 WELCOME TO YOUR BUDGET
------------------------
");
Console.WriteLine("Please select an option below");
Console.WriteLine(@"1 - Sign In
2 - Create Account");

int userChoice = Convert.ToInt32(Console.ReadLine());

Console.Clear();

switch (userChoice)
{
    case 1:

        string[] lines = File.ReadAllLines("users.txt");

        while (!loggedIn && attempts > 0)
        {
            Console.Write("Username: ");
            usernameInput = Console.ReadLine();
            Console.Write("Pin #: ");
            pinInput = Console.ReadLine();

            foreach(string line in lines)
            {
                string[] parts = line.Split(',');

                string username = parts[0];
                string pin = parts[1];

                if (usernameInput == username && pinInput == pin)
                {   
                    Console.Write("Sign in successful");
                    userFound = true;
                    loggedIn = true;
                    break;
                }
            }

            if (!userFound)
            {
                Console.WriteLine("Username or pin not recognized.");
                attempts--;
                Console.WriteLine($"{attempts} attempts remaining");
                continue;
            }
        }
        break;
    case 2:

        Console.WriteLine("Please create a username (between 8-20 characters long, no spaces)");

        do
        {
            Console.Write("Username: ");
            usernameInput = Console.ReadLine();
            if (ValidateUsername(usernameInput))
                usernameValidated = true;

        } while(!usernameValidated);

        break;
        
    default:
        Console.WriteLine("Please select an option above.");
        break;
}

static bool ValidateUsername(string input)
{
    if (input.Length > 20)
    {
        Console.WriteLine("Username must be less than 20 characters.\n");
        return false;
    }

    else if (input.Length < 5)
    {
        Console.WriteLine("Username must be at least 5 characters.\n");
        return false;
    }
    else if (input.Contains(' '))
    {
        Console.WriteLine("Username cannot contain spaces.\n");
        return false;
    }
    
    return true;
}

