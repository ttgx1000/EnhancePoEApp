using EnhancePoE.DataModels.CREModels;

namespace EnhancePoE.App.Factory.Managers.Implementation
{
    internal class CBootsManager : ABaseItemClassManager
    {
        #region Constructors

        public CBootsManager(string classColor, bool alwaysActive) : base(classColor, alwaysActive)
        {
            ClassName = "Boots";
            ClassFilterName = "\"Boots\"";
        }

        #endregion

        #region Methods

        public override ActiveItemTypes SetActiveTypes(ActiveItemTypes activeItems, bool newValue)
        {
            activeItems.BootsActive = newValue;
            return activeItems;
        }

        #endregion
    }
}