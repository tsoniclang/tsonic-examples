using System;

namespace Tsumo.Engine
{
    public static class Template_syntax_expressions
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class Expr
    {
    }
    public class TokenExpr : Expr
    {
        public string token;
        public TokenExpr(string token) : base()
        {
            this.token = token;
        }
    }
    public class PipelineExpr : Expr
    {
        public Pipeline pipeline;
        public PipelineExpr(Pipeline pipeline) : base()
        {
            this.pipeline = pipeline;
        }
    }
    public class CommandExpr : Expr
    {
        public Command command;
        public CommandExpr(Command command) : base()
        {
            this.command = command;
        }
    }
    public class AccessExpr : Expr
    {
        public Expr @base;
        public Tsonic.CSharp.Js.JSArray<string> segments;
        public AccessExpr(Expr @base, Tsonic.CSharp.Js.JSArray<string> segments) : base()
        {
            this.@base = @base;
            this.segments = segments;
        }
    }
    public class Command
    {
        public Expr head;
        public Tsonic.CSharp.Js.JSArray<Expr> args;
        public Command(Expr head, Tsonic.CSharp.Js.JSArray<Expr> args)
        {
            this.head = head;
            this.args = args;
        }
    }
    public class Pipeline
    {
        public Tsonic.CSharp.Js.JSArray<Command> stages;
        public Pipeline(Tsonic.CSharp.Js.JSArray<Command> stages)
        {
            this.stages = stages;
        }
    }
}
