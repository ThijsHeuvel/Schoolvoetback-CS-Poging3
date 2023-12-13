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
            string username = "";

            while (true)
            {
                // Ask for username
                username = Helpers.Ask("Gebruikersnaam:");

                // Validate username input
                bool canContinue = true;
                foreach (User item in userContext.Users)
                {
                    if (item.Username == username)
                    {
                        Console.WriteLine("Deze gebruikersnaam is al in gebruik!");
                        canContinue = false;
                        break;
                    }
                }

                // Break if username is unique
                if (canContinue)
                {
                    break;
                }
            }

            string password = Helpers.Ask("Wachtwoord:");
            string repeatedPass = "";

            // Make user repeat password until it matches
            {
                bool firstTime = true;
                while (repeatedPass != password)
                {
                    if (firstTime)
                    {
                        firstTime = false;
                    }
                    else
                    {
                        Console.WriteLine("Wachtwoorden komen niet overeen!");
                    }
                    repeatedPass = Helpers.Ask("Wachtwoord herhalen:");
                }
            }

            // Hash the password
            password = Helpers.HashPassword(password);

            User user = new User(username, password, 50, false);
            sessionUser = user;
            userContext.Users.Add(user);
            userContext.SaveChanges();
        }

        private void Login()
        {
            Console.Clear();
            string username = "";

            // Check if username is in database
            while (true)
            {
                // Ask for username
                username = Helpers.Ask("Gebruikersnaam:");

                // Validate username input
                foreach (User item in userContext.Users)
                {
                    if (item.Username == username)
                    {
                        while (true)
                        {
                            string password = Helpers.Ask("Wachtwoord:");

                            // Hash and validate password
                            if (Helpers.VerifyPassword(password, item.Password))
                            {
                                sessionUser = item;
                                Console.WriteLine($"Ingelogd als: {item.Username}");
                                Console.ReadLine();
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private string ShowMenu()
        {
            Console.Clear();

            if (sessionUser is not null)
            {
                Console.WriteLine();
                Console.WriteLine($"Welkom, {sessionUser.Username}!\n\n========================================\n\n");
            }

            Console.WriteLine("1. Registreren");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Toon alle wedstrijden");
            if (sessionUser is not null and sessionUser.IsAdmin)
            {
                Console.WriteLine("Admin. Beheerpagina");
            }
            Console.WriteLine("X. Verlaten");

            return Helpers.Ask("Maak uw keuze en druk op <ENTER>.");
        }
    }
}