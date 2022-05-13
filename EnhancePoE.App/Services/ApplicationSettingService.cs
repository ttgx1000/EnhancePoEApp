using System;
using System.IO;
using ConfOxide;
using EnhancePoE.App.Models;

namespace EnhancePoE.App.Services
{
    public class ApplicationSettingService
    {
        #region Fields
        
        private ApplicationSettings _applicationSettings;
        private string FolderName => "ChaosRecipeEnhancer";
        private string FileName => "CRE-Settings.json";
        private string AppDataFolderPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string SettingsFolderPath => Path.Combine(AppDataFolderPath, FolderName);
        private string SettingsFilePath => Path.Combine(SettingsFolderPath, FileName);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingService"/> class.
        /// </summary>
        public ApplicationSettingService()
        {
            _applicationSettings = new ApplicationSettings();
            if (!Directory.Exists(SettingsFolderPath))
            {
                Directory.CreateDirectory(SettingsFolderPath);
            }

            if (!File.Exists(SettingsFilePath))
            {
                CreateDefaultSettings();
            }
            else
            {
                try
                {
                    _applicationSettings.ReadJsonFile(SettingsFilePath);
                }
                catch
                {
                    File.Delete(SettingsFilePath);
                    CreateDefaultSettings();
                }
            }

            if (string.IsNullOrEmpty(_applicationSettings.CREClientId))
            {
                _applicationSettings.CREClientId = Guid.NewGuid().ToString();
                Save();
            }
        }

        #endregion
        
        #region Events

        /// <summary>
        /// Occurs when [on save].
        /// </summary>
        public event EventHandler OnSave;

        #endregion

        #region Properties

        #region User Account Settings

        public string AccountName
        {
            get => _applicationSettings.accName;
            set => _applicationSettings.accName = value;
        }

        public string SessionId
        {
            get => _applicationSettings.SessionId;
            set => _applicationSettings.SessionId = value;
        }

        public string CREClientId
        {
            get => _applicationSettings.CREClientId;
            set => _applicationSettings.CREClientId = value;
        }
        
        #endregion

        #region General App Settings

        public string GGGClientLogFileLocation
        {
            get => _applicationSettings.LogLocation;
            set => _applicationSettings.LogLocation = value;
        }

        public string CurrentLeague
        {
            get => _applicationSettings.League;
            set => _applicationSettings.League = value;
        }

        public bool SelectFromMainLeagues
        {
            get => _applicationSettings.MainLeague;
            set => _applicationSettings.MainLeague = value;
        }

        public bool InputCustomLeagueName
        {
            get => _applicationSettings.CustomLeague;
            set => _applicationSettings.CustomLeague = value;
        }

        public int SetsThreshold
        {
            get => _applicationSettings.Sets;
            set => _applicationSettings.Sets = value;
        }

        public int StashOverlayHighlightMode
        {
            get => _applicationSettings.HighlightMode;
            set => _applicationSettings.HighlightMode = value;
        }
        
        // Name may be the opposite of what it does
        public bool DoNotPreserveLowItemLevelGear
        {
            get => _applicationSettings.FillWithChaos;
            set => _applicationSettings.FillWithChaos = value;
        }

        public int MainOverlayMode
        {
            get => _applicationSettings.OverlayMode;
            set => _applicationSettings.OverlayMode = value;
        }

        public int MainOverlayItemAmountDisplayMode
        {
            get => _applicationSettings.ShowItemAmount;
            set => _applicationSettings.ShowItemAmount = value;
        }

        public bool IncludeIdentifiedItems
        {
            get => _applicationSettings.IncludeIdentified;
            set => _applicationSettings.IncludeIdentified = value;
        }

        public bool ChaosRecipe
        {
            get => _applicationSettings.ChaosRecipe;
            set => _applicationSettings.ChaosRecipe = value;
        }

        public bool RegalRecipe
        {
            get => _applicationSettings.RegalRecipe;
            set => _applicationSettings.RegalRecipe = value;
        }

        public bool ExaltRecipe
        {
            get => _applicationSettings.ExaltRecipe;
            set => _applicationSettings.ExaltRecipe = value;
        }

        public bool CloseToTray
        {
            get => _applicationSettings.hideOnClose;
            set => _applicationSettings.hideOnClose = value;
        }

        public bool AutoFetchStashContentsOnRezone
        {
            get => _applicationSettings.AutoFetch;
            set => _applicationSettings.AutoFetch = value;
        }

        // TODO Should be enum
        public int Language
        {
            get => _applicationSettings.Language;
            set => _applicationSettings.Language = value;
        }

        public string StashTabOverlayBackgroundColor
        {
            get => _applicationSettings.StashTabBackgroundColor;
            set => _applicationSettings.StashTabBackgroundColor = value;
        }

        public int StashTabQueryMode
        {
            get => _applicationSettings.StashTabMode;
            set => _applicationSettings.StashTabMode = value;
        }

        public string StashTabQueryString
        {
            get => _applicationSettings.StashTabName;
            set => _applicationSettings.StashTabName = value;
        }

        public string StashTabQueryIndices
        {
            get => _applicationSettings.StashTabIndices;
            set => _applicationSettings.StashTabIndices = value;
        }

        public bool LootFilterIcons
        {
            get => _applicationSettings.LootFilterIcons;
            set => _applicationSettings.LootFilterIcons = value;
        }

        public bool LootFilterManipulationActive
        {
            get => _applicationSettings.LootFilterActive;
            set => _applicationSettings.LootFilterActive = value;
        }

        public string LootFilterFileLocation
        {
            get => _applicationSettings.LootFilterLocation;
            set => _applicationSettings.LootFilterLocation = value;
        }

        public bool Sound
        {
            get => _applicationSettings.Sound;
            set => _applicationSettings.Sound = value;
        }

        public int Volume
        {
            get => _applicationSettings.Volume;
            set => _applicationSettings.Volume = value;
        }

        public string ItemPickupSoundFileLocation
        {
            get => _applicationSettings.ItemPickupSoundFileLocation;
            set => _applicationSettings.ItemPickupSoundFileLocation = value;
        }

        public string FilterChangeSoundFileLocation
        {
            get => _applicationSettings.FilterChangeSoundFileLocation;
            set => _applicationSettings.FilterChangeSoundFileLocation = value;
        }
        
        // The icons on the main overlay will stay at 100% opacity (i.e. active state) if these are set to true

        public bool HelmetIconAlwaysActive
        {
            get => _applicationSettings.HelmetsAlwaysActive;
            set => _applicationSettings.HelmetsAlwaysActive = value;
        }

        public bool ChestIconAlwaysActive
        {
            get => _applicationSettings.ChestsAlwaysActive;
            set => _applicationSettings.ChestsAlwaysActive = value;
        }

        public bool GloveIconAlwaysActive
        {
            get => _applicationSettings.GlovesAlwaysActive;
            set => _applicationSettings.GlovesAlwaysActive = value;
        }

        public bool BootIconAlwaysActive
        {
            get => _applicationSettings.BootsAlwaysActive;
            set => _applicationSettings.BootsAlwaysActive = value;
        }

        public bool WeaponIconAlwaysActive
        {
            get => _applicationSettings.WeaponsAlwaysActive;
            set => _applicationSettings.WeaponsAlwaysActive = value;
        }

        public bool RingIconAlwaysActive
        {
            get => _applicationSettings.RingsAlwaysActive;
            set => _applicationSettings.RingsAlwaysActive = value;
        }

        public bool AmuletIconAlwaysActive
        {
            get => _applicationSettings.AmuletsAlwaysActive;
            set => _applicationSettings.AmuletsAlwaysActive = value;
        }

        public bool BeltIconAlwaysActive
        {
            get => _applicationSettings.BeltsAlwaysActive;
            set => _applicationSettings.BeltsAlwaysActive = value;
        }
        
        #endregion
        
        // TODO: Rename a ton of these, because they're confusing
        #region App UI Settings

        public bool LockOverlayPosition
        {
            get => _applicationSettings.LockOverlayPosition;
            set => _applicationSettings.LockOverlayPosition = value;
        }
        
        public double TopMain
        {
            get => _applicationSettings.TopMain;
            set => _applicationSettings.TopMain = value;
        }

        public double LeftMain
        {
            get => _applicationSettings.LeftMain;
            set => _applicationSettings.LeftMain = value;
        }

        public double TopOverlay
        {
            get => _applicationSettings.TopOverlay;
            set => _applicationSettings.TopOverlay = value;
        }

        public double LeftOverlay
        {
            get => _applicationSettings.LeftOverlay;
            set => _applicationSettings.LeftOverlay = value;
        }

        public float Opacity
        {
            get => _applicationSettings.Opacity;
            set => _applicationSettings.Opacity = value;
        }

        public double TopStashTabOverlay
        {
            get => _applicationSettings.TopStashTabOverlay;
            set => _applicationSettings.TopStashTabOverlay = value;
        }

        public double LeftStashTabOverlay
        {
            get => _applicationSettings.LeftStashTabOverlay;
            set => _applicationSettings.LeftStashTabOverlay = value;
        }

        public double XStashTabOverlay
        {
            get => _applicationSettings.XStashTabOverlay;
            set => _applicationSettings.XStashTabOverlay = value;
        }

        public double YStashTabOverlay
        {
            get => _applicationSettings.YStashTabOverlay;
            set => _applicationSettings.YStashTabOverlay = value;
        }

        public float OpacityStashTab
        {
            get => _applicationSettings.OpacityStashTab;
            set => _applicationSettings.OpacityStashTab = value;
        }

        public double TabHeaderWidth
        {
            get => _applicationSettings.TabHeaderWidth;
            set => _applicationSettings.TabHeaderWidth = value;
        }

        public double TabHeaderGap
        {
            get => _applicationSettings.TabHeaderGap;
            set => _applicationSettings.TabHeaderGap = value;
        }

        public double TabMargin
        {
            get => _applicationSettings.TabMargin;
            set => _applicationSettings.TabMargin = value;
        }

        public string ColorStash
        {
            get => _applicationSettings.ColorStash;
            set => _applicationSettings.ColorStash = value;
        }
        
        #endregion

        #region Hotkey Settings

        // Is this even used?
        public string HotkeyToggle
        {
            get => _applicationSettings.HotkeyToggle;
            set => _applicationSettings.HotkeyToggle = value;
        }

        public string HotkeyRefresh
        {
            get => _applicationSettings.HotkeyRefresh;
            set => _applicationSettings.HotkeyRefresh = value;
        }

        public string HotkeyStashTab
        {
            get => _applicationSettings.HotkeyStashTab;
            set => _applicationSettings.HotkeyStashTab = value;
        }

        public string HotkeyReloadFilter
        {
            get => _applicationSettings.HotkeyReloadFilter;
            set => _applicationSettings.HotkeyReloadFilter = value;
        }

        #endregion

        #region Filter Style Changes

        public string ColorHelmet
        {
            get => _applicationSettings.ColorHelmet;
            set => _applicationSettings.ColorHelmet = value;
        }

        public string ColorChest
        {
            get => _applicationSettings.ColorChest;
            set => _applicationSettings.ColorChest = value;
        }

        public string ColorGloves
        {
            get => _applicationSettings.ColorGloves;
            set => _applicationSettings.ColorGloves = value;
        }

        public string ColorBoots
        {
            get => _applicationSettings.ColorBoots;
            set => _applicationSettings.ColorBoots = value;
        }

        public string ColorWeapon
        {
            get => _applicationSettings.ColorWeapon;
            set => _applicationSettings.ColorWeapon = value;
        }

        public string ColorAmulet
        {
            get => _applicationSettings.ColorAmulet;
            set => _applicationSettings.ColorAmulet = value;
        }

        public string ColorRing
        {
            get => _applicationSettings.ColorAmulet;
            set => _applicationSettings.ColorAmulet = value;
        }

        public string ColorBelt
        {
            get => _applicationSettings.ColorBelt;
            set => _applicationSettings.ColorBelt = value;
        }
        
        #endregion
        
        #endregion
        
        #region Methods

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="raiseEvent">if set to <c>true</c> [raise event].</param>
        public void Save(bool raiseEvent = true)
        {
            _applicationSettings.WriteJsonFile(SettingsFilePath);
            if (raiseEvent)
            {
                OnSave?.Invoke(this, EventArgs.Empty);
            }
        }
        
        /// <summary>
        /// Creates the default settings.
        /// </summary>
        private void CreateDefaultSettings()
        {
            using (File.Create(SettingsFilePath)) { }

            Save();
        }

        public void ResetSettings()
        {
            File.Delete(SettingsFilePath);

            CreateDefaultSettings();
            
            Save();
        }

        #endregion
    }
}