using EnhancePoE.DataModels.CREModels;

namespace EnhancePoE.App.Factory.Managers.Implementation
{
    internal class CAmuletsManager : ABaseItemClassManager
    {
        #region Constructors

        public CAmuletsManager(string classColor, bool alwaysActive) : base(classColor, alwaysActive)
        {
            ClassName = "Amulets";
            ClassFilterName = "\"Amulets\"";
        }

        #endregion

        #region Methods

        public override ActiveItemTypes SetActiveTypes(ActiveItemTypes activeItems, bool newValue)
        {
            activeItems.AmuletActive = newValue;
            return activeItems;
        }

        #endregion
    }
}