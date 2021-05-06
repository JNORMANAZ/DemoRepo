using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using ImageCallWrapper;
using Microsoft.Win32;

namespace TestImageCallWrapper
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IRotateImage _imageRotator;
        private readonly SystemDrawingProvider provider;

        public MainWindow()
        {
            InitializeComponent();
            provider = new SystemDrawingProvider();
            _imageRotator = provider;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            var selectedFileName = openFileDialog.FileName;
            try
            {
                var image = Image.FromFile(selectedFileName);
                BeforeImage.Source = WpfHelper.ConvertImageToBitmapSource(image);
                RotatedImage.Source =
                    WpfHelper.ConvertImageToBitmapSource(_imageRotator.Rotate(image));
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show(
                    "An out of memory exception was thrown.  This most likely means the file was not an a recognized image format.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception Logged: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}