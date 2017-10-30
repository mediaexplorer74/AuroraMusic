﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Music.Core.Storage;
using Aurora.Shared.Helpers;
using Aurora.Shared.MVVM;
using Windows.Storage;
using System.Diagnostics;
using Aurora.Music.Core.Player;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Aurora.Music.Pages;
using Windows.UI.Text;
using Windows.ApplicationModel.Core;

namespace Aurora.Music.ViewModels
{
    class MainPageViewModel : ViewModelBase, IDisposable
    {
        public static MainPageViewModel Current;

        public List<HamPanelItem> HamList { get; set; } = new List<HamPanelItem>()
        {
            new HamPanelItem
            {
                Title = "Home",
                TargetType = typeof(HomePage),
                IsCurrent = true
            },
            new HamPanelItem
            {
                Title = "Library",
                TargetType = typeof(LibraryPage)
            },
            new HamPanelItem
            {
                Title = "Playlist",
                TargetType = typeof(HomePage)
            },
        };

        private Player player;

        private bool needShowPanel = true;
        public bool NeedShowPanel
        {
            get { return needShowPanel; }
            set { SetProperty(ref needShowPanel, value); }
        }

        private BitmapImage currentArtwork;
        public BitmapImage CurrentArtwork
        {
            get { return currentArtwork; }
            set { SetProperty(ref currentArtwork, value); }
        }

        private double nowPlayingPosition;
        public double NowPlayingPosition
        {
            get { return nowPlayingPosition; }
            set { SetProperty(ref nowPlayingPosition, value); }
        }

        private bool? isPlaying;
        public bool? IsPlaying
        {
            get { return isPlaying; }
            set { SetProperty(ref isPlaying, value); }
        }

        private TimeSpan currentPosition;
        public TimeSpan CurrentPosition
        {
            get { return currentPosition; }
            set { SetProperty(ref currentPosition, value); }
        }

        private TimeSpan totalDuration;
        public TimeSpan TotalDuration
        {
            get { return totalDuration; }
            set { SetProperty(ref totalDuration, value); }
        }

        private string currentTitle;
        public string CurrentTitle
        {
            get { return currentTitle; }
            set { SetProperty(ref currentTitle, value); }
        }

        private string currentAlbum;
        private string lastUriPath;

        public string CurrentAlbum
        {
            get { return currentAlbum; }
            set { SetProperty(ref currentAlbum, value); }
        }

        public DelegateCommand GoPrevious
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    player?.GoPrevious();
                });
            }
        }

        public DelegateCommand GoNext
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    player?.GoNext();
                });
            }
        }

        public DelegateCommand PlayPause
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    player?.PlayPause();
                });
            }
        }

        public DelegateCommand ToggleShuffle
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    player?.ToggleShuffle();
                });
            }
        }

        public DelegateCommand ToggleLoop
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    player?.ToggleLoop();
                });
            }
        }

        public MainPageViewModel()
        {
            player = new Player();
            Current = this;
            player.StatusChanged += Player_StatusChanged;
            player.PositionUpdated += Player_PositionUpdated;
        }

        private async void Player_PositionUpdated(object sender, PositionUpdatedArgs e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                CurrentPosition = e.Current;
                TotalDuration = e.Total;
            });
        }

        private async void Player_StatusChanged(object sender, StatusChangedArgs e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                switch (e.State)
                {
                    case Windows.Media.Playback.MediaPlaybackState.None:
                    case Windows.Media.Playback.MediaPlaybackState.Opening:
                    case Windows.Media.Playback.MediaPlaybackState.Buffering:
                        IsPlaying = null;
                        break;
                    case Windows.Media.Playback.MediaPlaybackState.Playing:
                        IsPlaying = true;
                        break;
                    case Windows.Media.Playback.MediaPlaybackState.Paused:
                        IsPlaying = false;
                        break;
                    default:
                        break;
                }
                if (e.CurrentSong != null)
                {
                    var p = e.CurrentSong.GetDisplayProperties().MusicProperties;
                    CurrentTitle = p.Title;
                    CurrentAlbum = p.AlbumTitle;
                    if (e.CurrentSong.Source.CustomProperties["Artwork"] is Uri u)
                    {
                        if (lastUriPath == u.AbsolutePath)
                        {

                        }
                        else
                        {
                            CurrentArtwork = new BitmapImage(u);
                            lastUriPath = u.AbsolutePath;
                        }
                    }
                    else
                    {
                        CurrentArtwork = null;
                    }
                }
            });
        }

        public void Dispose()
        {
            ((IDisposable)player).Dispose();
        }

        internal async Task NewPlayList(IEnumerable<Song> songs)
        {
            await player.NewPlayList(songs);
        }
    }


    class HamPanelItem : ViewModelBase
    {
        public string Title { get; set; }

        public Type TargetType { get; set; }

        private bool isCurrent;
        public bool IsCurrent
        {
            get { return isCurrent; }
            set { SetProperty(ref isCurrent, value); }
        }

        public FontWeight ChangeWeight(bool b)
        {
            return b ? FontWeights.Bold : FontWeights.Normal;
        }

        public Visibility ChangeVisibility(bool b)
        {
            return b ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
