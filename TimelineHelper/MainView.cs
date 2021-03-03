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
using TimelineAssistant.Properties;

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
            SetupDataGrids();
            LoadFileComboBox();
            LoadEventsFromExcel();
            SetEventsView();
            LoadCharacters();
        }

        private void SetupDataGrids()
        {
            agesGridView.Columns.Add("Name", "Name");
            agesGridView.Columns.Add("Age", "Age");

            eventsGridView.Columns.Add("Date", "Date");
            eventsGridView.Columns.Add("EventType", "Type");
            eventsGridView.Columns.Add("Description", "Event");
            eventsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            eventsGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void LoadFileComboBox()
        {
            try
            {
                fileComboBox.SelectedIndexChanged -= new EventHandler(fileComboBox_SelectedIndexChanged);

                var currentDirectory = Directory.GetCurrentDirectory();
                string[] files = Directory.GetFiles(currentDirectory, "*.xlsx");

                List<string> fileNames = new List<string>();
                foreach (string file in files)
                {
                    fileNames.Add(Path.GetFileName(file).RemoveExcelExtension());
                }

                fileComboBox.DataSource = fileNames;
                fileComboBox.SelectedItem = Settings.Default.ExcelFileName.RemoveExcelExtension();

                fileComboBox.SelectedIndexChanged += new EventHandler(fileComboBox_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                filePanel.Enabled = false;
            }
        }

        private void LoadEventsFromExcel()
        {
            try
            {
                events = new List<Event>();

                using (var package = new ExcelPackage(new FileInfo(Settings.Default.ExcelFileName)))
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
                ShowErrorMessage($"Error encountered when loading the Excel file:\n\n{ex.Message}");
                Environment.Exit(0);
            }
        }

        private void SetEventsView()
        {
            try
            {
                eventsGridView.ClearSelection();
                eventsGridView.Rows.Clear();

                EventType selectedType = (EventType)(filterComboBox.SelectedItem ?? 0);

                var filteredEvents = events;
                if (selectedType != EventType.Any)
                {
                    filteredEvents = filteredEvents.Where(x => x.Type == selectedType).ToList();
                }

                foreach (var eventItem in filteredEvents)
                {
                    eventsGridView.Rows.Add(eventItem.Date.FormatDate(eventItem.DisplayYearOnly), eventItem.Type, eventItem.Description);
                }

                eventsGridView.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                eventsGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                foreach (DataGridViewBand band in eventsGridView.Columns)
                {
                    band.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error encountered when updating the Events view:\n\n{ex.Message}");
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
                ShowErrorMessage($"Error encountered when loading Characters:\n\n{ex.Message}");
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
                ShowErrorMessage($"Error encountered when calculating Character ages:\n\n{ex.Message}");
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
                System.Diagnostics.Process.Start(Settings.Default.ExcelFileName);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error encountered when attempting to open Excel file:\n\n{ex.Message}");
            }
        }

        private void reloadFileButton_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void filterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetEventsView();
        }

        private void fileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFileName = fileComboBox.SelectedItem.ToString();

            if (selectedFileName == Settings.Default.ExcelFileName.RemoveExcelExtension())
            {
                return;
            }

            Settings.Default.ExcelFileName = $"{selectedFileName.AppendExcelExtension()}";
            Settings.Default.Save();

            ReloadData();
        }

        private void ReloadData()
        {
            LoadEventsFromExcel();
            LoadCharacters();
            SetEventsView();
        }
    }
}
