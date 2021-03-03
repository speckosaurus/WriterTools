
using System;
using System.Windows.Forms;
using TimelineAssistant.Data;

namespace TimelineAssistant
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchYear = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.agesGridView = new System.Windows.Forms.DataGridView();
            this.eventsGridView = new System.Windows.Forms.DataGridView();
            this.openFileButton = new System.Windows.Forms.Button();
            this.reloadFileButton = new System.Windows.Forms.Button();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.filterComboBox = new System.Windows.Forms.ComboBox();
            this.filterLabel = new System.Windows.Forms.Label();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.agesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).BeginInit();
            this.filterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(7, 17);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(88, 17);
            this.searchLabel.TabIndex = 0;
            this.searchLabel.Text = "Enter a Year";
            // 
            // searchYear
            // 
            this.searchYear.Location = new System.Drawing.Point(119, 17);
            this.searchYear.Name = "searchYear";
            this.searchYear.Size = new System.Drawing.Size(100, 22);
            this.searchYear.TabIndex = 1;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(225, 17);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = "Go";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.searchButton);
            this.searchPanel.Controls.Add(this.searchYear);
            this.searchPanel.Controls.Add(this.searchLabel);
            this.searchPanel.Location = new System.Drawing.Point(519, 699);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(395, 89);
            this.searchPanel.TabIndex = 5;
            // 
            // agesGridView
            // 
            this.agesGridView.AllowUserToAddRows = false;
            this.agesGridView.AllowUserToDeleteRows = false;
            this.agesGridView.AllowUserToOrderColumns = true;
            this.agesGridView.AllowUserToResizeRows = false;
            this.agesGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.agesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.agesGridView.Enabled = false;
            this.agesGridView.Location = new System.Drawing.Point(519, 94);
            this.agesGridView.Name = "agesGridView";
            this.agesGridView.ReadOnly = true;
            this.agesGridView.RowHeadersVisible = false;
            this.agesGridView.RowHeadersWidth = 51;
            this.agesGridView.RowTemplate.Height = 24;
            this.agesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.agesGridView.Size = new System.Drawing.Size(395, 598);
            this.agesGridView.TabIndex = 7;
            // 
            // eventsGridView
            // 
            this.eventsGridView.AllowUserToAddRows = false;
            this.eventsGridView.AllowUserToDeleteRows = false;
            this.eventsGridView.AllowUserToOrderColumns = true;
            this.eventsGridView.AllowUserToResizeRows = false;
            this.eventsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.eventsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eventsGridView.Location = new System.Drawing.Point(13, 94);
            this.eventsGridView.MultiSelect = false;
            this.eventsGridView.Name = "eventsGridView";
            this.eventsGridView.RowHeadersVisible = false;
            this.eventsGridView.RowHeadersWidth = 51;
            this.eventsGridView.RowTemplate.Height = 24;
            this.eventsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.eventsGridView.Size = new System.Drawing.Size(500, 598);
            this.eventsGridView.TabIndex = 8;
            this.eventsGridView.SelectionChanged += new System.EventHandler(this.eventsGridView_SelectedIndexChanged);
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(40, 716);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(108, 45);
            this.openFileButton.TabIndex = 9;
            this.openFileButton.Text = "Open Timeline";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // reloadFileButton
            // 
            this.reloadFileButton.Location = new System.Drawing.Point(208, 716);
            this.reloadFileButton.Name = "reloadFileButton";
            this.reloadFileButton.Size = new System.Drawing.Size(128, 45);
            this.reloadFileButton.TabIndex = 10;
            this.reloadFileButton.Text = "Reload Timeline";
            this.reloadFileButton.UseVisualStyleBackColor = true;
            this.reloadFileButton.Click += new System.EventHandler(this.reloadFileButton_Click);
            // 
            // filterPanel
            // 
            this.filterPanel.Controls.Add(this.filterComboBox);
            this.filterPanel.Controls.Add(this.filterLabel);
            this.filterPanel.Location = new System.Drawing.Point(15, 12);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(498, 76);
            this.filterPanel.TabIndex = 0;
            // 
            // filterComboBox
            // 
            this.filterComboBox.DataSource = Enum.GetValues(typeof(EventType));
            this.filterComboBox.FormattingEnabled = true;
            this.filterComboBox.Location = new System.Drawing.Point(126, 29);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(121, 24);
            this.filterComboBox.TabIndex = 1;
            this.filterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.filterComboBox.SelectedIndexChanged += new System.EventHandler(this.filterComboBox_SelectedIndexChanged);
            // 
            // filterLabel
            // 
            this.filterLabel.AutoSize = true;
            this.filterLabel.Location = new System.Drawing.Point(4, 29);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(115, 17);
            this.filterLabel.TabIndex = 0;
            this.filterLabel.Text = "Filter Event Type";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 800);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.reloadFileButton);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.eventsGridView);
            this.Controls.Add(this.agesGridView);
            this.Controls.Add(this.searchPanel);
            this.Name = "MainView";
            this.Text = "Timeline Assistant";
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.agesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).EndInit();
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private void AgesGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            agesGridView.AutoResizeRows();
        }

        private void EventsGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            eventsGridView.AutoResizeRows();
        }

        #endregion
        private Label searchLabel;
        private TextBox searchYear;
        private Button searchButton;
        private Panel searchPanel;
        private DataGridView agesGridView;
        private DataGridView eventsGridView;
        private Button openFileButton;
        private Button reloadFileButton;
        private Panel filterPanel;
        private Label filterLabel;
        private ComboBox filterComboBox;
    }
}

