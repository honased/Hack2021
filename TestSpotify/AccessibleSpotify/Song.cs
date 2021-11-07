using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media.Imaging;

namespace AccessibleSpotify
{
    public class Song : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        private List<string> artists;
        private int maximum = 100;
        private string uri;

        public string Name
        {
            get => name;
            set
            {
                bool invoke = name != value;
                name = value;
                if (invoke)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        public string Uri
        {
            get => uri;
            set
            {
                bool invoke = uri != value;
                uri = value;
                if (invoke)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Uri"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Image"));
                }
            }
        }

        public BitmapImage Image
        {
            get
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(uri, UriKind.Absolute);
                bitmap.EndInit();
                return bitmap;
            }
        }

        public List<string> Artists
        {
            get => artists;
            set
            {
                bool invoke = artists != value;
                artists = value;
                if (invoke)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Artists"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ArtistNames"));
                }
            }
        }

        public int Maximum
        {
            get => maximum;
            set
            {
                bool invoke = maximum != value;
                maximum = value;
                if (invoke)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Maximum"));
                }
            }
        }

        public string ArtistNames
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < artists.Count; i++)
                {
                    sb.Append(artists[i] + ((i == artists.Count - 1) ? "" : ", "));
                }

                return sb.ToString();
            }
        }
    }
}
