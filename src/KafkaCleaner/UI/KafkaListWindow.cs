using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Extensions.Logging;

using Abstractions;
using KafkaCleanerApp;
using Models;

namespace KafkaCleaner.UI
{
    public partial class KafkaListWindow : Form
    {
        public KafkaListWindow(IKafkaServiceClient client, ILogger<KafkaListWindow> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            InitializeComponent();

            _defaultDeleteButtonCaption = bDeleteData.Text;

            _logger.LogDebug("Instance created.");
        }

        private void ShowProgress(int current, int total)
        {
            bDeleteData.Text = $"{++indexer}/{total}";
        }

        private void RefreshData()
        {
            try
            {
                _logger.LogDebug("Requesting topics list.");

                var items = _client.RequestTopicsList();
                cbList.DataSource = items;
                cbList.DisplayMember = nameof(Topic.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on loading data");

                MessageBox.Show("Error on loading data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cbList.UncheckAllItems();
        }

        private bool IsAnySelectedTopics()
        {
            _logger.LogDebug("Validating topics count for processing.");

            if (cbList.CheckedItems.Count == 0)
            {
                _ = MessageBox.Show("No topics selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _logger.LogDebug("No topics for processing.");

                return false;
            }

            _logger.LogDebug("{Count} topic(s) for processing.", cbList.CheckedItems.Count);

            return true;
        }

        private bool IsUserConfirmDeletion()
        {
            _logger.LogDebug("Checking user delete confirmation.");

            var dialogResult = MessageBox.Show($"Do you want to delete {cbList.CheckedItems.Count} topic(s)", "Delete confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            _logger.LogDebug("User reaction {Reaction}.", dialogResult);

            return dialogResult == DialogResult.Yes;
        }

        private void ChangeAvailability(bool isEnabled)
        {
            _logger.LogDebug("Changing UI availability to {status}.", isEnabled);

            bLoadData.Enabled = isEnabled;
            bDeleteData.Enabled = isEnabled;
            cbList.Enabled = isEnabled;
            if (isEnabled)
                bDeleteData.Text = _defaultDeleteButtonCaption;
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
                    var indexer = 0;
                    var total = cbList.CheckedItems.Count;
                    foreach (var item in cbList.CheckedItems.Cast<Topic>())
                    {
                        _logger.LogDebug("Trying to delete topic {topic}", item);

                        ShowProgress(++indexer, total);

                        await _client.DeleteTopicAsync(item);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error on deleting data");

                    MessageBox.Show("Error on deleting data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                RefreshData();

                ChangeAvailability(true);
            }
        }

        private readonly IKafkaServiceClient _client;
        private readonly ILogger<KafkaListWindow> _logger;
        private readonly string _defaultDeleteButtonCaption;
    }
}
