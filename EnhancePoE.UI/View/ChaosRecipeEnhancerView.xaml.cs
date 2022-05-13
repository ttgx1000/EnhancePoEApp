using System.ComponentModel;
using System.Windows.Input;
using Caliburn.Micro;
using EnhancePoE.App.Services;
using EnhancePoE.UI.Model;

namespace EnhancePoE.UI.View
{
    /// <summary>
    ///     Interaction logic for ChaosRecipeEnhancer.xaml
    /// </summary>
    public partial class ChaosRecipeEnhancerView
    {
        #region Fields

        private static LogWatcher Watcher { get; set; }

        #endregion

        #region Constructors

        public ChaosRecipeEnhancerView()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion

        #region Properties

        public bool IsOpen { get; set; }

        #endregion

        #region Methods

        public new virtual void Hide()
        {
            // Pause PoE client log watching while the main overlay is hidden
            if (LogWatcher.WorkerThread != null && LogWatcher.WorkerThread.IsAlive) LogWatcher.StopWatchingLogFile();

            IsOpen = false;
            base.Hide();
        }

        public new virtual void Show()
        {
            // When we show the window, start watching PoE client log to trigger auto-fetch on re-zone
            if (IoC.Get<ApplicationSettingService>().AutoFetchStashContentsOnRezone)
            {
                Watcher = new LogWatcher();
            }
            
            IsOpen = true;
            base.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
        }
        
        private void Window_MouseDown(object sender, MouseButtonEventArgs mouseButtonEvent)
        {
            if (mouseButtonEvent.ChangedButton != MouseButton.Left ||
                IoC.Get<ApplicationSettingService>().LockOverlayPosition)
            {
                return;
            }
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion
    }
}