using System.Collections.Generic;
using EnhancePoE.DataModels.Constants;
using EnhancePoE.DataModels.CREModels;

namespace EnhancePoE.App.Factory.Managers.Implementation
{
    internal class CTwoHandWeaponsManager : ABaseItemClassManager
    {
        #region Constructors

        public CTwoHandWeaponsManager(string classColor, bool alwaysActive) : base(classColor, alwaysActive)
        {
            ClassName = "TwoHandWeapons";
            ClassFilterName = "\"Two Hand\"";
        }

        #endregion

        #region Methods

        public override string SetBaseType()
        {
            var baseType = "Class ";
            baseType += "\"Two Hand Swords\" \"Two Hand Axes\" \"Two Hand Maces\" \"Staves\" \"Warstaves\" \"Bows\"";
            baseType += SharedStrings.NewLine + SharedStrings.Tab + "Width <= 2" + SharedStrings.NewLine + SharedStrings.Tab + "Height <= 3";
            baseType += SharedStrings.NewLine + SharedStrings.Tab + "Sockets <= 5" + SharedStrings.NewLine + SharedStrings.Tab +
                        "LinkedSockets <= 5";
            return baseType;
        }

        public override ActiveItemTypes SetActiveTypes(ActiveItemTypes activeItems, bool newValue)
        {
            activeItems.WeaponActive = newValue;
            return activeItems;
        }

        public override bool CheckIfMissing(HashSet<string> missingItemClasses)
        {
            // bad, dont like, no good ideas for now tho
            return missingItemClasses.Contains(ClassName) || missingItemClasses.Contains("OneHandWeapons");
        }

        #endregion
    }
}