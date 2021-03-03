using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TimelineAssistant.Data;
using TimelineAssistant.Extensions;
using TimelineAssistant.Models;

namespace TimelineAssistant
{
    public partial class MainView : Form
    {
        private List<Event> events;
        private List<Character> characters;

        public MainView()
        {
            InitializeComponent();
            CenterToScreen();
            LoadEventsFromExcel();
            SetEventsView();
            LoadCharacters();
        }

        private void LoadEventsFromExcel()
        {
            try
            {
                events = new List<Event>();

                using (var package = new ExcelPackage(new FileInfo("Timeline.xlsx")))
                {
                    var workSheet = package.Workbook.Worksheets.First();

                    var start = workSheet.Dimension.Start;
                    var end = workSheet.Dimension.End;

                    for (int row = start.Row+1; row <= end.Row; row++)
                    {
                        try
                        {
                            events.Add(new Event
                            {
                                Date = workSheet.Cells[row, 1].Text.SanitiseDate(),
                                Type = (EventType)Enum.Parse(typeof(EventType), workSheet.Cells[row, 2].Text),
                                Description = workSheet.Cells[row, 3].Text,
                                DisplayYearOnly = workSheet.Cells[row, 1].Text.DisplayYearOnly()
                            });
                        }
                        catch (Exception ex)
                        {
                            // Skip
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
                Environment.Exit(0);
            }
        }

        private void SetEventsView()
        {
            try
            {
                eventsGridView.ClearSelection();
                eventsGridView.Rows.Clear();

                foreach (var eventItem in events)
                {
                    eventsGridView.Rows.Add(eventItem.Date.FormatDate(eventItem.DisplayYearOnly), eventItem.Type, eventItem.Description);
                }

                eventsGridView.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                eventsGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void LoadCharacters()
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
                ShowErrorMessage(ex.Message);
            }
        }

        private void GetCharacterAges(DateTime date)
        {
            try
            {
                // Clear grid
                agesGridView.Rows.Clear();

                foreach (var character in characters)
                {
                    int age = date.Year - character.DateOfBirth.Year;

                    // Handle months discrepancy
                    if (character.DateOfBirth > date.AddYears(-age))
                        age--;

                    if (age >= 0)
                    {
                        int rowIndex = agesGridView.Rows.Add(character.Name, age);

                        if (character.IsDeceased && character.DateOfDeath <= date)
                        {
                            agesGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Gray;
                        }
                    }
                }

                // Remove highlighted selection
                agesGridView.ClearSelection();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = searchYear.Text.SanitiseDate();
                GetCharacterAges(date);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Oops",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void eventsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedYear = eventsGridView.SelectedRows[0].Cells["Date"].Value;

                DateTime date = selectedYear.ToString().SanitiseDate();
                GetCharacterAges(date);
            }
            catch (Exception ex)
            {
                // Skip
            }
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Timeline.xlsx");
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void reloadFileButton_Click(object sender, EventArgs e)
        {
            LoadEventsFromExcel();
            LoadCharacters();
            SetEventsView();
        }
    }
}
