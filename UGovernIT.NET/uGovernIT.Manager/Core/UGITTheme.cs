using System;
using System.Collections.Generic;
using uGovernIT.Utility;

namespace uGovernIT.Core
{
    [Serializable]
    public class UGITTheme
    {
        public string ThemeName { get; set; }
        public string DevExTheme { get; set; }
        public string SPThemeName { get; set; }
        public string Photo { get; set; }
        public string FontName { get; set; }
        public string MenuHighlightColor { get; set; }
        public static string ThemeNameValue { get; set; }
        public static string FontNameValue { get; set; }

        public UGITTheme()
        {
            ThemeName = string.Empty;
            DevExTheme = string.Empty;
            SPThemeName = string.Empty;
            Photo = string.Empty;
            FontName = string.Empty;
            MenuHighlightColor = string.Empty;
        }

        public List<UGITTheme> GetThemes()
        {
            var lstUgitThemes = new List<UGITTheme>();
            //string[] strThemes = { "UGITClassicDevEx", "UGITBlackDevEx", "UGITDarkOrangeDevEx", "UGITGreenDevEx", "UGITNavyBlueDevEx" };
            string[] strThemes = { "UGITClassicDevEx",  "UGITDarkOrangeDevEx", "UGITGreenDevEx", "UGITNavyBlueDevEx" };

            foreach (string theme in strThemes)
            {
                var item = new UGITTheme
                {
                    ThemeName = theme
                };

                if (theme == "UGITClassicDevEx")
                {
                    item.DevExTheme = "UGITClassicDevEx";
                    item.SPThemeName = "Classic";
                    item.Photo = UGITUtility.GetAbsoluteURL("/Content/Images/ClassicTheme.png");

                }
                else if (theme == "UGITBlackDevEx")
                {
                    item.DevExTheme = "UGITBlackDevEx";
                    item.SPThemeName = "Black Flat";
                    item.Photo = UGITUtility.GetAbsoluteURL("/Content/Images/UGITBlack.png");

                }
                else if (theme == "UGITDarkOrangeDevEx")
                {
                    item.DevExTheme = "UGITDarkOrangeDevEx";
                    item.SPThemeName = "Dark Orange";
                    item.Photo = UGITUtility.GetAbsoluteURL("/Content/Images/UGITDarkOrange.png");

                }
                else if (theme == "UGITGreenDevEx")
                {
                    item.DevExTheme = "UGITGreenDevEx";
                    item.SPThemeName = "Green";
                    item.Photo = UGITUtility.GetAbsoluteURL("/Content/Images/UGITGreen.png");

                }
                else if (theme == "UGITNavyBlueDevEx")
                {
                    item.DevExTheme = "UGITNavyBlueDevEx";
                    item.SPThemeName = "Navy Blue";
                    item.Photo = UGITUtility.GetAbsoluteURL("/Content/Images/UGITNavyBlue.png");

                }
                else
                {
                    item = GetDefaultTheme();
                }

                bool isThemeExist = DevExpress.Web.ASPxThemes.ThemesProviderEx.GetThemes().Exists(x => x == item.DevExTheme);
                if (isThemeExist)
                    lstUgitThemes.Add(item);
            }

            return lstUgitThemes;
        }

        public static UGITTheme GetDefaultTheme()
        {
            UGITTheme item = new UGITTheme
            {
                // item.ThemeName = "Black";
                DevExTheme = "UGITBlackDevEx",
                SPThemeName = "Black Flat",
                Photo = UGITUtility.GetAbsoluteURL("/Content/Images/UGITBlack.png")
            };
            return item;
        }

        public static string SetThemeName(string value)
        {
            ThemeNameValue = value;
            return ThemeNameValue;
        }

        public static string GetThemeName()
        {
            return ThemeNameValue;
        }

        public static string SetFontName(string value)
        {
            FontNameValue = value;
            return FontNameValue;
        }

        public static string GetFontName()
        {
            return FontNameValue;
        }
    }
}
