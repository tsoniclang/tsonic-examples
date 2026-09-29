namespace Tsumo.Engine
{
    public class ObjectShape_2cf98120a8ba : ObjectShape_47e63e0b71fa<int, string>
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
    public interface ObjectShape_47e63e0b71fa<Property0, Property1>
    {
        Property0 endPos { get; set; }
        Property1 inner { get; set; }
    }
    public class ObjectShape_4ae8488f53c8 : ObjectShape_9c2d50422db1<bool, Tsonic.CSharp.Js.Map<string, ParamValue>, Tsonic.CSharp.Js.JSArray<string>>
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
    public interface ObjectShape_9c2d50422db1<Property0, Property1, Property2>
    {
        Property0 isNamed { get; set; }
        Property1 __tsonic_member_a20b52fae57cc7a99c9651f1b573950fd211823e3ace3bb9c273c06430f24cd3 { get; set; }
        Property2 positional { get; set; }
    }
}
