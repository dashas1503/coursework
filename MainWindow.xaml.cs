using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace memes
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Meme> memes = new List<Meme>();
        public MainWindow()
        {
            InitializeComponent();
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
        void RefreshListBox()
      {
            lb.Items.Clear();
            if (memes.Count == 0) return;
            foreach(var meme in memes)
            {
                if (( meme.IsHasTag(tb.Text) || meme.Name.Contains(tb.Text)) && (cb.SelectedIndex == -1 || cb.SelectedIndex == 3 || meme.Type == cb.SelectedIndex))
                    lb.Items.Add(meme.Name);
            }
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try 
            {
                if (lb.SelectedItem == null)
                {
                    img.Source = null;
                    btn.IsEnabled = false;
                    return;
                }
                string path = "";
                foreach (var meme in memes)
                {
                    if (meme.Name == lb.SelectedItem.ToString())
                    {
                        path = meme.Path;
                        Uri fileUri = new Uri(path);
                        img.Source = new BitmapImage(fileUri);
                        img.ToolTip = "#"+string.Join("#", meme.Tags); 
                    }
                }
            }
            catch
            {
                MessageBox.Show("Картинка не найдена");
            }

             btn.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            memes.RemoveAt(lb.SelectedIndex);
            RefreshListBox();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListBox();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListBox();
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Json file (*.json)|*.json|Binary file (*.dat)|*.dat|XML file (*.xml)|*.xml";
                saveFileDialog.ShowDialog();

                switch (Path.GetExtension(saveFileDialog.FileName))
                {
                    case (".json"):
                        string json = "";
                        json = JsonConvert.SerializeObject(memes);
                        File.WriteAllText(saveFileDialog.FileName, json);
                        break;
                    case (".dat"):
                        BinaryFormatter formatter = new BinaryFormatter();
                        using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                        {
                            formatter.Serialize(fs, memes);
                        }
                        break;
                    case (".xml"):
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Meme>));
                        using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                        {
                            xmlSerializer.Serialize(fs, memes);
                        }
                        break;
                }
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
                openFileDialog.Filter = "Json file (*.json)|*.json|Binary file (*.dat)|*.dat|XML file (*.xml)|*.xml";
                openFileDialog.ShowDialog();

                switch(Path.GetExtension(openFileDialog.FileName))
                {
                    case (".json"):
                        string json = File.ReadAllText(openFileDialog.FileName);
                        memes = JsonConvert.DeserializeObject<List<Meme>>(json);
                        break;
                    case (".dat"):
                        BinaryFormatter formatter = new BinaryFormatter();
                        using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
                        {
                            object obj = formatter.Deserialize(fs);
                            memes = obj as List<Meme>;
                        }
                        break;
                    case (".xml"):
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Meme>));
                        using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
                        {
                            object obj = xmlSerializer.Deserialize(fs);
                            memes = obj as List<Meme>;
                        }
                        break;
                }

                RefreshListBox();
            }
            catch
            {
                MessageBox.Show("Выбран неверный файл!");
            }
        }
    }
}
