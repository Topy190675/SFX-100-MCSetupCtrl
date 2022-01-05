
using System;

namespace SFX100MCSetup
{
    partial class SFBMCSetupControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelHeadline = new System.Windows.Forms.Label();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.logBox = new System.Windows.Forms.ListBox();
            this.labelLink = new System.Windows.Forms.LinkLabel();
            this.lblControllerCount = new System.Windows.Forms.Label();
            this.btnAddController = new System.Windows.Forms.Button();
            this.buttonShowCurrent = new System.Windows.Forms.Button();
            this.groupBoxNewControllerConfig = new System.Windows.Forms.GroupBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.labelCOMPort = new System.Windows.Forms.Label();
            this.comboBoxCOMs = new System.Windows.Forms.ComboBox();
            this.textBoxControllerName = new System.Windows.Forms.TextBox();
            this.labelMCId = new System.Windows.Forms.Label();
            this.labelCtrlCount = new System.Windows.Forms.Label();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.comboBoxID = new System.Windows.Forms.ComboBox();
            this.btnEditController = new System.Windows.Forms.Button();
            this.btnDeleteController = new System.Windows.Forms.Button();
            this.groupBoxLog.SuspendLayout();
            this.groupBoxNewControllerConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeadline
            // 
            this.labelHeadline.AutoSize = true;
            this.labelHeadline.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeadline.Location = new System.Drawing.Point(3, 7);
            this.labelHeadline.Name = "labelHeadline";
            this.labelHeadline.Size = new System.Drawing.Size(311, 23);
            this.labelHeadline.TabIndex = 0;
            this.labelHeadline.Text = "SFX-100 Multi-Controller Setup";
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Controls.Add(this.logBox);
            this.groupBoxLog.Location = new System.Drawing.Point(10, 150);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(415, 177);
            this.groupBoxLog.TabIndex = 4;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "found config";
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(6, 11);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(405, 160);
            this.logBox.TabIndex = 0;
            this.logBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.logBox_MouseDoubleClick);
            // 
            // labelLink
            // 
            this.labelLink.AutoSize = true;
            this.labelLink.Location = new System.Drawing.Point(3, 36);
            this.labelLink.Name = "labelLink";
            this.labelLink.Size = new System.Drawing.Size(264, 13);
            this.labelLink.TabIndex = 8;
            this.labelLink.TabStop = true;
            this.labelLink.Text = "https://github.com/topy190675/SFX-100-MCSetupCtrl";
            this.labelLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelLink_LinkClicked_1);
            // 
            // lblControllerCount
            // 
            this.lblControllerCount.AutoSize = true;
            this.lblControllerCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblControllerCount.Location = new System.Drawing.Point(7, 72);
            this.lblControllerCount.Name = "lblControllerCount";
            this.lblControllerCount.Size = new System.Drawing.Size(114, 16);
            this.lblControllerCount.TabIndex = 1;
            this.lblControllerCount.Text = "No. of controllers :";
            this.lblControllerCount.UseMnemonic = false;
            // 
            // btnAddController
            // 
            this.btnAddController.Location = new System.Drawing.Point(245, 68);
            this.btnAddController.Name = "btnAddController";
            this.btnAddController.Size = new System.Drawing.Size(50, 23);
            this.btnAddController.TabIndex = 11;
            this.btnAddController.Text = "Add Controller";
            this.btnAddController.UseVisualStyleBackColor = true;
            this.btnAddController.Click += new System.EventHandler(this.btnAddController_Click);
            this.btnAddController.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnAddController_MouseClick);
            // 
            // buttonShowCurrent
            // 
            this.buttonShowCurrent.Location = new System.Drawing.Point(150, 68);
            this.buttonShowCurrent.Name = "buttonShowCurrent";
            this.buttonShowCurrent.Size = new System.Drawing.Size(85, 23);
            this.buttonShowCurrent.TabIndex = 17;
            this.buttonShowCurrent.Text = "Show existing";
            this.buttonShowCurrent.UseVisualStyleBackColor = true;
            this.buttonShowCurrent.Click += new System.EventHandler(this.buttonShowCurrent_Click);
            // 
            // groupBoxNewControllerConfig
            // 
            this.groupBoxNewControllerConfig.Controls.Add(this.buttonCancel);
            this.groupBoxNewControllerConfig.Controls.Add(this.buttonApply);
            this.groupBoxNewControllerConfig.Controls.Add(this.labelCOMPort);
            this.groupBoxNewControllerConfig.Controls.Add(this.comboBoxCOMs);
            this.groupBoxNewControllerConfig.Controls.Add(this.textBoxControllerName);
            this.groupBoxNewControllerConfig.Controls.Add(this.labelMCId);
            this.groupBoxNewControllerConfig.Enabled = false;
            this.groupBoxNewControllerConfig.Location = new System.Drawing.Point(10, 105);
            this.groupBoxNewControllerConfig.Name = "groupBoxNewControllerConfig";
            this.groupBoxNewControllerConfig.Size = new System.Drawing.Size(375, 40);
            this.groupBoxNewControllerConfig.TabIndex = 18;
            this.groupBoxNewControllerConfig.TabStop = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Image = global::SFX100MultiControllerSetup.Properties.Resources.cancel_15x15;
            this.buttonCancel.Location = new System.Drawing.Point(315, 10);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(50, 25);
            this.buttonCancel.TabIndex = 22;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Image = global::SFX100MultiControllerSetup.Properties.Resources.apply_15x15;
            this.buttonApply.Location = new System.Drawing.Point(250, 10);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(50, 25);
            this.buttonApply.TabIndex = 21;
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // labelCOMPort
            // 
            this.labelCOMPort.AutoSize = true;
            this.labelCOMPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCOMPort.Location = new System.Drawing.Point(126, 14);
            this.labelCOMPort.Name = "labelCOMPort";
            this.labelCOMPort.Size = new System.Drawing.Size(38, 16);
            this.labelCOMPort.TabIndex = 20;
            this.labelCOMPort.Text = "Port :";
            // 
            // comboBoxCOMs
            // 
            this.comboBoxCOMs.AllowDrop = true;
            this.comboBoxCOMs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCOMs.FormattingEnabled = true;
            this.comboBoxCOMs.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBoxCOMs.Location = new System.Drawing.Point(171, 12);
            this.comboBoxCOMs.Name = "comboBoxCOMs";
            this.comboBoxCOMs.Size = new System.Drawing.Size(55, 21);
            this.comboBoxCOMs.TabIndex = 19;
            // 
            // textBoxControllerName
            // 
            this.textBoxControllerName.Location = new System.Drawing.Point(40, 13);
            this.textBoxControllerName.Name = "textBoxControllerName";
            this.textBoxControllerName.Size = new System.Drawing.Size(80, 20);
            this.textBoxControllerName.TabIndex = 18;
            // 
            // labelMCId
            // 
            this.labelMCId.AutoSize = true;
            this.labelMCId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMCId.Location = new System.Drawing.Point(7, 14);
            this.labelMCId.Name = "labelMCId";
            this.labelMCId.Size = new System.Drawing.Size(27, 16);
            this.labelMCId.TabIndex = 17;
            this.labelMCId.Text = "ID :";
            // 
            // labelCtrlCount
            // 
            this.labelCtrlCount.AutoSize = true;
            this.labelCtrlCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCtrlCount.Location = new System.Drawing.Point(127, 72);
            this.labelCtrlCount.Name = "labelCtrlCount";
            this.labelCtrlCount.Size = new System.Drawing.Size(15, 16);
            this.labelCtrlCount.TabIndex = 19;
            this.labelCtrlCount.Text = "0";
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 1000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // comboBoxID
            // 
            this.comboBoxID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxID.FormattingEnabled = true;
            this.comboBoxID.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBoxID.Location = new System.Drawing.Point(351, 69);
            this.comboBoxID.Name = "comboBoxID";
            this.comboBoxID.Size = new System.Drawing.Size(30, 21);
            this.comboBoxID.TabIndex = 20;
            this.comboBoxID.Visible = false;
            this.comboBoxID.SelectedIndexChanged += new System.EventHandler(this.comboBoxID_SelectedIndexChanged);
            this.comboBoxID.SelectionChangeCommitted += new System.EventHandler(this.comboBoxID_SelectionChangeCommitted);
            // 
            // btnEditController
            // 
            this.btnEditController.Location = new System.Drawing.Point(305, 68);
            this.btnEditController.Name = "btnEditController";
            this.btnEditController.Size = new System.Drawing.Size(40, 23);
            this.btnEditController.TabIndex = 21;
            this.btnEditController.Text = "Edit";
            this.btnEditController.UseVisualStyleBackColor = true;
            this.btnEditController.Click += new System.EventHandler(this.btnEditController_Click);
            // 
            // btnDeleteController
            // 
            this.btnDeleteController.Location = new System.Drawing.Point(388, 68);
            this.btnDeleteController.Name = "btnDeleteController";
            this.btnDeleteController.Size = new System.Drawing.Size(40, 23);
            this.btnDeleteController.TabIndex = 22;
            this.btnDeleteController.Text = "Del";
            this.btnDeleteController.UseVisualStyleBackColor = true;
            this.btnDeleteController.Click += new System.EventHandler(this.btnDeleteController_Click);
            // 
            // SFBMCSetupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnDeleteController);
            this.Controls.Add(this.btnEditController);
            this.Controls.Add(this.comboBoxID);
            this.Controls.Add(this.labelCtrlCount);
            this.Controls.Add(this.groupBoxNewControllerConfig);
            this.Controls.Add(this.buttonShowCurrent);
            this.Controls.Add(this.btnAddController);
            this.Controls.Add(this.labelLink);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.lblControllerCount);
            this.Controls.Add(this.labelHeadline);
            this.Name = "SFBMCSetupControl";
            this.Size = new System.Drawing.Size(434, 332);
            this.groupBoxLog.ResumeLayout(false);
            this.groupBoxNewControllerConfig.ResumeLayout(false);
            this.groupBoxNewControllerConfig.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHeadline;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.ListBox logBox;
        private System.Windows.Forms.LinkLabel labelLink;
        private System.Windows.Forms.Label lblControllerCount;
        private System.Windows.Forms.Button btnAddController;
        private System.Windows.Forms.Button buttonShowCurrent;
        private System.Windows.Forms.GroupBox groupBoxNewControllerConfig;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Label labelCOMPort;
        private System.Windows.Forms.ComboBox comboBoxCOMs;
        private System.Windows.Forms.TextBox textBoxControllerName;
        private System.Windows.Forms.Label labelMCId;
        private System.Windows.Forms.Label labelCtrlCount;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.ComboBox comboBoxID;
        private System.Windows.Forms.Button btnEditController;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button btnDeleteController;
    }
}
