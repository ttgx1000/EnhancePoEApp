using EnhancePoE.DataModels.CREModels;

namespace EnhancePoE.App.Factory.Managers.Implementation
{
    internal class CBodyArmoursManager : ABaseItemClassManager
    {
        #region Constructors

        public CBodyArmoursManager(string classColor, bool alwaysActive) : base(classColor, alwaysActive)
        {
            ClassName = "BodyArmours";
            ClassFilterName = "\"Body Armours\"";
        }

        #endregion

        #region Methods

        public override ActiveItemTypes SetActiveTypes(ActiveItemTypes activeItems, bool newValue)
        {
            activeItems.ChestActive = newValue;
            return activeItems;
        }

        #endregion
    }
}