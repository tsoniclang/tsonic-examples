namespace Tsumo.Engine
{
    public class DeferredTemplateValue : TemplateValue
    {
        public string? key;
        public TemplateValue data;
        public DeferredTemplateValue(string? key, TemplateValue data) : base()
        {
            this.key = key;
            this.data = data;
        }
    }
}
