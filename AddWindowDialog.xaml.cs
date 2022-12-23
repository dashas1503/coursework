using Microsoft.Win32;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

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
                foreach (var meme in MainWindow.memes)
                {
                    if (meme.Name == tb.Text)
                    {
                        MainWindow.memes.Add(new Meme { Name = tb.Text + "-копия", Path = pathLb.Content.ToString(), Type = cb.SelectedIndex, Tags = tagsTb.Text.Split(' ').ToList() });
                        Close();
                        break;
                    }
                }
                if (IsActive)
                {
                    MainWindow.memes.Add(new Meme { Name = tb.Text, Path = pathLb.Content.ToString(), Type = cb.SelectedIndex, Tags = tagsTb.Text.Split(' ').ToList() });
                    Close();
                } 
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[а-яА-Яa-zA-Z0-9]");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
