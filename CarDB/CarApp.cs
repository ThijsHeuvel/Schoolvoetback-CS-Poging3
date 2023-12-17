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
                        ShowTournaments();
                        break;
                    case "4":
                        if (isAdmin)
                        {
                            ShowAdminPage();
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
            else
            {
                switch (userInput)
                {
                    case "1":
                        LogOut();
                        break;
                    case "2":
                        ShowTournaments();
                        break;
                    case "3":
                        if (isAdmin)
                        {
                            ShowAdminPage();
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
            isAdmin = user.IsAdmin;
        }

        private void ShowTournaments()
        {
            // Load screen
            Console.Clear();
            Console.WriteLine("Aan het laden...");

            // Populate list with tournaments
            List<Tournament> tournaments = new List<Tournament>();
            foreach (Tournament tournament in userContext.Tournaments)
            {
                tournaments.Add(tournament);
            }

            // Display tournaments
            Console.Clear();
            Console.WriteLine("Actuele wedstrijden:\n");
            Styling.AddLine();

            foreach (Tournament tournament in tournaments)
            {
                Console.WriteLine($"{tournament.Id} | {tournament.Name} | {tournament.Team1} - {tournament.Team2} | LOCATION | {tournament.Start_time} - {tournament.End_time}");
            }

            Styling.AddLine();

            // Await user input
            int selectedTournamentIndex = Helpers.AskForInt("\nMaak uw keuze en druk op <ENTER>.");

            foreach (Tournament tournament in tournaments)
            {
                if (tournament.Id == selectedTournamentIndex)
                {
                    ShowTournamentMenu(tournament);
                    break;
                }
            }
        }

        private void DisplayTournamentInfo(Tournament tournament)
        {
            Console.WriteLine($"Westrijd {tournament.Id}");
            Console.WriteLine(tournament.Name);
            Console.WriteLine($"{tournament.Team1} - {tournament.Team2}");
            //Console.WriteLine(tournament.Location);
            Console.WriteLine($"{tournament.Start_time} - {tournament.End_time}");
        }

        private void ShowTournamentMenu(Tournament tournament)
        {
            Console.Clear();
            DisplayTournamentInfo(tournament);
            Styling.SkipLine();

            Styling.AddLine();
            Console.WriteLine($"1 | {tournament.Team1}");
            Console.WriteLine($"2 | {tournament.Team2}");
            Styling.AddLine();

            Console.WriteLine("\nOp welk team wilt u gokken?\n");

            int selectedTeamIndex = Helpers.AskForInt("Maak uw keuze en druk op <ENTER>.");
            string selectedTeamName = (selectedTeamIndex == 1) ? tournament.Team1 : tournament.Team2;
            ShowSelectedTeamMenu(tournament, selectedTeamName);
        }

        private void ShowSelectedTeamMenu(Tournament tournament, string teamName)
        {
            Console.Clear();
            DisplayTournamentInfo(tournament);

            Console.WriteLine($"\n{teamName}");

            Styling.AddLine();
            Console.WriteLine("1 | Speler");
            Console.WriteLine("2 | Eindscore");
            Console.WriteLine("X | Ga terug");
            Styling.AddLine();

            Console.WriteLine("\nWaarop wilt u gokken?\n");

            string userInput = Helpers.Ask("Maak uw keuze en druk op <ENTER>.");
        }

        private void ShowAdminPage()
        {
            Console.WriteLine("TODO : SHOW ADMIN PAGE");
        }

        private string ShowMenu()
        {
            // Menu user display
            Console.Clear();
            if (isLoggedIn)
            {
                string accountExtension = isAdmin ? "[Admin]" : "";
                Console.WriteLine($"Welkom, {sessionUser.Username}! {accountExtension}");
                Console.WriteLine($"Balans: ${sessionUser.Dollars}");
            }
            else
            {
                Console.WriteLine("Welkom bij de Gamble App!\n");
            }
            Styling.AddLine();

            // Display menu
            List<string> menuOptions = new List<string>();
            if (!isLoggedIn)
            {
                menuOptions.Add("Registreren");
                menuOptions.Add("Inloggen");
                menuOptions.Add("Toon wedstrijden");
            }
            else
            {
                menuOptions.Add("Uitloggen");
                menuOptions.Add("Toon wedstrijden");
                if (isAdmin)
                {
                    menuOptions.Add("Beheerpagina [Admin]");
                }
            }

            for (int i = 0; i < menuOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1} | {menuOptions[i]}");
            }
            Console.WriteLine("X | Verlaten");
            Styling.AddLine();

            return Helpers.Ask("\nMaak uw keuze en druk op <ENTER>.");
        }
    }
}