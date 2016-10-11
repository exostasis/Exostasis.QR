using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Exostasis.QR.Generator;

namespace Exostasis.QR.Gui
{
    public partial class QrGui : Form
    {
        private QrCode _qrCode;
        public QrGui()
        {
            InitializeComponent();
        }

        private void setSaveLocationButton_Click(object sender, System.EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Bitmap Files (*.bmp)| *.bmp| All files (*.*)| *.*";
            saveDialog.FilterIndex = 1;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                SaveLocationTextBox.Text = saveDialog.FileName;
            }
            else
            {
                SaveLocationTextBox.Text = "";
            }
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(InputStringRichTextBox.Text))
            {
                MessageBox.Show("The input string cannot be empty", "Error empty input string", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(SaveLocationTextBox.Text))
            {
                MessageBox.Show("The filename cannot be empty", "Error filename not set", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                QrPictureBox.Image?.Dispose();
                QrPictureBox.Image = null;
                _qrCode.Generate(SaveLocationTextBox.Text, (int)ScaleComboBox.SelectedValue);
                SetQrPictureBox();
            }            
        }

        private void InputStringRichTextBox_TextChanged(object sender, System.EventArgs e)
        {
            _qrCode = new QrCode(InputStringRichTextBox.Text);
            SetScaleComboBoxValues(_qrCode.GetSize());
        }

        private void SetScaleComboBoxValues(int size)
        {
            var dictionary = new Dictionary<int, string>();
            for (int i = 1; i < 21; ++i)
            {
                dictionary.Add(i, $"{(size + 8) * i} x {(size + 8) * i}");
            }

            ScaleComboBox.DataSource = new BindingSource(dictionary, null);
            ScaleComboBox.DisplayMember = "Value";
            ScaleComboBox.ValueMember = "Key";
        }

        private void SetQrPictureBox()
        {
            Image qrImage = Image.FromFile(SaveLocationTextBox.Text);

            if (qrImage.Width > QrPictureBox.Width)
            {
                QrPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                QrPictureBox.SizeMode = PictureBoxSizeMode.Normal;
            }

            QrPictureBox.Image = qrImage;
        }
    }
}
