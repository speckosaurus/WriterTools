using System;

namespace TimelineAssistant.Models
{
    public class Character
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public bool IsDeceased => DateOfDeath.HasValue;
    }
}
