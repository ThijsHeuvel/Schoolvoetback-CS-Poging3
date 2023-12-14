using CarDB;
using CarDB.Data;
using CarDB.Model;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleCrud
{
    internal class CarApp
    {
        UserContext userContext;
        public User? sessionUser = null;
        public bool isLoggedIn = false;
        public bool isAdmin = false;

        public CarApp()
        {
            userContext = new UserContext();
        }

        internal void Run()
        {
            string userInput = "";

            while(userInput.ToLower() != "x")
            {
                userInput = ShowMenu();
                handleUserInput(userInput);
            }
        }

        private void handleUserInput(string userInput)
        {
            // WHEN *NOT* LOGGED IN
            if (!isLoggedIn)
            {
                switch (userInput)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        // ShowTournaments();
                        break;
                    case "4":
                        if (isAdmin)
                        {
                            // ShowAdmin();
                        }
                        else
                        {
                            Console.WriteLine("YOU ARE NOT ADMIN!");
                        }
                        break;
                    default:
                        // Invalid input
                        Console.WriteLine("Incorrect choice...");
                        break;
                }
            }
            // WHEN LOGGED IN
            else
            {
                switch (userInput)
                {
                    case "1":
                        LogOut();
                        break;
                    case "2":
                        // ShowTournaments();
                        Console.WriteLine("SHOW TOURNAMENTS");
                        break;
                    case "3":
                        if (isAdmin)
                        {
                            // ShowAdmin();
                            Console.WriteLine("SHOW ADMIN");
                        }
                        else
                        {
                            Console.WriteLine("YOU ARE NOT ADMIN!");
                        }
                        break;
                    default:
                        // Invalid input
                        Console.WriteLine("Incorrect choice...");
                        break;
                }
            }
            Helpers.Pause();
        }

        /*private void Delete()
        {
            Car car = SelectCar();
            dataContext.Cars.Remove(car);

            dataContext.SaveChanges();
            Console.WriteLine("Car Deleted.");

        }

        private void AddNew()
        {

            string brand = Helpers.Ask("Brand car:");
            string model = Helpers.Ask("Model car:");
            int year = Helpers.AskForInt("Year car:");
            string color = Helpers.Ask("Color car:");
            Car car = new Car(brand, model, year, color);
            dataContext.Cars.Add(car);
            dataContext.SaveChanges();

            Console.WriteLine("Car Added.");
        }

        private Car SelectCar()
        {
            ShowAll();
            Car? selectedCar = null;

            while (selectedCar == null)
            {
                int id = Helpers.AskForInt("Select ID Car.");
                selectedCar = dataContext.Cars.Find(id);
            }

            return selectedCar;


        }

        private void ShowAll()
        {
                               
            Console.WriteLine("================ All Cars ================");
            List<Car> cars = dataContext.Cars.ToList();

            foreach (Car car in dataContext.Cars)
            {
                Console.WriteLine(car);
            }
            Console.WriteLine("=============================================");
        }

        private void UpdateCar()
        {
            Car car = SelectCar();
            car.Brand = Helpers.Ask("New Brand car:");
            car.Model = Helpers.Ask("New Model car:");
            car.Year = Helpers.AskForInt("New Year car:");
            car.Color = Helpers.Ask("New Color car:");
            dataContext.Cars.Update(car);
            dataContext.SaveChanges();

        }*/

        // Methods used for Gamble App
        private void Register()
        {
            Console.Clear();
            string? username = null;
            while (true)
            {
                username = Helpers.Ask("Kies uw gebruikersnaam:");
                bool isUnique = true;

                foreach (User item in userContext.Users)
                {
                    if (item.Username.ToLower() == username.ToLower())
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Er bestaat al een account met deze gebruikersnaam");
                }
            }

            Console.Clear();
            string password = Helpers.Ask("Vul uw wachtwoord in:");
            string? repeatedPassword = null;

            while (true)
            {
                repeatedPassword = Helpers.Ask("Herhaal uw wachtwoord:");
                if (repeatedPassword == password)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Wachtwoorden komen niet overeen!");
                }
            }

            // Setup User class
            string hashedPassword = Helpers.HashPassword(password);
            User user = new User(username, hashedPassword, 50, false);
            SetSessionUser(user);

            // Notify user
            Console.WriteLine($"U bent nu ingelogd als {sessionUser.Username}");

            // Save registered user to database
            userContext.Users.Add(user);
            userContext.SaveChanges();
        }

        private void Login()
        {
            Console.Clear();
            string username = Helpers.Ask("Vul uw gebruikersnaam in:");
            bool foundUsername = false;

            foreach (User item in userContext.Users)
            {
                if (item.Username.ToLower() == username.ToLower())
                {
                    foundUsername = true;
                    string password = Helpers.Ask("Vul uw wachtwoord in:");

                    // Validate password
                    if (Helpers.VerifyPassword(password, item.Password))
                    {
                        SetSessionUser(item);
                        Console.WriteLine($"U bent nu ingelogd als {sessionUser.Username}!");
                    }
                    else {
                        Console.WriteLine("Incorrect wachtwoord!");
                    }
                    break;
                }
            }

            if (!foundUsername)
            {
                Console.WriteLine("Geen account gevonden met deze gebruikersnaam!");
            }
        }

        private void LogOut()
        {
            sessionUser = null;
            isLoggedIn = false;
            isAdmin = false;

            Console.Clear();
            Console.WriteLine("U bent nu uitgelogd.");
        }

        private void SetSessionUser(User user)
        {
            sessionUser = user;
            isLoggedIn = true;
            isAdmin = sessionUser.IsAdmin;
        }

        private string ShowMenu()
        {
            // Menu user display
            Console.Clear();
            if (isLoggedIn)
            {

                Console.WriteLine($"Welkom, {sessionUser.Username}! [ADMIN]");
                Console.WriteLine($"{sessionUser.Dollars}");
            }
            else
            {
                Console.WriteLine("Welkom bij de Gamble App!");
            }
            Console.WriteLine("\n========================================");

            // Populate menu options
            List<string> menuOptions = new List<string>();
            if (!isLoggedIn)
            {
                menuOptions.Add("Registreren");
                menuOptions.Add("Login");
            }
            else
            {
                menuOptions.Add("Uitloggen");
            }
            menuOptions.Add("Toon geplande wedstrijden");
            if (isAdmin)
            {
                menuOptions.Add("Beheerpagina [ADMIN-ONLY]");
            }

            // Display menu options
            for (int i = 0; i < menuOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1} | {menuOptions[i]}");
            }
            Console.WriteLine("X | Verlaten");

            // Display divider
            Console.WriteLine("========================================\n");

            return Helpers.Ask("Maak uw keuze en druk op <ENTER>.");
        }
    }
}