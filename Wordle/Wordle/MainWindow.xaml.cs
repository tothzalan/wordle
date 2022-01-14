using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace Wordle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> FileContent { get; set; } = new List<string>();
        private int Row { get; set; } = 0;
        private string RandomWord { get; set; } = "";

        public MainWindow()
        {
            InitializeComponent();
            ReadInFile(fileName: "words.txt");
        }

        private void LoadFile_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            bool? result = openFileDialog.ShowDialog();

            if ((bool)result)
            {
                Stream fileStream = openFileDialog.OpenFile();
                ReadInFile(fileStream: fileStream);
            }

            if(FileContent.Count != 0)
            {
                RandomWord = NewRandomWord();
            }
            else
            {
                MessageBox.Show("Something wrong happened");
            }
        }

        private void TryWord_Button_Click(object sender, RoutedEventArgs e)
        {
            string text = textBox.Text.Trim().ToLower();
            if(text.Length == 5 && RandomWord != "") {
                if (Row < 5)
                {
                    int correct = 0;
                    for (int i = 0; i < text.Length; i++)
                    {
                        Label newLabel = new Label();
                        newLabel.Content = text[i].ToString();
                        newLabel.VerticalAlignment = VerticalAlignment.Center;
                        newLabel.HorizontalAlignment = HorizontalAlignment.Center;

                        if (RandomWord[i] == text[i])
                        {
                            newLabel.Background = Brushes.LightGreen;
                            correct += 1;
                        }
                        else if (RandomWord.Contains(text[i]))
                        {
                            newLabel.Background = Brushes.LightYellow;
                        }



                        Grid.SetColumn(newLabel, i);
                        Grid.SetRow(newLabel, Row);
                        wordGrid.Children.Add(newLabel);
                    }
                    Row++;
                    if(correct == 5) {
                        MessageBox.Show("You won!");
                        solutionLabel.Visibility = Visibility.Visible;
                    }
                }

                if (Row == 5)
                {
                    solutionLabel.Visibility = Visibility.Visible;
                }
            }
        }

        private void NewGame_Button_Click(object sender, RoutedEventArgs e)
        {
            RandomWord = NewRandomWord();
            Row = 0;
            wordGrid.Children.Clear();
            solutionLabel.Visibility = Visibility.Hidden;
        }

        private void ReadInFile(Stream fileStream = null, string fileName = "")
        {

            if (fileStream != null)
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    var line = streamReader.ReadToEnd().Split(" ");
                    FileContent = line.ToList();
                }
            }
            else
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    var line = streamReader.ReadToEnd().Split(" ");
                    FileContent = line.ToList();
                }
            }
            RandomWord = NewRandomWord();
        }

        private string NewRandomWord()
        {
            if(FileContent.Count != 0)
            {
                Random random = new Random();
                string newWord = FileContent[random.Next(0, FileContent.Count)];
                solutionLabel.Content = $"The solution is {newWord}";
                return newWord;
            }
            return "";
        }
    }
}