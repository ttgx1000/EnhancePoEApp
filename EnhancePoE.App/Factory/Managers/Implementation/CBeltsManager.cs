using EnhancePoE.DataModels.CREModels;

namespace EnhancePoE.App.Factory.Managers.Implementation
{
    internal class CBeltsManager : ABaseItemClassManager
    {
        #region Constructors

        public CBeltsManager(string classColor, bool alwaysActive) : base(classColor, alwaysActive)
        {
            ClassName = "Belts";
            ClassFilterName = "\"Belts\"";
        }

        #endregion

        #region Methods

        public override ActiveItemTypes SetActiveTypes(ActiveItemTypes activeItems, bool newValue)
        {
            activeItems.BeltActive = newValue;
            return activeItems;
        }

        #endregion
    }
}