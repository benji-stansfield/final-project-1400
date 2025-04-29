/*Benji Stansfield, 04-09-25, Final Project*/
using System.Diagnostics;

Console.Clear();

/*Debug Tests*/

//ValidateUsername tests
Debug.Assert(ValidateUsername("benji stansfield") == false, "User shouldn't be allowed to use a space in username");
Debug.Assert(ValidateUsername("14b") == false, "username should be over 4 characters");
Debug.Assert(ValidateUsername("bms17") == true);
Debug.Assert(ValidateUsername("bstansfield123456789") == true);
Debug.Assert(ValidateUsername("bstansfield") == false, "username should already be found");

//ValidatePin tests
Debug.Assert(ValidatePin("23") == false, "Password should be 4 numbers");
Debug.Assert(ValidatePin("1234") == true);
Debug.Assert(ValidatePin("12345") == false, "Password should be 4 numbers");
Debug.Assert(ValidatePin("g3*4") == false, "Pin cannot contain characters or letters");

//ValidateAllotment tests
Debug.Assert(ValidateAllotment("123456789.12") == true);
Debug.Assert(ValidateAllotment("-5") == false, "Cannot propose a negative allotment");
Debug.Assert(ValidateAllotment("2j0") == false, "Cannot have letters in proposal");

Console.Clear();

int attempts = 3; //tracks login attempts
string usernameInput = "";
string pinInput = "";
bool loggedIn = false; //used to log in a user
bool userFound;
bool usernameValidated = false; //used to validate a username while creating account
bool pinValidated = false; //used to validate a pin while creating account
string categoryName = ""; //lists the name of the category the user creates
decimal categoryAllotment; //amount allowed to spend to fit within budget
decimal spentInCategory; //amount spent in a specific category
decimal paycheckAmount;

Console.WriteLine(@"------------------------
 WELCOME TO YOUR BUDGET
------------------------
");
Console.WriteLine(@"1 - Sign In
2 - Create Account");
Console.Write("Please select an option: ");

int userChoice = Convert.ToInt32(Console.ReadLine());

Console.Clear();

/*Code for the option to sign in or create an account*/
switch (userChoice)
{
    case 1: //choice for them to log in

        string[] usersLines = File.ReadAllLines("users.txt");

        while (!loggedIn && attempts > 0)
        {   
            userFound = false;

            Console.Write("Username: ");
            usernameInput = Console.ReadLine();
            Console.Write("Pin #: ");
            pinInput = Console.ReadLine();

            foreach(string line in usersLines)
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

    case 2: //choice for them to create account

        Console.WriteLine("Please create a username (between 5-20 characters long, no spaces)");

        do
        {
            Console.Write("Username: ");
            usernameInput = Console.ReadLine();
            if (ValidateUsername(usernameInput))
                usernameValidated = true;

        } while(!usernameValidated);

        do
        {
            Console.Write("Pin (4 digits): ");
            pinInput = Console.ReadLine();
            if (ValidatePin(pinInput))
                pinValidated = true;

        } while(!pinValidated);

        File.AppendAllText("users.txt", $"{usernameInput},{pinInput}\n");

        loggedIn = true;

        break;
        
    default:

        Console.WriteLine("Please select an option above.");
        break;
}


/*Method makes sure the username input meets the qualifications*/
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

    /*Check to see if the username already exists*/
    string[] lines = File.ReadAllLines("users.txt");
    foreach (string line in lines)
    {
        string[] parts = line.Split(',');
        if (parts[0] == input)
        {
            Console.WriteLine("Username already exists.\n");
            return false;
        }
    }

    
    return true;
}

/*Method will validate the pin to fit in the parameters*/
static bool ValidatePin(string input)
{   
    /*Checks to see if the pin input contains letters*/
    try
    {
        int pinTry = Convert.ToInt32(input);
    }
    catch (FormatException)
    {
        Console.WriteLine("Pin must only contain numbers.\n");
        return false;
    }

    if (input.Length != 4)
    {
        Console.WriteLine("Pin must be 4 characters long.\n");
        return false;
    }
    
    return true;
}


/*Inside the program-menu select*/
while (loggedIn)
{
    Console.WriteLine(@"
    1 - View Current Budget
    2 - Input Deposit
    3 - Input Purchase
    4 - Create New Budget
    5 - Exit Program
    ");
    Console.WriteLine("What would you like to do?: ");
    int menuSelection = Convert.ToInt32(Console.ReadLine());

    /*Code for them to choose one of the options above*/
    switch (menuSelection)
    {
        case 1:
    
            string[] lines = File.ReadAllLines($"{usernameInput}.txt");

            foreach (string line in lines) //prints the contents of the file
            {
                Console.WriteLine(line);
            }

            break;

        case 4:
                
            List<(string category, decimal allotment, decimal spent)> categoryValues = new List<(string category, decimal allotment, Decimal spent)>();
            Console.Write("How much are your paychecks?: ");
            string paycheckString = Console.ReadLine();
            if (ValidateAllotment(paycheckString))
            {
                paycheckAmount = Convert.ToDecimal(paycheckString);
            }
            else {continue;}

            while (true)
            {
                Console.Write("Please create a name for your category: ");
                categoryName = Console.ReadLine();

                Console.Write($"How much would you like to spend per paycheck on {categoryName}?: ");
                string proposedAllotment = Console.ReadLine();
                if (ValidateAllotment(proposedAllotment))
                {
                    categoryAllotment = Convert.ToDecimal(proposedAllotment);
                    categoryValues.Add((categoryName, categoryAllotment, 0));
                    foreach (var item in categoryValues)
                    {
                        File.WriteAllText($"{usernameInput}.txt", $"{item.category},{item.allotment},{item.spent}\n");
                    }

                    Console.WriteLine($"\n{categoryName} category created with a ${categoryAllotment} allotment.");
                    Console.Write("Please press 1 to create another category. Otherwise, press any key to return to menu: ");

                    string choice = Console.ReadLine();
                    if (choice != "1")
                        break;
                }
                else{continue;}
            }

            break;

        case 5:

            Console.WriteLine("Thank you!");
            loggedIn = false;
            break;

        default:

            Console.WriteLine("Please pick a valid option.\n");
            break;
    }
}

/*Method makes sure that the proposed allotment for each category is a number*/
static bool ValidateAllotment(string proposedAllotment)
{
    decimal allotment = 0;

    try
    {
        allotment = Convert.ToDecimal(proposedAllotment);
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid number.\n");
        return false;
    }
    catch (OverflowException)
    {
        Console.WriteLine("I'm sorry, that number is too big.\n");
        return false;
    }
    catch (Exception)
    {
        Console.WriteLine("Please try again.\n");
        return false;
    }

    if (allotment < 0)
    {
        Console.WriteLine("Number must be a positive number.");
        return false;
    }

    return true;
}
