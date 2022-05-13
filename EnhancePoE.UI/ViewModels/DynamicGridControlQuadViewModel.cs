using System.Windows.Controls;
using Caliburn.Micro;
using EnhancePoE.App.Services;
using EnhancePoE.UI.Model;

namespace EnhancePoE.UI.ViewModels
{
    public class DynamicGridControlQuadViewModel : ItemsControl
    {
        private float _opacityStashTab;
        private string _colorStash;
        private bool _active;
        private string _buttonName;

        public DynamicGridControlQuadViewModel()
        {
            _opacityStashTab = IoC.Get<ApplicationSettingService>().OpacityStashTab;
            _colorStash = IoC.Get<ApplicationSettingService>().ColorStash;
        }

        public float OpacityStashTab
        {
            get => _opacityStashTab;
            set => _opacityStashTab = value;
        }

        public string ColorStash
        {
            get => _colorStash;
            set => _colorStash = value;
        }

        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        public string ButtonName
        {
            get => _buttonName;
            set => _buttonName = value;
        }

        public Button GetButtonFromCell(object cell)
        {
            for (var i = 0; i < Items.Count; i++)
                if (Items[i] == cell)
                {
                    //Trace.WriteLine(cell.XIndex + " x " + cell.YIndex + " y");

                    var container = ItemContainerGenerator.ContainerFromIndex(i);
                    var t = Utility.GetChild<Button>(container);
                    return t;
                }

            return null;
        }
    }
}