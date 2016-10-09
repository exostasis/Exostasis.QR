namespace Exostasis.QR.Gui
{
    partial class QrGui
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
            this.QrPictureBox = new System.Windows.Forms.PictureBox();
            this.InputStringLabel = new System.Windows.Forms.Label();
            this.InputStringRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SizeLabel = new System.Windows.Forms.Label();
            this.ScaleComboBox = new System.Windows.Forms.ComboBox();
            this.SaveLocationLabel = new System.Windows.Forms.Label();
            this.SaveLocationTextBox = new System.Windows.Forms.TextBox();
            this.SetSaveLocationButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.QrPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // QrPictureBox
            // 
            this.QrPictureBox.Location = new System.Drawing.Point(12, 12);
            this.QrPictureBox.Name = "QrPictureBox";
            this.QrPictureBox.Size = new System.Drawing.Size(440, 440);
            this.QrPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.QrPictureBox.TabIndex = 0;
            this.QrPictureBox.TabStop = false;
            // 
            // InputStringLabel
            // 
            this.InputStringLabel.AutoSize = true;
            this.InputStringLabel.Location = new System.Drawing.Point(458, 12);
            this.InputStringLabel.Name = "InputStringLabel";
            this.InputStringLabel.Size = new System.Drawing.Size(64, 13);
            this.InputStringLabel.TabIndex = 1;
            this.InputStringLabel.Text = "Input String:";
            // 
            // InputStringRichTextBox
            // 
            this.InputStringRichTextBox.Location = new System.Drawing.Point(459, 29);
            this.InputStringRichTextBox.Name = "InputStringRichTextBox";
            this.InputStringRichTextBox.Size = new System.Drawing.Size(378, 105);
            this.InputStringRichTextBox.TabIndex = 2;
            this.InputStringRichTextBox.Text = "";
            this.InputStringRichTextBox.TextChanged += new System.EventHandler(this.InputStringRichTextBox_TextChanged);
            // 
            // SizeLabel
            // 
            this.SizeLabel.AutoSize = true;
            this.SizeLabel.Location = new System.Drawing.Point(459, 141);
            this.SizeLabel.Name = "SizeLabel";
            this.SizeLabel.Size = new System.Drawing.Size(30, 13);
            this.SizeLabel.TabIndex = 3;
            this.SizeLabel.Text = "Size:";
            // 
            // ScaleComboBox
            // 
            this.ScaleComboBox.FormattingEnabled = true;
            this.ScaleComboBox.Location = new System.Drawing.Point(459, 158);
            this.ScaleComboBox.Name = "ScaleComboBox";
            this.ScaleComboBox.Size = new System.Drawing.Size(378, 21);
            this.ScaleComboBox.TabIndex = 4;
            // 
            // SaveLocationLabel
            // 
            this.SaveLocationLabel.AutoSize = true;
            this.SaveLocationLabel.Location = new System.Drawing.Point(459, 186);
            this.SaveLocationLabel.Name = "SaveLocationLabel";
            this.SaveLocationLabel.Size = new System.Drawing.Size(26, 13);
            this.SaveLocationLabel.TabIndex = 5;
            this.SaveLocationLabel.Text = "File:";
            // 
            // SaveLocationTextBox
            // 
            this.SaveLocationTextBox.Location = new System.Drawing.Point(458, 202);
            this.SaveLocationTextBox.Name = "SaveLocationTextBox";
            this.SaveLocationTextBox.Size = new System.Drawing.Size(379, 20);
            this.SaveLocationTextBox.TabIndex = 6;
            // 
            // SetSaveLocationButton
            // 
            this.SetSaveLocationButton.Location = new System.Drawing.Point(459, 229);
            this.SetSaveLocationButton.Name = "SetSaveLocationButton";
            this.SetSaveLocationButton.Size = new System.Drawing.Size(79, 23);
            this.SetSaveLocationButton.TabIndex = 7;
            this.SetSaveLocationButton.Text = "Select File";
            this.SetSaveLocationButton.UseVisualStyleBackColor = true;
            this.SetSaveLocationButton.Click += new System.EventHandler(this.setSaveLocationButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(544, 229);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 8;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // QrGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 464);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.SetSaveLocationButton);
            this.Controls.Add(this.SaveLocationTextBox);
            this.Controls.Add(this.SaveLocationLabel);
            this.Controls.Add(this.ScaleComboBox);
            this.Controls.Add(this.SizeLabel);
            this.Controls.Add(this.InputStringRichTextBox);
            this.Controls.Add(this.InputStringLabel);
            this.Controls.Add(this.QrPictureBox);
            this.Name = "QrGui";
            this.Text = "QR Code Generator Gui";
            ((System.ComponentModel.ISupportInitialize)(this.QrPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox QrPictureBox;
        private System.Windows.Forms.Label InputStringLabel;
        private System.Windows.Forms.RichTextBox InputStringRichTextBox;
        private System.Windows.Forms.Label SizeLabel;
        private System.Windows.Forms.ComboBox ScaleComboBox;
        private System.Windows.Forms.Label SaveLocationLabel;
        private System.Windows.Forms.TextBox SaveLocationTextBox;
        private System.Windows.Forms.Button SetSaveLocationButton;
        private System.Windows.Forms.Button SaveButton;
    }
}

