using System;

namespace Tsumo.Engine
{
    public static class Template_values_scratch
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Template_values_base.__tsonic_module_init();
            Template_values_dict.__tsonic_module_init();
            Template_values_arrays.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ScratchStore
    {
        public Tsonic.CSharp.Js.Map<string, TemplateValue> values;
        public ScratchStore()
        {
            this.values = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
        }
        public DictValue getValues()
        {
            return new DictValue(this.values);
        }
        public TemplateValue get(string key)
        {
            TemplateValue? v = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(this.values, key);
            return v is not null ? v : new NilValue();
        }
        public void set(string key, TemplateValue value)
        {
            this.values.set(key, value);
        }
        public void add(string key, TemplateValue value)
        {
            TemplateValue? cur = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(this.values, key);
            if (cur is null)
            {
                this.set(key, value);
                return;
            }
            if (cur is AnyArrayValue)
            {
                AnyArrayValue curArray = (AnyArrayValue)(AnyArrayValue)cur;
                Tsonic.CSharp.Js.JSArray<TemplateValue> mergedList = new Tsonic.CSharp.Js.JSArray<TemplateValue>(new TemplateValue[] { });
                for (int i = 0; i < curArray.value.length; i++)
                {
                    mergedList.push(curArray.value[i]);
                }
                if (value is AnyArrayValue)
                {
                    AnyArrayValue valueArray = (AnyArrayValue)value;
                    for (int i_1 = 0; i_1 < valueArray.value.length; i_1++)
                    {
                        mergedList.push(valueArray.value[i_1]);
                    }
                }
                else
                {
                    mergedList.push(value);
                }
                this.set(key, new AnyArrayValue(mergedList));
                return;
            }
            Tsonic.CSharp.Js.JSArray<TemplateValue> pairList = new Tsonic.CSharp.Js.JSArray<TemplateValue>(new TemplateValue[] { });
            pairList.push(cur);
            pairList.push(value);
            this.set(key, new AnyArrayValue(pairList));
        }
        public void delete(string key)
        {
            this.values.delete(key);
        }
        public void setInMap(string mapName, string key, TemplateValue value)
        {
            TemplateValue? cur = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(this.values, mapName);
            if (cur is not null)
            {
                if (cur is DictValue)
                {
                    DictValue dict = (DictValue)(DictValue)cur;
                    dict.value.set(key, value);
                    return;
                }
            }
            Tsonic.CSharp.Js.Map<string, TemplateValue> map = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
            map.set(key, value);
            this.values.set(mapName, new DictValue(map));
        }
        public void deleteInMap(string mapName, string key)
        {
            TemplateValue? cur = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(this.values, mapName);
            if (cur is not null)
            {
                if (cur is DictValue)
                {
                    DictValue dict = (DictValue)(DictValue)cur;
                    dict.value.delete(key);
                }
            }
        }
    }
    public class ScratchValue : TemplateValue
    {
        public ScratchStore value;
        public ScratchValue(ScratchStore value) : base()
        {
            this.value = value;
        }
    }
}
