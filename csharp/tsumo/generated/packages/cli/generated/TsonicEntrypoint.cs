namespace Tsumo.Cli
{
    public static class TsonicEntrypoint
    {
        public static void Main()
        {
            CliMain.__tsonic_module_init();
            Tsonic.CSharp.Js.JsEventLoop.Run();
        }
    }
}
