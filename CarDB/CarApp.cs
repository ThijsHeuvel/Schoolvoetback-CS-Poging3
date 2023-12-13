using CarDB;
using CarDB.Data;
using CarDB.Model;
using System.Runtime.CompilerServices;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleCrud
{
    internal class CarApp
    {
        UserContext userContext;
        public User? sessionUser = null;

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
            switch (userInput)
            {
                case "1":
                    // Register
                    Register();
                    break;
                case "2":
                    // Login
                    Login();
                    break;
                case "3":
                    // Show all
                    // ShowAll();
                    break;
                case "admin":
                    if (sessionUser is not null && sessionUser.IsAdmin)
                    {
                        Console.WriteLine("WELKOM ADMIN TEST!");
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
            string username = Helpers.Ask("Kies uw gebruikersnaam:");
            bool isUnique = true;

            foreach (User item in userContext.Users)
            {
                if (item.Username == username)
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
            {
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
                sessionUser = user;

                // Notify user
                Console.WriteLine($"U bent nu ingelogd als {sessionUser.Username}");

                // Save registered user to database
                userContext.Users.Add(user);
                userContext.SaveChanges();
            }
        }

        private void Login()
        {
            Console.Clear();
            string username = Helpers.Ask("Vul uw gebruikersnaam in:");
            bool foundUsername = false;

            foreach (User item in userContext.Users)
            {
                if (item.Username == username)
                {
                    foundUsername = true;
                    string password = Helpers.Ask("Vul uw wachtwoord in:");

                    // Validate password
                    if (Helpers.VerifyPassword(password, item.Password))
                    {
                        sessionUser = item;
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

        private string ShowMenu()
        {
            Console.Clear();
            if (sessionUser is not null)
            {
                if (!sessionUser.IsAdmin)
                {
                    Console.WriteLine($"Welkom, {sessionUser.Username}!\n${sessionUser.Dollars}\n========================================\n\n");
                }
                else
                {
                    Console.WriteLine($"Welkom, {sessionUser.Username}! [ADMIN]\n${sessionUser.Dollars}\n========================================\n\n");
                }
            }
            else
            {
                Console.WriteLine("Welkom bij de Gamble App!\n\n========================================\n\n");
            }

            Console.WriteLine("1. Registreren");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Toon alle wedstrijden");
            if (sessionUser is not null && sessionUser.IsAdmin)
            {
                Console.WriteLine("Admin. Beheerpagina");
            }
            Console.WriteLine("X. Verlaten");

            return Helpers.Ask("Maak uw keuze en druk op <ENTER>.");
        }
    }
}