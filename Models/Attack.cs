using IronDomeApi.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace IronDomeApi.Models
{
    public class Attack
    {
        public Guid? Id { get; set; }
        public string origin { get; set; }
        public string type { get; set; } = string.Empty;
        public string[] missilesTypes { get; set; } = new string[0];
        public AttackStatuses? attackStatus { get; set; }
        public DateTime? dateTime { get; set; } = null;
        public InterceptedStatuses interceptedStatus { get; set; }
        public DateTime StartedAt { get; set; }
        public int missileCount { get; set; }
        public string? MissilesDefined { get; set; }
    }
}

