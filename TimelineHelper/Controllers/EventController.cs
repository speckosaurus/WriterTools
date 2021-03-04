using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimelineAssistant.Data;
using TimelineAssistant.Extensions;
using TimelineAssistant.Models;
using TimelineAssistant.Properties;

namespace TimelineAssistant.Controllers
{
    public class EventController
    {
        public List<Event> Events { get { return events; } }
        public List<Character> Characters { get { return characters; } }
        public List<CharacterAge> CharacterAges { get { return ages; } }

        private List<Event> events;
        private List<Character> characters;
        private List<CharacterAge> ages;

        public EventController()
        {
            events = new List<Event>();
            characters = new List<Character>();
            ages = new List<CharacterAge>();
        }

        public List<string> GetExcelFileNames()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(currentDirectory, "*.xlsx");

            List<string> fileNames = new List<string>();
            foreach (string file in files)
            {
                fileNames.Add(Path.GetFileName(file).RemoveExcelExtension());
            }

            return fileNames;
        }

        public void GetEventsFromExcel()
        {
            try
            {
                events = new List<Event>();

                using (var package = new ExcelPackage(new FileInfo(Settings.Default.ExcelFileName)))
                {
                    var workSheet = package.Workbook.Worksheets.First();

                    var start = workSheet.Dimension.Start;
                    var end = workSheet.Dimension.End;

                    for (int row = start.Row + 1; row <= end.Row; row++)
                    {
                        string dateCell = workSheet.Cells[row, 1].Text;
                        if (string.IsNullOrWhiteSpace(dateCell))
                        {
                            break;
                        }

                        events.Add(new Event
                        {
                            Date = dateCell.SanitiseDate(),
                            Type = (EventType)Enum.Parse(typeof(EventType), workSheet.Cells[row, 2].Text),
                            Description = workSheet.Cells[row, 3].Text,
                            DisplayYearOnly = workSheet.Cells[row, 1].Text.DisplayYearOnly()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error encountered when loading the Excel file:\n\n{ex.Message}");
            }
        }

        public void GetCharacters()
        {
            try
            {
                characters = new List<Character>();

                foreach (var eventItem in events.Where(x => x.Type == EventType.Birth))
                {
                    characters.Add(new Character
                    {
                        Name = eventItem.Description,
                        DateOfBirth = eventItem.Date
                    });
                }

                foreach (var character in characters)
                {
                    Event characterDeath = events.FirstOrDefault(x => x.Description == character.Name && x.Type == EventType.Death);
                    if (characterDeath != null)
                    {
                        character.DateOfDeath = characterDeath.Date;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error encountered when loading Characters:\n\n{ex.Message}");
            }
        }

        public void GetCharacterAges(DateTime date)
        {
            ages = new List<CharacterAge>();

            foreach (var character in characters)
            {
                int age = date.Year - character.DateOfBirth.Year;

                // Handle months discrepancy
                if (character.DateOfBirth > date.AddYears(-age))
                    age--;

                if (age >= 0)
                {
                    ages.Add(new CharacterAge
                    {
                        Name = character.Name,
                        Age = age,
                        IsDeceased = character.IsDeceased && character.DateOfDeath <= date
                    });
                }
            }
        }
    }
}
