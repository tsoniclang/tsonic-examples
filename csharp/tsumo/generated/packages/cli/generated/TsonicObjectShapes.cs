namespace Tsumo.Cli
{
    public class ObjectShape_5338cd423f9f : ObjectShape_6409d8cac3e5
    {
        public required bool isNamed
        {
            get;
            set;
        }
        public required Tsonic.CSharp.Js.Map<string, ParamValue> __tsonic_member_a20b52fae57cc7a99c9651f1b573950fd211823e3ace3bb9c273c06430f24cd3
        {
            get;
            set;
        }
        public required Tsonic.CSharp.Js.JSArray<string> positional
        {
            get;
            set;
        }
    }
    public interface ObjectShape_6409d8cac3e5
    {
        Tsonic.CSharp.Js.Map<string, ParamValue> __tsonic_member_a20b52fae57cc7a99c9651f1b573950fd211823e3ace3bb9c273c06430f24cd3 { get; set; }
        Tsonic.CSharp.Js.JSArray<string> positional { get; set; }
        bool isNamed { get; set; }
    }
    public class ObjectShape_d9144763d433 : ObjectShape_ec37db37c8f1
    {
        public required int endPos
        {
            get;
            set;
        }
        public required string inner
        {
            get;
            set;
        }
    }
    public interface ObjectShape_ec37db37c8f1
    {
        string inner { get; set; }
        int endPos { get; set; }
    }
}
