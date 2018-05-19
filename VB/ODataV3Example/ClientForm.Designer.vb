Namespace ODataV3Example.Northwind
    Partial Public Class ClientForm
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        #Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.panel1 = New System.Windows.Forms.Panel()
            Me.checkRemove = New System.Windows.Forms.Button()
            Me.categoryInteceptor = New System.Windows.Forms.GroupBox()
            Me.dontUse = New System.Windows.Forms.RadioButton()
            Me.use = New System.Windows.Forms.RadioButton()
            Me.entityToShow = New System.Windows.Forms.ComboBox()
            Me.pictureBox = New System.Windows.Forms.PictureBox()
            Me.AuthorizeButton = New System.Windows.Forms.Button()
            Me.gclODataV3ServiceMain = New DevExpress.XtraGrid.GridControl()
            Me.gvlODataV3Service = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.panel1.SuspendLayout()
            Me.categoryInteceptor.SuspendLayout()
            CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.gclODataV3ServiceMain, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.gvlODataV3Service, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' panel1
            ' 
            Me.panel1.Controls.Add(Me.checkRemove)
            Me.panel1.Controls.Add(Me.categoryInteceptor)
            Me.panel1.Controls.Add(Me.entityToShow)
            Me.panel1.Controls.Add(Me.pictureBox)
            Me.panel1.Controls.Add(Me.AuthorizeButton)
            Me.panel1.Dock = System.Windows.Forms.DockStyle.Top
            Me.panel1.Location = New System.Drawing.Point(0, 0)
            Me.panel1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.panel1.Name = "panel1"
            Me.panel1.Size = New System.Drawing.Size(924, 138)
            Me.panel1.TabIndex = 1
            ' 
            ' checkRemove
            ' 
            Me.checkRemove.Anchor = (CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.checkRemove.Enabled = False
            Me.checkRemove.Location = New System.Drawing.Point(580, 34)
            Me.checkRemove.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.checkRemove.Name = "checkRemove"
            Me.checkRemove.Size = New System.Drawing.Size(131, 28)
            Me.checkRemove.TabIndex = 7
            Me.checkRemove.Text = "Check Removable"
            Me.checkRemove.UseVisualStyleBackColor = True
'            Me.checkRemove.Click += New System.EventHandler(Me.checkRemove_Click)
            ' 
            ' categoryInteceptor
            ' 
            Me.categoryInteceptor.Anchor = (CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.categoryInteceptor.Controls.Add(Me.dontUse)
            Me.categoryInteceptor.Controls.Add(Me.use)
            Me.categoryInteceptor.Enabled = False
            Me.categoryInteceptor.Location = New System.Drawing.Point(388, 15)
            Me.categoryInteceptor.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.categoryInteceptor.Name = "categoryInteceptor"
            Me.categoryInteceptor.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.categoryInteceptor.Size = New System.Drawing.Size(179, 76)
            Me.categoryInteceptor.TabIndex = 6
            Me.categoryInteceptor.TabStop = False
            Me.categoryInteceptor.Text = "Interceptor on Category"
            ' 
            ' dontUse
            ' 
            Me.dontUse.AutoSize = True
            Me.dontUse.Location = New System.Drawing.Point(8, 43)
            Me.dontUse.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.dontUse.Name = "dontUse"
            Me.dontUse.Size = New System.Drawing.Size(98, 21)
            Me.dontUse.TabIndex = 1
            Me.dontUse.Text = "Do not use"
            Me.dontUse.UseVisualStyleBackColor = True
'            Me.dontUse.Click += New System.EventHandler(Me.dontUse_Click)
            ' 
            ' use
            ' 
            Me.use.AutoSize = True
            Me.use.Checked = True
            Me.use.Location = New System.Drawing.Point(8, 20)
            Me.use.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.use.Name = "use"
            Me.use.Size = New System.Drawing.Size(54, 21)
            Me.use.TabIndex = 0
            Me.use.TabStop = True
            Me.use.Text = "Use"
            Me.use.UseVisualStyleBackColor = True
'            Me.use.Click += New System.EventHandler(Me.use_Click)
            ' 
            ' entityToShow
            ' 
            Me.entityToShow.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.entityToShow.DisplayMember = "1"
            Me.entityToShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.entityToShow.Enabled = False
            Me.entityToShow.FormattingEnabled = True
            Me.entityToShow.Location = New System.Drawing.Point(155, 37)
            Me.entityToShow.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.entityToShow.Name = "entityToShow"
            Me.entityToShow.Size = New System.Drawing.Size(224, 24)
            Me.entityToShow.TabIndex = 5
'            Me.entityToShow.SelectedIndexChanged += New System.EventHandler(Me.entityToShow_SelectedIndexChanged)
            ' 
            ' pictureBox
            ' 
            Me.pictureBox.Anchor = (CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.pictureBox.Location = New System.Drawing.Point(719, 4)
            Me.pictureBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.pictureBox.Name = "pictureBox"
            Me.pictureBox.Size = New System.Drawing.Size(200, 129)
            Me.pictureBox.TabIndex = 0
            Me.pictureBox.TabStop = False
            ' 
            ' AuthorizeButton
            ' 
            Me.AuthorizeButton.Location = New System.Drawing.Point(16, 34)
            Me.AuthorizeButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.AuthorizeButton.Name = "AuthorizeButton"
            Me.AuthorizeButton.Size = New System.Drawing.Size(131, 28)
            Me.AuthorizeButton.TabIndex = 1
            Me.AuthorizeButton.Text = "Authorize"
            Me.AuthorizeButton.UseVisualStyleBackColor = True
'            Me.AuthorizeButton.Click += New System.EventHandler(Me.Authorize)
            ' 
            ' gclODataV3ServiceMain
            ' 
            Me.gclODataV3ServiceMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.gclODataV3ServiceMain.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
            Me.gclODataV3ServiceMain.Location = New System.Drawing.Point(0, 138)
            Me.gclODataV3ServiceMain.MainView = Me.gvlODataV3Service
            Me.gclODataV3ServiceMain.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.gclODataV3ServiceMain.Name = "gclODataV3ServiceMain"
            Me.gclODataV3ServiceMain.Size = New System.Drawing.Size(924, 405)
            Me.gclODataV3ServiceMain.TabIndex = 3
            Me.gclODataV3ServiceMain.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() { Me.gvlODataV3Service})
            ' 
            ' gvlODataV3Service
            ' 
            Me.gvlODataV3Service.GridControl = Me.gclODataV3ServiceMain
            Me.gvlODataV3Service.Name = "gvlODataV3Service"
            Me.gvlODataV3Service.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
            Me.gvlODataV3Service.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False
            Me.gvlODataV3Service.OptionsBehavior.AllowIncrementalSearch = True
            Me.gvlODataV3Service.OptionsBehavior.Editable = False
'            Me.gvlODataV3Service.FocusedRowChanged += New DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(Me.gvlODataV3Service_FocusedRowChanged)
'            Me.gvlODataV3Service.CellValueChanged += New DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(Me.gvlODataV3Service_CellValueChanged)
            ' 
            ' ClientForm
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(924, 543)
            Me.Controls.Add(Me.gclODataV3ServiceMain)
            Me.Controls.Add(Me.panel1)
            Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.MinimumSize = New System.Drawing.Size(942, 588)
            Me.Name = "ClientForm"
            Me.Text = "True"
            Me.panel1.ResumeLayout(False)
            Me.categoryInteceptor.ResumeLayout(False)
            Me.categoryInteceptor.PerformLayout()
            CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.gclODataV3ServiceMain, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.gvlODataV3Service, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        #End Region

        Private panel1 As System.Windows.Forms.Panel
        Private WithEvents AuthorizeButton As System.Windows.Forms.Button
        Private gclODataV3ServiceMain As DevExpress.XtraGrid.GridControl
        Private WithEvents gvlODataV3Service As DevExpress.XtraGrid.Views.Grid.GridView
        Private pictureBox As System.Windows.Forms.PictureBox
        Private WithEvents entityToShow As System.Windows.Forms.ComboBox
        Private categoryInteceptor As System.Windows.Forms.GroupBox
        Private WithEvents dontUse As System.Windows.Forms.RadioButton
        Private WithEvents use As System.Windows.Forms.RadioButton
        Private WithEvents checkRemove As System.Windows.Forms.Button

    End Class
End Namespace