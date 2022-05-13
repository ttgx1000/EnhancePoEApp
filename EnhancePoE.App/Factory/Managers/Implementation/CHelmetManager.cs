using EnhancePoE.DataModels.CREModels;

namespace EnhancePoE.App.Factory.Managers.Implementation
{
    internal class CHelmetManager : ABaseItemClassManager
    {
        #region Constructors

        public CHelmetManager(string classColor, bool alwaysActive) : base(classColor, alwaysActive)
        {
            ClassName = "Helmets";
            ClassFilterName = "\"Helmets\"";
        }

        #endregion

        #region Methods

        public override ActiveItemTypes SetActiveTypes(ActiveItemTypes activeItems, bool newValue)
        {
            activeItems.HelmetActive = newValue;
            return activeItems;
        }

        #endregion
    }
}