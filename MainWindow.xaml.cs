using System;
using System.Threading.Tasks;
using System.Windows;
using Windows.Media.Control;

namespace FoobarNowPlaying
{
    public partial class MainWindow : Window
    {
        private GlobalSystemMediaTransportControlsSessionManager? _sessionManager;
        private GlobalSystemMediaTransportControlsSession? _currentSession;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeMediaManager();
        }

        private async Task InitializeMediaManager()
        {
            _sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            _sessionManager.CurrentSessionChanged += SessionManager_CurrentSessionChanged;
            AttachToCurrentSession();
        }

        private void SessionManager_CurrentSessionChanged(
            GlobalSystemMediaTransportControlsSessionManager sender,
            CurrentSessionChangedEventArgs args)
        {
            Dispatcher.Invoke(AttachToCurrentSession);
        }

        private void AttachToCurrentSession()
        {
            if (_currentSession != null)
                _currentSession.MediaPropertiesChanged -= Session_MediaPropertiesChanged;

            _currentSession = _sessionManager?.GetCurrentSession();

            if (_currentSession != null)
            {
                _currentSession.MediaPropertiesChanged += Session_MediaPropertiesChanged;
                _ = UpdateMediaPropertiesAsync();
            }
            else
            {
                ArtistText.Text = "No track playing";
                TitleText.Text = "";
            }
        }

        private void Session_MediaPropertiesChanged(
            GlobalSystemMediaTransportControlsSession sender,
            MediaPropertiesChangedEventArgs args)
        {
            Dispatcher.Invoke(() => _ = UpdateMediaPropertiesAsync());
        }

        private async Task UpdateMediaPropertiesAsync()
        {
            try
            {
                if (_currentSession == null)
                {
                    ArtistText.Text = "No track playing";
                    TitleText.Text = "";
                    return;
                }

                var props = await _currentSession.TryGetMediaPropertiesAsync();

                if (string.IsNullOrWhiteSpace(props.Artist) && string.IsNullOrWhiteSpace(props.Title))
                {
                    ArtistText.Text = "No track playing";
                    TitleText.Text = "";
                }
                else
                {
                    ArtistText.Text = props.Artist;
                    TitleText.Text = props.Title;
                }
            }
            catch (Exception ex)
            {
                ArtistText.Text = "Error reading media";
                TitleText.Text = ex.Message;
            }
        }
    }
}
