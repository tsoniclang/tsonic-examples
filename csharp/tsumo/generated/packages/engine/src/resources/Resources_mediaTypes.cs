using System;

namespace Tsumo.Engine
{
    public static class Resources_mediaTypes
    {
        public static Func<string, string> resourceMediaTypeForExtension
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, bool> isImageResourceExtension
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, string, bool> resourceMatchesMediaType
        {
            get;
            private set;
        } = default(Func<string, string, bool>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            resourceMediaTypeForExtension = (string extension) =>
            {
                string value = Tsonic.CSharp.Js.String.toLowerCase(extension);
                if (value == ".png")
                {
                    return "image/png";
                }
                if (value == ".jpg" || value == ".jpeg")
                {
                    return "image/jpeg";
                }
                if (value == ".gif")
                {
                    return "image/gif";
                }
                if (value == ".webp")
                {
                    return "image/webp";
                }
                if (value == ".svg")
                {
                    return "image/svg+xml";
                }
                if (value == ".ico")
                {
                    return "image/x-icon";
                }
                if (value == ".bmp")
                {
                    return "image/bmp";
                }
                if (value == ".tiff" || value == ".tif")
                {
                    return "image/tiff";
                }
                if (value == ".js" || value == ".mjs")
                {
                    return "application/javascript";
                }
                if (value == ".ts" || value == ".tsx")
                {
                    return "text/typescript";
                }
                if (value == ".jsx")
                {
                    return "text/jsx";
                }
                if (value == ".json")
                {
                    return "application/json";
                }
                if (value == ".yaml" || value == ".yml")
                {
                    return "application/yaml";
                }
                if (value == ".toml")
                {
                    return "application/toml";
                }
                if (value == ".css")
                {
                    return "text/css";
                }
                if (value == ".scss" || value == ".sass")
                {
                    return "text/x-scss";
                }
                if (value == ".html" || value == ".htm")
                {
                    return "text/html";
                }
                if (value == ".xml")
                {
                    return "application/xml";
                }
                if (value == ".txt")
                {
                    return "text/plain";
                }
                if (value == ".woff")
                {
                    return "font/woff";
                }
                if (value == ".woff2")
                {
                    return "font/woff2";
                }
                if (value == ".ttf")
                {
                    return "font/ttf";
                }
                if (value == ".otf")
                {
                    return "font/otf";
                }
                if (value == ".eot")
                {
                    return "application/vnd.ms-fontobject";
                }
                if (value == ".pdf")
                {
                    return "application/pdf";
                }
                if (value == ".zip")
                {
                    return "application/zip";
                }
                return "application/octet-stream";
            };
            isImageResourceExtension = (string extension) =>
            {
                string value = Tsonic.CSharp.Js.String.toLowerCase(extension);
                return value == ".png" || value == ".jpg" || value == ".jpeg" || value == ".gif" || value == ".webp" || value == ".bmp";
            };
            resourceMatchesMediaType = (string actual, string requested) =>
            {
                string target = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(requested));
                if (target == "")
                {
                    return false;
                }
                string mediaType = Tsonic.CSharp.Js.String.toLowerCase(actual);
                return Tsonic.CSharp.Js.String.includes(target, "/") ? mediaType == target : Tsonic.CSharp.Js.String.startsWith(mediaType, target + "/");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
