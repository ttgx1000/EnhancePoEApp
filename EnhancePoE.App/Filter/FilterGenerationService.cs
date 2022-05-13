using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EnhancePoE.App.Factory;
using EnhancePoE.App.Factory.Managers;
using EnhancePoE.App.Services;
using EnhancePoE.App.Storage;
using EnhancePoE.DataModels.Constants;
using EnhancePoE.DataModels.CREModels;
using EnhancePoE.DataModels.Enums;

namespace EnhancePoE.App.Filter
{
    //add interfaces
    public class FilterGenerationService
    {
        #region Fields

        private ApplicationSettingService _applicationSettingService;
        
        private ABaseItemClassManager _itemClassManager;
        private readonly List<string> _customStyle = new List<string>();
        private readonly List<string> _customStyleInfluenced = new List<string>();

        #endregion

        #region Constructors

        public FilterGenerationService(ApplicationSettingService applicationSettingService)
        {
            _applicationSettingService = applicationSettingService;
            
            LoadCustomStyle();
            if (applicationSettingService.ExaltRecipe)
            {
                LoadCustomStyleInfluenced();
            }
        }

        #endregion

        #region Methods

        public async Task<ActiveItemTypes> GenerateSectionsAndUpdateFilterAsync(HashSet<string> missingItemClasses)
        {
            ActiveItemTypes activeItemTypes = new ActiveItemTypes();

            if (_applicationSettingService.LootFilterManipulationActive)
            {
                // Pass settings into this
                CItemClassManagerFactory visitor = new CItemClassManagerFactory();
                var sectionList = new HashSet<string>();
                var sectionListExalted = new HashSet<string>();

                foreach (EnumItemClass item in Enum.GetValues(typeof(EnumItemClass)))
                {
                    _itemClassManager = visitor.GetItemClassManager(item, _applicationSettingService);

                    var stillMissing = _itemClassManager.CheckIfMissing(missingItemClasses);

                    // weapons might be buggy, will try to do some tests
                    if ((_applicationSettingService.ChaosRecipe || _applicationSettingService.RegalRecipe)
                        && (_itemClassManager.AlwaysActive || stillMissing))
                    {
                        sectionList.Add(this.GenerateSection(false));

                        // find better way to handle active items and sound notification on changes
                        activeItemTypes = _itemClassManager.SetActiveTypes(activeItemTypes, true);
                    }
                    else
                    {
                        activeItemTypes = _itemClassManager.SetActiveTypes(activeItemTypes, false);
                    }

                    if (_applicationSettingService.ExaltRecipe)
                    {
                        sectionListExalted.Add(GenerateSection(true));
                    }
                }

                await UpdateFilterAsync(sectionList, sectionListExalted);
            }

            return activeItemTypes;
        }

        public string GenerateSection(bool isInfluenced)
        {
            var result = "Show";

            if (isInfluenced)
            {
                result += SharedStrings.NewLine + SharedStrings.Tab + "HasInfluence Crusader Elder Hunter Redeemer Shaper Warlord";
            }
            else
            {
                result += SharedStrings.NewLine + SharedStrings.Tab + "HasInfluence None";
            }

            result = result + SharedStrings.NewLine + SharedStrings.Tab + "Rarity Rare" + SharedStrings.NewLine + SharedStrings.Tab;
            if (!_applicationSettingService.IncludeIdentifiedItems) result += "Identified False" + SharedStrings.NewLine + SharedStrings.Tab;

            switch (isInfluenced)
            {
                case false when !_itemClassManager.AlwaysActive && !_applicationSettingService.RegalRecipe:
                    result += "ItemLevel >= 60" + SharedStrings.NewLine + SharedStrings.Tab + "ItemLevel <= 74" + SharedStrings.NewLine +
                              SharedStrings.Tab;
                    break;
                case false when _applicationSettingService.RegalRecipe:
                    result += "ItemLevel > 75" + SharedStrings.NewLine + SharedStrings.Tab;
                    break;
                default:
                    result += "ItemLevel >= 60" + SharedStrings.NewLine + SharedStrings.Tab;
                    break;
            }

            result = _itemClassManager.SetSocketRules(result);

            var baseType = _itemClassManager.SetBaseType();

            result = result + baseType + SharedStrings.NewLine + SharedStrings.Tab;

            var colors = GetRGB();
            var bgColor = colors.Aggregate("SetBackgroundColor", (current, t) => current + " " + t);

            result = result + bgColor + SharedStrings.NewLine + SharedStrings.Tab;
            result = isInfluenced
                ? _customStyleInfluenced.Aggregate(result, (current, cs) => current + cs + SharedStrings.NewLine + SharedStrings.Tab)
                : _customStyle.Aggregate(result, (current, cs) => current + cs + SharedStrings.NewLine + SharedStrings.Tab);

            if (_applicationSettingService.LootFilterIcons)
                result = result + "MinimapIcon 2 White Star" + SharedStrings.NewLine + SharedStrings.Tab;

            return result;
        }

        public string GenerateLootFilter(string oldFilter, IEnumerable<string> sections, bool isChaos = true)
        {
            // order has to be:
            // 1. exa start
            // 2. exa end
            // 3. chaos start
            // 4. chaos end
            string sectionName = isChaos ? "Chaos" : "Exalted";
            const string newLine = "\n";
            string sectionStart = "#Chaos Recipe Enhancer by kosace " + sectionName + " Recipe Start";
            string sectionEnd = "#Chaos Recipe Enhancer by kosace " + sectionName + " Recipe End";
            var sectionBody = "";
            var beforeSection = "";
            string afterSection;

            // generate chaos recipe section
            sectionBody += sectionStart + newLine + newLine;
            sectionBody = sections.Aggregate(sectionBody, (current, s) => current + s + newLine);
            sectionBody += sectionEnd + newLine;

            string[] sep = { sectionEnd + newLine };
            var split = oldFilter.Split(sep, StringSplitOptions.None);

            if (split.Length > 1)
            {
                afterSection = split[1];

                string[] sep2 = { sectionStart };
                var split2 = split[0].Split(sep2, StringSplitOptions.None);

                if (split2.Length > 1)
                    beforeSection = split2[0];
                else
                    afterSection = oldFilter;
            }
            else
            {
                afterSection = oldFilter;
            }

            return beforeSection + sectionBody + afterSection;
        }

        private async Task UpdateFilterAsync(IEnumerable<string> sectionList, IEnumerable<string> sectionListExalted)
        {
            var filterStorage = FilterStorageFactory.Create(_applicationSettingService.LootFilterFileLocation);

            var oldFilter = await filterStorage.ReadLootFilterAsync();
            if (oldFilter == null) return;

            var newFilter = GenerateLootFilter(oldFilter, sectionList);
            oldFilter = newFilter;
            newFilter = GenerateLootFilter(oldFilter, sectionListExalted, false);

            await filterStorage.WriteLootFilterAsync(newFilter);
        }

        private IEnumerable<int> GetRGB()
        {
            int r;
            int g;
            int b;
            int a;
            var color = _itemClassManager.ClassColor;
            var colorList = new List<int>();

            if (color != "")
            {
                a = Convert.ToByte(color.Substring(1, 2), 16);
                r = Convert.ToByte(color.Substring(3, 2), 16);
                g = Convert.ToByte(color.Substring(5, 2), 16);
                b = Convert.ToByte(color.Substring(7, 2), 16);
            }
            else
            {
                a = 255;
                r = 255;
                g = 0;
                b = 0;
            }

            colorList.Add(r);
            colorList.Add(g);
            colorList.Add(b);
            colorList.Add(a);

            return colorList;
        }

        private void LoadCustomStyle()
        {
            _customStyle.Clear();
            var pathNormalItemsStyle =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                    @"Assets\FilterStyles\NormalItemsStyle.txt");

            var style = File.ReadAllLines(pathNormalItemsStyle);
            foreach (var line in style)
            {
                if (line == "") continue;
                if (line.Contains("#")) continue;
                _customStyle.Add(line.Trim());
            }
        }

        private void LoadCustomStyleInfluenced()
        {
            _customStyleInfluenced.Clear();
            var pathInfluencedItemsStyle =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                    @"Assets\FilterStyles\InfluencedItemsStyle.txt");
            var style = File.ReadAllLines(pathInfluencedItemsStyle);
            foreach (var line in style)
            {
                if (line == "") continue;
                if (line.Contains("#")) continue;
                _customStyleInfluenced.Add(line.Trim());
            }
        }

        #endregion
    }
}