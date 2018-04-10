using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using OElite;

namespace rider_view_syntaxhighlightissues.Base
{
    public class TestFileProvider : IFileProvider
    {
        private readonly List<string> _preservedPaths = new List<string>
        {
            "/oes",
            "/_ViewImports.cshtml",
            "/Views/_ViewImports.cshtml"
        };

        public TestFileProvider(IHostingEnvironment env)
        {
            _physicalFileProvider = new PhysicalFileProvider(env.ContentRootPath);
        }


        private readonly IServiceProvider _spv;

        private IFileProvider CloudFileProvider;
        private readonly IFileProvider _physicalFileProvider;

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _physicalFileProvider.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var subpathSegments = subpath?.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            long siteId = 0;
            if (subpathSegments?.Length > 2)
            {
                siteId = NumericUtils.GetLongIntegerValueFromObject(subpathSegments[0]);
            }

            subpath = VerifyPath(subpath);

            return _physicalFileProvider.GetFileInfo(subpath);
        }

        public string VerifyPath(string subpath)
        {
            var cloudPrefix = string.Empty;

            var subpathSegments = subpath?.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            long siteId = 0;
            long siteActiveThemeId = 0;
            if (subpathSegments?.Length > 2)
            {
                siteId = NumericUtils.GetLongIntegerValueFromObject(subpathSegments[0]);
                siteActiveThemeId = NumericUtils.GetLongIntegerValueFromObject(subpathSegments[1]);
            }

            try
            {
                cloudPrefix = $"/{siteId}/{siteActiveThemeId}/";

                if (subpath.StartsWith(cloudPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    subpath = $"{subpath.Substring(cloudPrefix.Length - 1)}";
                }
                else if (subpath.StartsWith($"/{siteId}/", StringComparison.OrdinalIgnoreCase))
                {
                    subpath = $"{subpath.Substring($"/{siteId}/".Length - 1)}";
                }
            }
            catch (Exception ex)
            {
                //OE.LogError(ex.Message, ex);
            }

            var isPreserved = IsPreserved(siteId, subpath);
            if (!isPreserved)
            {
                if (!subpath.StartsWith("/local/", StringComparison.InvariantCultureIgnoreCase))
                    subpath = $"/local{subpath}";
            }
            else
            {
                if (!subpath.StartsWith("/local/", StringComparison.InvariantCultureIgnoreCase))
                    subpath = $"/local{subpath}";
            }

            return subpath.Trim('/');
        }

        private bool IsPreserved(long siteId, string subpath)
        {
            return siteId <= 0 ||
                   _preservedPaths.Count(
                       item => subpath?.StartsWith(item, StringComparison.CurrentCultureIgnoreCase) ==
                               true) >
                   0;
        }

        public IChangeToken Watch(string filter)
        {
            var subpathSegments = filter?.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            long siteId = 0;
            long siteActiveThemeId = 0;
            if (subpathSegments?.Length > 2)
            {
                siteId = NumericUtils.GetLongIntegerValueFromObject(subpathSegments[0]);
            }

            var path = VerifyPath(filter);
            return _physicalFileProvider.Watch($"{path}");
        }
    }
}