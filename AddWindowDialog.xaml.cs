using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace memes
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AddWindowDialog : Window
    {
        public AddWindowDialog()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
                openFileDialog.ShowDialog();
                pathLb.Content = openFileDialog.FileName;
            }
            catch { MessageBox.Show("Выбран неверный файл"); };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cb.SelectedIndex == -1 || tb.Text == "" || tb.Text == null || pathLb.Content == null || pathLb.Content.ToString() == "")
            {
                MessageBox.Show("Пожалуйста, дайте название, выберите категорию и выберите файл!");
            }
            else
            {
                MainWindow.memes.Add(new Meme { Name = tb.Text, Path = pathLb.Content.ToString(), Type = cb.SelectedIndex});
                Close();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
