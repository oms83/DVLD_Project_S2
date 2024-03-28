namespace DVLD
{
    partial class ctrlUserCardWithFilter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbFilterBy = new System.Windows.Forms.GroupBox();
            this.txtFilterBy = new System.Windows.Forms.TextBox();
            this.cmbFilterBy = new System.Windows.Forms.ComboBox();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ucPersonInfo1 = new DVLD.ctrlPersonInfo();
            this.gbFilterBy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbFilterBy
            // 
            this.gbFilterBy.Controls.Add(this.txtFilterBy);
            this.gbFilterBy.Controls.Add(this.cmbFilterBy);
            this.gbFilterBy.Controls.Add(this.btnAddNew);
            this.gbFilterBy.Controls.Add(this.btnSearch);
            this.gbFilterBy.Controls.Add(this.label1);
            this.gbFilterBy.Font = new System.Drawing.Font("Cascadia Code", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFilterBy.Location = new System.Drawing.Point(6, -1);
            this.gbFilterBy.Name = "gbFilterBy";
            this.gbFilterBy.Size = new System.Drawing.Size(866, 106);
            this.gbFilterBy.TabIndex = 1;
            this.gbFilterBy.TabStop = false;
            this.gbFilterBy.Text = "Filter";
            // 
            // txtFilterBy
            // 
            this.txtFilterBy.Location = new System.Drawing.Point(412, 47);
            this.txtFilterBy.Name = "txtFilterBy";
            this.txtFilterBy.Size = new System.Drawing.Size(254, 31);
            this.txtFilterBy.TabIndex = 4;
            this.txtFilterBy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilterBy_KeyPress);
            this.txtFilterBy.Validating += new System.ComponentModel.CancelEventHandler(this.txtFilterBy_Validating);
            // 
            // cmbFilterBy
            // 
            this.cmbFilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterBy.FormattingEnabled = true;
            this.cmbFilterBy.Items.AddRange(new object[] {
            "National No.",
            "Person ID"});
            this.cmbFilterBy.Location = new System.Drawing.Point(152, 45);
            this.cmbFilterBy.Name = "cmbFilterBy";
            this.cmbFilterBy.Size = new System.Drawing.Size(254, 35);
            this.cmbFilterBy.TabIndex = 3;
            this.cmbFilterBy.SelectedIndexChanged += new System.EventHandler(this.cmbFilterBy_SelectedIndexChanged);
            // 
            // btnAddNew
            // 
            this.btnAddNew.BackColor = System.Drawing.Color.White;
            this.btnAddNew.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNew.Image = global::DVLD.Properties.Resources.Add_Person_40;
            this.btnAddNew.Location = new System.Drawing.Point(768, 33);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(58, 57);
            this.btnAddNew.TabIndex = 2;
            this.btnAddNew.UseVisualStyleBackColor = false;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Image = global::DVLD.Properties.Resources.SearchPerson;
            this.btnSearch.Location = new System.Drawing.Point(704, 33);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(58, 57);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cascadia Code", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filter By:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ucPersonInfo1
            // 
            this.ucPersonInfo1.Font = new System.Drawing.Font("Cascadia Code", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucPersonInfo1.Location = new System.Drawing.Point(3, 105);
            this.ucPersonInfo1.Name = "ucPersonInfo1";
            this.ucPersonInfo1.Size = new System.Drawing.Size(866, 283);
            this.ucPersonInfo1.TabIndex = 0;
            this.ucPersonInfo1.OnPersonEdited += new System.Action<string>(this.ucPersonInfo1_OnPersonEdited);
            // 
            // ctrlUserCardWithFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gbFilterBy);
            this.Controls.Add(this.ucPersonInfo1);
            this.Font = new System.Drawing.Font("Cascadia Code", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ctrlUserCardWithFilter";
            this.Size = new System.Drawing.Size(877, 395);
            this.Load += new System.EventHandler(this.ctrlUserCardWithFilter_Load);
            this.gbFilterBy.ResumeLayout(false);
            this.gbFilterBy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DVLD.ctrlPersonInfo ucPersonInfo1;
        private System.Windows.Forms.GroupBox gbFilterBy;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilterBy;
        private System.Windows.Forms.ComboBox cmbFilterBy;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
