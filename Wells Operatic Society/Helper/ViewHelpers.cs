using System;
using System.Linq;
using Umbraco.Web;
using WellsOperaticSociety.Models.UmbracoModels;

namespace WellsOperaticSociety.Web.Helper
{
    public static class ViewHelpers
    {
        private static readonly Random _rnd = new Random();

        public static string GetImage(Function function)
        {
            return function.Image.IsNullOrEmpty() ? "" : function.Image.First().GetCropUrl();
        }

        public static string GetRandomBackgroundColour()
        {
            string[] colours = { "#d2691e", "#a52a2a", "#008080", "#00bfff" };

            var selection = _rnd.Next(0, colours.Length);

            return colours[selection];

        }
    }
}