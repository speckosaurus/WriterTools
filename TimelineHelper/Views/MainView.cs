using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TimelineAssistant.Controllers;
using TimelineAssistant.Data;
using TimelineAssistant.Extensions;
using TimelineAssistant.Models;
using TimelineAssistant.Properties;

namespace TimelineAssistant.Views
{
    public partial class MainView : Form, IView
    {
        private EventController eventController = new EventController();

        public MainView()
        {
            InitializeComponent();
            CenterToScreen();
            SetupDataGrids();
            LoadView();
        }

        private void LoadView()
        {
            LoadFiles();
            LoadEvents();
            LoadCharacters();
            LoadFilterComboBox();
        }

        private void LoadFiles()
        {
            DisableFileComboBox();
            var fileNames = eventController.GetExcelFileNames();
            LoadFileComboBox(fileNames);
        }

        private void LoadEvents()
        {
            eventController.GetEventsFromExcel();
            LoadEvents(eventController.Events);
        }

        private void LoadCharacters()
        {
            eventController.GetCharacters();
        }

        private void LoadFilterComboBox()
        {
            filterComboBox.DataSource = Enum.GetValues(typeof(EventType));
            filterComboBox.SelectedIndexChanged += new EventHandler(this.filterComboBox_SelectedIndexChanged);
        }

        private void LoadCharacterAges(DateTime? date = null)
        {
            if (date == null)
            {
                date = GetSelectedDate();
            }

            if (!date.HasValue)
            {
                return;
            }

            eventController.GetCharacterAges(date.Value);

            // Clear grid
            agesGridView.Rows.Clear();

            foreach (var character in eventController.CharacterAges)
            {
                int rowIndex = agesGridView.Rows.Add(character.Name, character.Age);

                if (character.IsDeceased)
                {
                    agesGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Gray;
                }
            }

            // Remove highlighted selection
            agesGridView.ClearSelection();
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

        public void DisableFileComboBox()
        {
            fileComboBox.SelectedIndexChanged -= new EventHandler(fileComboBox_SelectedIndexChanged);
        }

        public void LoadFileComboBox(List<string> fileNames)
        {
            try
            {
                fileComboBox.DataSource = fileNames;
                fileComboBox.SelectedItem = Settings.Default.ExcelFileName.RemoveExcelExtension();

                fileComboBox.SelectedIndexChanged += new EventHandler(fileComboBox_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                filePanel.Enabled = false;
            }
        }

        public void LoadEvents(List<Event> events)
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

        public void LoadCharacters(List<CharacterAge> characters)
        {
            try
            {
                // Clear grid
                agesGridView.Rows.Clear();

                foreach (var character in characters)
                {
                    int rowIndex = agesGridView.Rows.Add(character.Name, character.Age);

                    if (character.IsDeceased)
                    {
                        agesGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Gray;
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
                LoadCharacterAges(date);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Oops",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void eventsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadCharacterAges();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error encountered when calculating character ages:\n\n{ex.Message}");
            }
        }

        private DateTime? GetSelectedDate()
        {
            if (eventsGridView.SelectedRows.Count == 0)
            {
                return null;
            }

            var selectedDate = eventsGridView.SelectedRows[0].Cells["Date"]?.Value;

            return selectedDate.ToString().SanitiseDate();
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
            ReloadData();
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
            LoadEvents();
            LoadCharacters();
            LoadCharacterAges();
        }
    }
}
