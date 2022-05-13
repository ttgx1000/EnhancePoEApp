using EnhancePoE.DataModels.CREModels;

namespace EnhancePoE.App.Factory.Managers.Implementation
{
    internal class CGlovesManager : ABaseItemClassManager
    {
        #region Constructors

        public CGlovesManager(string classColor, bool alwaysActive) : base(classColor, alwaysActive)
        {
            ClassName = "Gloves";
            ClassFilterName = "\"Gloves\"";
        }

        #endregion

        #region Methods

        public override ActiveItemTypes SetActiveTypes(ActiveItemTypes activeItems, bool newValue)
        {
            activeItems.GlovesActive = newValue;
            return activeItems;
        }

        #endregion
    }
}