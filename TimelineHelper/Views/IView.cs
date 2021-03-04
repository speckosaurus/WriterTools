using System.Collections.Generic;
using TimelineAssistant.Controllers;
using TimelineAssistant.Models;

namespace TimelineAssistant.Views
{
    public interface IView
    {
        void LoadEvents(List<Event> events);
        void LoadCharacters(List<CharacterAge> characters);
        void LoadFileComboBox(List<string> fileNames);
        void ShowErrorMessage(string message);
    }
}
