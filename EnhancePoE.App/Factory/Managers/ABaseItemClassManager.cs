using System.Collections.Generic;
using EnhancePoE.App.Services;
using EnhancePoE.DataModels.CREModels;

namespace EnhancePoE.App.Factory.Managers
{
    public abstract class ABaseItemClassManager
    {
        #region Fields

        private ApplicationSettingService _applicationSettingService;

        #endregion
        
        #region Properties

        public string ClassName { get; set; }
        public string ClassFilterName { get; set; }
        public string ClassColor { get; set; }
        public bool AlwaysActive { get; set; }

        #endregion

        public ABaseItemClassManager(string classColor, bool alwaysActive)
        {
            ClassColor = classColor;
            AlwaysActive = alwaysActive;
        }

        #region Methods

        public virtual string SetBaseType()
        {
            var baseType = "Class " + ClassFilterName;
            return baseType;
        }

        public string SetSocketRules(string result)
        {
            return result;
        }

        public virtual bool CheckIfMissing(HashSet<string> missingItemClasses)
        {
            return missingItemClasses.Contains(ClassName);
        }

        public abstract ActiveItemTypes SetActiveTypes(ActiveItemTypes activeItems, bool newValue);

        #endregion
    }
}