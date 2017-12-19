using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Emgu.CV.Structure;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Events;
using foosballv2s.Droid.Shared.Source.Services.GameLogger;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(Tournament))]
namespace foosballv2s.Droid.Shared.Source.Entities
{
    public class Tournament
    {
        public int Id { get; set; }

        public int CurrentStage { get; set; } = 1;

        public DateTime? StartTime { get; set; } = null;
        public DateTime? EndTime { get; set; } = null;

        public int NumberOfTeamsRequired { get; set; }
        public int NumberOfStages { get; set; }

        public bool IsEnoughTeams { get; set; }

        public ICollection<TournamentTeam> Teams { get; set; } = new Collection<TournamentTeam>();
        public ICollection<TournamentPair> Pairs { get; set; } = new Collection<TournamentPair>();
    }
}
