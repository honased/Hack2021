using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestSpotify;

namespace AccessibleSpotify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> textCollection;

        public MainWindow()
        {
            InitializeComponent();
            Song song = new Song();
            textCollection = new ObservableCollection<string>();
            commandList.ItemsSource = textCollection;

            SpotifyCommands.Initialize();
            var task = Task.Run(() => SpotifyCommands.GetCurrentStatus());
            task.Wait();
            var status = task.Result;
            if (status == null) return;
            var fullTrack = status.Item as FullTrack;

            song.Name = fullTrack.Name;
            song.Artists = fullTrack.Artists.Select(x => x.Name).ToList();
            song.Uri = fullTrack.Album.Images[0].Url;
            Bar.DataContext = new PlayingStatus() { Song = song, IsPlaying = status.IsPlaying };
        }
    }
}
