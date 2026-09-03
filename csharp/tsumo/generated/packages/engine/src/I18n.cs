using System;

namespace Tsumo.Engine
{
    public static class I18n
    {
        public static Tsonic.CSharp.Js.JSArray<string> pluralVariantNames
        {
            get;
            private set;
        } = default(Tsonic.CSharp.Js.JSArray<string>)!;
        public static Func<string, bool> isPluralVariantName
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<TemplateValue, string, string, string> i18nText
        {
            get;
            private set;
        } = default(Func<TemplateValue, string, string, string>)!;
        public static Func<TemplateValue, string, string, I18nMessage?> messageFromValue
        {
            get;
            private set;
        } = default(Func<TemplateValue, string, string, I18nMessage?>)!;
        public static Action<Tsonic.CSharp.Js.Map<string, I18nMessage>, string, I18nMessage, string> setLayerMessage
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.Map<string, I18nMessage>, string, I18nMessage, string>)!;
        public static Action<TemplateValue, string, Tsonic.CSharp.Js.Map<string, I18nMessage>, string> collectMessageTree
        {
            get;
            private set;
        } = default(Action<TemplateValue, string, Tsonic.CSharp.Js.Map<string, I18nMessage>, string>)!;
        public static Action<AnyArrayValue, Tsonic.CSharp.Js.Map<string, I18nMessage>, string> collectLegacyMessages
        {
            get;
            private set;
        } = default(Action<AnyArrayValue, Tsonic.CSharp.Js.Map<string, I18nMessage>, string>)!;
        public static Action<string, string, string, Tsonic.CSharp.Js.Map<string, I18nMessage>> collectI18nFile
        {
            get;
            private set;
        } = default(Action<string, string, string, Tsonic.CSharp.Js.Map<string, I18nMessage>>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            Template_evaluation_structuredData.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            pluralVariantNames = new Tsonic.CSharp.Js.JSArray<string>(new string[] { "zero", "one", "two", "few", "many", "other" });
            isPluralVariantName = (string name) =>
            {
                string normalized = Tsonic.CSharp.Js.String.toLowerCase(name);
                for (int index = 0; index < pluralVariantNames.length; index++)
                {
                    if (pluralVariantNames[index] == normalized)
                    {
                        return true;
                    }
                }
                return false;
            };
            i18nText = (TemplateValue value, string identity, string sourcePath) =>
            {
                if (value is StringValue)
                {
                    return ((StringValue)value).value;
                }
                throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_VALUE_INVALID", $"Internationalization message '{identity}' must contain text values", sourcePath);
            };
            messageFromValue = (TemplateValue value, string identity, string sourcePath) =>
            {
                if (value is StringValue)
                {
                    Tsonic.CSharp.Js.Map<string, string> variants = new Tsonic.CSharp.Js.Map<string, string>();
                    variants.set("other", ((StringValue)value).value);
                    return new I18nMessage(variants);
                }
                if (!(value is DictValue))
                {
                    return null;
                }
                Tsonic.CSharp.Js.Map<string, TemplateValue> fields = ((DictValue)value).value;
                TemplateValue? translation = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(fields, "translation");
                if (translation is not null)
                {
                    return messageFromValue(translation, identity, sourcePath);
                }
                Tsonic.CSharp.Js.Map<string, string> variants_1 = new Tsonic.CSharp.Js.Map<string, string>();
                foreach (string key in fields.keys())
                {
                    if (!isPluralVariantName(key))
                    {
                        continue;
                    }
                    TemplateValue? field = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(fields, key);
                    if (field is null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_INCONSISTENT", $"Internationalization variant '{identity}.{key}' disappeared", sourcePath);
                    }
                    variants_1.set(Tsonic.CSharp.Js.String.toLowerCase(key), i18nText(field, $"{identity}.{key}", sourcePath));
                }
                return variants_1.size == 0 ? null : new I18nMessage(variants_1);
            };
            setLayerMessage = (Tsonic.CSharp.Js.Map<string, I18nMessage> layer, string identity, I18nMessage message, string sourcePath) =>
            {
                if (identity == "")
                {
                    throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_IDENTITY_INVALID", "Internationalization message identity cannot be empty", sourcePath);
                }
                if (layer.has(identity))
                {
                    throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_CONFLICT", $"Internationalization message '{identity}' is declared more than once in the same layer", sourcePath);
                }
                layer.set(identity, message);
            };
            collectMessageTree = (TemplateValue value, string identity, Tsonic.CSharp.Js.Map<string, I18nMessage> layer, string sourcePath) =>
            {
                I18nMessage? message = messageFromValue(value, identity, sourcePath);
                if (message is not null)
                {
                    setLayerMessage(layer, identity, message, sourcePath);
                    return;
                }
                if (!(value is DictValue))
                {
                    throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_SHAPE_INVALID", $"Internationalization value '{identity}' must be text or a message dictionary", sourcePath);
                }
                foreach (string key in ((DictValue)value).value.keys())
                {
                    TemplateValue? child = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(((DictValue)value).value, key);
                    if (child is null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_INCONSISTENT", $"Internationalization value '{key}' disappeared", sourcePath);
                    }
                    collectMessageTree(child, identity == "" ? key : $"{identity}.{key}", layer, sourcePath);
                }
            };
            collectLegacyMessages = (AnyArrayValue values, Tsonic.CSharp.Js.Map<string, I18nMessage> layer, string sourcePath) =>
            {
                for (int index = 0; index < values.value.length; index++)
                {
                    TemplateValue item = values.value[index];
                    if (!(item is DictValue))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_SHAPE_INVALID", "Internationalization message list entries must be dictionaries", sourcePath);
                    }
                    TemplateValue? identityValue = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(((DictValue)item).value, "id");
                    TemplateValue? translation = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(((DictValue)item).value, "translation");
                    if (!(identityValue is StringValue) || translation is null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_SHAPE_INVALID", "Internationalization message list entries require text 'id' and 'translation' fields", sourcePath);
                    }
                    I18nMessage? message = messageFromValue(translation, ((StringValue)identityValue).value, sourcePath);
                    if (message is null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_SHAPE_INVALID", $"Internationalization message '{((StringValue)identityValue).value}' has an invalid translation", sourcePath);
                    }
                    setLayerMessage(layer, ((StringValue)identityValue).value, message, sourcePath);
                }
            };
            collectI18nFile = (string content, string format, string sourcePath, Tsonic.CSharp.Js.Map<string, I18nMessage> layer) =>
            {
                TemplateValue value = Template_evaluation_structuredData.parseTemplateDataText(content, format, sourcePath);
                if (value is AnyArrayValue)
                {
                    AnyArrayValue legacyMessages = (AnyArrayValue)value;
                    collectLegacyMessages(legacyMessages, layer, sourcePath);
                }
                else
                {
                    collectMessageTree(value, "", layer, sourcePath);
                }
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class I18nMessage
    {
        public Tsonic.CSharp.Js.Map<string, string> variants;
        public I18nMessage(Tsonic.CSharp.Js.Map<string, string> variants)
        {
            this.variants = variants;
        }
        public string select(int? count)
        {
            if (count is not null)
            {
                string exactName = count.Value == 0 ? "zero" : count.Value == 1 ? "one" : count.Value == 2 ? "two" : "other";
                string? exact = Tsonic.CSharp.Js.Map.getReference<string, string>(this.variants, exactName);
                if (exact is not null)
                {
                    return exact;
                }
            }
            string? other = Tsonic.CSharp.Js.Map.getReference<string, string>(this.variants, "other");
            if (other is not null)
            {
                return other;
            }
            for (int index = 0; index < I18n.pluralVariantNames.length; index++)
            {
                string? value = Tsonic.CSharp.Js.Map.getReference<string, string>(this.variants, I18n.pluralVariantNames[index]);
                if (value is not null)
                {
                    return value;
                }
            }
            throw Diagnostics.createTsumoError("TSUMO_I18N_MESSAGE_EMPTY", "An internationalization message has no text variants");
        }
    }
    public class I18nStore
    {
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.Map<string, I18nMessage>> translations;
        public I18nStore()
        {
            this.translations = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.Map<string, I18nMessage>>();
        }
        public void loadFromDir(string dir)
        {
            Tsonic.CSharp.Js.JSArray<string> files = Fs.listFilesTopDirectory(dir, "*");
            files.sort();
            Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.Map<string, I18nMessage>> layer = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.Map<string, I18nMessage>>();
            for (int index = 0; index < files.length; index++)
            {
                string file = files[index];
                string extension = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Node.path.extname(file));
                string format = "";
                if (extension == ".yaml" || extension == ".yml")
                {
                    format = "yaml";
                }
                else
                {
                    if (extension == ".toml")
                    {
                        format = "toml";
                    }
                    else
                    {
                        if (extension == ".json")
                        {
                            format = "json";
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                string fullFileName = Tsonic.CSharp.Node.path.basename(file);
                string fileName = Tsonic.CSharp.Js.String.slice(fullFileName, 0, fullFileName.Length - extension.Length);
                if (fileName == "")
                {
                    continue;
                }
                string language = Tsonic.CSharp.Js.String.toLowerCase(fileName);
                Tsonic.CSharp.Js.Map<string, I18nMessage>? languageLayer = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.Map<string, I18nMessage>>(layer, language);
                if (languageLayer is null)
                {
                    languageLayer = new Tsonic.CSharp.Js.Map<string, I18nMessage>();
                    layer.set(language, languageLayer);
                }
                I18n.collectI18nFile(Fs.readTextFile(file), format, file, languageLayer);
            }
            foreach (string language_1 in layer.keys())
            {
                Tsonic.CSharp.Js.Map<string, I18nMessage>? selected = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.Map<string, I18nMessage>>(this.translations, language_1);
                if (selected is null)
                {
                    selected = new Tsonic.CSharp.Js.Map<string, I18nMessage>();
                    this.translations.set(language_1, selected);
                }
                Tsonic.CSharp.Js.Map<string, I18nMessage>? messages = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.Map<string, I18nMessage>>(layer, language_1);
                if (messages is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_I18N_LAYER_INCONSISTENT", $"Internationalization layer '{language_1}' disappeared", dir);
                }
                foreach (string identity in messages.keys())
                {
                    I18nMessage? message = Tsonic.CSharp.Js.Map.getReference<string, I18nMessage>(messages, identity);
                    if (message is null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_I18N_LAYER_INCONSISTENT", $"Internationalization message '{identity}' disappeared", dir);
                    }
                    selected.set(identity, message);
                }
            }
        }
        public string translate(string language, string key, int? count = null)
        {
            string normalized = Tsonic.CSharp.Js.String.toLowerCase(language);
            Tsonic.CSharp.Js.Map<string, I18nMessage>? messages = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.Map<string, I18nMessage>>(this.translations, normalized);
            int separator = Tsonic.CSharp.Js.String.indexOf(normalized, "-");
            if (messages is null && separator > 0)
            {
                messages = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.Map<string, I18nMessage>>(this.translations, Tsonic.CSharp.Js.String.slice(normalized, 0, separator));
            }
            if (messages is null)
            {
                messages = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.Map<string, I18nMessage>>(this.translations, "en");
            }
            if (messages is null)
            {
                return key;
            }
            I18nMessage? message = Tsonic.CSharp.Js.Map.getReference<string, I18nMessage>(messages, key);
            return message is null ? key : message.select(count);
        }
    }
}
