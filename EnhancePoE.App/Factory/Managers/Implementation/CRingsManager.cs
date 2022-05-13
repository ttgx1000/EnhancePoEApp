using EnhancePoE.DataModels.CREModels;

namespace EnhancePoE.App.Factory.Managers.Implementation
{
    internal class CRingsManager : ABaseItemClassManager
    {
        #region Constructors

        public CRingsManager(string classColor, bool alwaysActive) : base(classColor, alwaysActive)
        {
            ClassName = "Rings";
            ClassFilterName = "\"Rings\"";
        }

        #endregion

        #region Methods

        public override ActiveItemTypes SetActiveTypes(ActiveItemTypes activeItems, bool newValue)
        {
            activeItems.RingActive = newValue;
            return activeItems;
        }

        #endregion
    }
}