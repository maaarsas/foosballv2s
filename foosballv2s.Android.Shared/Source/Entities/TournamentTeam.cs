using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Emgu.CV.Structure;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Events;
using foosballv2s.Droid.Shared.Source.Services.GameLogger;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(TournamentTeam))]
namespace foosballv2s.Droid.Shared.Source.Entities
{
    public class TournamentTeam
    {
        public int Id { get; set; }

        public Team Team { get; set; }
    }
}
