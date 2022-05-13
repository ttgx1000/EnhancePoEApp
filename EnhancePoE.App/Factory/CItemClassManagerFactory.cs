using System;
using EnhancePoE.App.Factory.Managers;
using EnhancePoE.App.Factory.Managers.Implementation;
using EnhancePoE.App.Services;
using EnhancePoE.DataModels.Enums;

namespace EnhancePoE.App.Factory
{
    public class CItemClassManagerFactory
    {
        #region Methods

        public ABaseItemClassManager GetItemClassManager(EnumItemClass itemClass,
            ApplicationSettingService applicationSettingService)
        {
            switch (itemClass)
            {
                case EnumItemClass.Helmets:
                    return new CHelmetManager(applicationSettingService.ColorHelmet, applicationSettingService.HelmetIconAlwaysActive);
                case EnumItemClass.BodyArmours:
                    return new CBodyArmoursManager(applicationSettingService.ColorChest, applicationSettingService.ChestIconAlwaysActive);
                case EnumItemClass.Gloves:
                    return new CGlovesManager(applicationSettingService.ColorGloves, applicationSettingService.GloveIconAlwaysActive);
                case EnumItemClass.Boots:
                    return new CBootsManager(applicationSettingService.ColorBoots, applicationSettingService.BootIconAlwaysActive);
                case EnumItemClass.Rings:
                    return new CRingsManager(applicationSettingService.ColorRing, applicationSettingService.RingIconAlwaysActive);
                case EnumItemClass.Amulets:
                    return new CAmuletsManager(applicationSettingService.ColorAmulet, applicationSettingService.AmuletIconAlwaysActive);
                case EnumItemClass.Belts:
                    return new CBeltsManager(applicationSettingService.ColorBelt, applicationSettingService.BeltIconAlwaysActive);
                case EnumItemClass.OneHandWeapons:
                    return new COneHandWeaponsManager(applicationSettingService.ColorWeapon, applicationSettingService.WeaponIconAlwaysActive);
                case EnumItemClass.TwoHandWeapons:
                    return new CTwoHandWeaponsManager(applicationSettingService.ColorWeapon, applicationSettingService.WeaponIconAlwaysActive);
                default:
                    throw new Exception("Wrong item class.");
            }
        }

        #endregion
    }
}