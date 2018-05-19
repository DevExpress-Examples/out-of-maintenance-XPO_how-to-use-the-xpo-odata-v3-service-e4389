namespace ODataV3Example.Northwind {
    partial class ClientForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkRemove = new System.Windows.Forms.Button();
            this.categoryInteceptor = new System.Windows.Forms.GroupBox();
            this.dontUse = new System.Windows.Forms.RadioButton();
            this.use = new System.Windows.Forms.RadioButton();
            this.entityToShow = new System.Windows.Forms.ComboBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.AuthorizeButton = new System.Windows.Forms.Button();
            this.gclODataV3ServiceMain = new DevExpress.XtraGrid.GridControl();
            this.gvlODataV3Service = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1.SuspendLayout();
            this.categoryInteceptor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gclODataV3ServiceMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvlODataV3Service)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkRemove);
            this.panel1.Controls.Add(this.categoryInteceptor);
            this.panel1.Controls.Add(this.entityToShow);
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Controls.Add(this.AuthorizeButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(694, 112);
            this.panel1.TabIndex = 1;
            // 
            // checkRemove
            // 
            this.checkRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkRemove.Enabled = false;
            this.checkRemove.Location = new System.Drawing.Point(436, 28);
            this.checkRemove.Name = "checkRemove";
            this.checkRemove.Size = new System.Drawing.Size(98, 23);
            this.checkRemove.TabIndex = 7;
            this.checkRemove.Text = "Check Removable";
            this.checkRemove.UseVisualStyleBackColor = true;
            this.checkRemove.Click += new System.EventHandler(this.checkRemove_Click);
            // 
            // categoryInteceptor
            // 
            this.categoryInteceptor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryInteceptor.Controls.Add(this.dontUse);
            this.categoryInteceptor.Controls.Add(this.use);
            this.categoryInteceptor.Enabled = false;
            this.categoryInteceptor.Location = new System.Drawing.Point(292, 12);
            this.categoryInteceptor.Name = "categoryInteceptor";
            this.categoryInteceptor.Size = new System.Drawing.Size(134, 62);
            this.categoryInteceptor.TabIndex = 6;
            this.categoryInteceptor.TabStop = false;
            this.categoryInteceptor.Text = "Interceptor on Category";
            // 
            // dontUse
            // 
            this.dontUse.AutoSize = true;
            this.dontUse.Location = new System.Drawing.Point(6, 35);
            this.dontUse.Name = "dontUse";
            this.dontUse.Size = new System.Drawing.Size(77, 17);
            this.dontUse.TabIndex = 1;
            this.dontUse.Text = "Do not use";
            this.dontUse.UseVisualStyleBackColor = true;
            this.dontUse.Click += new System.EventHandler(this.dontUse_Click);
            // 
            // use
            // 
            this.use.AutoSize = true;
            this.use.Checked = true;
            this.use.Location = new System.Drawing.Point(6, 16);
            this.use.Name = "use";
            this.use.Size = new System.Drawing.Size(44, 17);
            this.use.TabIndex = 0;
            this.use.TabStop = true;
            this.use.Text = "Use";
            this.use.UseVisualStyleBackColor = true;
            this.use.Click += new System.EventHandler(this.use_Click);
            // 
            // entityToShow
            // 
            this.entityToShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entityToShow.DisplayMember = "1";
            this.entityToShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.entityToShow.Enabled = false;
            this.entityToShow.FormattingEnabled = true;
            this.entityToShow.Location = new System.Drawing.Point(116, 30);
            this.entityToShow.Name = "entityToShow";
            this.entityToShow.Size = new System.Drawing.Size(170, 21);
            this.entityToShow.TabIndex = 5;
            this.entityToShow.SelectedIndexChanged += new System.EventHandler(this.entityToShow_SelectedIndexChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(540, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 105);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // AuthorizeButton
            // 
            this.AuthorizeButton.Location = new System.Drawing.Point(12, 28);
            this.AuthorizeButton.Name = "AuthorizeButton";
            this.AuthorizeButton.Size = new System.Drawing.Size(98, 23);
            this.AuthorizeButton.TabIndex = 1;
            this.AuthorizeButton.Text = "Authorize";
            this.AuthorizeButton.UseVisualStyleBackColor = true;
            this.AuthorizeButton.Click += new System.EventHandler(this.Authorize);
            // 
            // gclODataV3ServiceMain
            // 
            this.gclODataV3ServiceMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gclODataV3ServiceMain.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gclODataV3ServiceMain.Location = new System.Drawing.Point(0, 112);
            this.gclODataV3ServiceMain.MainView = this.gvlODataV3Service;
            this.gclODataV3ServiceMain.Name = "gclODataV3ServiceMain";
            this.gclODataV3ServiceMain.Size = new System.Drawing.Size(694, 334);
            this.gclODataV3ServiceMain.TabIndex = 3;
            this.gclODataV3ServiceMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvlODataV3Service});
            // 
            // gvlODataV3Service
            // 
            this.gvlODataV3Service.GridControl = this.gclODataV3ServiceMain;
            this.gvlODataV3Service.Name = "gvlODataV3Service";
            this.gvlODataV3Service.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvlODataV3Service.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvlODataV3Service.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvlODataV3Service.OptionsBehavior.Editable = false;
            this.gvlODataV3Service.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvlODataV3Service_FocusedRowChanged);
            this.gvlODataV3Service.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvlODataV3Service_CellValueChanged);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 446);
            this.Controls.Add(this.gclODataV3ServiceMain);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(710, 485);
            this.Name = "ClientForm";
            this.Text = "OData V3 Example";
            this.panel1.ResumeLayout(false);
            this.categoryInteceptor.ResumeLayout(false);
            this.categoryInteceptor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gclODataV3ServiceMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvlODataV3Service)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button AuthorizeButton;
        private DevExpress.XtraGrid.GridControl gclODataV3ServiceMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvlODataV3Service;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox entityToShow;
        private System.Windows.Forms.GroupBox categoryInteceptor;
        private System.Windows.Forms.RadioButton dontUse;
        private System.Windows.Forms.RadioButton use;
        private System.Windows.Forms.Button checkRemove;

    }
}