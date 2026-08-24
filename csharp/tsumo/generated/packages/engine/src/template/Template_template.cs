using System;

namespace Tsumo.Engine
{
    public static class Template_template
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_textBuilder.__tsonic_module_init();
            Diagnostics.__tsonic_module_init();
            Models.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Template_scope.__tsonic_module_init();
            Template_nodes.__tsonic_module_init();
            Template_evaluation_render.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class Template
    {
        public Tsonic.CSharp.Js.JSArray<TemplateNode> nodes;
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> defines;
        public string? sourcePath;
        public Template(Tsonic.CSharp.Js.JSArray<TemplateNode> nodes, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> defines, string? sourcePath = null)
        {
            this.nodes = nodes;
            this.defines = defines;
            this.sourcePath = sourcePath;
        }
        public Template withInheritedDefinitions(Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> inherited)
        {
            if (inherited.size == 0)
            {
                return this;
            }
            Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> definitions = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>();
            foreach (string name in inherited.keys())
            {
                Tsonic.CSharp.Js.JSArray<TemplateNode>? inheritedBody = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>(inherited, name);
                if (inheritedBody is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFINE_INVENTORY_INVALID", $"Inherited template definition '{name}' has no body", this.sourcePath);
                }
                definitions.set(name, inheritedBody);
            }
            foreach (string name_1 in this.defines.keys())
            {
                Tsonic.CSharp.Js.JSArray<TemplateNode>? body = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>(this.defines, name_1);
                if (body is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFINE_INVENTORY_INVALID", $"Template definition '{name_1}' has no body", this.sourcePath);
                }
                Tsonic.CSharp.Js.JSArray<TemplateNode>? existing = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>(definitions, name_1);
                if (existing is not null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFINE_CONFLICT", $"Template definition '{name_1}' conflicts with an inherited definition", this.sourcePath);
                }
                definitions.set(name_1, body);
            }
            return new Template(this.nodes, definitions, this.sourcePath);
        }
        public string render(PageContext root, TemplateEnvironment env, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>? overrides = null, RenderState? state = null)
        {
            TextBuilder sb = new TextBuilder();
            PageValue pageValue = new PageValue(root);
            RenderScope scope = new RenderScope(pageValue, pageValue, root.site, env, null, state, this.sourcePath);
            Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> defs = overrides ?? new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>();
            this.renderInto(sb, scope, env, defs);
            return sb.toString();
        }
        public void renderInto(TextBuilder sb, RenderScope scope, TemplateEnvironment env, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides)
        {
            string control = Template_evaluation_render.renderTemplateNodes(this.nodes, sb, scope, env, overrides, this.defines, "html");
            if (control != "normal")
            {
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_CONTROL_FLOW_INVALID", "Template loop control escaped the checked template root");
            }
        }
        public void renderTextInto(TextBuilder sb, RenderScope scope, TemplateEnvironment env, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides)
        {
            string control = Template_evaluation_render.renderTemplateNodes(this.nodes, sb, scope, env, overrides, this.defines, "text");
            if (control != "normal")
            {
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_CONTROL_FLOW_INVALID", "Template loop control escaped the checked template root");
            }
        }
    }
}
