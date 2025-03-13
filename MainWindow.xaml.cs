using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

namespace Shaders
{
    public partial class MainWindow : Window
    {
        private BitmapSource originalImage;
        private BitmapImage monkeyBitmap;

        public MainWindow()
        {
            InitializeComponent();
            LoadImages();
        }

        private void LoadImages()
        {
            Uri muskUri = new Uri("C:\\Users\\macbe\\Desktop\\Cours C#\\Shaders\\Shaders\\Musk.png", UriKind.Absolute);
            Uri monkeyUri = new Uri("C:\\Users\\macbe\\Desktop\\Cours C#\\Shaders\\Shaders\\Monkey.png", UriKind.Absolute);
            BitmapImage muskBitmap = new BitmapImage(muskUri);
            monkeyBitmap = new BitmapImage(monkeyUri);
            originalImage = muskBitmap;
            DisplayedImage.Source = muskBitmap;
        }

        private void OnSliderValueChanged(object sender, RoutedEventArgs e)
        {
            DisplayedImage.Source = BlendImages(originalImage);
        }

        private BitmapSource BlendImages(BitmapSource source)
        {
            int sliderValue = (int)ColorSlider.Value;

            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] muskPixels = new byte[height * stride];
            byte[] monkeyPixels = new byte[height * stride];

            source.CopyPixels(muskPixels, stride, 0);
            monkeyBitmap.CopyPixels(monkeyPixels, stride, 0);

            for (int i = 0; i < muskPixels.Length; i += 4)
            {
                muskPixels[i] = BlendColorChannel(muskPixels[i], monkeyPixels[i], sliderValue); // Bleu
                muskPixels[i + 1] = BlendColorChannel(muskPixels[i + 1], monkeyPixels[i + 1], sliderValue); // Vert
                muskPixels[i + 2] = BlendColorChannel(muskPixels[i + 2], monkeyPixels[i + 2], sliderValue); // Rouge
                muskPixels[i + 3] = BlendColorChannel(muskPixels[i + 3], monkeyPixels[i + 3], sliderValue); // Alpha
            }

            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, muskPixels, stride);
        }
        private static byte BlendColorChannel(byte valueA, byte valueB, int ratio)
        {
            return (byte)((valueA * (100 - ratio) + valueB * ratio) / 100);
        }

        private void InvertButtonClicked(object sender, RoutedEventArgs e)
        {
            // a coder
        }
    }
}
