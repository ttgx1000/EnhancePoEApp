using System.Windows;
using Caliburn.Micro;
using EnhancePoE.App.Services;
using EnhancePoE.UI.Model;

//TODO: Auto-focus on open
namespace EnhancePoE.UI.View
{
    /// <summary>
    /// Interaction logic for HotkeyWindow.xaml
    /// </summary>
    public partial class HotkeyWindowView
    {
        private readonly MainWindowView _mainWindowView;
        private readonly string _type;

        public HotkeyWindowView(MainWindowView mainWindowView, string hotkeyType)
        {
            _mainWindowView = mainWindowView;
            _type = hotkeyType;
            InitializeComponent();
        }

        private void SaveHotkeyButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case "refresh":
                {
                    IoC.Get<HotkeysManager>().RemoveHotkey(HotkeysManager.refreshModifier, HotkeysManager.refreshKey);
                    if (CustomHotkeyToggle.Hotkey == null)
                    {
                        IoC.Get<ApplicationSettingService>().HotkeyRefresh = "< not set >";
                    }
                    else
                    {
                        IoC.Get<ApplicationSettingService>().HotkeyRefresh = CustomHotkeyToggle.Hotkey.ToString();
                        IoC.Get<HotkeysManager>().GetRefreshHotkey();
                    }

                    ReApplyHotkeys();
                    break;
                }
                case "toggle":
                {
                    IoC.Get<HotkeysManager>().RemoveHotkey(HotkeysManager.toggleModifier, HotkeysManager.toggleKey);
                    if (CustomHotkeyToggle.Hotkey == null)
                    {
                        IoC.Get<ApplicationSettingService>().HotkeyToggle = "< not set >";
                    }
                    else
                    {
                        IoC.Get<ApplicationSettingService>().HotkeyToggle = CustomHotkeyToggle.Hotkey.ToString();
                        IoC.Get<HotkeysManager>().GetToggleHotkey();
                    }

                    ReApplyHotkeys();
                    break;
                }
                case "stashtab":
                {
                    IoC.Get<HotkeysManager>().RemoveHotkey(HotkeysManager.stashTabModifier, HotkeysManager.stashTabKey);
                    if (CustomHotkeyToggle.Hotkey == null)
                    {
                        IoC.Get<ApplicationSettingService>().HotkeyStashTab = "< not set >";
                    }
                    else
                    {
                        IoC.Get<ApplicationSettingService>().HotkeyStashTab = CustomHotkeyToggle.Hotkey.ToString();
                        IoC.Get<HotkeysManager>().GetStashTabHotkey();
                    }

                    ReApplyHotkeys();
                    break;
                }
            }

            Close();
        }

        private void ReApplyHotkeys()
        {
            _mainWindowView.RemoveAllHotkeys();
            _mainWindowView.AddAllHotkeys();
        }
    }
}