namespace DVLD
{
    partial class frmShowPersonLicenseHistory
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.ctrlDriverLicenses1 = new DVLD.ctrlDriverLicenses();
            this.ctrlUserCardWithFilter1 = new DVLD.ctrlUserCardWithFilter();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cascadia Code", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(430, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(304, 44);
            this.label2.TabIndex = 121;
            this.label2.Text = "License History";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnClose.FlatAppearance.BorderSize = 2;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Cascadia Code", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::DVLD.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(1001, 730);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(131, 42);
            this.btnClose.TabIndex = 122;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pictureBox13
            // 
            this.pictureBox13.Image = global::DVLD.Properties.Resources.PersonLicenseHistory_512;
            this.pictureBox13.Location = new System.Drawing.Point(9, 62);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(247, 365);
            this.pictureBox13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox13.TabIndex = 120;
            this.pictureBox13.TabStop = false;
            // 
            // ctrlDriverLicenses1
            // 
            this.ctrlDriverLicenses1.Font = new System.Drawing.Font("Cascadia Code", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlDriverLicenses1.Location = new System.Drawing.Point(12, 437);
            this.ctrlDriverLicenses1.Name = "ctrlDriverLicenses1";
            this.ctrlDriverLicenses1.Size = new System.Drawing.Size(1127, 292);
            this.ctrlDriverLicenses1.TabIndex = 1;
            // 
            // ctrlUserCardWithFilter1
            // 
            this.ctrlUserCardWithFilter1.BackColor = System.Drawing.Color.White;
            this.ctrlUserCardWithFilter1.FilterEnabled = true;
            this.ctrlUserCardWithFilter1.Font = new System.Drawing.Font("Cascadia Code", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlUserCardWithFilter1.Location = new System.Drawing.Point(261, 47);
            this.ctrlUserCardWithFilter1.Name = "ctrlUserCardWithFilter1";
            this.ctrlUserCardWithFilter1.ShowAddPerson = true;
            this.ctrlUserCardWithFilter1.Size = new System.Drawing.Size(871, 393);
            this.ctrlUserCardWithFilter1.TabIndex = 0;
            this.ctrlUserCardWithFilter1.OnPersonSelected += new System.Action<int>(this.ctrlUserCardWithFilter1_OnPersonSelected);
            // 
            // frmShowPersonLicenseHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1144, 777);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox13);
            this.Controls.Add(this.ctrlDriverLicenses1);
            this.Controls.Add(this.ctrlUserCardWithFilter1);
            this.Font = new System.Drawing.Font("Cascadia Code", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmShowPersonLicenseHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmShowPersonLicenseHistory";
            this.Load += new System.EventHandler(this.frmShowPersonLicenseHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ctrlUserCardWithFilter ctrlUserCardWithFilter1;
        private ctrlDriverLicenses ctrlDriverLicenses1;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
    }
}