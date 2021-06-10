
namespace KafkaCleaner.UI
{
    partial class KafkaListWindow
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
            this.bLoadData = new System.Windows.Forms.Button();
            this.cbList = new System.Windows.Forms.CheckedListBox();
            this.bDeleteData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bLoadData
            // 
            this.bLoadData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bLoadData.Location = new System.Drawing.Point(12, 12);
            this.bLoadData.Name = "bLoadData";
            this.bLoadData.Size = new System.Drawing.Size(776, 39);
            this.bLoadData.TabIndex = 0;
            this.bLoadData.Text = "Load data";
            this.bLoadData.UseVisualStyleBackColor = true;
            this.bLoadData.Click += new System.EventHandler(this.bLoadData_Click);
            // 
            // cbList
            // 
            this.cbList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbList.FormattingEnabled = true;
            this.cbList.Location = new System.Drawing.Point(12, 57);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(776, 328);
            this.cbList.TabIndex = 1;
            // 
            // bDeleteData
            // 
            this.bDeleteData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bDeleteData.Location = new System.Drawing.Point(12, 399);
            this.bDeleteData.Name = "bDeleteData";
            this.bDeleteData.Size = new System.Drawing.Size(776, 39);
            this.bDeleteData.TabIndex = 2;
            this.bDeleteData.Text = "Delete data";
            this.bDeleteData.UseVisualStyleBackColor = true;
            this.bDeleteData.Click += new System.EventHandler(this.bDeleteData_Click);
            // 
            // KafkaListWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bDeleteData);
            this.Controls.Add(this.cbList);
            this.Controls.Add(this.bLoadData);
            this.Name = "KafkaListWindow";
            this.Text = "Kafka Topics Cleaner";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bLoadData;
        private System.Windows.Forms.CheckedListBox cbList;
        private System.Windows.Forms.Button bDeleteData;
    }
}