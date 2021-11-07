using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AccessibleSpotify
{
    public class PlayingStatus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool playing = false;
        private int current = 50;
        private Song song;

        public bool IsPlaying
        {
            get => playing;
            set
            {
                bool invoke = playing != value;
                playing = value;
                if (invoke)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPlaying"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlayingText"));
                }
            }
        }

        public Song Song
        {
            get => song;
            set
            {
                bool invoke = song != value;
                song = value;
                if(invoke)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Song"));
                }
            }
        }

        public int Current
        {
            get => current;
            set
            {
                bool invoke = current != value;
                current = value;
                if (invoke)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Current"));
                }
            }
        }

        public string PlayingText
        {
            get
            {
                return (!playing) ? "https://static.thenounproject.com/png/117815-200.png" : "https://w7.pngwing.com/pngs/879/589/png-transparent-pause-logo-computer-icons-button-media-player-pause-button-rectangle-black-internet-thumbnail.png";
            }
        }
    }
}
