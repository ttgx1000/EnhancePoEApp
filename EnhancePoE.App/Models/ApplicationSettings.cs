using System;
using System.ComponentModel;
using ConfOxide;

// TODO: [Refactor] After we remove the 'Settings.settings.' from app properties we should rename this to 'Settings'
// TODO: [Refactor] Lots of properties here that we can rename to make it really clear what settings do what (also just for coding convention purposes)

namespace EnhancePoE.App.Models
{
    /// <summary>
    /// Represents our user-specific Application Settings.
    /// </summary>
    public sealed class ApplicationSettings : SettingsBase<ApplicationSettings>
    {
        #region User Account Settings
        
        public string accName { get; set; }
        
        public string SessionId { get; set; }

        public string CREClientId { get; set; }
        
        #endregion
        
        #region General App Settings
        
        public string LogLocation { get; set; }

        [DefaultValue("Standard")]
        public string League { get; set; }
        
        // TODO: [Refactor] Don't know if we need 2 separate settings for this, can quite literally be tracked with a single bool
        [DefaultValue(true)]
        public bool MainLeague { get; set; }
        
        [DefaultValue(false)]
        public bool CustomLeague { get; set; }

        // TODO: [Refactor] This is set to default at 'unlimited' sets and it confuses some people; maybe change it to like 3 sets?
        [DefaultValue(0)]
        public int Sets { get; set; }

        public int HighlightMode { get; set; }

        // TODO: [Refactor] Probably rename this (it's the 'Fill Greedy' option, I think)
        [DefaultValue(true)]
        public bool FillWithChaos { get; set; }

        // TODO: [Refactor] Make main overlay modes enums (these represent the 'Standard', 'Minimal', and 'Icons Only' overlay modes)
        [DefaultValue(0)]
        public int OverlayMode { get; set; }

        // TODO: [Refactor] Should be enum for 3 different states: None, Total Items, Items Missing
        [DefaultValue(1)]
        public int ShowItemAmount { get; set; }

        [DefaultValue(false)]
        public bool IncludeIdentified { get; set; }
        
        [DefaultValue(true)]
        public bool ChaosRecipe { get; set; }
        
        [DefaultValue(false)]
        public bool RegalRecipe { get; set; }
        
        [DefaultValue(false)]
        public bool ExaltRecipe { get; set; }
        
        [DefaultValue(false)]
        public bool hideOnClose { get; set; }
        
        [DefaultValue(false)]
        public bool AutoFetch { get; set; }
        
        // TODO: [Refactor] We should make Languages enums to make it less confusing for devs
        [DefaultValue(0)]
        public int Language { get; set; }

        [DefaultValue("#14FFFFFF")]
        public string StashTabBackgroundColor { get; set; }

        // TODO: [Refactor] We should make Stash Tab Query Mode enums (e.g. QueryByIndex, QueryByNamePrefix, QueryByNameSuffix, etc.)
        [DefaultValue(0)]
        public int StashTabMode { get; set; }

        public string StashTabName { get; set; }
        
        [DefaultValue("0")]
        public string StashTabIndices { get; set; }
        
        [DefaultValue(false)]
        public bool LootFilterActive { get; set; }
        
        [DefaultValue(false)]
        public bool LootFilterIcons { get; set; }

        public string LootFilterLocation { get; set; }
        
        [DefaultValue(true)]
        public bool Sound { get; set; }

        [DefaultValue(50)]
        public int Volume { get; set; }

        public string ItemPickupSoundFileLocation { get; set; }

        public string FilterChangeSoundFileLocation { get; set; }

        // The icons on the main overlay will stay at 100% opacity (i.e. active state) if these are set to true
        
        [DefaultValue(false)]
        public bool HelmetsAlwaysActive { get; set; }
        
        [DefaultValue(false)]
        public bool ChestsAlwaysActive { get; set; }
        
        [DefaultValue(false)]
        public bool GlovesAlwaysActive { get; set; }
        
        [DefaultValue(false)]
        public bool BootsAlwaysActive { get; set; }
        
        [DefaultValue(false)]
        public bool WeaponsAlwaysActive { get; set; }
        
        [DefaultValue(true)]
        public bool RingsAlwaysActive { get; set; }
                
        [DefaultValue(true)]
        public bool AmuletsAlwaysActive { get; set; }
                
        [DefaultValue(true)]
        public bool BeltsAlwaysActive { get; set; }
        
        #endregion
        
        #region App UI Settings
        
        [DefaultValue(false)]
        public bool LockOverlayPosition { get; set; }
        
        [DefaultValue(0)]
        public double TopMain { get; set; }

        [DefaultValue(0)]
        public double LeftMain { get; set; }

        [DefaultValue(0)]
        public double TopOverlay { get; set; }
        
        [DefaultValue(0)]
        public double LeftOverlay { get; set; }

        [DefaultValue(1)]
        public float Opacity { get; set; }
        
        [DefaultValue(74)]
        public double TopStashTabOverlay { get; set; }

        [DefaultValue(12)]
        public double LeftStashTabOverlay { get; set; }

        [DefaultValue(641)]
        public double XStashTabOverlay { get; set; }

        [DefaultValue(690)]
        public double YStashTabOverlay { get; set; }

        [DefaultValue(1)]
        public float OpacityStashTab { get; set; }
        
        [DefaultValue(20.4)]
        public double TabHeaderWidth { get; set; }

        [DefaultValue(5)]
        public double TabHeaderGap { get; set; }

        [DefaultValue(23)]
        public double TabMargin { get; set; }

        [DefaultValue("#96F90000")]
        public string ColorStash { get; set; }

        #endregion
        
        #region Hotkey Settings
        
        [DefaultValue("< not set >")]
        public string HotkeyToggle { get; set; }
        
        [DefaultValue("< not set >")]
        public string HotkeyRefresh { get; set; }
        
        [DefaultValue("< not set >")]
        public string HotkeyStashTab { get; set; }
        
        [DefaultValue("< not set >")]
        public string HotkeyReloadFilter { get; set; }
        
        #endregion
        
        #region Filter Style Settings
        
        [DefaultValue("#FFF8FF04")]
        public string ColorHelmet { get; set; }

        [DefaultValue("#FFDD00FF")]
        public string ColorChest { get; set; }

        [DefaultValue("#FF04FF00")]
        public string ColorGloves { get; set; }

        [DefaultValue("#FF0018FF")]
        public string ColorBoots { get; set; }
        
        [DefaultValue("#FF00DCFF")]
        public string ColorWeapon { get; set; }

        [DefaultValue("#FFFF0303")]
        public string ColorAmulet { get; set; }
        
        [DefaultValue("#FFFF0303")]
        public string ColorRing { get; set; }
        
        [DefaultValue("#FFFF0303")]
        public string ColorBelt { get; set; }
        
        #endregion
    }
}