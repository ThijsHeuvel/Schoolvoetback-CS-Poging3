using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Model
{
    internal class Bet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TournamentId { get; set; }
        public int Amount { get; set; }
        public int BetTeamId { get; set; }
        public int? BetPlayerId { get; set; }
        public int? BetScoreAmount { get; set; }
        public bool PaidOut { get; set; }

        public Bet(int userId, int tournamentId, int amount, int betTeamId, int? betPlayerId, int? betScoreAmount)
        {
            UserId = userId;
            TournamentId = tournamentId;
            Amount = amount;
            BetTeamId = betTeamId;
            BetPlayerId = betPlayerId;
            BetScoreAmount = betScoreAmount;
            PaidOut = false;
        }

        public Bet(int id, int userId, int tournamentId, int amount, int betTeamId, int? betPlayerId, int? betScoreAmount)
        {
            Id = id;
            UserId = userId;
            TournamentId = tournamentId;
            Amount = amount;
            BetTeamId = betTeamId;
            BetPlayerId = betPlayerId;
            BetScoreAmount = betScoreAmount;
            PaidOut = false;
        }

        public override string ToString()
        {
            return $"{Id} | {UserId} | {TournamentId} | {Amount} | {BetTeamId} | {BetPlayerId} | {BetScoreAmount} | {PaidOut.ToString()}";
        }
    }
}
