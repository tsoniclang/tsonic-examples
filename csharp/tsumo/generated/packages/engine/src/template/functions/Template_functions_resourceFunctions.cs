using System;

namespace Tsumo.Engine
{
    public static class Template_functions_resourceFunctions
    {
        internal static TemplateValue? resourceBuildOption(DictValue options, string name)
        {
            TemplateValue? exact = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(options.value, name);
            if (exact is not null)
            {
                return exact;
            }
            string normalized = Tsonic.CSharp.Js.String.toLowerCase(name);
            foreach (string key in options.value.keys())
            {
                if (Tsonic.CSharp.Js.String.toLowerCase(key) != normalized)
                {
                    continue;
                }
                return Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(options.value, key);
            }
            return null;
        }
        internal static void validateCssBuildOptions(DictValue options)
        {
            foreach (string key in options.value.keys())
            {
                string normalized = Tsonic.CSharp.Js.String.toLowerCase(key);
                if (normalized == "targetpath" || normalized == "minify" || normalized == "sourcemap")
                {
                    continue;
                }
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_CSS_BUILD_OPTION_UNKNOWN", $"css.Build does not support option '{key}'");
            }
        }
        internal static ResourceValue buildCssResource(ResourceManager manager, Resource source, DictValue options)
        {
            validateCssBuildOptions(options);
            TemplateValue? sourceMap = resourceBuildOption(options, "sourceMap");
            if (sourceMap is not null && Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(sourceMap))) != "none")
            {
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_CSS_BUILD_SOURCE_MAP_UNSUPPORTED", "css.Build supports only sourceMap 'none'");
            }
            Resource result = source;
            TemplateValue? targetPath = resourceBuildOption(options, "targetPath");
            if (targetPath is not null && Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(targetPath)) != "")
            {
                result = manager.copy(Template_runtimeHelpers.toPlainString(targetPath), result);
            }
            TemplateValue? minify = resourceBuildOption(options, "minify");
            if ((object?)minify is BoolValue)
            {
                if (((BoolValue)minify).value)
                {
                    result = manager.minify(result);
                }
            }
            else
            {
                if (minify is not null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_CSS_BUILD_MINIFY_INVALID", "css.Build minify must be a boolean");
                }
            }
            return new ResourceValue(manager, result);
        }
        internal static bool javascriptOptionIsOneOf(string value, Tsonic.CSharp.Js.JSArray<string> accepted)
        {
            for (double index = 0; index < accepted.length; index++)
            {
                if (value == accepted[index])
                {
                    return true;
                }
            }
            return false;
        }
        internal static string serializeJavaScriptBuildParams(TemplateValue value)
        {
            if ((object?)value is DictValue)
            {
                return Template_evaluation_serialization.toJson((DictValue)value);
            }
            if ((object?)value is AnyArrayValue)
            {
                return Template_evaluation_serialization.toJson((AnyArrayValue)value);
            }
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_JAVASCRIPT_BUILD_PARAMS_INVALID", "js.Build params must be a dictionary or slice");
        }
        internal static JavaScriptBuildOptions parseJavaScriptBuildOptionDictionary(DictValue value)
        {
            JavaScriptBuildOptions options = new JavaScriptBuildOptions();
            foreach (string key in value.value.keys())
            {
                string normalized = Tsonic.CSharp.Js.String.toLowerCase(key);
                if (normalized == "targetpath" || normalized == "minify" || normalized == "format" || normalized == "target" || normalized == "platform" || normalized == "sourcemap" || normalized == "params" || normalized == "jsxfactory")
                {
                    continue;
                }
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_JAVASCRIPT_BUILD_OPTION_UNKNOWN", $"js.Build does not support option '{key}'");
            }
            TemplateValue? targetPath = resourceBuildOption(value, "targetPath");
            if (targetPath is not null)
            {
                options.targetPath = Template_runtimeHelpers.toPlainString(targetPath);
            }
            TemplateValue? minify = resourceBuildOption(value, "minify");
            if (minify is not null)
            {
                if (!((object?)minify is BoolValue))
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_JAVASCRIPT_BUILD_MINIFY_INVALID", "js.Build minify must be a boolean");
                }
                options.minify = ((BoolValue)minify).value;
            }
            TemplateValue? format = resourceBuildOption(value, "format");
            if (format is not null)
            {
                string selected = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(format)));
                if (!javascriptOptionIsOneOf(selected, Tsonic.CSharp.Js.JSArray<string>.of(["iife", "cjs", "esm"])))
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_JAVASCRIPT_BUILD_FORMAT_INVALID", $"js.Build format '{selected}' is invalid");
                }
                options.format = selected;
            }
            TemplateValue? target = resourceBuildOption(value, "target");
            if (target is not null)
            {
                string selected_1 = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(target)));
                if (!javascriptOptionIsOneOf(selected_1, Tsonic.CSharp.Js.JSArray<string>.of(["es5", "es2015", "es2016", "es2017", "es2018", "es2019", "es2020", "es2021", "es2022", "es2023", "es2024", "esnext"])))
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_JAVASCRIPT_BUILD_TARGET_INVALID", $"js.Build target '{selected_1}' is invalid");
                }
                options.target = selected_1;
            }
            TemplateValue? platform = resourceBuildOption(value, "platform");
            if (platform is not null)
            {
                string selected_2 = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(platform)));
                if (!javascriptOptionIsOneOf(selected_2, Tsonic.CSharp.Js.JSArray<string>.of(["browser", "node", "neutral"])))
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_JAVASCRIPT_BUILD_PLATFORM_INVALID", $"js.Build platform '{selected_2}' is invalid");
                }
                options.platform = selected_2;
            }
            TemplateValue? sourceMap = resourceBuildOption(value, "sourceMap");
            if (sourceMap is not null)
            {
                options.sourceMap = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(sourceMap)));
            }
            TemplateValue? @params = resourceBuildOption(value, "params");
            if (@params is not null)
            {
                options.paramsJson = serializeJavaScriptBuildParams(@params);
            }
            TemplateValue? jsxFactory = resourceBuildOption(value, "JSXFactory");
            if (jsxFactory is not null)
            {
                options.jsxFactory = Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(jsxFactory));
            }
            return options;
        }
        internal static JavaScriptBuildOptions parseJavaScriptBuildOptions(TemplateValue value)
        {
            if ((object?)value is StringValue)
            {
                JavaScriptBuildOptions options = new JavaScriptBuildOptions();
                options.targetPath = ((StringValue)value).value;
                return options;
            }
            if ((object?)value is DictValue)
            {
                return parseJavaScriptBuildOptionDictionary((DictValue)value);
            }
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_JAVASCRIPT_BUILD_OPTIONS_INVALID", "js.Build options must be a dictionary or target path string");
        }
        public static Func<string, Tsonic.CSharp.Js.JSArray<TemplateValue>, TemplateFunctionContext, TemplateValue?> callResourceFunction
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<TemplateValue>, TemplateFunctionContext, TemplateValue?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Resources.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Template_runtimeHelpers.__tsonic_module_init();
            Template_evaluation_serialization.__tsonic_module_init();
            callResourceFunction = (string name, Tsonic.CSharp.Js.JSArray<TemplateValue> args, TemplateFunctionContext context) =>
            {
                RenderScope scope = context.scope;
                TemplateEnvironment env = context.environment;
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides = context.overrides;
                if (name == "resources.get" && args.length >= 1)
                {
                    ResourceManager? mgr = env.getResourceManager();
                    if (mgr is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    string path = Template_runtimeHelpers.toPlainString(args[0]);
                    Resource? res = mgr.get(path);
                    return res is not null ? new ResourceValue(mgr, res) : Template_runtimeHelpers.nil;
                }
                if (name == "resources.getmatch" && args.length >= 1)
                {
                    ResourceManager? mgr_1 = env.getResourceManager();
                    if (mgr_1 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    string pattern = Template_runtimeHelpers.toPlainString(args[0]);
                    Resource? res_1 = mgr_1.getMatch(pattern);
                    return res_1 is not null ? new ResourceValue(mgr_1, res_1) : Template_runtimeHelpers.nil;
                }
                if (name == "resources.match" && args.length >= 1)
                {
                    ResourceManager? mgr_2 = env.getResourceManager();
                    if (mgr_2 is null)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> emptyItems = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        return new AnyArrayValue(emptyItems);
                    }
                    string pattern_1 = Template_runtimeHelpers.toPlainString(args[0]);
                    Tsonic.CSharp.Js.JSArray<Resource> resources = mgr_2.match(pattern_1);
                    Tsonic.CSharp.Js.JSArray<TemplateValue> result = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                    for (double i = 0; i < resources.length; i++)
                    {
                        result.push(new ResourceValue(mgr_2, resources[i]));
                    }
                    return new AnyArrayValue(result);
                }
                if (name == "resources.bytype" && args.length >= 1)
                {
                    ResourceManager? mgr_3 = env.getResourceManager();
                    if (mgr_3 is null)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> emptyItems_1 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        return new AnyArrayValue(emptyItems_1);
                    }
                    string mediaType = Template_runtimeHelpers.toPlainString(args[0]);
                    Tsonic.CSharp.Js.JSArray<Resource> resources_1 = mgr_3.byType(mediaType);
                    Tsonic.CSharp.Js.JSArray<TemplateValue> result_1 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                    for (double i_1 = 0; i_1 < resources_1.length; i_1++)
                    {
                        result_1.push(new ResourceValue(mgr_3, resources_1[i_1]));
                    }
                    return new AnyArrayValue(result_1);
                }
                if (name == "resources.concat" && args.length >= 2)
                {
                    ResourceManager? mgr_4 = env.getResourceManager();
                    if (mgr_4 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    string targetPath = Template_runtimeHelpers.toPlainString(args[0]);
                    TemplateValue input = args[args.length - 1];
                    Tsonic.CSharp.Js.JSArray<Resource> resources_2 = Tsonic.CSharp.Js.JSArray<Resource>.of([]);
                    if ((object?)input is AnyArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> arr = ((AnyArrayValue)input).value;
                        for (double i_2 = 0; i_2 < arr.length; i_2++)
                        {
                            TemplateValue item = arr[i_2];
                            if ((object?)item is ResourceValue)
                            {
                                resources_2.push(((ResourceValue)item).value);
                            }
                        }
                    }
                    else
                    {
                        if ((object?)input is ResourceValue)
                        {
                            resources_2.push(((ResourceValue)input).value);
                        }
                    }
                    if (resources_2.length == 0)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    Resource res_2 = mgr_4.concat(targetPath, resources_2);
                    return new ResourceValue(mgr_4, res_2);
                }
                if (name == "resources.fromstring" && args.length >= 2)
                {
                    ResourceManager? mgr_5 = env.getResourceManager();
                    if (mgr_5 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    string nameArg = Template_runtimeHelpers.toPlainString(args[0]);
                    string content = Template_runtimeHelpers.toPlainString(args[1]);
                    Resource res_3 = mgr_5.fromString(nameArg, content);
                    return new ResourceValue(mgr_5, res_3);
                }
                if (name == "resources.executeastemplate" && args.length >= 2)
                {
                    ResourceManager? mgr_6 = env.getResourceManager();
                    if (mgr_6 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    TemplateValue piped = args.length >= 3 ? args[args.length - 1] : Template_runtimeHelpers.nil;
                    bool isResource = (object?)piped is ResourceValue;
                    if (isResource == false)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    Resource src = ((ResourceValue)piped).value;
                    string targetName = Template_runtimeHelpers.toPlainString(args[0]);
                    TemplateValue ctx = args[1];
                    string templateText = Resources_text.readResourceText(src, "resources.ExecuteAsTemplate");
                    string rendered = env.renderTextTemplateSource(templateText, ctx, scope.site, overrides);
                    Tsonic.CSharp.Node.Buffer bytes = Tsonic.CSharp.Node.Buffer.from(rendered, "utf8");
                    string lang = scope.site.Language.Lang;
                    string id = $"{src.id}|executeAsTemplate:{targetName}|lang:{lang}";
                    Resource @out = new Resource(id, src.sourcePath, src.publishable, targetName, bytes, rendered, new ResourceData(""));
                    return new ResourceValue(mgr_6, @out);
                }
                if (name == "resources.minify" || name == "minify")
                {
                    ResourceManager? mgr_7 = env.getResourceManager();
                    if (mgr_7 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    TemplateValue piped_1 = args.length >= 1 ? args[args.length - 1] : Template_runtimeHelpers.nil;
                    bool isResource_1 = (object?)piped_1 is ResourceValue;
                    if (isResource_1 == false)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    Resource src_1 = ((ResourceValue)piped_1).value;
                    Resource res_4 = mgr_7.minify(src_1);
                    return new ResourceValue(mgr_7, res_4);
                }
                if ((name == "resources.fingerprint" || name == "fingerprint") && args.length >= 1)
                {
                    ResourceManager? mgr_8 = env.getResourceManager();
                    if (mgr_8 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    TemplateValue piped_2 = args[args.length - 1];
                    bool isResource_2 = (object?)piped_2 is ResourceValue;
                    if (isResource_2 == false)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    Resource src_2 = ((ResourceValue)piped_2).value;
                    Resource res_5 = mgr_8.fingerprint(src_2);
                    return new ResourceValue(mgr_8, res_5);
                }
                if (name == "resources.copy" && args.length >= 2)
                {
                    ResourceManager? mgr_9 = env.getResourceManager();
                    if (mgr_9 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    string targetPath_1 = Template_runtimeHelpers.toPlainString(args[0]);
                    TemplateValue piped_3 = args[args.length - 1];
                    bool isResource_3 = (object?)piped_3 is ResourceValue;
                    if (isResource_3 == false)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    Resource src_3 = ((ResourceValue)piped_3).value;
                    Resource res_6 = mgr_9.copy(targetPath_1, src_3);
                    return new ResourceValue(mgr_9, res_6);
                }
                if ((name == "images.resize" || name == "resize") && args.length >= 1)
                {
                    ResourceManager? mgr_10 = env.getResourceManager();
                    if (mgr_10 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    string spec = args.length >= 2 ? Template_runtimeHelpers.toPlainString(args[0]) : "";
                    TemplateValue piped_4 = args[args.length - 1];
                    bool isResource_4 = (object?)piped_4 is ResourceValue;
                    if (isResource_4 == false)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    Resource src_4 = ((ResourceValue)piped_4).value;
                    Resource res_7 = mgr_10.resize(src_4, spec);
                    return new ResourceValue(mgr_10, res_7);
                }
                if (name == "css.sass" && args.length >= 1)
                {
                    ResourceManager? mgr_11 = env.getResourceManager();
                    if (mgr_11 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    TemplateValue piped_5 = args[args.length - 1];
                    bool isResource_5 = (object?)piped_5 is ResourceValue;
                    if (isResource_5 == false)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    Resource src_5 = ((ResourceValue)piped_5).value;
                    Resource res_8 = mgr_11.sassCompile(src_5);
                    return new ResourceValue(mgr_11, res_8);
                }
                if (name == "css.build" && args.length >= 1)
                {
                    ResourceManager? mgr_12 = env.getResourceManager();
                    if (mgr_12 is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    TemplateValue piped_6 = args[args.length - 1];
                    if ((object?)piped_6 is ResourceValue)
                    {
                        if (args.length < 2)
                        {
                            return buildCssResource(mgr_12, ((ResourceValue)piped_6).value, new DictValue(new Tsonic.CSharp.Js.Map<string, TemplateValue>()));
                        }
                        TemplateValue options = args[0];
                        if ((object?)options is DictValue)
                        {
                            return buildCssResource(mgr_12, ((ResourceValue)piped_6).value, (DictValue)options);
                        }
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_CSS_BUILD_OPTIONS_INVALID", "css.Build options must be a dictionary");
                    }
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_CSS_BUILD_INPUT_INVALID", "css.Build requires a CSS resource input");
                }
                if (name == "js.build" && args.length >= 1)
                {
                    ResourceManager? manager = env.getResourceManager();
                    if (manager is null)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    TemplateValue piped_7 = args[args.length - 1];
                    if (!((object?)piped_7 is ResourceValue))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_JAVASCRIPT_BUILD_INPUT_INVALID", "js.Build requires a JavaScript resource input");
                    }
                    if (args.length > 2)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_JAVASCRIPT_BUILD_OPTIONS_INVALID", "js.Build accepts at most one options argument");
                    }
                    JavaScriptBuildOptions options_1 = args.length == 2 ? parseJavaScriptBuildOptions(args[0]) : new JavaScriptBuildOptions();
                    return new ResourceValue(manager, manager.javascriptBuild(((ResourceValue)piped_7).value, options_1));
                }
                return null;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
