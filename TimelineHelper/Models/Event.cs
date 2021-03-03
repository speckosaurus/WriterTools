using System;
using TimelineAssistant.Data;

namespace TimelineAssistant.Models
{
    public class Event
    {
        public DateTime Date { get; set; }
        public EventType Type { get; set; }
        public string Description { get; set; }
        public bool DisplayYearOnly { get; set; }
    }
}
