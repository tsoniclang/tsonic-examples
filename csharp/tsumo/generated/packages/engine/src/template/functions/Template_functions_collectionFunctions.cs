using System;

namespace Tsumo.Engine
{
    public static class Template_functions_collectionFunctions
    {
        internal static bool? unionElementsEqual(TemplateValue left, TemplateValue right)
        {
            if ((object?)left is NilValue || (object?)right is NilValue)
            {
                return (object?)left is NilValue && (object?)right is NilValue;
            }
            if ((object?)left is StringValue || (object?)right is StringValue)
            {
                if (!((object?)left is StringValue) || !((object?)right is StringValue))
                {
                    return false;
                }
                return ((StringValue)left).value == ((StringValue)right).value;
            }
            if ((object?)left is NumberValue || (object?)right is NumberValue)
            {
                if (!((object?)left is NumberValue) || !((object?)right is NumberValue))
                {
                    return false;
                }
                return ((NumberValue)left).value == ((NumberValue)right).value;
            }
            if ((object?)left is BoolValue || (object?)right is BoolValue)
            {
                if (!((object?)left is BoolValue) || !((object?)right is BoolValue))
                {
                    return false;
                }
                return ((BoolValue)left).value == ((BoolValue)right).value;
            }
            if ((object?)left is PageValue || (object?)right is PageValue)
            {
                if (!((object?)left is PageValue) || !((object?)right is PageValue))
                {
                    return false;
                }
                return object.ReferenceEquals(((PageValue)left).value, ((PageValue)right).value);
            }
            return null;
        }
        internal static bool requireElementEquality(TemplateValue left, TemplateValue right, string diagnosticCode, string diagnosticMessage)
        {
            bool? equals = unionElementsEqual(left, right);
            if (equals is null)
            {
                throw Diagnostics.createTsumoError(diagnosticCode, diagnosticMessage);
            }
            return equals.Value;
        }
        internal static void appendUniqueUnionValue(Tsonic.CSharp.Js.JSArray<TemplateValue> result, TemplateValue candidate)
        {
            for (double index = 0; index < result.length; index++)
            {
                if (requireElementEquality(result[index], candidate, "TSUMO_TEMPLATE_UNION_ELEMENT_UNSUPPORTED", "collections.Union cannot compare values with the supplied element type"))
                {
                    return;
                }
            }
            result.push(candidate);
        }
        internal static Tsonic.CSharp.Js.JSArray<TemplateValue>? unionValues(TemplateValue value)
        {
            if ((object?)value is NilValue)
            {
                return Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
            }
            if ((object?)value is AnyArrayValue)
            {
                return ((AnyArrayValue)value).value;
            }
            if ((object?)value is StringArrayValue)
            {
                Tsonic.CSharp.Js.JSArray<TemplateValue> result = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                for (double index = 0; index < ((StringArrayValue)value).value.length; index++)
                {
                    result.push(new StringValue(((StringArrayValue)value).value[index]));
                }
                return result;
            }
            if ((object?)value is PageArrayValue)
            {
                Tsonic.CSharp.Js.JSArray<TemplateValue> result_1 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                for (double index_1 = 0; index_1 < ((PageArrayValue)value).value.length; index_1++)
                {
                    result_1.push(new PageValue(((PageArrayValue)value).value[index_1]));
                }
                return result_1;
            }
            return null;
        }
        internal static bool complementContains(Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Js.JSArray<TemplateValue>> collections, TemplateValue candidate)
        {
            for (double collectionIndex = 0; collectionIndex < collections.length; collectionIndex++)
            {
                Tsonic.CSharp.Js.JSArray<TemplateValue> collection = collections[collectionIndex];
                for (double valueIndex = 0; valueIndex < collection.length; valueIndex++)
                {
                    if (requireElementEquality(collection[valueIndex], candidate, "TSUMO_TEMPLATE_COMPLEMENT_ELEMENT_UNSUPPORTED", "collections.Complement cannot compare values with the supplied element type"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static Func<string, Tsonic.CSharp.Js.JSArray<TemplateValue>, TemplateFunctionContext, TemplateValue?> callCollectionFunction
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<TemplateValue>, TemplateFunctionContext, TemplateValue?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Template_evaluation_pageSemantics.__tsonic_module_init();
            Template_evaluation_propertySemantics.__tsonic_module_init();
            Template_runtimeHelpers.__tsonic_module_init();
            Template_functions_sequenceSemantics.__tsonic_module_init();
            callCollectionFunction = (string name, Tsonic.CSharp.Js.JSArray<TemplateValue> args, TemplateFunctionContext context) =>
            {
                RenderScope scope = context.scope;
                if (name == "seq")
                {
                    return Template_functions_sequenceSemantics.createIntegerSequence(args);
                }
                if (name == "complement" && args.length >= 2)
                {
                    Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Js.JSArray<TemplateValue>> exclusions = Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Js.JSArray<TemplateValue>>.of([]);
                    for (double index = 0; index < args.length - 1; index++)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue>? values = unionValues(args[index]);
                        if (values is null)
                        {
                            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_COMPLEMENT_COLLECTION_UNSUPPORTED", "collections.Complement requires slice arguments");
                        }
                        exclusions.push(values);
                    }
                    TemplateValue source = args[args.length - 1];
                    Tsonic.CSharp.Js.JSArray<TemplateValue>? sourceValues = unionValues(source);
                    if (sourceValues is null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_COMPLEMENT_COLLECTION_UNSUPPORTED", "collections.Complement requires slice arguments");
                    }
                    if ((object?)source is PageArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<PageContext> pages = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                        for (double index_1 = 0; index_1 < ((PageArrayValue)source).value.length; index_1++)
                        {
                            PageContext page = ((PageArrayValue)source).value[index_1];
                            if (!complementContains(exclusions, new PageValue(page)))
                            {
                                pages.push(page);
                            }
                        }
                        return new PageArrayValue(pages);
                    }
                    if ((object?)source is StringArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<string> strings = Tsonic.CSharp.Js.JSArray<string>.of([]);
                        for (double index_2 = 0; index_2 < ((StringArrayValue)source).value.length; index_2++)
                        {
                            string value = ((StringArrayValue)source).value[index_2];
                            if (!complementContains(exclusions, new StringValue(value)))
                            {
                                strings.push(value);
                            }
                        }
                        return new StringArrayValue(strings);
                    }
                    Tsonic.CSharp.Js.JSArray<TemplateValue> values_1 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                    for (double index_3 = 0; index_3 < sourceValues.length; index_3++)
                    {
                        TemplateValue value_1 = sourceValues[index_3];
                        if (!complementContains(exclusions, value_1))
                        {
                            values_1.push(value_1);
                        }
                    }
                    return new AnyArrayValue(values_1);
                }
                if (name == "where" && (args.length == 3 || args.length == 4))
                {
                    TemplateValue collection = args[0];
                    string path = Template_runtimeHelpers.toPlainString(args[1]);
                    string opRaw = args.length == 3 ? "eq" : Tsonic.CSharp.Js.String.toLowerCase(Template_runtimeHelpers.toPlainString(args[2]));
                    TemplateValue expected = args[args.length - 1];
                    Tsonic.CSharp.Js.JSArray<string> empty = Tsonic.CSharp.Js.JSArray<string>.of([]);
                    Tsonic.CSharp.Js.JSArray<string> segs = Tsonic.CSharp.Js.String.trim(path) == "" ? empty : Tsonic.CSharp.Js.String.split(path, ".");
                    if ((object?)collection is PageArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<PageContext> @out = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                        for (double i = 0; i < ((PageArrayValue)collection).value.length; i++)
                        {
                            PageContext page_1 = ((PageArrayValue)collection).value[i];
                            TemplateValue actual = segs.length == 0 ? new PageValue(page_1) : Template_evaluation_propertySemantics.resolvePath(new PageValue(page_1), segs, scope);
                            if (Template_evaluation_pageSemantics.matchWhere(actual, opRaw, expected))
                            {
                                @out.push(page_1);
                            }
                        }
                        return new PageArrayValue(@out);
                    }
                    if ((object?)collection is AnyArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> out_1 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        for (double i_1 = 0; i_1 < ((AnyArrayValue)collection).value.length; i_1++)
                        {
                            TemplateValue item = ((AnyArrayValue)collection).value[i_1];
                            TemplateValue actual_1 = segs.length == 0 ? item : Template_evaluation_propertySemantics.resolvePath(item, segs, scope);
                            if (Template_evaluation_pageSemantics.matchWhere(actual_1, opRaw, expected))
                            {
                                out_1.push(item);
                            }
                        }
                        return new AnyArrayValue(out_1);
                    }
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_WHERE_COLLECTION_UNSUPPORTED", "collections.Where requires a page collection or slice");
                }
                if (name == "sort" && args.length >= 1)
                {
                    TemplateValue collection_1 = args[0];
                    string sortKey = args.length >= 2 ? Template_runtimeHelpers.toPlainString(args[1]) : "";
                    string sortOrder = args.length >= 3 ? Tsonic.CSharp.Js.String.toLowerCase(Template_runtimeHelpers.toPlainString(args[2])) : "asc";
                    bool isDesc = sortOrder == "desc";
                    Tsonic.CSharp.Js.JSArray<string> empty_1 = Tsonic.CSharp.Js.JSArray<string>.of([]);
                    Tsonic.CSharp.Js.JSArray<string> keySegs = Tsonic.CSharp.Js.String.trim(sortKey) == "" ? empty_1 : Tsonic.CSharp.Js.String.split(sortKey, ".");
                    if ((object?)collection_1 is PageArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<PageContext> arr = Template_evaluation_pageSemantics.copyPageArray(((PageArrayValue)collection_1).value);
                        for (double i_2 = 0; i_2 < arr.length; i_2++)
                        {
                            for (double j = i_2 + 1; j < arr.length; j++)
                            {
                                TemplateValue aVal = keySegs.length == 0 ? new PageValue(arr[i_2]) : Template_evaluation_propertySemantics.resolvePath(new PageValue(arr[i_2]), keySegs, scope);
                                TemplateValue bVal = keySegs.length == 0 ? new PageValue(arr[j]) : Template_evaluation_propertySemantics.resolvePath(new PageValue(arr[j]), keySegs, scope);
                                int cmp = Template_evaluation_pageSemantics.compareValues(aVal, bVal);
                                bool shouldSwap = isDesc ? cmp < 0 : cmp > 0;
                                if (shouldSwap)
                                {
                                    PageContext tmp = arr[i_2];
                                    arr[i_2] = arr[j];
                                    arr[j] = tmp;
                                }
                            }
                        }
                        return new PageArrayValue(arr);
                    }
                    if ((object?)collection_1 is AnyArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> items = ((AnyArrayValue)collection_1).value;
                        Tsonic.CSharp.Js.JSArray<TemplateValue> arr_1 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        for (double i_3 = 0; i_3 < items.length; i_3++)
                        {
                            arr_1.push(items[i_3]);
                        }
                        for (double i_4 = 0; i_4 < arr_1.length; i_4++)
                        {
                            for (double j_1 = i_4 + 1; j_1 < arr_1.length; j_1++)
                            {
                                TemplateValue aVal_1 = keySegs.length == 0 ? arr_1[i_4] : Template_evaluation_propertySemantics.resolvePath(arr_1[i_4], keySegs, scope);
                                TemplateValue bVal_1 = keySegs.length == 0 ? arr_1[j_1] : Template_evaluation_propertySemantics.resolvePath(arr_1[j_1], keySegs, scope);
                                int cmp_1 = Template_evaluation_pageSemantics.compareValues(aVal_1, bVal_1);
                                bool shouldSwap_1 = isDesc ? cmp_1 < 0 : cmp_1 > 0;
                                if (shouldSwap_1)
                                {
                                    TemplateValue tmp_1 = arr_1[i_4];
                                    arr_1[i_4] = arr_1[j_1];
                                    arr_1[j_1] = tmp_1;
                                }
                            }
                        }
                        return new AnyArrayValue(arr_1);
                    }
                    return collection_1;
                }
                if (name == "after" && args.length >= 2)
                {
                    int n = Template_runtimeHelpers.toNumber(args[0]);
                    TemplateValue collection_2 = args[1];
                    if ((object?)collection_2 is PageArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<PageContext> pages_1 = Template_evaluation_pageSemantics.copyPageArray(((PageArrayValue)collection_2).value);
                        Tsonic.CSharp.Js.JSArray<PageContext> result = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                        for (double i_5 = n; i_5 < pages_1.length; i_5++)
                        {
                            result.push(pages_1[i_5]);
                        }
                        return new PageArrayValue(result);
                    }
                    if ((object?)collection_2 is AnyArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> items_1 = ((AnyArrayValue)collection_2).value;
                        Tsonic.CSharp.Js.JSArray<TemplateValue> result_1 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        for (double i_6 = n; i_6 < items_1.length; i_6++)
                        {
                            result_1.push(items_1[i_6]);
                        }
                        return new AnyArrayValue(result_1);
                    }
                    return Template_runtimeHelpers.nil;
                }
                if (name == "first" && args.length >= 2)
                {
                    int count = Template_runtimeHelpers.toNumber(args[0]);
                    TemplateValue collection_3 = args[1];
                    if (count < 0)
                    {
                        return Template_runtimeHelpers.nil;
                    }
                    if ((object?)collection_3 is PageArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<PageContext> result_2 = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                        int length = ((PageArrayValue)collection_3).value.length;
                        int limit = count < length ? count : length;
                        for (double i_7 = 0; i_7 < limit; i_7++)
                        {
                            result_2.push(((PageArrayValue)collection_3).value[i_7]);
                        }
                        return new PageArrayValue(result_2);
                    }
                    if ((object?)collection_3 is AnyArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> result_3 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        int length_1 = ((AnyArrayValue)collection_3).value.length;
                        int limit_1 = count < length_1 ? count : length_1;
                        for (double i_8 = 0; i_8 < limit_1; i_8++)
                        {
                            result_3.push(((AnyArrayValue)collection_3).value[i_8]);
                        }
                        return new AnyArrayValue(result_3);
                    }
                    if ((object?)collection_3 is StringArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<string> result_4 = Tsonic.CSharp.Js.JSArray<string>.of([]);
                        int length_2 = ((StringArrayValue)collection_3).value.length;
                        int limit_2 = count < length_2 ? count : length_2;
                        for (double i_9 = 0; i_9 < limit_2; i_9++)
                        {
                            result_4.push(((StringArrayValue)collection_3).value[i_9]);
                        }
                        return new StringArrayValue(result_4);
                    }
                    return Template_runtimeHelpers.nil;
                }
                if (name == "last" && args.length >= 2)
                {
                    int n_1 = Template_runtimeHelpers.toNumber(args[0]);
                    TemplateValue collection_4 = args[1];
                    if ((object?)collection_4 is PageArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<PageContext> pages_2 = Template_evaluation_pageSemantics.copyPageArray(((PageArrayValue)collection_4).value);
                        int start = pages_2.length > n_1 ? pages_2.length - n_1 : 0;
                        Tsonic.CSharp.Js.JSArray<PageContext> result_5 = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                        for (double i_10 = start; i_10 < pages_2.length; i_10++)
                        {
                            result_5.push(pages_2[i_10]);
                        }
                        return new PageArrayValue(result_5);
                    }
                    if ((object?)collection_4 is AnyArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> items_2 = ((AnyArrayValue)collection_4).value;
                        int start_1 = items_2.length > n_1 ? items_2.length - n_1 : 0;
                        Tsonic.CSharp.Js.JSArray<TemplateValue> result_6 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        for (double i_11 = start_1; i_11 < items_2.length; i_11++)
                        {
                            result_6.push(items_2[i_11]);
                        }
                        return new AnyArrayValue(result_6);
                    }
                    return Template_runtimeHelpers.nil;
                }
                if (name == "uniq" && args.length >= 1)
                {
                    TemplateValue collection_5 = args[0];
                    if ((object?)collection_5 is PageArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<PageContext> pages_3 = Template_evaluation_pageSemantics.copyPageArray(((PageArrayValue)collection_5).value);
                        Tsonic.CSharp.Js.Map<string, bool> seen = new Tsonic.CSharp.Js.Map<string, bool>();
                        Tsonic.CSharp.Js.JSArray<PageContext> uniqResult = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                        for (double i_12 = 0; i_12 < pages_3.length; i_12++)
                        {
                            PageContext p = pages_3[i_12];
                            string key = p.relPermalink;
                            if (!seen.has(key))
                            {
                                seen.set(key, true);
                                uniqResult.push(p);
                            }
                        }
                        return new PageArrayValue(uniqResult);
                    }
                    if ((object?)collection_5 is AnyArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> items_3 = ((AnyArrayValue)collection_5).value;
                        Tsonic.CSharp.Js.Map<string, bool> seen_1 = new Tsonic.CSharp.Js.Map<string, bool>();
                        Tsonic.CSharp.Js.JSArray<TemplateValue> uniqResult_1 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        for (double i_13 = 0; i_13 < items_3.length; i_13++)
                        {
                            string key_1 = Template_runtimeHelpers.toPlainString(items_3[i_13]);
                            if (!seen_1.has(key_1))
                            {
                                seen_1.set(key_1, true);
                                uniqResult_1.push(items_3[i_13]);
                            }
                        }
                        return new AnyArrayValue(uniqResult_1);
                    }
                    return collection_5;
                }
                if (name == "union" && args.length >= 2)
                {
                    TemplateValue first = args[0];
                    TemplateValue second = args[1];
                    if ((object?)first is PageArrayValue && (object?)second is PageArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<PageContext> result_7 = Template_evaluation_pageSemantics.copyPageArray(((PageArrayValue)first).value);
                        for (double index_4 = 0; index_4 < ((PageArrayValue)second).value.length; index_4++)
                        {
                            PageContext candidate = ((PageArrayValue)second).value[index_4];
                            bool present = false;
                            for (double resultIndex = 0; resultIndex < result_7.length; resultIndex++)
                            {
                                if (object.ReferenceEquals(result_7[resultIndex], candidate))
                                {
                                    present = true;
                                    break;
                                }
                            }
                            if (!present)
                            {
                                result_7.push(candidate);
                            }
                        }
                        return new PageArrayValue(result_7);
                    }
                    if ((object?)first is StringArrayValue && (object?)second is StringArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<string> result_8 = Tsonic.CSharp.Js.JSArray<string>.of([]);
                        for (double index_5 = 0; index_5 < ((StringArrayValue)first).value.length; index_5++)
                        {
                            if (!Tsonic.CSharp.Js.Array.includes(result_8, ((StringArrayValue)first).value[index_5]))
                            {
                                result_8.push(((StringArrayValue)first).value[index_5]);
                            }
                        }
                        for (double index_6 = 0; index_6 < ((StringArrayValue)second).value.length; index_6++)
                        {
                            if (!Tsonic.CSharp.Js.Array.includes(result_8, ((StringArrayValue)second).value[index_6]))
                            {
                                result_8.push(((StringArrayValue)second).value[index_6]);
                            }
                        }
                        return new StringArrayValue(result_8);
                    }
                    if ((object?)first is NilValue && (object?)second is PageArrayValue)
                    {
                        return new PageArrayValue(Template_evaluation_pageSemantics.copyPageArray(((PageArrayValue)second).value));
                    }
                    if ((object?)second is NilValue && (object?)first is PageArrayValue)
                    {
                        return new PageArrayValue(Template_evaluation_pageSemantics.copyPageArray(((PageArrayValue)first).value));
                    }
                    if ((object?)first is NilValue && (object?)second is StringArrayValue)
                    {
                        return new StringArrayValue(Tsonic.CSharp.Js.Array.slice(((StringArrayValue)second).value));
                    }
                    if ((object?)second is NilValue && (object?)first is StringArrayValue)
                    {
                        return new StringArrayValue(Tsonic.CSharp.Js.Array.slice(((StringArrayValue)first).value));
                    }
                    Tsonic.CSharp.Js.JSArray<TemplateValue>? firstValues = unionValues(first);
                    Tsonic.CSharp.Js.JSArray<TemplateValue>? secondValues = unionValues(second);
                    if (firstValues is not null && secondValues is not null)
                    {
                        Tsonic.CSharp.Js.JSArray<TemplateValue> result_9 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        for (double index_7 = 0; index_7 < firstValues.length; index_7++)
                        {
                            appendUniqueUnionValue(result_9, firstValues[index_7]);
                        }
                        for (double index_8 = 0; index_8 < secondValues.length; index_8++)
                        {
                            appendUniqueUnionValue(result_9, secondValues[index_8]);
                        }
                        return new AnyArrayValue(result_9);
                    }
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNION_COLLECTIONS_INVALID", "collections.Union requires two slices or nil values");
                }
                if (name == "group" && args.length >= 2)
                {
                    string key_2 = Template_runtimeHelpers.toPlainString(args[0]);
                    TemplateValue collection_6 = args[1];
                    Tsonic.CSharp.Js.JSArray<string> empty_2 = Tsonic.CSharp.Js.JSArray<string>.of([]);
                    Tsonic.CSharp.Js.JSArray<string> keySegs_1 = Tsonic.CSharp.Js.String.trim(key_2) == "" ? empty_2 : Tsonic.CSharp.Js.String.split(key_2, ".");
                    if ((object?)collection_6 is PageArrayValue)
                    {
                        Tsonic.CSharp.Js.JSArray<PageContext> pages_4 = Template_evaluation_pageSemantics.copyPageArray(((PageArrayValue)collection_6).value);
                        Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> groups = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>();
                        Tsonic.CSharp.Js.JSArray<string> groupOrder = Tsonic.CSharp.Js.JSArray<string>.of([]);
                        for (double i_14 = 0; i_14 < pages_4.length; i_14++)
                        {
                            PageContext page_2 = pages_4[i_14];
                            TemplateValue val = Template_evaluation_propertySemantics.resolvePath(new PageValue(page_2), keySegs_1, scope);
                            string groupKey = Template_runtimeHelpers.toPlainString(val);
                            Tsonic.CSharp.Js.JSArray<PageContext>? group = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<PageContext>>(groups, groupKey);
                            if (group is null)
                            {
                                group = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                                groups.set(groupKey, group);
                                groupOrder.push(groupKey);
                            }
                            group.push(page_2);
                        }
                        Tsonic.CSharp.Js.JSArray<TemplateValue> groupResult = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                        Tsonic.CSharp.Js.JSArray<string> keys = groupOrder;
                        for (double i_15 = 0; i_15 < keys.length; i_15++)
                        {
                            Tsonic.CSharp.Js.JSArray<PageContext>? group_1 = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<PageContext>>(groups, keys[i_15]);
                            if (group_1 is null)
                            {
                                continue;
                            }
                            Tsonic.CSharp.Js.Map<string, TemplateValue> groupDict = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
                            groupDict.set("Key", new StringValue(keys[i_15]));
                            groupDict.set("Pages", new PageArrayValue(group_1));
                            groupResult.push(new DictValue(groupDict));
                        }
                        return new AnyArrayValue(groupResult);
                    }
                    return Template_runtimeHelpers.nil;
                }
                if (name == "plainify" && args.length >= 1)
                {
                    TemplateValue v = args[0];
                    string s = Template_runtimeHelpers.toPlainString(v);
                    TextBuilder sb = new TextBuilder();
                    bool inTag = false;
                    for (int i_16 = 0; i_16 < s.Length; i_16 = Utils_strings.nextCodePointIndex(s, i_16))
                    {
                        string ch = Utils_strings.codePointAtText(s, i_16);
                        if (ch == "<")
                        {
                            inTag = true;
                            continue;
                        }
                        if (ch == ">")
                        {
                            inTag = false;
                            continue;
                        }
                        if (!inTag)
                        {
                            sb.append(ch);
                        }
                    }
                    return new StringValue(sb.toString());
                }
                if (name == "cond" && args.length >= 3)
                {
                    return Template_runtimeHelpers.isTruthy(args[0]) ? args[1] : args[2];
                }
                if (name == "dict")
                {
                    Tsonic.CSharp.Js.Map<string, TemplateValue> map = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
                    for (double i_17 = 0; i_17 + 1 < args.length; i_17 += 2)
                    {
                        string k = Template_runtimeHelpers.toPlainString(args[i_17]);
                        map.set(k, args[i_17 + 1]);
                    }
                    return new DictValue(map);
                }
                if (name == "slice")
                {
                    Tsonic.CSharp.Js.JSArray<TemplateValue> items_4 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                    for (double i_18 = 0; i_18 < args.length; i_18++)
                    {
                        items_4.push(args[i_18]);
                    }
                    return new AnyArrayValue(items_4);
                }
                if (name == "reverse" && args.length >= 1)
                {
                    return Template_functions_sequenceSemantics.reverseTemplateCollection(args[0]);
                }
                if (name == "append" && args.length >= 2)
                {
                    TemplateValue listValue = args[args.length - 1];
                    Tsonic.CSharp.Js.JSArray<TemplateValue> items_5 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                    if ((object?)listValue is AnyArrayValue)
                    {
                        for (double i_19 = 0; i_19 < ((AnyArrayValue)listValue).value.length; i_19++)
                        {
                            items_5.push(((AnyArrayValue)listValue).value[i_19]);
                        }
                    }
                    else
                    {
                        items_5.push(listValue);
                    }
                    for (double i_20 = 0; i_20 < args.length - 1; i_20++)
                    {
                        TemplateValue v_1 = args[i_20];
                        if ((object?)v_1 is AnyArrayValue)
                        {
                            for (double j_2 = 0; j_2 < ((AnyArrayValue)v_1).value.length; j_2++)
                            {
                                items_5.push(((AnyArrayValue)v_1).value[j_2]);
                            }
                        }
                        else
                        {
                            items_5.push(v_1);
                        }
                    }
                    return new AnyArrayValue(items_5);
                }
                if (name == "merge" && args.length >= 2)
                {
                    TemplateValue a = args[0];
                    TemplateValue b = args[1];
                    Tsonic.CSharp.Js.Map<string, TemplateValue> merged = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
                    if ((object?)a is DictValue)
                    {
                        foreach (string k_1 in ((DictValue)a).value.keys())
                        {
                            TemplateValue? v_2 = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(((DictValue)a).value, k_1);
                            if (v_2 is null)
                            {
                                continue;
                            }
                            merged.set(k_1, v_2);
                        }
                    }
                    if ((object?)b is DictValue)
                    {
                        foreach (string k_2 in ((DictValue)b).value.keys())
                        {
                            TemplateValue? v_3 = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(((DictValue)b).value, k_2);
                            if (v_3 is null)
                            {
                                continue;
                            }
                            merged.set(k_2, v_3);
                        }
                    }
                    return new DictValue(merged);
                }
                if (name == "isset" && args.length >= 2)
                {
                    TemplateValue container = args[0];
                    string key_3 = Template_runtimeHelpers.toPlainString(args[1]);
                    if ((object?)container is DictValue)
                    {
                        return new BoolValue(((DictValue)container).value.has(key_3));
                    }
                    return new BoolValue(false);
                }
                if (name == "index" && args.length >= 2)
                {
                    TemplateValue container_1 = args[0];
                    TemplateValue keyValue = args[1];
                    if ((object?)container_1 is DictValue)
                    {
                        string key_4 = Template_runtimeHelpers.toPlainString(keyValue);
                        TemplateValue? v_4 = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(((DictValue)container_1).value, key_4);
                        return v_4 is not null ? v_4 : Template_runtimeHelpers.nil;
                    }
                    if ((object?)container_1 is StringArrayValue && (object?)keyValue is NumberValue)
                    {
                        int index_9 = ((NumberValue)keyValue).value;
                        if (index_9 < 0 || index_9 >= ((StringArrayValue)container_1).value.length)
                        {
                            return Template_runtimeHelpers.nil;
                        }
                        return new StringValue(((StringArrayValue)container_1).value[index_9]);
                    }
                    if ((object?)container_1 is AnyArrayValue)
                    {
                        if ((object?)keyValue is NumberValue)
                        {
                            int idx = ((NumberValue)keyValue).value;
                            if (idx < 0 || idx >= ((AnyArrayValue)container_1).value.length)
                            {
                                return Template_runtimeHelpers.nil;
                            }
                            return ((AnyArrayValue)container_1).value[idx];
                        }
                    }
                    if ((object?)container_1 is PageArrayValue)
                    {
                        if ((object?)keyValue is NumberValue)
                        {
                            int idx_1 = ((NumberValue)keyValue).value;
                            return idx_1 >= 0 && idx_1 < ((PageArrayValue)container_1).value.length ? new PageValue(((PageArrayValue)container_1).value[idx_1]) : Template_runtimeHelpers.nil;
                        }
                    }
                    return Template_runtimeHelpers.nil;
                }
                if (name == "delimit" && args.length >= 2)
                {
                    TemplateValue listValue_1 = args[0];
                    string delim = Template_runtimeHelpers.toPlainString(args[1]);
                    Tsonic.CSharp.Js.JSArray<string> parts = Tsonic.CSharp.Js.JSArray<string>.of([]);
                    if ((object?)listValue_1 is AnyArrayValue)
                    {
                        for (double i_21 = 0; i_21 < ((AnyArrayValue)listValue_1).value.length; i_21++)
                        {
                            parts.push(Template_runtimeHelpers.toPlainString(((AnyArrayValue)listValue_1).value[i_21]));
                        }
                    }
                    else
                    {
                        if ((object?)listValue_1 is StringArrayValue)
                        {
                            for (double i_22 = 0; i_22 < ((StringArrayValue)listValue_1).value.length; i_22++)
                            {
                                parts.push(((StringArrayValue)listValue_1).value[i_22]);
                            }
                        }
                    }
                    Tsonic.CSharp.Js.JSArray<string> arr_2 = parts;
                    string out_2 = "";
                    for (double i_23 = 0; i_23 < arr_2.length; i_23++)
                    {
                        if (i_23 > 0)
                        {
                            out_2 += delim;
                        }
                        out_2 += arr_2[i_23];
                    }
                    return new StringValue(out_2);
                }
                if (name == "in" && args.length >= 2)
                {
                    TemplateValue container_2 = args[0];
                    string needle = Template_runtimeHelpers.toPlainString(args[1]);
                    if ((object?)container_2 is AnyArrayValue)
                    {
                        for (double i_24 = 0; i_24 < ((AnyArrayValue)container_2).value.length; i_24++)
                        {
                            if (Template_runtimeHelpers.toPlainString(((AnyArrayValue)container_2).value[i_24]) == needle)
                            {
                                return new BoolValue(true);
                            }
                        }
                        return new BoolValue(false);
                    }
                    if ((object?)container_2 is StringValue)
                    {
                        return new BoolValue(Tsonic.CSharp.Js.String.includes(((StringValue)container_2).value, needle));
                    }
                    return new BoolValue(false);
                }
                if (name == "split" && args.length >= 2)
                {
                    string s_1 = Template_runtimeHelpers.toPlainString(args[0]);
                    string delim_1 = Template_runtimeHelpers.toPlainString(args[1]);
                    Tsonic.CSharp.Js.JSArray<TemplateValue> items_6 = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
                    if (delim_1 == "")
                    {
                        for (int i_25 = 0; i_25 < s_1.Length; i_25++)
                        {
                            items_6.push(new StringValue(Utils_strings.substringCount(s_1, i_25, 1)));
                        }
                        return new AnyArrayValue(items_6);
                    }
                    int start_2 = 0;
                    while (true)
                    {
                        int idx_2 = Tsonic.CSharp.Js.String.indexOf(s_1, delim_1, start_2);
                        if (idx_2 < 0)
                        {
                            break;
                        }
                        items_6.push(new StringValue(Utils_strings.substringCount(s_1, start_2, idx_2 - start_2)));
                        start_2 = idx_2 + delim_1.Length;
                    }
                    items_6.push(new StringValue(Utils_strings.substringFrom(s_1, start_2)));
                    return new AnyArrayValue(items_6);
                }
                return null;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
