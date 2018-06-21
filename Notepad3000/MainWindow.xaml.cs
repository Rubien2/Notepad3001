using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Controls.Ribbon;
using System.Collections.Generic;
using Media = System.Windows.Media;
using System.Drawing;
using System.Windows.Documents;
using System;

namespace Notepad3000
{
    /// <summary>
    /// Interaction logic for RibbonWindow.xaml
    /// </summary>
    public partial class RibbonWindow : Window
    {
        string filePath = @"C:\TestNotepad\";
        string txtboxContent;
        double d = 0.00;
        bool ctrlCheck = false;
        bool sCheck = false;
        bool btnCheck = false;
        bool illegal = false;
        List<FontFamily> fontList = new List<FontFamily>();
        FontFamily familyFont;

        public RibbonWindow()
        {
            InitializeComponent();
            txtPerfectNotepad.PreviewKeyDown += ThePerfectNotepad_PreviewKeyDown;
            txtPerfectNotepad.PreviewKeyUp += ThePerfectNotepad_PreviewKeyUp;

            foreach(FontFamily font in System.Drawing.FontFamily.Families)
            {
                fontList.Add(font);
                cbxFonts.Items.Add(font.Name);
            }
            for (int i = 8; i < 21; i++)
            {
                cbxFontSize.Items.Add(i);
            }
        }

        private void GetText()
        {
            //Every time you need to save, call this method
            txtboxContent = new TextRange(txtPerfectNotepad.Document.ContentStart, txtPerfectNotepad.Document.ContentEnd).Text;
        }

        private void RibbonSaveButton_Click(object sender, RoutedEventArgs e)
        {
            btnCheck = true;
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
            if (txtFileName.Text != "")
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
                    GetText();
                    File.WriteAllText(filePath + txtFileName.Text + ".txt", txtboxContent);
                    illegal = false;
                }
                else
                {
                    System.Windows.MessageBox.Show(@"Ongeldige karakters in bestandsnaam (<, >, :, /, \, |, ?, *), werk bestandsnaam bij");
                }
            }
            else
            {
                if(sCheck && ctrlCheck || btnCheck)
                {
                    ctrlCheck = false;
                    sCheck = false;
                    btnCheck = false;
                    System.Windows.MessageBox.Show("Bestandsnaam mag niet leeg zijn. Geef het bestand a.u.b. een naam");
                }
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
            string fileContents;
            GetText();
            try
            {
                fileContents = File.ReadAllText(filePath + txtFileName.Text + ".txt");
            }
            catch (FileNotFoundException)
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("Wilt u dit als een nieuw bestand opslaan?","Save",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if(result == System.Windows.Forms.DialogResult.Yes)
                {
                    File.WriteAllText(filePath + txtFileName.Text + ".txt", txtboxContent);
                    fileContents = File.ReadAllText(filePath + txtFileName.Text + ".txt");
                }
                else
                {
                    fileContents = txtboxContent;
                }
            }

            if (txtboxContent != fileContents)
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("U verliest de veranderingen die u heeft gemaakt als u een nieuw bestand opent. Wilt u opslaan voordat u verdergaat?", "Save", MessageBoxButtons.YesNoCancel);

                if (result == System.Windows.Forms.DialogResult.Yes && newOrOpen)
                {
                    File.WriteAllText(filePath + txtFileName.Text + ".txt", txtboxContent);
                    txtFileName.Text = "";
                    txtPerfectNotepad.Document.Blocks.Clear();
                }
                else if (result == System.Windows.Forms.DialogResult.Yes && !newOrOpen)
                {
                    File.WriteAllText(filePath + txtFileName.Text + ".txt", txtboxContent);
                    OpenExplorer();
                }
                else if (result == System.Windows.Forms.DialogResult.No && newOrOpen)
                {
                    txtFileName.Text = "";
                    txtPerfectNotepad.Document.Blocks.Clear();
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
                    txtPerfectNotepad.Document.Blocks.Clear();
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

                    txtPerfectNotepad.Document.Blocks.Clear();
                    txtPerfectNotepad.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(selectedFile))));
                    //txtPerfectNotepad.Text = File.ReadAllText(selectedFile);
                }
            }
        }

        private void RibbonCopyButton_Click(object sender, RoutedEventArgs e)
        {
            GetText();
            System.Windows.Clipboard.SetText(txtboxContent);
        }

        private void RibbonPasteButton_Click(object sender, RoutedEventArgs e)
        {
            txtPerfectNotepad.Document.Blocks.Add(new Paragraph(new Run(System.Windows.Clipboard.GetText())));
        }

        private void CbxFonts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedFont = cbxFonts.SelectedValue.ToString();

            foreach (FontFamily font in fontList)
            {
                if (font.Name == selectedFont)
                {
                    familyFont = font;
                }
            }
            //To change the font of the textbox, it needs a FontFamily from System.Media
            Media.FontFamily mfont = new Media.FontFamily(familyFont.Name);

            if (txtPerfectNotepad.Selection.IsEmpty)
            {
                txtPerfectNotepad.FontFamily = mfont;
            }
            else
            {
                txtPerfectNotepad.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, mfont);
            }
        }

        private void CbxFontSize_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (txtPerfectNotepad.Selection.IsEmpty)
            {
                d = Convert.ToDouble(cbxFontSize.SelectedValue);
                txtPerfectNotepad.FontSize = d;
            }
            else
            {
                d = Convert.ToDouble(cbxFontSize.SelectedValue);
                txtPerfectNotepad.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, d);
            }
        }
    }
}
