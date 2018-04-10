using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;
using OElite;

namespace rider_view_syntaxhighlightissues.Base
{
    public class TestViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var siteId = 1;
            var siteActiveThemeId = 1;
            context.Values["siteId"] = siteId.ToString();
            context.Values["siteActiveThemeId"] = siteActiveThemeId.ToString();
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            var siteId = 1;
            var siteActiveThemeId = 1;

            var formats = new List<string>
            {
                $"{siteId}/{siteActiveThemeId}/{{0}}.cshtml"
            };
            if (context.IsMainPage)
            {
                formats.Insert(0,
                    context.AreaName.IsNotNullOrEmpty()
                        ? $"{siteId}/{siteActiveThemeId}/Views/{{2}}/{{0}}.cshtml"
                        : $"{siteId}/{siteActiveThemeId}/Views/{{0}}.cshtml");
            }
            else
            {
                formats.Insert(0, $"{siteId}/{siteActiveThemeId}/Views/Partials/{{0}}.cshtml");

                if (context.AreaName.IsNotNullOrEmpty())
                    formats.Insert(0, $"{siteId}/{siteActiveThemeId}/Views/{{2}}/Partials/{{0}}.cshtml");
            }

            return formats;
        }
    }
}