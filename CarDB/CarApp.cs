using CarDB;
using CarDB.Data;
using CarDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
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

            // Test data
            if (userContext.Tournaments.Count() == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Tournament tournament = new Tournament($"Tournament {i + 1}", "Description", "Team1", "Team2", DateTime.Now, DateTime.Now.AddHours(1));
                    userContext.Tournaments.Add(tournament);
                }
                userContext.SaveChanges();
            }
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
            Styling.AddHeader("Registreren");

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
                    Styling.ShowError("\nGebruikersnaam is al in gebruik!\n");
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
                    Styling.ShowError("\nWachtwoorden komen niet overeen!\n");
                }
            }

            // Setup User class
            string hashedPassword = Helpers.HashPassword(password);
            User user = new User(username, hashedPassword, 50, false);
            SetSessionUser(user);

            // Notify user
            Styling.ShowInfo($"\nU bent nu ingelogd als {user.Username}!\n");

            // Save registered user to database
            userContext.Users.Add(user);
            userContext.SaveChanges();
        }

        private void Login()
        {
            Console.Clear();
            Styling.AddHeader("Inloggen");

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
                        Styling.ShowInfo($"\nU bent nu ingelogd als {item.Username}!\n");
                    }
                    else
                    {
                        Styling.ShowError("\nWachtwoord is onjuist!\n");
                    }
                    break;
                }
            }

            if (!foundUsername)
            {
                Styling.ShowError("\nGebruikersnaam niet gevonden!\n");
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
                int id = tournament.Id;
                string name = tournament.Name;
                string team1 = tournament.Team1;
                string team2 = tournament.Team2;
                //string location = tournament.Location;
                string start_time = tournament.Start_time.ToString("HH:mm");
                string end_time = tournament.End_time.ToString("HH:mm");

                Styling.AddListOption($"{id} | {name} | {team1} - {team2} | LOCATION | {start_time} - {end_time}");
            }
            Styling.AddListOption("X | Ga terug");

            Styling.AddLine();

            // Await user input
            string userInput = Helpers.Ask("\nMaak uw keuze en druk op <ENTER>.");
            if (int.TryParse(userInput, out int result))
            {
                Tournament? selectedTournament = null;
                foreach (Tournament tournament in tournaments)
                {
                    if (tournament.Id == result)
                    {
                        selectedTournament = tournament;
                        break;
                    }
                }
                if (selectedTournament is not null)
                {
                    if (isLoggedIn)
                    {
                        ShowTournamentMenu(selectedTournament);
                    }
                    else
                    {
                        Styling.ShowError("\nU moet ingelogd zijn om te kunnen gokken!\n");
                    }
                }
            }
        }

        private void DisplayTournamentInfo(Tournament tournament)
        {
            Styling.ShowInfo($"Wedstrijd id: {tournament.Id}");
            Styling.ShowInfo(tournament.Name);
            Styling.ShowInfo($"{tournament.Team1} - {tournament.Team2}");
            Styling.ShowInfo("LOCATION");

            string startTime = tournament.Start_time.ToString("HH:mm");
            string endTime = tournament.End_time.ToString("HH:mm");
            Styling.ShowInfo($"{startTime} - {endTime}");
        }

        private void ShowTournamentMenu(Tournament tournament)
        {
            Console.Clear();
            DisplayTournamentInfo(tournament);
            
            Styling.SkipLine();

            Styling.AddLine();
            Styling.AddListOption($"1 | {tournament.Team1}");
            Styling.AddListOption($"2 | {tournament.Team2}");
            Styling.AddListOption("X | Ga terug");
            Styling.AddLine();

            Console.WriteLine("\nOp welk team wilt u gokken?\n");

            // Check user input
            string userInput = Helpers.Ask("Maak uw keuze en druk op <ENTER>.");
            if (userInput == "1" || userInput == "2")
            {
                int selectedTeamIndex = int.Parse(userInput);
                ShowSelectedTeamMenu(tournament, selectedTeamIndex);
            }
            else
            {
                // Go back one step
                ShowTournaments();
            }
        }

        private void ShowSelectedTeamMenu(Tournament tournament, int teamIndex)
        {
            Console.Clear();
            DisplayTournamentInfo(tournament);

            string teamName = (teamIndex == 1) ? tournament.Team1 : tournament.Team2;
            Console.WriteLine($"\n{teamName}");

            Styling.SkipLine();

            Styling.AddLine();
            Styling.AddListOption("1 | Speler");
            Styling.AddListOption("2 | Eindscore");
            Styling.AddListOption("X | Ga terug");
            Styling.AddLine();

            Console.WriteLine("\nWaarop wilt u gokken?\n");

            string userInput = Helpers.Ask("Maak uw keuze en druk op <ENTER>.");
            switch (userInput)
            {
                case "1":
                    ShowPlayerMenu(tournament, teamIndex);
                    break;
                case "2":
                    ShowScoreMenu(tournament, teamIndex);
                    break;
                default:
                    // Go back one step
                    ShowTournamentMenu(tournament);
                    break;
            }
        }

        private void ShowPlayerMenu(Tournament tournament, int teamIndex)
        {
            Console.Clear();
            DisplayTournamentInfo(tournament);

            string teamName = (teamIndex == 1) ? tournament.Team1 : tournament.Team2;
            Console.WriteLine($"\n{teamName}");

            Styling.SkipLine();

            Styling.AddLine();
            Styling.AddLine();
        }

        private void ShowScoreMenu(Tournament tournament, int teamIndex)
        {
            Console.Clear();
            DisplayTournamentInfo(tournament);

            string teamName = (teamIndex == 1) ? tournament.Team1 : tournament.Team2;
            Console.WriteLine($"\n{teamName}");

            Styling.SkipLine();

            Styling.AddLine();

            Styling.SkipLine();

            int userScoreInput;
            while (true)
            {
                userScoreInput = Helpers.AskForInt("Welke score denkt u dat het team gaat halen?");
                if (userScoreInput >= 0)
                {
                    break;
                }
                else
                {
                    Styling.ShowError("\nScore moet positief zijn!\n");
                }
            }
        }

        private void ShowAdminPage()
        {
            Console.Clear();
            Styling.AddHeader("Beheerpagina");

            Styling.AddLine();
            Styling.AddListOption("1 | Matches Tonen");
            Styling.AddListOption("2 | Resultaten Matches");
            Styling.AddListOption("X | Ga terug");
            Styling.AddLine();

            string userInput = Helpers.Ask("\nMaak uw keuze en druk op <ENTER>.");
            switch (userInput)
            {
                case "1":
                    Admin.ShowMatches();
                    break;
                case "2":
                    Admin.ShowMatchResults();
                    break;
                default:
                    ShowMenu();
                    break;
            }
        }

        private string ShowMenu()
        {
            // Menu user display
            Console.Clear();
            if (isLoggedIn)
            {
                string accountExtension = isAdmin ? "[Admin]" : "";
                Console.WriteLine($"Welkom, {sessionUser.Username}! {accountExtension}");
                Styling.ShowBalance(sessionUser);
            }
            else
            {
                Console.WriteLine("Welkom bij de Gamble App!");
            }
            Styling.SkipLine();

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
                Styling.AddListOption($"{i + 1} | {menuOptions[i]}");
            }
            Styling.AddListOption("X | Verlaten");
            Styling.AddLine();

            return Helpers.Ask("\nMaak uw keuze en druk op <ENTER>.");
        }
    }
}