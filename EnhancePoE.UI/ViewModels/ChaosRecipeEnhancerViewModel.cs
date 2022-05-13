using Caliburn.Micro;
using EnhancePoE.App.Services;

namespace EnhancePoE.UI.ViewModels
{
    public class ChaosRecipeEnhancerViewModel : PropertyChangedBase
    {
        private string _fullSetsText = "0";
        private float _opacity;
        private float _shadowOpacity;
        private double _topOverlay;
        private double _leftOverlay;

        public ChaosRecipeEnhancerViewModel()
        {
            _opacity = IoC.Get<ApplicationSettingService>().Opacity;
            _topOverlay = IoC.Get<ApplicationSettingService>().TopOverlay;
            _leftOverlay = IoC.Get<ApplicationSettingService>().LeftOverlay;
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

        public float Opacity
        {
            get => _opacity;
            set
            {
                if (value.Equals(_opacity)) return;
                _opacity = value;
                NotifyOfPropertyChange(() => Opacity);
            }
        }

        public float ShadowOpacity
        {
            get => _shadowOpacity;
            set
            {
                if (value.Equals(_shadowOpacity)) return;
                _shadowOpacity = value;
                NotifyOfPropertyChange(() => ShadowOpacity);
            }
        }

        public double TopOverlay
        {
            get => _topOverlay;
            set
            {
                if (value.Equals(_topOverlay)) return;
                _topOverlay = value;
                NotifyOfPropertyChange(() => TopOverlay);
            }
        }

        public double LeftOverlay
        {
            get => _leftOverlay;
            set
            {
                if (value.Equals(_leftOverlay)) return;
                _leftOverlay = value;
                NotifyOfPropertyChange(() => LeftOverlay);
            }
        }
    }
}