using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestMic;
using TestSpotify;

namespace AccessibleSpotify
{
    /// <summary>
    /// Interaction logic for PlayBar.xaml
    /// </summary>
    public partial class PlayBar : UserControl
    {
        public PlayBar()
        {
            InitializeComponent();
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => UpdateStatus() ));
            
        }

        private void UpdateStatus()
        {
            if (DataContext is PlayingStatus status)
            {
                var task = Task.Run(() => SpotifyCommands.GetCurrentStatus());
                task.Wait();
                var newStatus = task.Result;
                var fullTrack = newStatus.Item as FullTrack;

                if (fullTrack == null) return;

                status.Song.Name = fullTrack.Name;
                status.Song.Artists = fullTrack.Artists.Select(x => x.Name).ToList();
                status.Song.Uri = fullTrack.Album.Images[0].Url;
                status.IsPlaying = newStatus.IsPlaying;
                status.Song.Maximum = fullTrack.DurationMs;
                status.Current = newStatus.ProgressMs ?? 0;
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext is PlayingStatus status)
            {
                var task = Task.Run(() => SpotifyCommands.SetPlaying(!status.IsPlaying));
                task.Wait();
                status.IsPlaying = !status.IsPlaying;
            }
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            var task = Task.Run(() => SpotifyCommands.SkipTrackNext());
            task.Wait();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext is PlayingStatus status && sender is Button button)
            {
                button.IsEnabled = false;
                bool originallyPlaying = status.IsPlaying;
                if(originallyPlaying)
                {
                    var task = Task.Run(() => SpotifyCommands.SetPlaying(false));
                    task.Wait();
                }

                var newTask = Task.Run(() => SpeechToText.GetText());
                newTask.Wait();
                string text = newTask.Result;

                var newTask2 = Task.Run(() => Commander.Command(text));
                newTask2.Wait();
                bool remainPaused = newTask2.Result;

                if(this.GetParent<MainWindow>() is MainWindow window)
                {
                    window.textCollection.Add(text);
                }

                if(originallyPlaying && !remainPaused)
                {
                    var task = Task.Run(() => SpotifyCommands.SetPlaying(true));
                    task.Wait();
                }

                button.IsEnabled = true;
            }
        }

        private void btnRewind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var task = Task.Run(() => SpotifyCommands.SkipTrackPrevious());
                task.Wait();
            }
            catch
            {
                var task = Task.Run(() => SpotifyCommands.SetProgress(0));
                task.Wait();
            }
        }
    }
}
