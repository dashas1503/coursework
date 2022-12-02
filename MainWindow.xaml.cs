using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace memes
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Meme> Memes = new List<Meme>();
        public MainWindow()
        {
            InitializeComponent();
        }

        void RefreshListBox()
        {
            lb.Items.Clear();
            if (Memes.Count == 0) return;
            foreach(var meme in Memes)
            {
                if (meme.Name.Contains(tb.Text) && (cb.SelectedIndex == -1 || cb.SelectedIndex == 3 || meme.Type == cb.SelectedIndex))
                    lb.Items.Add(meme.Name);
            }
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lb.SelectedItem == null) 
            {
                img.Source = null;
                btn.IsEnabled = false;
                return;
            }
            string path = "";
            foreach(var meme in Memes)
            {
                if(meme.Name == lb.SelectedItem.ToString())
                {
                    path = meme.Path;
                }
            }
            try 
            {
                Uri fileUri = new Uri(path);
                img.Source = new BitmapImage(fileUri);
            }
            catch
            {
                MessageBox.Show("Картинка не найдена");
            }

             btn.IsEnabled = true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(var meme in Memes)
            {
                if(meme.Name == lb.SelectedItem.ToString())
                {
                    Memes.Remove(meme);
                    RefreshListBox();
                    return;
                }
            }
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListBox();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListBox();
        }
        private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
        {
            AddWindowDialog addWindow = new AddWindowDialog
            {
                Owner = this
            };
            addWindow.ShowDialog();
            RefreshListBox();
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(Memes);
            try 
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt| Json files (*.json)|*.json";
                saveFileDialog.ShowDialog();
                File.WriteAllText(saveFileDialog.FileName, json);
            }
            catch
            {
                MessageBox.Show("Выбран неверный файл!");
            }
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                openFileDialog.ShowDialog();
                string json = File.ReadAllText(openFileDialog.FileName);
                Memes = JsonConvert.DeserializeObject<List<Meme>>(json);
                RefreshListBox();
            }
            catch
            {
                MessageBox.Show("Выбран неверный файл!");
            }
        }
    }
}
