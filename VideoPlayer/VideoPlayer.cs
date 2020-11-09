using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VideoPlayer
{
    [TemplatePart(Name = "PART_MediaElement", Type = typeof(MediaElement))]
    public class VideoPlayer : Control
    {
        public static readonly RoutedEvent StateChangedEvent;
        public static readonly RoutedUICommand LoadCommand;
        public static readonly DependencyProperty StateProperty;
        internal static readonly DependencyPropertyKey StatePropertyKey;
        private MediaElement _mediaElement;


        static VideoPlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoPlayer), new FrameworkPropertyMetadata(typeof(VideoPlayer)));
            StatePropertyKey = DependencyProperty.RegisterReadOnly("State", typeof(VideoPlayerState), typeof(VideoPlayer), new FrameworkPropertyMetadata(VideoPlayerState.Stopped, new PropertyChangedCallback(OnStateChanged)));
            StateProperty = StatePropertyKey.DependencyProperty;
            StateChangedEvent = EventManager.RegisterRoutedEvent("StateChanged", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(VideoPlayer));
            LoadCommand = new RoutedUICommand("Load", "Load Video", typeof(VideoPlayer));
            CommandManager.RegisterClassCommandBinding(typeof(VideoPlayer), new CommandBinding(LoadCommand, OnLoadExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(VideoPlayer), new CommandBinding(MediaCommands.Play, OnPlayExecuted, OnPlayCanExecute));
            CommandManager.RegisterClassCommandBinding(typeof(VideoPlayer), new CommandBinding(MediaCommands.Stop, OnStopExecuted, OnStopCanExecute));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template != null)
            {
                _mediaElement = Template.FindName("PART_MediaElement", this) as MediaElement;
                if (_mediaElement != null)
                {
                    _mediaElement.LoadedBehavior = MediaState.Manual;
                    _mediaElement.MediaOpened += MediaElement_MediaOpened;
                    _mediaElement.MediaEnded += MediaElement_MediaEnded;
                    _mediaElement.Source = Source;
                }
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            if (_mediaElement != null)
            {
                _mediaElement.Stop();
                State = VideoPlayerState.Stopped;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            Play();
        }

        private void Play()
        {
            if (_mediaElement != null)
            {
                _mediaElement.Play();
                State = VideoPlayerState.Playing;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Sourceource", typeof(Uri), typeof(VideoPlayer), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoPlayer videoPlayer = d as VideoPlayer;
            if (e.NewValue != null)
            {
                videoPlayer._mediaElement.Source = (Uri)e.NewValue;
                videoPlayer.Play();
            }
            else
            {
                videoPlayer._mediaElement.Source = null;
            }
        }

        public VideoPlayerState State
        {
            get { return (VideoPlayerState)GetValue(StateProperty); }
            internal set { SetValue(StatePropertyKey, value); }
        }

        public event RoutedEventHandler StateChanged
        {
            add { AddHandler(StateChangedEvent, value); }
            remove { RemoveHandler(StateChangedEvent, value); }
        }

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoPlayer videoPlayer = d as VideoPlayer;
            if (e.NewValue != e.OldValue)
            {
                RoutedEventArgs args = new RoutedEventArgs(StateChangedEvent);
                videoPlayer.RaiseEvent(args);
            }
        }

        private static void OnStopCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is VideoPlayer videoPlayer && videoPlayer.State != VideoPlayerState.Stopped) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private static void OnStopExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as VideoPlayer).Stop();
        }

        private static void OnPlayCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is VideoPlayer videoPlayer && videoPlayer.State != VideoPlayerState.Playing) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private static void OnPlayExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as VideoPlayer).Play();
        }

        private static void OnLoadExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "*.wmv|*.wmv|*.mpg|*.mpg|*.mpeg|*.mpeg|*.avi|*.avi"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                (sender as VideoPlayer).Source = new Uri(openFileDialog.FileName);
            }
        }

        public enum VideoPlayerState
        {
            Playing,
            Stopped
        }
    }
}
