using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Controls.Ribbon;

namespace Notepad3000
{
    /// <summary>
    /// Interaction logic for RibbonWindow.xaml
    /// </summary>
    public partial class RibbonWindow : Window
    {
        string filePath = @"C:\TestNotepad\";
        bool ctrlCheck = false;
        bool sCheck = false;
        bool illegal = false;
        

        public RibbonWindow()
        {
            InitializeComponent();
            txtPerfectNotepad.PreviewKeyDown += ThePerfectNotepad_PreviewKeyDown;
            txtPerfectNotepad.PreviewKeyUp += ThePerfectNotepad_PreviewKeyUp;
        }

        private void RibbonSaveButton_Click(object sender, RoutedEventArgs e)
        {
            CheckForIllegalChars();
        }

        private void ThePerfectNotepad_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                ctrlCheck = false;
            }
            if (e.Key == Key.S)
            {
                sCheck = false;
            }
        }

        private void ThePerfectNotepad_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                ctrlCheck = true;
            }
            if (e.Key == Key.S)
            {
                sCheck = true;
            }

            if(ctrlCheck && sCheck)
            {
                CheckForIllegalChars();
            }
        }

        private void CheckForIllegalChars()
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                if (txtFileName.Text.Contains(invalidChar))
                {
                    illegal = true;
                }
            }

            if (!illegal)
            {
                File.WriteAllText(filePath + txtFileName.Text + ".txt", txtPerfectNotepad.Text);
                illegal = false;
            }
            else
            {
                System.Windows.MessageBox.Show(@"Ongeldige karakters in bestandsnaam (<, >, :, /, \, |, ?, *), werk bestandsnaam bij");
            }
        }

        private void btnNewFile_Click(object sender, RoutedEventArgs e)
        {
            bool newFile = true;
            OpenNewFile(newFile);
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            bool openFile = false;
            OpenNewFile(openFile);
        }

        private void OpenNewFile(bool newOrOpen)
        {
            //if newOrOpen is true, it means btnNewFile has been clicked. If it's false, it means btnOpenFile has been clicked.
            string fileContents = File.ReadAllText(filePath + txtFileName.Text + ".txt");

            if (txtPerfectNotepad.Text != fileContents)
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("You will lose progress if you open a new file. Would you like to save before opening a new file?", "Save", MessageBoxButtons.YesNoCancel);

                if (result == System.Windows.Forms.DialogResult.Yes && newOrOpen)
                {
                    File.WriteAllText(filePath + txtFileName.Text + ".txt", txtPerfectNotepad.Text);
                    txtFileName.Text = "";
                    txtPerfectNotepad.Text = "";
                }
                else if (result == System.Windows.Forms.DialogResult.Yes && !newOrOpen)
                {
                    File.WriteAllText(filePath + txtFileName.Text + ".txt", txtPerfectNotepad.Text);
                    OpenExplorer();
                }
                else if (result == System.Windows.Forms.DialogResult.No && newOrOpen)
                {
                    txtFileName.Text = "";
                    txtPerfectNotepad.Text = "";
                }
                else if(result == System.Windows.Forms.DialogResult.No && !newOrOpen)
                {
                    OpenExplorer();
                }
            }
            else
            {
                if (newOrOpen)
                {
                    txtFileName.Text = "";
                    txtPerfectNotepad.Text = "";
                }
                else if (!newOrOpen)
                {
                    OpenExplorer();
                }
            }
        }

        private void OpenExplorer()
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.InitialDirectory = filePath;
                open.Filter = "Plain Text Files (*.txt)|*.txt";
                open.FilterIndex = 1;
                open.RestoreDirectory = true;

                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFile = open.FileName;
                    string selectedFileName = Path.GetFileNameWithoutExtension(open.FileName);
                    txtFileName.Text = selectedFileName;
                    txtPerfectNotepad.Text = File.ReadAllText(selectedFile);
                }
            }
        }
    }
}
