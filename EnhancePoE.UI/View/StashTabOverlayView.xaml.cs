using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using Caliburn.Micro;
using EnhancePoE.App;
using EnhancePoE.App.Services;
using EnhancePoE.UI.Model;

namespace EnhancePoE.UI.View
{
    /// <summary>
    ///     Interaction logic for StashTabOverlayView.xaml
    /// </summary>
    public partial class StashTabOverlayView : INotifyPropertyChanged
    {
        // I don't know what i'm doing here
        private static StashTabOverlayView _instance = new StashTabOverlayView();
        public static StashTabOverlayView Instance => _instance;
        
        private static readonly ObservableCollection<TabItem> OverlayStashTabList = new ObservableCollection<TabItem>();
        private Visibility _stashBorderVisibility = Visibility.Hidden;
        private Thickness _tabHeaderGap;
        private Thickness _tabMargin;

        private double _leftStashTabOverlay;
        private double _topStashTabOverlay;
        private double _yStashTabOverlay;
        private double _xStashTabOverlay;
        private float _opacityStashTab;
        private string _stashTabBackgroundColor;
        
        public StashTabOverlayView()
        {
            InitializeComponent();
            StashTabOverlayTabControl.ItemsSource = OverlayStashTabList;

            _leftStashTabOverlay = IoC.Get<ApplicationSettingService>().LeftStashTabOverlay;
            _topStashTabOverlay = IoC.Get<ApplicationSettingService>().TopStashTabOverlay;
            _yStashTabOverlay = IoC.Get<ApplicationSettingService>().YStashTabOverlay;
            _xStashTabOverlay = IoC.Get<ApplicationSettingService>().XStashTabOverlay;
            _opacityStashTab = IoC.Get<ApplicationSettingService>().OpacityStashTab;
            _stashTabBackgroundColor = IoC.Get<ApplicationSettingService>().StashTabOverlayBackgroundColor;
        }

        #region Properties
        
        public bool IsOpen { get; set; }
        private bool IsEditing { get; set; }

        public Thickness TabHeaderGap
        {
            get => _tabHeaderGap;
            set
            {
                if (value != _tabHeaderGap)
                {
                    _tabHeaderGap = value;
                    OnPropertyChanged("TabHeaderGap");
                }
            }
        }

        public Thickness TabMargin
        {
            get => _tabMargin;
            set
            {
                if (value != _tabMargin)
                {
                    _tabMargin = value;
                    OnPropertyChanged("TabMargin");
                }
            }
        }

        public Visibility StashBorderVisibility
        {
            get => _stashBorderVisibility;
            set
            {
                _stashBorderVisibility = value;
                OnPropertyChanged("StashBorderVisibility");
            }
        }

        public double LeftStashTabOverlay
        {
            get => _leftStashTabOverlay;
            set => _leftStashTabOverlay = value;
        }

        public double TopStashTabOverlay
        {
            get => _topStashTabOverlay;
            set => _topStashTabOverlay = value;
        }

        public double YStashTabOverlay
        {
            get => _yStashTabOverlay;
            set => _yStashTabOverlay = value;
        }

        public double XStashTabOverlay
        {
            get => _xStashTabOverlay;
            set => _xStashTabOverlay = value;
        }

        public float OpacityStashTab
        {
            get => _opacityStashTab;
            set => _opacityStashTab = value;
        }

        public string StashTabBackgroundColor
        {
            get => _stashTabBackgroundColor;
            set => _stashTabBackgroundColor = value;
        }
        
        #endregion
        
        public new virtual void Hide()
        {
            Transparentize();
            EditModeButton.Content = "Edit";
            IsEditing = false;
            MouseHook.Stop();

            foreach (var i in StashTabList.StashTabs)
            {
                i.OverlayCellsList.Clear();
                i.TabHeader = null;
            }

            IsOpen = false;
            IsEditing = false;
            // MainWindow.Overlay.OpenStashOverlayButtonContent = "Stash";

            base.Hide();
        }

        // TODO: rework tab items and tab headers
        public new virtual void Show()
        {
            if (StashTabList.StashTabs.Count != 0)
            {
                IsOpen = true;
                OverlayStashTabList.Clear();
                _tabHeaderGap.Right = IoC.Get<ApplicationSettingService>().TabHeaderGap;
                _tabHeaderGap.Left = IoC.Get<ApplicationSettingService>().TabHeaderGap;
                TabMargin = new Thickness(IoC.Get<ApplicationSettingService>().TabMargin, 0, 0, 0);

                foreach (var i in StashTabList.StashTabs)
                {
                    TabItem newStashTabItem;
                    var tbk = new TextBlock
                    {
                        Text = i.TabName,
                        DataContext = i
                    };

                    tbk.SetBinding(TextBlock.BackgroundProperty, new Binding("TabHeaderColor"));
                    tbk.SetBinding(TextBlock.PaddingProperty, new Binding("TabHeaderWidth"));
                    tbk.FontSize = 16;
                    i.TabHeader = tbk;

                    if (i.Quad)
                        newStashTabItem = new TabItem
                        {
                            Header = tbk,
                            Content = new DynamicGridControlQuadView()
                            {
                                ItemsSource = i.OverlayCellsList
                            }
                        };
                    else
                        newStashTabItem = new TabItem
                        {
                            Header = tbk,
                            Content = new DynamicGridControlView
                            {
                                ItemsSource = i.OverlayCellsList
                            }
                        };

                    OverlayStashTabList.Add(newStashTabItem);
                }

                StashTabOverlayTabControl.SelectedIndex = 0;

                Data.PrepareSelling();
                Data.ActivateNextCell(true, null);
                if (IoC.Get<ApplicationSettingService>().StashOverlayHighlightMode == 2)
                    foreach (var set in Data.ItemSetListHighlight)
                    foreach (var i in set.ItemList)
                    {
                        var currTab = Data.GetStashTabFromItem(i);
                        currTab.ActivateItemCells(i);
                    }

                // MainWindow.Overlay.OpenStashOverlayButtonContent = "Hide";

                MouseHook.Start();
                base.Show();
            }
            else
            {
                MessageBox.Show("No StashTabs Available! Fetch before opening Overlay.", "Stashtab Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void StartEditMode()
        {
            MouseHook.Stop();
            EditModeButton.Content = "Save";
            StashBorderVisibility = Visibility.Visible;
            Normalize();
            IsEditing = true;
        }

        public void StopEditMode()
        {
            Transparentize();
            EditModeButton.Content = "Edit";
            StashBorderVisibility = Visibility.Hidden;
            MouseHook.Start();
            IsEditing = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Get this window's handle
            var hwnd = new WindowInteropHelper(this).Handle;

            Win32.makeTransparent(hwnd);
        }

        public void Transparentize()
        {
            Trace.WriteLine("make transparent");
            var hwnd = new WindowInteropHelper(this).Handle;

            Win32.makeTransparent(hwnd);
        }

        public void Normalize()
        {
            Trace.WriteLine("make normal");
            var hwnd = new WindowInteropHelper(this).Handle;

            Win32.makeNormal(hwnd);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
        }

        public void HandleEditButton()
        {
            if (StashTabOverlayView.Instance.IsEditing)
                StopEditMode();
            else
                StartEditMode();
        }

        private void EditModeButton_Click(object sender, RoutedEventArgs e)
        {
            HandleEditButton();
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