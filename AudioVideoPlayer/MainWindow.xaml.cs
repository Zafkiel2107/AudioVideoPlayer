using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Threading;

namespace AudioVideoPlayer
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            VolumeSlider.Value = 0.5;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0.1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            VideoPanel.MediaEnded += VideoPanel_MediaEnded;
        }

        private void VideoPanel_MediaEnded(object sender, RoutedEventArgs e)
        {
            VideoPanel.Position = TimeSpan.Zero;
            VideoPanel.Stop();
            dispatcherTimer.Stop();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Time.Content = VideoPanel.Position.ToString(@"mm\:ss");
            TimeSlider.Value = VideoPanel.Position.TotalSeconds;
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            VideoPanel.Play();
            dispatcherTimer.Start();
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            VideoPanel.Pause();
            dispatcherTimer.Stop();
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                VideoPanel.Source = new Uri(openFileDialog.FileName);
                VideoPanel.Play();
                dispatcherTimer.Start();
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (VideoPanel != null)
            {
                VideoPanel.Volume = VolumeSlider.Value;
                VolumeValue.Content = string.Format("{0:0.##}", VolumeSlider.Value);
            }
        }

        private void VideoPanel_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSlider.Maximum = VideoPanel.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoPanel.Pause();
            VideoPanel.Position = TimeSpan.FromSeconds(TimeSlider.Value);
            VideoPanel.Play();
        }
    }
}
