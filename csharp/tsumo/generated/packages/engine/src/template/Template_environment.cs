namespace Tsumo.Engine
{
    public static class Template_environment
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Template_values.__tsonic_module_init();
            Template_paths.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class DeferredTemplateRequest
    {
        public string? key;
        public Tsonic.CSharp.Js.JSArray<TemplateNode> body;
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> definitions;
        public string? sourcePath;
        public string sourceText;
        public int sourceSegmentIndex;
        public TemplateValue data;
        public SiteContext site;
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides;
        public RenderState state;
        public string? result;
        public DeferredTemplateRequest(DeferredTemplateValue value, Tsonic.CSharp.Js.JSArray<TemplateNode> body, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> definitions, string? sourcePath, string sourceText, int sourceSegmentIndex, SiteContext site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides, RenderState state)
        {
            this.key = value.key;
            this.body = body;
            this.definitions = definitions;
            this.sourcePath = sourcePath;
            this.sourceText = sourceText;
            this.sourceSegmentIndex = sourceSegmentIndex;
            this.data = value.data;
            this.site = site;
            this.overrides = overrides;
            this.state = state;
            this.result = null;
        }
    }
    public class DeferredTemplatePlacement
    {
        public string token;
        public DeferredTemplateRequest request;
        public DeferredTemplatePlacement(string token, DeferredTemplateRequest request)
        {
            this.token = token;
            this.request = request;
        }
    }
    public class PartialTemplateResolution
    {
        public string kind;
        public Tsonic.CSharp.Js.JSArray<TemplateNode>? definition;
        public Template? template;
        public string? sourcePath;
        public PartialTemplateResolution(string kind, Tsonic.CSharp.Js.JSArray<TemplateNode>? definition, Template? template, string? sourcePath)
        {
            this.kind = kind;
            this.definition = definition;
            this.template = template;
            this.sourcePath = sourcePath;
        }
    }
    public class TemplateEnvironment
    {
        public bool isProduction = true;
        public Tsonic.CSharp.Js.Date buildTime;
        public Tsonic.CSharp.Js.JSArray<DeferredTemplateRequest> deferredRequests;
        public Tsonic.CSharp.Js.JSArray<DeferredTemplatePlacement> deferredPlacements;
        public string deferredPhase;
        public DictValue siteData;
        public ScratchStore globalStore;
        public TemplateEnvironment(Tsonic.CSharp.Js.Date? buildTime = null, DictValue? siteData = null)
        {
            this.buildTime = buildTime ?? new Tsonic.CSharp.Js.Date();
            this.deferredRequests = new Tsonic.CSharp.Js.JSArray<DeferredTemplateRequest>(new DeferredTemplateRequest[] { });
            this.deferredPlacements = new Tsonic.CSharp.Js.JSArray<DeferredTemplatePlacement>(new DeferredTemplatePlacement[] { });
            this.deferredPhase = "collecting";
            this.siteData = siteData ?? new DictValue(new Tsonic.CSharp.Js.Map<string, TemplateValue>());
            this.globalStore = new ScratchStore();
        }
        public string registerDeferredTemplate(DeferredTemplateValue value, Tsonic.CSharp.Js.JSArray<TemplateNode> body, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> definitions, string? sourcePath, string sourceText, int sourceSegmentIndex, SiteContext site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides, RenderState state)
        {
            if (this.deferredPhase != "collecting")
            {
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFER_LIFECYCLE_INVALID", "templates.Defer cannot register work after deferred-template finalization begins");
            }
            DeferredTemplateRequest? request = null;
            if (value.key is not null)
            {
                for (int index = 0; index < this.deferredRequests.length; index++)
                {
                    DeferredTemplateRequest candidate = this.deferredRequests[index];
                    if (candidate.key == value.key && candidate.sourcePath == sourcePath && candidate.sourceText == sourceText && candidate.sourceSegmentIndex == sourceSegmentIndex)
                    {
                        request = candidate;
                        break;
                    }
                }
            }
            if (request is null)
            {
                request = new DeferredTemplateRequest(value, body, definitions, sourcePath, sourceText, sourceSegmentIndex, site, overrides, state);
                this.deferredRequests.push(request);
            }
            int ordinal = this.deferredPlacements.length;
            string token = $"\0TSUMO-DEFERRED-TEMPLATE:{ordinal}\0";
            this.deferredPlacements.push(new DeferredTemplatePlacement(token, request));
            return token;
        }
        public Tsonic.CSharp.Js.Map<string, string> finalizeDeferredTemplates()
        {
            if (this.deferredPhase == "finalizing")
            {
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFER_LIFECYCLE_INVALID", "Deferred-template finalization is already running");
            }
            if (this.deferredPhase == "collecting")
            {
                this.deferredPhase = "finalizing";
                for (int index = 0; index < this.deferredRequests.length; index++)
                {
                    DeferredTemplateRequest request = this.deferredRequests[index];
                    request.result = this.renderTemplateDefinition(request.body, request.definitions, request.sourcePath, request.data, request.site, request.overrides, request.state);
                }
                this.deferredPhase = "finalized";
            }
            Tsonic.CSharp.Js.Map<string, string> results = new Tsonic.CSharp.Js.Map<string, string>();
            for (int index_1 = 0; index_1 < this.deferredPlacements.length; index_1++)
            {
                DeferredTemplatePlacement placement = this.deferredPlacements[index_1];
                string? result = placement.request.result;
                if (result is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFER_LIFECYCLE_INVALID", "A deferred template has no finalized output");
                }
                results.set(placement.token, result);
            }
            return results;
        }
        public virtual string? getEnvironmentVariable(string _name)
        {
            return null;
        }
        public void setSiteData(DictValue value)
        {
            this.siteData = value;
        }
        public DictValue getSiteData()
        {
            return this.siteData;
        }
        public ScratchStore getGlobalStore()
        {
            return this.globalStore;
        }
        public virtual bool sourceFileExists(string _path)
        {
            return false;
        }
        public virtual Template? getTemplate(string _relPath)
        {
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_ENVIRONMENT_OPERATION_UNAVAILABLE", "TemplateEnvironment.getTemplate is not implemented");
        }
        public virtual string? getTemplateSourceRelativePath(string _sourcePath)
        {
            return null;
        }
        public PartialTemplateResolution? resolvePartialTemplate(string name, string? callerSourcePath, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> definitions)
        {
            string? callerRelativePath = null;
            if (callerSourcePath is not null)
            {
                string selectedSourcePath = callerSourcePath;
                callerRelativePath = this.getTemplateSourceRelativePath(selectedSourcePath);
            }
            Tsonic.CSharp.Js.JSArray<string> candidates = Template_paths.partialTemplateCandidates(name, callerRelativePath);
            for (int index = 0; index < candidates.length; index++)
            {
                string candidate = candidates[index];
                Tsonic.CSharp.Js.JSArray<TemplateNode>? definition = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>(definitions, candidate);
                if (definition is not null)
                {
                    return new PartialTemplateResolution("definition", definition, null, callerSourcePath);
                }
                Template? template = this.getTemplate(candidate);
                if (template is not null)
                {
                    Template selected = template.withInheritedDefinitions(definitions);
                    return new PartialTemplateResolution("template", null, selected, selected.sourcePath);
                }
            }
            return null;
        }
        public virtual string? renderPageView(PageContext _page, string _view, RenderState? _state)
        {
            return null;
        }
        public virtual Template? getShortcodeTemplate(string _name)
        {
            return null;
        }
        public virtual Template? getRenderHookTemplate(string _hookName)
        {
            return null;
        }
        public virtual ResourceManager? getResourceManager()
        {
            return null;
        }
        public virtual string renderTextTemplateSource(string _source, TemplateValue _context, SiteContext _site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> _overrides, RenderState? _state = null)
        {
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_ENVIRONMENT_OPERATION_UNAVAILABLE", "TemplateEnvironment.renderTextTemplateSource is not implemented");
        }
        public virtual string renderTemplate(Template _template, TemplateValue _context, SiteContext _site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> _overrides, RenderState? _state = null)
        {
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_ENVIRONMENT_OPERATION_UNAVAILABLE", "TemplateEnvironment.renderTemplate is not implemented");
        }
        public virtual string renderTextTemplate(Template _template, TemplateValue _context, SiteContext _site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> _overrides, RenderState? _state = null)
        {
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_ENVIRONMENT_OPERATION_UNAVAILABLE", "TemplateEnvironment.renderTextTemplate is not implemented");
        }
        public virtual string renderTemplateDefinition(Tsonic.CSharp.Js.JSArray<TemplateNode> _nodes, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> _definitions, string? _sourcePath, TemplateValue _context, SiteContext _site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> _overrides, RenderState? _state = null)
        {
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_ENVIRONMENT_OPERATION_UNAVAILABLE", "TemplateEnvironment.renderTemplateDefinition is not implemented");
        }
        public virtual string getI18n(string _lang, string _key, int? _count = null)
        {
            return _key;
        }
    }
}
