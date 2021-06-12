﻿using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Abstractions;
using KafkaCleanerApp;
using Models;

namespace KafkaCleaner.UI
{
    public partial class KafkaListWindow : Form
    {
        public KafkaListWindow(IKafkaServiceClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            InitializeComponent();
        }

        private void RefreshData()
        {
            try
            {
                var items = _client.RequestTopicsList();
                cbList.DataSource = items;
                cbList.DisplayMember = nameof(Topic.Name);
            }
            catch
            {
                MessageBox.Show("Error on loading data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cbList.UncheckAllItems();
        }

        private bool IsAnySelectedTopics()
        {
            if (cbList.CheckedItems.Count == 0)
            {
                _ = MessageBox.Show("No topics selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private bool IsUserConfirmDeletion()
        {
            var dialog = MessageBox.Show($"Do you want to delete {cbList.CheckedItems.Count} topic(s)", "Delete confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return dialog == DialogResult.Yes;
        }

        private void ChangeAvailability(bool isEnabled)
        {
            bLoadData.Enabled = isEnabled;
            bDeleteData.Enabled = isEnabled;
            cbList.Enabled = isEnabled;
        }

        private void bLoadData_Click(object sender, EventArgs e)
        {
            ChangeAvailability(false);
            RefreshData();
            ChangeAvailability(true);
        }

        private async void bDeleteData_Click(object sender, EventArgs e)
        {
            if (IsAnySelectedTopics() && IsUserConfirmDeletion())
            {
                ChangeAvailability(false);

                try
                {
                    foreach (var item in cbList.CheckedItems.Cast<Topic>())
                    {
                        await _client.DeleteTopicAsync(item);
                    }
                }
                catch
                {
                    MessageBox.Show("Error on deleting data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                RefreshData();

                ChangeAvailability(true);
            }
        }

        private readonly IKafkaServiceClient _client;
    }
}
