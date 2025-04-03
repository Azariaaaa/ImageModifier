using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Shaders
{
    public partial class MainWindow : Window
    {
        private BitmapSource originalImage;
        private BitmapSource currentImage;
        private BitmapImage monkeyBitmap;
        private BitmapSource MemorisedBitmap { get ; set; }
        private bool IsBlackAndWhiteFilterActivated { get; set; } = false;

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
            currentImage = originalImage;
            DisplayedImage.Source = currentImage;
        }

        private void UpdateImage(BitmapSource newImage)
        {
            currentImage = newImage;
            DisplayedImage.Source = newImage;
        }

        private BitmapSource BlendImages(BitmapSource source)
        {
            int sliderValue = (int)ColorSlider.Value;

            PixelData muskData = GetPixelData(source);
            PixelData monkeyData = GetPixelData(monkeyBitmap);

            byte[] pixels = muskData.Pixels;

            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i] = BlendColorChannel(pixels[i], monkeyData.Pixels[i], sliderValue);         // Bleu
                pixels[i + 1] = BlendColorChannel(pixels[i + 1], monkeyData.Pixels[i + 1], sliderValue); // Vert
                pixels[i + 2] = BlendColorChannel(pixels[i + 2], monkeyData.Pixels[i + 2], sliderValue); // Rouge
                pixels[i + 3] = BlendColorChannel(pixels[i + 3], monkeyData.Pixels[i + 3], sliderValue); // Alpha
            }

            return BitmapSource.Create(muskData.Width, muskData.Height, 96, 96, PixelFormats.Bgra32, null, pixels, muskData.Stride);
        }

        private static byte BlendColorChannel(byte valueA, byte valueB, int ratio)
        {
            return (byte)((valueA * (100 - ratio) + valueB * ratio) / 100);
        }

        private BitmapSource InvertImage(BitmapSource source)
        {
            PixelData data = GetPixelData(source);
            byte[] pixels = data.Pixels;
            int width = data.Width;
            int height = data.Height;
            int stride = data.Stride;
            int bytesPerPixel = 4;
            byte temp;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width / 2; x++)
                {
                    int leftIndex = y * stride + x * bytesPerPixel;
                    int rightIndex = y * stride + (width - 1 - x) * bytesPerPixel;

                    for (int i = 0; i < bytesPerPixel; i++)
                    {
                        temp = pixels[leftIndex + i];
                        pixels[leftIndex + i] = pixels[rightIndex + i];
                        pixels[rightIndex + i] = temp;
                    }
                }
            }

            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, pixels, stride);
        }

        private BitmapSource BlackAndWhite(BitmapSource source)
        {

            if (!IsBlackAndWhiteFilterActivated)
            {
                MemorisedBitmap = source;
                PixelData data = GetPixelData(source);
                byte[] pixels = data.Pixels;
                int width = data.Width;
                int height = data.Height;
                int stride = data.Stride;
                int bytesPerPixel = 4;

                for (int i = 0; i < pixels.Length; i += bytesPerPixel)
                {
                    int average = (pixels[i] + pixels[i + 1] + pixels[i + 2]) / 3;
                    pixels[i] = (byte)average;
                    pixels[i + 1] = (byte)average;
                    pixels[i + 2] = (byte)average;
                }

                IsBlackAndWhiteFilterActivated = !IsBlackAndWhiteFilterActivated;

                return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, pixels, stride);
            }

            IsBlackAndWhiteFilterActivated = !IsBlackAndWhiteFilterActivated;
            return MemorisedBitmap;
        }

        private PixelData GetPixelData(BitmapSource source)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4; 
            byte[] pixels = new byte[height * stride];
            source.CopyPixels(pixels, stride, 0);
            return new PixelData { Width = width, Height = height, Stride = stride, Pixels = pixels };
        }
        private void OnSliderValueChanged(object sender, RoutedEventArgs e)
        {
            UpdateImage(BlendImages(originalImage));
        }
        private void InvertButtonClicked(object sender, RoutedEventArgs e)
        {
            UpdateImage(InvertImage(currentImage));
        }
        private void BlackAndWhiteButtonClicked(object sender, RoutedEventArgs e)
        {
            UpdateImage(BlackAndWhite(currentImage));
        }

        private void ResetButtonClicked(object sender, RoutedEventArgs e)
        {
            ColorSlider.Value = 0;
            LoadImages();
        }
    }
}
