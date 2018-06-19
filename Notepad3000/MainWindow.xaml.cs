using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Ribbon;

namespace Notepad3000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        string filePath = @"C:\TestNotepad\";
        bool ctrlCheck = false;
        bool sCheck = false;
        bool illegal = false;

        public MainWindow()
        {
            InitializeComponent();
            thePerfectNotepad.PreviewKeyDown += ThePerfectNotepad_PreviewKeyDown;
            thePerfectNotepad.PreviewKeyUp += ThePerfectNotepad_PreviewKeyUp;
            ribbonSaveButton.Click += RibbonSaveButton_Click;
        }

        private void RibbonSaveButton_Click(object sender, RoutedEventArgs e)
        {
            CheckForIllegalChars();
        }

        private void ThePerfectNotepad_PreviewKeyUp(object sender, KeyEventArgs e)
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

        private void ThePerfectNotepad_PreviewKeyDown(object sender, KeyEventArgs e)
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
                File.WriteAllText(filePath + txtFileName.Text + ".txt", thePerfectNotepad.Text);
                illegal = false;
            }
            else
            {
                MessageBox.Show(@"Ongeldige karakters in bestandsnaam (<, >, :, /, \, |, ?, *), werk bestandsnaam bij");
            }
        }
    }
}
