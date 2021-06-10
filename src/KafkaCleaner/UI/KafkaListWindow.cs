using Abstractions;
using KafkaCleanerApp;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

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

        private readonly IKafkaServiceClient _client;
    }
}
