using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using EnhancePoE.App.Services;
using EnhancePoE.UI.Properties;

namespace EnhancePoE.UI.Model
{
    internal static class StashTabList
    {
        public static List<StashTab> StashTabs { get; set; } = new List<StashTab>();
        public static List<int> StashTabIndices { get; set; }

        public static void GetStashTabIndices()
        {
            if (IoC.Get<ApplicationSettingService>().StashTabQueryIndices != "")
            {
                var ret = new List<int>();
                var indices = IoC.Get<ApplicationSettingService>().StashTabQueryIndices;
                string[] sep = { "," };
                var split = indices.Split(sep, StringSplitOptions.None);

                foreach (var s in split)
                {
                    if (int.TryParse(s.Trim(), out var parsedIndex))
                    {
                        if (!ret.Contains(parsedIndex)) ret.Add(parsedIndex);
                    }
                    else
                    {
                        MessageBox.Show("Stashtab Index has to be a number!", "Stashtab Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                if (ret.Count == 0) MessageBox.Show("Stashtab Indices empty!", "Stashtab Error", MessageBoxButton.OK, MessageBoxImage.Error);

                StashTabIndices = ret;

                //Trace.WriteLine(ret, "ret");
                //foreach(int i in StashTabIndices)
                //{
                //    Trace.WriteLine(i, "stash tab index");
                //}
            }
            else
            {
                MessageBox.Show("Stashtab Indices empty!", "Stashtab Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}