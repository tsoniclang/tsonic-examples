namespace Tsumo.Engine
{
    internal static class TsonicModuleInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Usage", "CA2255")]
        internal static void Initialize()
        {
            Index.__tsonic_module_init();
        }
    }
}
