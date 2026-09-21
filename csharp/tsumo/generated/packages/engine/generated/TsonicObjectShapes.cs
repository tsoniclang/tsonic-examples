namespace Tsumo.Engine
{
    public interface ObjectShape_0b74f910f8f5
    {
        Tsonic.CSharp.Js.Map<string, ParamValue> __tsonic_member_a20b52fae57cc7a99c9651f1b573950fd211823e3ace3bb9c273c06430f24cd3 { get; set; }
        Tsonic.CSharp.Js.JSArray<string> positional { get; set; }
        bool isNamed { get; set; }
    }
    public class ObjectShape_d8df0b9373c7 : ObjectShape_0b74f910f8f5
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
