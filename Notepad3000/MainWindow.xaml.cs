using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notepad3000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
                foreach(char invalidChar in System.IO.Path.GetInvalidFileNameChars())
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
}
