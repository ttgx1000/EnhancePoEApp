using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Caliburn.Micro;
using EnhancePoE.App.Helpers;
using EnhancePoE.App.Services;
using EnhancePoE.UI.Model;
using EnhancePoE.UI.ViewModels;
using Application = System.Windows.Application;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;
using MessageBox = System.Windows.MessageBox;

namespace EnhancePoE.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : INotifyPropertyChanged
    {
        private const string AppVersion = "1.5.0.0";

        private ApplicationSettingService _applicationSettingService;

        public static readonly ChaosRecipeEnhancerView Overlay = new ChaosRecipeEnhancerView();

        private readonly NotifyIcon _notifyIcon = new NotifyIcon();

        private Visibility _indicesVisible = Visibility.Hidden;
        private Visibility _nameVisible = Visibility.Hidden;
        private Visibility _nameSuffixVisible = Visibility.Hidden;
        private Visibility _mainLeagueVisible = Visibility.Visible;
        private Visibility _customLeagueVisible = Visibility.Hidden;
        private ContextMenu _contextMenu;
        private MenuItem _menuItem;
        private MenuItem _menuItemUpdate;
        private bool _trayClose;

        public ApplicationSettingService ApplicationSettingService
        {
            get => _applicationSettingService;
            set => _applicationSettingService = value;
        }

        public MainWindowView()
        {
            _applicationSettingService = new ApplicationSettingService();
            
            InitializeComponent();
            DataContext = this;
            AutoUpdateHelper.InitializeAutoUpdater(AppVersion);

            // if (!string.IsNullOrEmpty(Settings.Default.FilterChangeSoundFileLocation) && !FilterSoundLocationDialog.Content.Equals("Default Sound"))
            //     Data.Player.Open(new Uri(Settings.Default.FilterChangeSoundFileLocation));
            if (!string.IsNullOrEmpty(_applicationSettingService.FilterChangeSoundFileLocation) &&
                !FilterSoundLocationDialog.Content.Equals("Default Sound"))
                Data.Player.Open(new Uri(_applicationSettingService.FilterChangeSoundFileLocation));
            else
                Data.Player.Open(new Uri(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    @"Sounds\filterchanged.mp3")));

            // if (!string.IsNullOrEmpty(Settings.Default.ItemPickupSoundFileLocation) && !ItemPickupLocationDialog.Content.Equals("Default Sound"))
            //     Data.PlayerSet.Open(new Uri(Settings.Default.ItemPickupSoundFileLocation));
            if (!string.IsNullOrEmpty(_applicationSettingService.ItemPickupSoundFileLocation) &&
                !ItemPickupLocationDialog.Content.Equals("Default Sound"))
                Data.PlayerSet.Open(new Uri(_applicationSettingService.ItemPickupSoundFileLocation));
            else
                Data.PlayerSet.Open(new Uri(Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Sounds\itemsPickedUp.mp3")));

            //TESTING SETTINGS RESET
            //if (Debugger.IsAttached)
            //    Properties.Settings.Default.Reset();

            // initialize stash tabs
            //DataContext = stashTabsModel;

            // Populate the league dropdown
            // if (Settings.Default.MainLeague)
            if (_applicationSettingService.SelectFromMainLeagues)
            {
                MainLeagueComboBox.ItemsSource = ApiAdapter.GetAllLeagueNames();
            }

            InitializeColors();
            // InitializeHotkeys();
            InitializeTray();
            LoadModeVisibility();

            // add Action to MouseHook
            MouseHook.MouseAction += Coordinates.Event;
        }

        public static bool SettingsComplete { get; set; }

        // ReSharper disable once UnusedMember.Global
        public static string AppVersionText { get; set; } = "v." + AppVersion;

        // ReSharper disable once UnusedMember.Local
        private static string RunButtonContent { get; set; } = "Run Overlay";

        public Visibility IndicesVisible
        {
            get => _indicesVisible;
            set
            {
                if (_indicesVisible != value)
                {
                    _indicesVisible = value;
                    OnPropertyChanged("IndicesVisible");
                }
            }
        }

        public Visibility MainLeagueVisible
        {
            get => _mainLeagueVisible;
            set
            {
                if (_mainLeagueVisible != value)
                {
                    _mainLeagueVisible = value;
                    OnPropertyChanged("MainLeagueVisible");
                }
            }
        }

        public Visibility CustomLeagueVisible
        {
            get => _customLeagueVisible;
            set
            {
                if (_customLeagueVisible != value)
                {
                    _customLeagueVisible = value;
                    OnPropertyChanged("CustomLeagueVisible");
                }
            }
        }

        public Visibility NameVisible
        {
            get => _nameVisible;
            set
            {
                if (_nameVisible != value)
                {
                    _nameVisible = value;
                    OnPropertyChanged("NameVisible");
                }
            }
        }

        public Visibility NameSuffixVisible
        {
            get => _nameSuffixVisible;
            set
            {
                if (_nameSuffixVisible != value)
                {
                    _nameSuffixVisible = value;
                    OnPropertyChanged("NameSuffixVisible");
                }
            }
        }

        private void InitializeHotkeys()
        {
            IoC.Get<HotkeysManager>().SetupSystemHook();
            IoC.Get<HotkeysManager>().GetRefreshHotkey();
            IoC.Get<HotkeysManager>().GetToggleHotkey();
            IoC.Get<HotkeysManager>().GetStashTabHotkey();
            AddAllHotkeys();
        }

        private void InitializeColors()
        {
            // if (Settings.Default.ColorBoots != "") ColorBootsPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.ColorBoots);
            // if (Settings.Default.ColorChest != "") ColorChestPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.ColorChest);
            // if (Settings.Default.ColorWeapon != "") ColorWeaponsPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.ColorWeapon);
            // if (Settings.Default.ColorGloves != "") ColorGlovesPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.ColorGloves);
            // if (Settings.Default.ColorHelmet != "") ColorHelmetPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.ColorHelmet);
            // if (Settings.Default.ColorStash != "") ColorStashPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.ColorStash);
            // if (Settings.Default.StashTabBackgroundColor != "") ColorStashBackgroundPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.StashTabBackgroundColor);
            // if (Settings.Default.ColorRing != "") ColorRingPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.ColorRing);
            // if (Settings.Default.ColorAmulet != "") ColorAmuletPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.ColorAmulet);
            // if (Settings.Default.ColorBelt != "") ColorBeltPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(Settings.Default.ColorBelt);
            if (_applicationSettingService.ColorBoots != "")
                ColorBootsPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.ColorBoots);
            if (_applicationSettingService.ColorChest != "")
                ColorChestPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.ColorChest);
            if (_applicationSettingService.ColorWeapon != "")
                ColorWeaponsPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.ColorWeapon);
            if (_applicationSettingService.ColorGloves != "")
                ColorGlovesPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.ColorGloves);
            if (_applicationSettingService.ColorHelmet != "")
                ColorHelmetPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.ColorHelmet);
            if (_applicationSettingService.ColorStash != "")
                ColorStashPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.ColorStash);
            if (_applicationSettingService.StashTabOverlayBackgroundColor != "")
                ColorStashBackgroundPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.StashTabOverlayBackgroundColor);
            if (_applicationSettingService.ColorRing != "")
                ColorRingPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.ColorRing);
            if (_applicationSettingService.ColorAmulet != "")
                ColorAmuletPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.ColorAmulet);
            if (_applicationSettingService.ColorBelt != "")
                ColorBeltPicker.SelectedColor =
                    (Color)ColorConverter.ConvertFromString(_applicationSettingService.ColorBelt);
        }

        // creates tray icon with menu
        private void InitializeTray()
        {
            _notifyIcon.Icon = Properties.Resources.coin;
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick +=
                delegate
                {
                    Show();
                    WindowState = WindowState.Normal;
                };

            new Container();
            _contextMenu = new ContextMenu();
            _menuItem = new MenuItem();
            _menuItemUpdate = new MenuItem();

            // Initialize contextMenu1
            _contextMenu.MenuItems.AddRange(new[] { _menuItem, _menuItemUpdate });

            // Initialize menuItem1
            _menuItem.Index = 1;
            _menuItem.Text = "E&xit";
            _menuItem.Click += MenuItem_Click;

            // Initialize menuItemUpdate
            _menuItemUpdate.Index = 0;
            _menuItemUpdate.Text = "C&heck for Updates";
            _menuItemUpdate.Click += CheckForUpdates_Click;

            _notifyIcon.ContextMenu = _contextMenu;
        }

        private void CheckForUpdates_Click(object Sender, EventArgs e)
        {
            AutoUpdateHelper.CheckForUpdates();
        }

        // Close the form, which closes the application.
        private void MenuItem_Click(object Sender, EventArgs e)
        {
            _trayClose = true;
            Close();
        }

        // Minimize to system tray when application is closed.
        protected override void OnClosing(CancelEventArgs e)
        {
            // if hideOnClose
            // setting cancel to true will cancel the close request
            // so the application is not closed
            if (_applicationSettingService.CloseToTray && !_trayClose)
            {
                e.Cancel = true;
                Hide();
                base.OnClosing(e);
            }

            if (!_applicationSettingService.CloseToTray || _trayClose)
            {
                _notifyIcon.Visible = false;
                MouseHook.Stop();
                HotkeysManager.ShutdownSystemHook();

                // Settings.Default.Save();
                _applicationSettingService.Save();

                if (LogWatcher.WorkerThread != null && LogWatcher.WorkerThread.IsAlive)
                    LogWatcher.StopWatchingLogFile();
                Application.Current.Shutdown();
            }
        }

        public void RunOverlay()
        {
            if (Overlay.IsOpen)
            {
                Overlay.Hide();
                if (StashTabOverlayView.Instance.IsOpen) StashTabOverlayView.Instance.Hide();
                RunButton.Content = "Run Overlay";
            }
            else
            {
                if (CheckAllSettings())
                {
                    Overlay.Show();
                    RunButton.Content = "Stop Overlay";
                }
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            var ready = CheckAllSettings();

            if (ready)
            {
                RunOverlay();
            }
        }

        public static void RunStashTabOverlay()
        {
            if (StashTabOverlayView.Instance.IsOpen)
                StashTabOverlayView.Instance.Hide();
            else
                StashTabOverlayView.Instance.Show();
        }

        public void AddAllHotkeys()
        {
            // if (Settings.Default.HotkeyRefresh != "< not set >") HotkeysManager.AddHotkey(HotkeysManager.refreshModifier, HotkeysManager.refreshKey, Overlay.RunFetching);
            // if (Settings.Default.HotkeyToggle != "< not set >") HotkeysManager.AddHotkey(HotkeysManager.toggleModifier, HotkeysManager.toggleKey, RunOverlay);
            // if (Settings.Default.HotkeyStashTab != "< not set >") HotkeysManager.AddHotkey(HotkeysManager.stashTabModifier, HotkeysManager.stashTabKey, RunStashTabOverlay);
            if (_applicationSettingService.HotkeyRefresh != "< not set >")
                IoC.Get<HotkeysManager>().AddHotkey(HotkeysManager.refreshModifier, HotkeysManager.refreshKey,
                    MainOverlayBase.Instance.RunFetching);
            if (_applicationSettingService.HotkeyToggle != "< not set >")
                IoC.Get<HotkeysManager>().AddHotkey(HotkeysManager.toggleModifier, HotkeysManager.toggleKey, RunOverlay);
            if (_applicationSettingService.HotkeyStashTab != "< not set >")
                IoC.Get<HotkeysManager>().AddHotkey(HotkeysManager.stashTabModifier, HotkeysManager.stashTabKey,
                    RunStashTabOverlay);
        }

        public void RemoveAllHotkeys()
        {
            IoC.Get<HotkeysManager>().RemoveRefreshHotkey();
            IoC.Get<HotkeysManager>().RemoveStashTabHotkey();
            IoC.Get<HotkeysManager>().RemoveToggleHotkey();
        }

        private string GetSoundFilePath()
        {
            var open = new OpenFileDialog();
            open.Filter = "MP3|*.mp3";
            var res = open.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.OK) return open.FileName;

            return null;
        }

        private void ColorBootsPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.ColorBoots = ColorBootsPicker.SelectedColor.ToString();
            _applicationSettingService.ColorBoots = ColorBootsPicker.SelectedColor.ToString();
        }

        private void ColorGlovesPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.ColorGloves = ColorGlovesPicker.SelectedColor.ToString();
            _applicationSettingService.ColorGloves = ColorGlovesPicker.SelectedColor.ToString();
        }

        private void ColorHelmetPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.ColorHelmet = ColorHelmetPicker.SelectedColor.ToString();
            _applicationSettingService.ColorHelmet = ColorHelmetPicker.SelectedColor.ToString();
        }

        private void ColorChestPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.ColorChest = ColorChestPicker.SelectedColor.ToString();
            _applicationSettingService.ColorChest = ColorChestPicker.SelectedColor.ToString();
        }

        private void ColorWeaponsPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.ColorWeapon = ColorWeaponsPicker.SelectedColor.ToString();
            _applicationSettingService.ColorWeapon = ColorWeaponsPicker.SelectedColor.ToString();
        }

        private void ColorStashPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.ColorStash = ColorStashPicker.SelectedColor.ToString();
            _applicationSettingService.ColorStash = ColorStashPicker.SelectedColor.ToString();
        }

        private void ColorRingPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.ColorRing = ColorRingPicker.SelectedColor.ToString();
            _applicationSettingService.ColorRing = ColorRingPicker.SelectedColor.ToString();
        }

        private void ColorAmuletPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.ColorAmulet = ColorAmuletPicker.SelectedColor.ToString();
            _applicationSettingService.ColorAmulet = ColorAmuletPicker.SelectedColor.ToString();
        }

        private void ColorBeltPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.ColorBelt = ColorBeltPicker.SelectedColor.ToString();
            _applicationSettingService.ColorBelt = ColorBeltPicker.SelectedColor.ToString();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainGrid.Focus();
        }

        // TODO: [Revist] Lots of validation we can move to the ApplicationSettings service no?
        private bool CheckAllSettings()
        {
            // var accName = Settings.Default.accName;
            // var sessId = Settings.Default.SessionId;
            // var league = Settings.Default.League;
            // var lootFilterLocation = Settings.Default.LootFilterLocation;
            // var lootFilterActive = Settings.Default.LootFilterActive;
            // var logLocation = Settings.Default.LogLocation;
            // var autoFetch = Settings.Default.AutoFetch;            
            var accName = _applicationSettingService.AccountName;
            var sessId = _applicationSettingService.SessionId;
            var league = _applicationSettingService.CurrentLeague;
            var lootFilterLocation = _applicationSettingService.LootFilterFileLocation;
            var lootFilterActive = _applicationSettingService.LootFilterManipulationActive;
            var logLocation = _applicationSettingService.GGGClientLogFileLocation;
            var autoFetch = _applicationSettingService.AutoFetchStashContentsOnRezone;

            var missingSettings = new List<string>();

            if (accName == "") missingSettings.Add("- Account Name \n");
            if (sessId == "") missingSettings.Add("- PoE Session ID \n");
            if (league == "") missingSettings.Add("- League \n");

            if (lootFilterActive)
            {
                if (lootFilterLocation == "") missingSettings.Add("- Loot Filter Location \n");
            }

            if (autoFetch)
                if (logLocation == "")
                    missingSettings.Add("- Log File Location \n");

            // switch (Settings.Default.StashTabMode)
            switch (_applicationSettingService.StashTabQueryMode)
            {
                case 0:
                {
                    // if (Settings.Default.StashTabIndices == "") missingSettings.Add("- StashTab Index");
                    if (_applicationSettingService.StashTabQueryIndices == "") missingSettings.Add("- StashTab Index");
                    break;
                }
                case 1:
                case 2:
                {
                    if (_applicationSettingService.StashTabQueryString == "") missingSettings.Add("- StashTab Name");
                    break;
                }
            }

            if (missingSettings.Count > 0)
            {
                SettingsComplete = false;
            }
            else
            {
                SettingsComplete = true;
                return true;
            }

            var errorMessage = missingSettings.Aggregate("Please add: \n", (current, setting) => current + setting);
            MessageBox.Show(errorMessage, "Missing Settings", MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        private void VolumeSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Data.PlayNotificationSound();
        }

        private void ColorStashBackgroundPicker_SelectedColorChanged(object sender,
            RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Settings.Default.StashTabBackgroundColor = ColorStashBackgroundPicker.SelectedColor.ToString();
            _applicationSettingService.StashTabOverlayBackgroundColor =
                ColorStashBackgroundPicker.SelectedColor.ToString();
        }

        private void CustomHotkeyToggle_Click(object sender, RoutedEventArgs e)
        {
            var isWindowOpen = false;
            foreach (Window w in Application.Current.Windows)
                if (w is HotkeyWindowView)
                    isWindowOpen = true;

            if (isWindowOpen) return;
            var hotkeyDialog = new HotkeyWindowView(this, "toggle");
            hotkeyDialog.Show();
        }

        private void RefreshHotkey_Click(object sender, RoutedEventArgs e)
        {
            var isWindowOpen = false;
            foreach (Window w in Application.Current.Windows)
                if (w is HotkeyWindowView)
                    isWindowOpen = true;

            if (isWindowOpen) return;
            var hotkeyDialog = new HotkeyWindowView(this, "refresh");
            hotkeyDialog.Show();
        }

        private void StashTabHotkey_Click(object sender, RoutedEventArgs e)
        {
            var isWindowOpen = false;
            foreach (Window w in Application.Current.Windows)
                if (w is HotkeyWindowView)
                    isWindowOpen = true;

            if (!isWindowOpen)
            {
                var hotkeyDialog = new HotkeyWindowView(this, "stashtab");
                hotkeyDialog.Show();
            }
        }

        private void LootFilterFileDialog_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "LootFilter|*.filter";
            var res = open.ShowDialog();

            if (res != System.Windows.Forms.DialogResult.OK) return;

            var filename = open.FileName;
            // Settings.Default.LootFilterLocation = filename;
            _applicationSettingService.LootFilterFileLocation = filename;

            LootFilterFileDialog.Content = filename;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadModeVisibility();
        }

        private void LoadModeVisibility()
        {
            // switch (Settings.Default.StashTabMode)
            switch (_applicationSettingService.StashTabQueryMode)
            {
                case 1:
                    NameVisible = Visibility.Visible;
                    NameSuffixVisible = Visibility.Hidden;
                    IndicesVisible = Visibility.Hidden;
                    break;
                case 2:
                    NameSuffixVisible = Visibility.Visible;
                    NameVisible = Visibility.Hidden;
                    IndicesVisible = Visibility.Hidden;
                    break;
                default:
                    IndicesVisible = Visibility.Visible;
                    NameVisible = Visibility.Hidden;
                    NameSuffixVisible = Visibility.Hidden;
                    break;
            }

            // if (Settings.Default.CustomLeague == false)
            if (_applicationSettingService.InputCustomLeagueName == false)
            {
                CustomLeagueVisible = Visibility.Hidden;
                MainLeagueVisible = Visibility.Visible;
            }
            else
            {
                CustomLeagueVisible = Visibility.Visible;
                MainLeagueVisible = Visibility.Hidden;
            }
        }

        private void TabHeaderGapSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // StashTabOverlay.TabHeaderGap = new Thickness(Settings.Default.TabHeaderGap, 0, Settings.Default.TabHeaderGap, 0);
            StashTabOverlayView.Instance.TabHeaderGap = new Thickness(_applicationSettingService.TabHeaderGap, 0,
                _applicationSettingService.TabHeaderGap, 0);
        }

        private void TabHeaderWidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (StashTabList.StashTabs.Count <= 0) return;

            foreach (var s in StashTabList.StashTabs)
                // s.TabHeaderWidth = new Thickness(Settings.Default.TabHeaderWidth, 2, Settings.Default.TabHeaderWidth, 2);
                s.TabHeaderWidth = new Thickness(_applicationSettingService.TabHeaderWidth, 2,
                    _applicationSettingService.TabHeaderWidth, 2);
        }

        private void TabHeaderMarginSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // StashTabOverlay.TabMargin = new Thickness(Settings.Default.TabMargin, 0, 0, 0);
            StashTabOverlayView.Instance.TabMargin = new Thickness(_applicationSettingService.TabMargin, 0, 0, 0);
        }

        private void SaveButton_Click_1(object sender, RoutedEventArgs e)
        {
            // Settings.Default.Save();
            _applicationSettingService.Save();
        }

        private void OverlayModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // switch (Settings.Default.OverlayMode)
            switch (_applicationSettingService.MainOverlayMode)
            {
                case 0:
                    Overlay.MainOverlayContentControl.Content = new MainOverlayContentView();
                    break;
                case 1:
                    Overlay.MainOverlayContentControl.Content = new MainOverlayContentMinifiedView();
                    break;
                case 2:
                    Overlay.MainOverlayContentControl.Content = new MainOverlayOnlyButtonsView();
                    break;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This will reset all of your settings!", "Reset Settings",
                MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    // Settings.Default.Reset();
                    _applicationSettingService.ResetSettings();
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Cancel:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChaosRecipeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Settings.Default.RegalRecipe = false;
            _applicationSettingService.RegalRecipe = false;
        }

        private void CustomLeagueCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Settings.Default.CustomLeague = true;
            // Settings.Default.MainLeague = false;
            _applicationSettingService.SelectFromMainLeagues = false;
            _applicationSettingService.InputCustomLeagueName = true;

            LoadModeVisibility();
        }

        private void CustomLeagueCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Settings.Default.CustomLeague = false;
            // Settings.Default.MainLeague = true;
            _applicationSettingService.SelectFromMainLeagues = true;
            _applicationSettingService.InputCustomLeagueName = false;

            MainLeagueComboBox.ItemsSource = ApiAdapter.GetAllLeagueNames();
            LoadModeVisibility();
        }

        private void RegalRecipeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Settings.Default.ChaosRecipe = false;
            _applicationSettingService.ChaosRecipe = false;
        }

        private void LogLocationDialog_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "Text|Client.txt";
            var res = open.ShowDialog();

            if (res != System.Windows.Forms.DialogResult.OK) return;

            var filename = open.FileName;

            if (filename.EndsWith("Client.txt"))
            {
                // Settings.Default.LogLocation = filename;
                _applicationSettingService.GGGClientLogFileLocation = filename;

                LogLocationDialog.Content = filename;
            }
            else
            {
                MessageBox.Show(
                    "Invalid file selected. Make sure you're selecting the \"Client.txt\" file located in your main Path of Exile installation folder.",
                    "Missing Settings", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AutoFetchCheckBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void AutoFetchCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LogWatcher.WorkerThread != null && LogWatcher.WorkerThread.IsAlive) LogWatcher.WorkerThread.Abort();
        }

        // TODO: [Revisit] This was a combo box but later changed to a selection list; does this even work?
        private void ShowNumbersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainOverlayBase.Instance.AmountsVisibility =
                // Settings.Default.ShowItemAmount != 0 
                _applicationSettingService.MainOverlayItemAmountDisplayMode != 0
                    ? Visibility.Visible
                    : Visibility.Hidden;
        }

        private void FilterSoundLocationDialog_OnClick(object sender, RoutedEventArgs e)
        {
            var soundFilePath = GetSoundFilePath();

            if (soundFilePath == null) return;

            // Settings.Default.FilterChangeSoundFileLocation = soundFilePath;
            _applicationSettingService.FilterChangeSoundFileLocation = soundFilePath;

            FilterSoundLocationDialog.Content = soundFilePath;
            Data.Player.Open(new Uri(soundFilePath));

            Data.PlayNotificationSound();
        }

        private void ItemPickupLocationDialog_OnClick(object sender, RoutedEventArgs e)
        {
            var soundFilePath = GetSoundFilePath();

            if (soundFilePath == null) return;

            // Settings.Default.ItemPickupSoundFileLocation = soundFilePath;
            _applicationSettingService.ItemPickupSoundFileLocation = soundFilePath;

            ItemPickupLocationDialog.Content = soundFilePath;
            Data.PlayerSet.Open(new Uri(soundFilePath));

            Data.PlayNotificationSoundSetPicked();
        }

        #region INotifyPropertyChanged implementation

        // Basically, the UI thread subscribes to this event and update the binding if the received Property Name correspond to the Binding Path element
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}