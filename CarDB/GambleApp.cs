using CarDB;
using CarDB.Data;
using CarDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleCrud
{
    internal class GambleApp
    {
        UserContext userContext;
        public User? sessionUser = null;
        public bool isLoggedIn = false;
        public bool isAdmin = false;
        TournamentApi tournamentApi = new TournamentApi();

        public GambleApp()
        {
            userContext = new UserContext();

            // TEST DATA //
            if (userContext.Users.Find(-1) == null)
            {
                userContext.Users.Add(new User(-1, "admin", Helpers.HashPassword("admin"), 6969, true));
                userContext.SaveChanges();
            }
            if (userContext.Users.Find(1) == null)
            {
                userContext.Users.Add(new User(1, "user", Helpers.HashPassword("user"), 50, false));
                userContext.SaveChanges();
            }

            if (userContext.Users.Find(1) is not null)
            {
                userContext.Users.Find(1).Dollars = 50;
            }

            if (userContext.Teams.Count() == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Team team = new Team($"Team {i + 1}");
                    userContext.Teams.Add(team);
                }

                userContext.SaveChanges();
            }

            if (userContext.Tournaments.Count() == 0)
            {
                for (int i = 1; i < 101; i++) // Add 100 tournaments (1-100)
                {
                    Team team1 = userContext.Teams.Find(new Random().Next(1, 11));
                    Team team2 = userContext.Teams.Find(new Random().Next(1, 11));
                    Debug.WriteLine($"{team1.Id} - {team2.Id}");
                    Tournament tournament = new Tournament($"Tournament {i + 1}", "Description", team1.Id, team1.Name, team2.Id, team2.Name, DateTime.Now, DateTime.Now.AddHours(1));
                    userContext.Tournaments.Add(tournament);
                }
                userContext.SaveChanges();
            }

            if (userContext.Players.Count() == 0)
            {
                Random random = new Random();
                
                for (int teamId = 1; teamId < userContext.Teams.Count() + 1; teamId++) // Iterate through teams (1-X)
                {
                    for (int i2 = 1; i2 < 6; i2++) // Add 5 players to each team (1-5)
                    {
                        TeamPlayer player = new TeamPlayer($"Player {i2}", random.Next(1, 11));
                        player.TeamId = teamId;
                        userContext.Players.Add(player);
                    }
                }

                userContext.SaveChanges();
            }

            if (userContext.Bets.Count() == 0)
            {
                Random random = new Random();

                for (int i = 0; i < 10; i++)
                {
                    Tournament randomTournament = userContext.Tournaments.Find(random.Next(1, 11));
                    Bet bet = new Bet(1, random.Next(1, 11), random.Next(25, 100), randomTournament.Id, random.Next(1, 11), null);
                    userContext.Bets.Add(bet);
                }

                userContext.SaveChanges();
            }
            // END OF TEST DATA //
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
                            // Toon mijn weddenschappen (geen admin)
                            ShowUserBets();
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
                string start_time = tournament.Start_time.ToString("HH:mm");
                string end_time = tournament.End_time.ToString("HH:mm");

                Styling.AddListOption($"{id} | {name} | {team1} - {team2} | {start_time} - {end_time}");
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
            Styling.AddListOption($"{tournament.Team1Id} | {tournament.Team1}");
            Styling.AddListOption($"{tournament.Team2Id} | {tournament.Team2}");
            Styling.AddListOption("X | Ga terug");
            Styling.AddLine();

            Console.WriteLine("\nOp welk team wilt u gokken?\n");

            // Check user input
            string selectedTeamId;
            while (true)
            {
                selectedTeamId = Helpers.Ask("Maak uw keuze en druk op <ENTER>.");
                if (selectedTeamId == tournament.Team1Id.ToString() || selectedTeamId == tournament.Team2Id.ToString())
                {
                    ShowSelectedTeamMenu(tournament, int.Parse(selectedTeamId));
                }
                else if (selectedTeamId.ToLower() == "x")
                {
                    break;
                }
            }
        }

        private void ShowSelectedTeamMenu(Tournament tournament, int teamId)
        {
            Console.Clear();
            DisplayTournamentInfo(tournament);

            Team? team = null;
            foreach(Team item in userContext.Teams)
            {
                if (item.Id == teamId)
                {
                    team = item;
                    break;
                }
            }

            if (team is not null)
            {
                Console.WriteLine($"\n{team.Name}");

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
                        ShowPlayerMenu(tournament, team);
                        break;
                    case "2":
                        ShowScoreMenu(tournament, team);
                        break;
                    default:
                        // Go back one step
                        ShowTournamentMenu(tournament);
                        break;
                }
            }
        }

        private void ShowPlayerMenu(Tournament tournament, Team team)
        {
            Console.Clear();
            DisplayTournamentInfo(tournament);

            Console.WriteLine($"\n{team.Name}");

            Styling.SkipLine();

            Styling.AddLine();

            List<TeamPlayer> playerList = new List<TeamPlayer>();
            foreach (TeamPlayer player in userContext.Players)
            {
                if (player.TeamId == team.Id)
                {
                    playerList.Add(player);
                    Styling.AddListOption($"{player.Id} | {player.Name}");
                }
            }

            Styling.AddLine();

            Styling.SkipLine();

            int userPlayerInput;
            int userAmountInput;
            while (true)
            {
                userPlayerInput = Helpers.AskForInt("Welke speler denkt u dat het hoogst gaat scoren?");
                if (userPlayerInput >= 0)
                {
                    bool foundPlayer = false;
                    foreach (TeamPlayer player in playerList)
                    {
                        if (player.Id == userPlayerInput)
                        {
                            foundPlayer = true;
                            break;
                        }
                    }

                    if (foundPlayer)
                    {
                        break;
                    }
                    else
                    {
                        Styling.ShowError("Speler niet gevonden!\n");
                    }
                }
                else
                {
                    Styling.ShowError("Spelernummer moet positief zijn!\n");
                }
            }
            while (true)
            {
                userAmountInput = Helpers.AskForInt("Hoeveel wilt u inzetten?");
                if (userAmountInput >= 1)
                {
                    if (userAmountInput <= sessionUser.Dollars)
                    {
                        break;
                    }
                    else
                    {
                        Styling.ShowError("U heeft niet genoeg balans!\n");
                    }
                }
                else
                {
                    Styling.ShowError("Bedrag moet positief- en minimaal 1 zijn!\n");
                }
            }

            // Haal geld af van de balans
            sessionUser.Dollars -= Math.Clamp(userAmountInput, 0, sessionUser.Dollars);

            // Plaats een weddenschap waarbij score van team wordt ingevuld en NIET de speler
            userContext.Bets.Add(new Bet(sessionUser.Id, tournament.Id, userAmountInput, team.Id, userPlayerInput, null));
            userContext.SaveChanges();
        }

        private void ShowScoreMenu(Tournament tournament, Team team)
        {
            Console.Clear();
            DisplayTournamentInfo(tournament);

            Console.WriteLine($"\n{team.Name}");

            Styling.SkipLine();

            Styling.AddLine();

            Styling.SkipLine();

            int userScoreInput;
            int userAmountInput;
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
            while (true)
            {
                userAmountInput = Helpers.AskForInt("Hoeveel wilt u inzetten?");
                if (userAmountInput >= 0)
                {
                    if (userAmountInput <= sessionUser.Dollars)
                    {
                        break;
                    }
                    else
                    {
                        Styling.ShowError("U heeft niet genoeg balans!\n");
                    }
                }
                else
                {
                    Styling.ShowError("Bedrag moet positief- en minimaal 1 zijn!\n");
                }
            }

            // Haal geld af van de balans
            sessionUser.Dollars -= Math.Clamp(userAmountInput, 0, sessionUser.Dollars);

            // Plaats een weddenschap waarbij score van team wordt ingevuld en NIET de speler
            userContext.Bets.Add(new Bet(sessionUser.Id, tournament.Id, userAmountInput, team.Id, null, userScoreInput));
            userContext.SaveChanges();
        }

        private void ShowAdminPage()
        {
            Console.Clear();
            Styling.AddHeader("Beheerpagina");

            Styling.AddLine();
            Styling.AddListOption("1 | Wedstrijden tonen");
            Styling.AddListOption("2 | Resultaten tonen");
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
            }
        }

        private void ShowUserBets()
        {
            Console.Clear();
            Styling.AddHeader("Mijn weddenschappen");

            Styling.AddLine();

            if (sessionUser is not null)
            {
                foreach (Bet bet in userContext.Bets)
                {
                    if (bet.UserId == sessionUser.Id)
                    {
                        using(UserContext userContext = new UserContext()) // Dit is blijkbaar nodig om het te laten werken, lol
                        {
                            foreach (Tournament tournament in userContext.Tournaments)
                            {
                                if (tournament.Id == bet.TournamentId)
                                {
                                    Styling.AddListOption($"{tournament.Name} | {tournament.Team1} - {tournament.Team2} | ${bet.Amount} ingezet");
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            Styling.AddLine();
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
                if (!isAdmin)
                {
                    menuOptions.Add("Toon mijn weddenschappen");
                }
                else
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