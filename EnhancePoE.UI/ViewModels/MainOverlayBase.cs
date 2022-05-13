using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Caliburn.Micro;
using EnhancePoE.App.Services;
using EnhancePoE.UI.Model;
using EnhancePoE.UI.View;
using Microsoft.Xaml.Behaviors.Core;

namespace EnhancePoE.UI.ViewModels
{
    public class MainOverlayBase : PropertyChangedBase
    {
        // I don't know what i'm doing here
        private static MainOverlayBase _instance = new MainOverlayBase();
        public static MainOverlayBase Instance => _instance;

        #region Fields

        // Tracks whether or not we're currently fetching (i.e. If the 'Fetch' button was pressed recently)
        // This is to avoid fetching multiple times in quick succession and causing rate limit exceeded problems
        private static bool FetchingActive { get; set; }

        // Tracks whether or not calculations are currently active
        // TODO What is a 'calculation'? 
        private static bool CalculationActive { get; set; }
        
        private const double DeactivatedOpacity = .1;
        private const double ActivatedOpacity = 1;
        private const int FetchCooldown = 30;

        private string _openStashOverlayButtonContent = "Stash";
        private double _shadowOpacity;
        private string _warningMessage;
        private bool _isIndeterminate;
        private bool _fetchButtonEnabled = true;
        private SolidColorBrush _fetchButtonColor = Brushes.Green;
        private Visibility _amountsVisibility = Visibility.Hidden;
        private Visibility _warningMessageVisibility = Visibility.Hidden;

        // Defines the number of a given piece of gear you currently have
        private int _amuletsAmount;
        private int _ringsAmount;
        private int _beltsAmount;
        private int _helmetsAmount;
        private int _glovesAmount;
        private int _bootsAmount;
        private int _chestsAmount;
        private int _weaponsAmount;
        
        // Defines whether or not to 'Activate' a gear icon based on the full set threshold
        // Upon initialization, the icons are clearly visible (i.e. 'Activated', not translucent at all)
        private double _amuletsOpacity = ActivatedOpacity;
        private double _ringsOpacity = ActivatedOpacity;
        private double _beltsOpacity = ActivatedOpacity;
        private double _helmetOpacity = ActivatedOpacity;
        private double _glovesOpacity = ActivatedOpacity;
        private double _bootsOpacity = ActivatedOpacity;
        private double _chestsOpacity = ActivatedOpacity;
        private double _weaponOpacity = ActivatedOpacity;

        private string _colorGloves;
        private string _colorBoots;
        private string _colorChest;
        private string _colorWeapon;
        private string _colorHelmet;
        private string _colorRing;
        private string _colorAmulet;
        private string _colorBelt;
        
        private string _fullSetsText = "0";
        
        #endregion

        #region Constructor

        public MainOverlayBase()
        {
            FetchCommand = new ActionCommand(FetchData);
            ReloadItemFilterCommand = new ActionCommand(ReloadItemFilter);
            RunStashOverlayCommand = new ActionCommand(RunStashOverlay);
            
            _colorGloves = IoC.Get<ApplicationSettingService>().ColorGloves;
            _colorBoots = IoC.Get<ApplicationSettingService>().ColorBoots;
            _colorChest = IoC.Get<ApplicationSettingService>().ColorChest;
            _colorWeapon = IoC.Get<ApplicationSettingService>().ColorWeapon;
            _colorHelmet = IoC.Get<ApplicationSettingService>().ColorHelmet;
            _colorRing = IoC.Get<ApplicationSettingService>().ColorRing;
            _colorAmulet = IoC.Get<ApplicationSettingService>().ColorAmulet;
            _colorBelt = IoC.Get<ApplicationSettingService>().ColorBelt;
        }

        #endregion
        
        #region Properties

        public ICommand FetchCommand { get; }
        
        public ICommand ReloadItemFilterCommand { get; }
        
        public ICommand RunStashOverlayCommand { get; }
        
         public string ColorGloves
        {
            get => _colorGloves;
            set
            {
                if (value == _colorGloves) return;
                _colorGloves = value;
                NotifyOfPropertyChange(() => ColorGloves);
            }
        }

        public string ColorBoots
        {
            get => _colorBoots;
            set
            {
                if (value == _colorBoots) return;
                _colorBoots = value;
                NotifyOfPropertyChange(() => ColorBoots);
            }
        }

        public string ColorChest
        {
            get => _colorChest;
            set
            {
                if (value == _colorChest) return;
                _colorChest = value;
                NotifyOfPropertyChange(() => ColorChest);
            }
        }

        public string ColorWeapon
        {
            get => _colorWeapon;
            set
            {
                if (value == _colorWeapon) return;
                _colorWeapon = value;
                NotifyOfPropertyChange(() => ColorWeapon);
            }
        }

        public string ColorHelmet
        {
            get => _colorHelmet;
            set
            {
                if (value == _colorHelmet) return;
                _colorHelmet = value;
                NotifyOfPropertyChange(() => ColorHelmet);
            }
        }

        public string ColorRing
        {
            get => _colorRing;
            set
            {
                if (value == _colorRing) return;
                _colorRing = value;
                NotifyOfPropertyChange(() => ColorRing);
            }
        }

        public string ColorAmulet
        {
            get => _colorAmulet;
            set
            {
                if (value == _colorAmulet) return;
                _colorAmulet = value;
                NotifyOfPropertyChange(() => ColorAmulet);
            }
        }

        public string ColorBelt
        {
            get => _colorBelt;
            set
            {
                if (value == _colorBelt) return;
                _colorBelt = value;
                NotifyOfPropertyChange(() => ColorBelt);
            }
        }
        
        public string WarningMessage
        {
            get => _warningMessage;
            set
            {
                _warningMessage = value;
                NotifyOfPropertyChange(() => WarningMessage);
            }
        }

        public Visibility WarningMessageVisibility
        {
            get => _warningMessageVisibility;
            set
            {
                _warningMessageVisibility = value;
                NotifyOfPropertyChange(() => WarningMessageVisibility);
            }
        }

        public double ShadowOpacity
        {
            get => _shadowOpacity;
            set
            {
                _shadowOpacity = value;
                NotifyOfPropertyChange(() => ShadowOpacity);
            }
        }

        public string FullSetsText
        {
            get => _fullSetsText;
            set
            {
                _fullSetsText = value;
                NotifyOfPropertyChange(() => FullSetsText);
            }
        }

        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            set
            {
                _isIndeterminate = value;
                NotifyOfPropertyChange(() => IsIndeterminate);
            }
        }

        public string OpenStashOverlayButtonContent
        {
            get => _openStashOverlayButtonContent;
            set
            {
                _openStashOverlayButtonContent = value;
                NotifyOfPropertyChange(() => OpenStashOverlayButtonContent);
            }
        }

        #region Gear Icon Opacity Getters & Setters

        public double AmuletsOpacity
        {
            get => _amuletsOpacity;
            set
            {
                _amuletsOpacity = value;
                NotifyOfPropertyChange(() => AmuletsOpacity);
            }
        }

        public double RingsOpacity
        {
            get => _ringsOpacity;
            set
            {
                _ringsOpacity = value;
                NotifyOfPropertyChange(() => RingsOpacity);
            }
        }

        public double BeltsOpacity
        {
            get => _beltsOpacity;
            set
            {
                _beltsOpacity = value;
                NotifyOfPropertyChange(() => BeltsOpacity);
            }
        }

        public double HelmetOpacity
        {
            get => _helmetOpacity;
            set
            {
                _helmetOpacity = value;
                NotifyOfPropertyChange(() => HelmetOpacity);
            }
        }

        public double GlovesOpacity
        {
            get => _glovesOpacity;
            set
            {
                _glovesOpacity = value;
                NotifyOfPropertyChange(() => GlovesOpacity);
            }
        }

        public double BootsOpacity
        {
            get => _bootsOpacity;
            set
            {
                _bootsOpacity = value;
                NotifyOfPropertyChange(() => BootsOpacity);
            }
        }

        public double ChestsOpacity
        {
            get => _chestsOpacity;
            set
            {
                _chestsOpacity = value;
                NotifyOfPropertyChange(() => ChestsOpacity);
            }
        }

        public double WeaponsOpacity
        {
            get => _weaponOpacity;
            set
            {
                _weaponOpacity = value;
                NotifyOfPropertyChange(() => WeaponsOpacity);
            }
        }

        #endregion

        #region Gear Counter Getters & Setters

        public int AmuletsAmount
        {
            get => _amuletsAmount;
            set
            {
                _amuletsAmount = value;
                NotifyOfPropertyChange(() => AmuletsAmount);
            }
        }

        public int RingsAmount
        {
            get => _ringsAmount;
            set
            {
                _ringsAmount = value;
                NotifyOfPropertyChange(() => RingsAmount);
            }
        }

        public int BeltsAmount
        {
            get => _beltsAmount;
            set
            {
                _beltsAmount = value;
                NotifyOfPropertyChange(() => BeltsAmount);
            }
        }

        public int HelmetsAmount
        {
            get => _helmetsAmount;
            set
            {
                _helmetsAmount = value;
                NotifyOfPropertyChange(() => HelmetsAmount);
            }
        }

        public int GlovesAmount
        {
            get => _glovesAmount;
            set
            {
                _glovesAmount = value;
                NotifyOfPropertyChange(() => GlovesAmount);
            }
        }

        public int BootsAmount
        {
            get => _bootsAmount;
            set
            {
                _bootsAmount = value;
                NotifyOfPropertyChange(() => BootsAmount);
            }
        }

        public int ChestsAmount
        {
            get => _chestsAmount;
            set
            {
                _chestsAmount = value;
                NotifyOfPropertyChange(() => ChestsAmount);
            }
        }

        public int WeaponsAmount
        {
            get => _weaponsAmount;
            set
            {
                _weaponsAmount = value;
                NotifyOfPropertyChange(() => WeaponsAmount);
            }
        }

        #endregion

        public Visibility AmountsVisibility
        {
            get => _amountsVisibility;
            set
            {
                _amountsVisibility = value;
                NotifyOfPropertyChange(() => AmountsVisibility);
            }
        }

        public SolidColorBrush FetchButtonColor
        {
            get => _fetchButtonColor;
            set
            {
                _fetchButtonColor = value;
                NotifyOfPropertyChange(() => FetchButtonColor);
            }
        }

        public bool FetchButtonEnabled
        {
            get => _fetchButtonEnabled;
            set
            {
                _fetchButtonEnabled = value;
                NotifyOfPropertyChange(() => FetchButtonEnabled);
            }
        }
        
        #endregion

        public void DisableWarnings()
        {
            WarningMessage = "";
            ShadowOpacity = 0;
            WarningMessageVisibility = Visibility.Hidden;
        }
        
        public void ReloadItemFilter()
        {
            Model.ReloadItemFilter.ReloadFilter();
        }
        
        public void RunFetching()
        {
            if (!MainWindowView.SettingsComplete) return;

            // I don't think we're ever going to be in a state where this method is called while the window isn't open
            // Only in artificial testing, but maybe we can seal it properly, somehow
            // if (!IsOpen) return;

            switch (IoC.Get<ApplicationSettingService>().StashTabQueryMode)
            {
                case 0 when IoC.Get<ApplicationSettingService>().StashTabQueryIndices == "":
                    MessageBox.Show("Missing Settings!" + Environment.NewLine + "Please set stash tab indices.");
                    return;
                case 1 when IoC.Get<ApplicationSettingService>().StashTabQueryString == "":
                    MessageBox.Show("Missing Settings!" + Environment.NewLine + "Please set stash tab prefix.");
                    return;
                case 2 when IoC.Get<ApplicationSettingService>().StashTabQueryString == "":
                    MessageBox.Show("Missing Settings!" + Environment.NewLine + "Please set stash tab suffix.");
                    return;
            }

            if (CalculationActive)
            {
                Data.cs.Cancel();
                FetchingActive = false;
            }
            else
            {
                if (ApiAdapter.IsFetching) return;

                Data.cs = new CancellationTokenSource();
                Data.CancellationToken = Data.cs.Token;
                if (StashTabOverlayView.Instance.IsOpen) StashTabOverlayView.Instance.Hide();
                FetchData();
                FetchingActive = true;
            }
        }
        
        private async void FetchData()
        {
            if (FetchingActive) return;

            if (!IoC.Get<ApplicationSettingService>().ChaosRecipe 
                && !IoC.Get<ApplicationSettingService>().RegalRecipe 
                && !IoC.Get<ApplicationSettingService>().ExaltRecipe)
            {
                MessageBox.Show("No recipes are enabled. Please pick a recipe.", "No Recipes", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DisableWarnings();
            
            FetchingActive = true;
            CalculationActive = true;
            IsIndeterminate = true;
            FetchButtonEnabled = false;
            FetchButtonColor = Brushes.DimGray;

            if (await ApiAdapter.GenerateUri())
            {
                if (await ApiAdapter.GetItems())
                {
                    try
                    {
                        await Task.Run(async () =>
                        {
                            await Data.CheckActives();

                            SetOpacity();
                            CalculationActive = false;
                            IsIndeterminate = false;
                        }, Data.CancellationToken);

                        await Task.Delay(FetchCooldown * 1000).ContinueWith(_ =>
                        {
                            Trace.WriteLine("Waited before fetching again (don't want to trigger an API lockout)");
                        });
                    }
                    catch (OperationCanceledException ex) when (ex.CancellationToken == Data.CancellationToken)
                    {
                        Trace.WriteLine("Error while fetching data; aborting operation");
                    }
                }
            }
            
            if (RateLimit.RateLimitExceeded)
            {
                var secondsToWait = RateLimit.GetSecondsToWait();
                WarningMessage = $"Rate Limit Exceeded! Waiting {secondsToWait} seconds...";
                ShadowOpacity = 1;
                WarningMessageVisibility = Visibility.Visible;

                await Task.Delay(secondsToWait * 1000);
                RateLimit.Reset();
            }

            if (RateLimit.BanTime > 0)
            {
                WarningMessage = "Temporary Ban! Waiting...";
                ShadowOpacity = 1;
                WarningMessageVisibility = Visibility.Visible;

                await Task.Delay(RateLimit.BanTime * 1000);
                RateLimit.BanTime = 0;
            }

            CalculationActive = false;
            FetchingActive = false;
            IsIndeterminate = false;
            FetchButtonEnabled = true;
            FetchButtonColor = Brushes.Green;
            FetchingActive = false;
            
            Trace.WriteLine("end of fetch function reached");
        }

        private void RunStashOverlay()
        {
            MainWindowView.RunStashTabOverlay();
        }
        
        private void SetOpacity()
        {
            if (!Data.ActiveItems.HelmetActive)
                HelmetOpacity = DeactivatedOpacity;
            else
                HelmetOpacity = ActivatedOpacity;

            if (!Data.ActiveItems.GlovesActive)
                GlovesOpacity = DeactivatedOpacity;
            else
                GlovesOpacity = ActivatedOpacity;

            if (!Data.ActiveItems.BootsActive)
                BootsOpacity = DeactivatedOpacity;
            else
                BootsOpacity = ActivatedOpacity;

            if (!Data.ActiveItems.WeaponActive)
                WeaponsOpacity = DeactivatedOpacity;
            else
                WeaponsOpacity = ActivatedOpacity;

            if (!Data.ActiveItems.ChestActive)
                ChestsOpacity = DeactivatedOpacity;
            else
                ChestsOpacity = ActivatedOpacity;

            if (!Data.ActiveItems.RingActive)
                RingsOpacity = DeactivatedOpacity;
            else
                RingsOpacity = ActivatedOpacity;

            if (!Data.ActiveItems.AmuletActive)
                AmuletsOpacity = DeactivatedOpacity;
            else
                AmuletsOpacity = ActivatedOpacity;

            if (!Data.ActiveItems.BeltActive)
                BeltsOpacity = DeactivatedOpacity;
            else
                BeltsOpacity = ActivatedOpacity;
        }

    }
}