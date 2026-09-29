using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_pageSemantics
    {
        public static string toTitleCase(string text)
        {
            string trimmed = Tsonic.CSharp.Js.String.trim(text);
            if (trimmed == "")
            {
                return "";
            }
            Tsonic.CSharp.Js.JSArray<string> parts = Tsonic.CSharp.Js.String.split(trimmed, " ");
            TextBuilder sb = new TextBuilder();
            for (int i = 0; i < parts.length; i++)
            {
                string word = parts[i];
                if (Tsonic.CSharp.Js.String.trim(word) == "")
                {
                    continue;
                }
                if (sb.length > 0)
                {
                    sb.append(" ");
                }
                string first = Tsonic.CSharp.Js.String.toUpperCase(Utils_strings.substringCount(word, 0, 1));
                string rest = word.Length > 1 ? Tsonic.CSharp.Js.String.toLowerCase(Utils_strings.substringFrom(word, 1)) : "";
                sb.append(first);
                sb.append(rest);
            }
            return sb.toString();
        }
        public static Tsonic.CSharp.Js.JSArray<PageContext> toPages(TemplateValue value)
        {
            if ((object?)value is PageArrayValue)
            {
                return ((PageArrayValue)value).value;
            }
            if ((object?)value is AnyArrayValue)
            {
                Tsonic.CSharp.Js.JSArray<PageContext> @out = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                for (int i = 0; i < ((AnyArrayValue)value).value.length; i++)
                {
                    TemplateValue cur = ((AnyArrayValue)value).value[i];
                    if ((object?)cur is PageValue)
                    {
                        @out.push(((PageValue)cur).value);
                    }
                }
                return @out;
            }
            Tsonic.CSharp.Js.JSArray<PageContext> empty = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
            return empty;
        }
        public static PageArrayValue getPageTerms(PageContext page, string taxonomyRaw)
        {
            string taxonomy = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(taxonomyRaw));
            Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>? memberships = Tsonic.CSharp.Js.Map.getOptional<string, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>>(page.site.Taxonomies, taxonomy);
            Tsonic.CSharp.Js.Map<string, PageContext>? termPages = Tsonic.CSharp.Js.Map.getOptional<string, Tsonic.CSharp.Js.Map<string, PageContext>>(page.site.taxonomyTermPages, taxonomy);
            Tsonic.CSharp.Js.JSArray<PageContext> selected = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
            if (memberships is null || termPages is null)
            {
                return new PageArrayValue(selected);
            }
            foreach (string termSlug in termPages.keys())
            {
                Tsonic.CSharp.Js.JSArray<PageContext>? pages = Tsonic.CSharp.Js.Map.getOptional<string, Tsonic.CSharp.Js.JSArray<PageContext>>(memberships, termSlug);
                if (pages is null)
                {
                    continue;
                }
                bool includesPage = false;
                for (int index = 0; index < pages.length; index++)
                {
                    if (object.ReferenceEquals(pages[index], page))
                    {
                        includesPage = true;
                        break;
                    }
                }
                if (!includesPage)
                {
                    continue;
                }
                PageContext? termPage = Tsonic.CSharp.Js.Map.getOptional<string, PageContext>(termPages, termSlug);
                if (termPage is not null)
                {
                    selected.push(termPage);
                }
            }
            return new PageArrayValue(selected);
        }
        public static bool pageHasShortcode(PageContext page, string name)
        {
            return page.shortcodeNames.has(name);
        }
        public static string siteLastModification(SiteContext site)
        {
            string selected = "";
            double selectedTime = 0;
            bool hasSelected = false;
            Tsonic.CSharp.Js.JSArray<PageContext> pages = site.allPages.length > 0 ? site.allPages : site.pages;
            for (int index = 0; index < pages.length; index++)
            {
                string lastmod = pages[index].lastmod;
                double time = Tsonic.CSharp.Js.Date.parse(lastmod);
                if (Tsonic.CSharp.Js.Number.isNaN(time) || (hasSelected && time <= selectedTime))
                {
                    continue;
                }
                selected = lastmod;
                selectedTime = time;
                hasSelected = true;
            }
            return selected;
        }
        public static Tsonic.CSharp.Js.JSArray<PageContext> sortPagesByDate(Tsonic.CSharp.Js.JSArray<PageContext> pages, string field)
        {
            Tsonic.CSharp.Js.JSArray<PageContext> copy = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
            for (int i = 0; i < pages.length; i++)
            {
                copy.push(pages[i]);
            }
            copy.sort((PageContext left, PageContext right) => Utils_strings.compareText(field == "lastmod" ? left.lastmod : left.date, field == "lastmod" ? right.lastmod : right.date));
            return copy;
        }
        public static Tsonic.CSharp.Js.JSArray<PageContext> sortPagesByTitle(Tsonic.CSharp.Js.JSArray<PageContext> pages)
        {
            Tsonic.CSharp.Js.JSArray<PageContext> copy = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
            for (int i = 0; i < pages.length; i++)
            {
                copy.push(pages[i]);
            }
            copy.sort((PageContext left, PageContext right) => Utils_strings.compareText(left.title, right.title));
            return copy;
        }
        public static Func<PageContext, int> pageWeight
        {
            get;
            private set;
        } = default(Func<PageContext, int>)!;
        public static Tsonic.CSharp.Js.JSArray<PageContext> sortPagesByWeight(Tsonic.CSharp.Js.JSArray<PageContext> pages)
        {
            Tsonic.CSharp.Js.JSArray<PageContext> sorted = copyPageArray(pages);
            sorted.sort((PageContext left, PageContext right) =>
            {
                int leftWeight = pageWeight(left);
                int rightWeight = pageWeight(right);
                return leftWeight < rightWeight ? -1 : leftWeight > rightWeight ? 1 : 0;
            });
            return sorted;
        }
        public static Tsonic.CSharp.Js.JSArray<PageContext> reversePages(Tsonic.CSharp.Js.JSArray<PageContext> pages)
        {
            int len = pages.length;
            Tsonic.CSharp.Js.JSArray<PageContext> reversed = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
            for (double i = len; i > 0; )
            {
                i--;
                reversed.push(pages[i]);
            }
            return reversed;
        }
        public static Tsonic.CSharp.Js.JSArray<PageContext> copyPageArray(Tsonic.CSharp.Js.JSArray<PageContext> pages)
        {
            Tsonic.CSharp.Js.JSArray<PageContext> copy = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
            for (int i = 0; i < pages.length; i++)
            {
                copy.push(pages[i]);
            }
            return copy;
        }
        public static TemplateValue? resolvePageCollectionProperty(PageArrayValue collection, string propertyRaw)
        {
            string property = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(propertyRaw));
            if (property == "bylastmod")
            {
                return new PageArrayValue(sortPagesByDate(collection.value, "lastmod"));
            }
            if (property == "bydate")
            {
                return new PageArrayValue(sortPagesByDate(collection.value, "date"));
            }
            if (property == "bypublishdate")
            {
                return new PageArrayValue(sortPagesByDate(collection.value, "publishdate"));
            }
            if (property == "bytitle" || property == "bylinktitle")
            {
                return new PageArrayValue(sortPagesByTitle(collection.value));
            }
            if (property == "byweight")
            {
                return new PageArrayValue(sortPagesByWeight(collection.value));
            }
            if (property == "reverse")
            {
                return new PageArrayValue(reversePages(collection.value));
            }
            if (property == "len")
            {
                return new NumberValue(collection.value.length);
            }
            return null;
        }
        public static Tsonic.CSharp.Js.JSArray<PageContext> pagesWithKind(Tsonic.CSharp.Js.JSArray<PageContext> pages, string kind)
        {
            Tsonic.CSharp.Js.JSArray<PageContext> selected = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
            for (int index = 0; index < pages.length; index++)
            {
                PageContext page = pages[index];
                if (page.kind == kind)
                {
                    selected.push(page);
                }
            }
            return selected;
        }
        internal static bool sharesExactText(Tsonic.CSharp.Js.JSArray<string> left, Tsonic.CSharp.Js.JSArray<string> right)
        {
            for (int leftIndex = 0; leftIndex < left.length; leftIndex++)
            {
                for (int rightIndex = 0; rightIndex < right.length; rightIndex++)
                {
                    if (left[leftIndex] == right[rightIndex])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        internal static string? keywordValue(PageContext page)
        {
            foreach (string name in page.Params.keys())
            {
                if (Tsonic.CSharp.Js.String.toLowerCase(name) != "keywords")
                {
                    continue;
                }
                ParamValue? value = Tsonic.CSharp.Js.Map.getOptional<string, ParamValue>(page.Params, name);
                return value?.stringValue;
            }
            return null;
        }
        internal static PageArrayValue defaultRelatedPages(Tsonic.CSharp.Js.JSArray<PageContext> pages, PageContext source)
        {
            Tsonic.CSharp.Js.JSArray<RelatedPageCandidate> candidates = Tsonic.CSharp.Js.JSArray<RelatedPageCandidate>.of([]);
            double sourceDate = pageDateMilliseconds(source);
            string? sourceKeyword = keywordValue(source);
            for (int index = 0; index < pages.length; index++)
            {
                PageContext candidate = pages[index];
                if (object.ReferenceEquals(candidate, source))
                {
                    continue;
                }
                double candidateDate = pageDateMilliseconds(candidate);
                if (sourceDate > 0 && candidateDate > sourceDate)
                {
                    continue;
                }
                bool sharesTags = sharesExactText(source.tags, candidate.tags);
                string? candidateKeyword = keywordValue(candidate);
                bool sharesKeyword = sourceKeyword is not null && candidateKeyword == sourceKeyword;
                int score = sharesKeyword ? (sharesTags ? 180 : 100) : sharesTags ? 80 : 0;
                if (score >= 80)
                {
                    candidates.push(new RelatedPageCandidate(candidate, score));
                }
            }
            candidates.sort((RelatedPageCandidate left, RelatedPageCandidate right) =>
            {
                if (left.score != right.score)
                {
                    return left.score > right.score ? -1 : 1;
                }
                int dateOrder = Utils_strings.compareText(right.page.date, left.page.date);
                return dateOrder != 0 ? dateOrder : Utils_strings.compareText(left.page.relPermalink, right.page.relPermalink);
            });
            Tsonic.CSharp.Js.JSArray<PageContext> result = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
            for (int index_1 = 0; index_1 < candidates.length; index_1++)
            {
                result.push(candidates[index_1].page);
            }
            return new PageArrayValue(result);
        }
        internal static TemplateValue pageGroupingValue(PageContext page, string fieldRaw)
        {
            string field = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(fieldRaw));
            if (field == "weight")
            {
                return new NumberValue(pageWeight(page));
            }
            if (field == "title" || field == "linktitle")
            {
                return new StringValue(page.title);
            }
            if (field == "date" || field == "publishdate")
            {
                return new StringValue(page.date);
            }
            if (field == "lastmod")
            {
                return new StringValue(page.lastmod);
            }
            if (field == "draft")
            {
                return new BoolValue(page.draft);
            }
            if (field == "kind")
            {
                return new StringValue(page.kind);
            }
            if (field == "section")
            {
                return new StringValue(page.section);
            }
            if (field == "type")
            {
                return new StringValue(page.type);
            }
            if (field == "slug")
            {
                return new StringValue(page.slug);
            }
            if (field == "relpermalink")
            {
                return new StringValue(page.relPermalink);
            }
            if (Tsonic.CSharp.Js.String.startsWith(field, "params."))
            {
                ParamValue? parameter = Template_evaluation_paramSemantics.findParam(page.Params, Utils_strings.substringFrom(Tsonic.CSharp.Js.String.trim(fieldRaw), "params.".Length));
                if (parameter is not null)
                {
                    return Template_evaluation_paramSemantics.paramToTemplateValue(parameter);
                }
            }
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_PAGE_GROUP_FIELD_UNSUPPORTED", $"Pages.GroupBy cannot resolve page field '{fieldRaw}'");
        }
        internal static AnyArrayValue groupPagesByField(Tsonic.CSharp.Js.JSArray<PageContext> pages, string field, bool ascending)
        {
            Tsonic.CSharp.Js.JSArray<PageGroupBuild> groups = Tsonic.CSharp.Js.JSArray<PageGroupBuild>.of([]);
            for (int pageIndex = 0; pageIndex < pages.length; pageIndex++)
            {
                PageContext page = pages[pageIndex];
                TemplateValue key = pageGroupingValue(page, field);
                PageGroupBuild? selected = null;
                for (int groupIndex = 0; groupIndex < groups.length; groupIndex++)
                {
                    if (compareValues(groups[groupIndex].key, key) == 0)
                    {
                        selected = groups[groupIndex];
                        break;
                    }
                }
                if (selected is null)
                {
                    selected = new PageGroupBuild(key);
                    groups.push(selected);
                }
                selected.pages.push(page);
            }
            groups.sort((PageGroupBuild left, PageGroupBuild right) => ascending ? compareValues(left.key, right.key) : compareValues(right.key, left.key));
            Tsonic.CSharp.Js.JSArray<TemplateValue> result = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
            for (int index = 0; index < groups.length; index++)
            {
                PageGroupBuild group = groups[index];
                result.push(new PageGroupValue(group.key, group.pages));
            }
            return new AnyArrayValue(result);
        }
        public static TemplateValue? callPageCollectionMethod(PageArrayValue collection, string method, Tsonic.CSharp.Js.JSArray<TemplateValue> args)
        {
            if ((method == "first" || method == "limit") && args.length >= 1)
            {
                int count = (object?)args[0] is NumberValue ? ((NumberValue)args[0]).value : 0;
                Tsonic.CSharp.Js.JSArray<PageContext> result = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                for (double index = 0; index < collection.value.length && index < count; index++)
                {
                    result.push(collection.value[index]);
                }
                return new PageArrayValue(result);
            }
            if (method == "groupby" && args.length >= 1)
            {
                string order = args.length >= 2 ? Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(args[1]))) : "asc";
                if (order != "asc" && order != "desc")
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_PAGE_GROUP_ORDER_INVALID", $"Page group order must be 'asc' or 'desc', received '{order}'");
                }
                return groupPagesByField(collection.value, Template_runtimeHelpers.toPlainString(args[0]), order == "asc");
            }
            if (method == "groupbydate" && args.length >= 1)
            {
                string order_1 = args.length >= 2 ? Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(args[1]))) : "desc";
                if (order_1 != "asc" && order_1 != "desc")
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_PAGE_GROUP_ORDER_INVALID", $"Page date group order must be 'asc' or 'desc', received '{order_1}'");
                }
                return groupPagesByDate(collection.value, Template_runtimeHelpers.toPlainString(args[0]), order_1 == "asc");
            }
            if (method == "related" && args.length == 1)
            {
                TemplateValue source = args[0];
                if ((object?)source is PageValue)
                {
                    return defaultRelatedPages(collection.value, ((PageValue)source).value);
                }
                if ((object?)source is DictValue)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_RELATED_OPTIONS_UNSUPPORTED", "Page collection Related options require a configured related-content model");
                }
            }
            return null;
        }
        public static Func<PageContext, double> pageDateMilliseconds
        {
            get;
            private set;
        } = default(Func<PageContext, double>)!;
        public static AnyArrayValue groupPagesByDate(Tsonic.CSharp.Js.JSArray<PageContext> pages, string layout, bool ascending)
        {
            Tsonic.CSharp.Js.JSArray<(double, PageContext)> ordered = pages.map<(double, PageContext)>((PageContext page, int _, Tsonic.CSharp.Js.JSArray<PageContext> _) => (pageDateMilliseconds(page), page));
            ordered.sort(((double, PageContext) left, (double, PageContext) right) => ascending ? left.Item1 - right.Item1 : right.Item1 - left.Item1);
            Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> groups = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>();
            Tsonic.CSharp.Js.JSArray<string> keys = Tsonic.CSharp.Js.JSArray<string>.of([]);
            for (int index = 0; index < ordered.length; index++)
            {
                PageContext page = ordered[index].Item2;
                string? key = formatPageDate(page.date, layout);
                if (key is null)
                {
                    continue;
                }
                Tsonic.CSharp.Js.JSArray<PageContext>? group = Tsonic.CSharp.Js.Map.getOptional<string, Tsonic.CSharp.Js.JSArray<PageContext>>(groups, key);
                if (group is null)
                {
                    group = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                    groups.set(key, group);
                    keys.push(key);
                }
                group.push(page);
            }
            Tsonic.CSharp.Js.JSArray<TemplateValue> result = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
            for (int index_1 = 0; index_1 < keys.length; index_1++)
            {
                string key_1 = keys[index_1];
                Tsonic.CSharp.Js.JSArray<PageContext>? group_1 = Tsonic.CSharp.Js.Map.getOptional<string, Tsonic.CSharp.Js.JSArray<PageContext>>(groups, key_1);
                if (group_1 is null)
                {
                    continue;
                }
                result.push(new PageGroupValue(new StringValue(key_1), group_1));
            }
            return new AnyArrayValue(result);
        }
        internal static string? formatPageDate(string value, string layout)
        {
            return Template_evaluation_scalarSemantics.formatDateTime(value, layout);
        }
        public static Tsonic.CSharp.Js.JSArray<string> copyStringArray(Tsonic.CSharp.Js.JSArray<string> strings)
        {
            Tsonic.CSharp.Js.JSArray<string> copy = Tsonic.CSharp.Js.JSArray<string>.of([]);
            for (int i = 0; i < strings.length; i++)
            {
                copy.push(strings[i]);
            }
            return copy;
        }
        public static Func<TemplateValue, TemplateValue, int> compareValues
        {
            get;
            private set;
        } = default(Func<TemplateValue, TemplateValue, int>)!;
        public static bool matchWhere(TemplateValue actual, string op, TemplateValue expected)
        {
            string opLower = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(op));
            string actualText = Template_runtimeHelpers.toPlainString(actual);
            if (opLower == "eq" || opLower == "==")
            {
                return actualText == Template_runtimeHelpers.toPlainString(expected);
            }
            if (opLower == "ne" || opLower == "!=")
            {
                return actualText != Template_runtimeHelpers.toPlainString(expected);
            }
            if (opLower == "in")
            {
                if ((object?)expected is AnyArrayValue)
                {
                    for (int i = 0; i < ((AnyArrayValue)expected).value.length; i++)
                    {
                        if (Template_runtimeHelpers.toPlainString(((AnyArrayValue)expected).value[i]) == actualText)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                if ((object?)expected is StringArrayValue)
                {
                    for (int i_1 = 0; i_1 < ((StringArrayValue)expected).value.length; i_1++)
                    {
                        if (((StringArrayValue)expected).value[i_1] == actualText)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                if ((object?)expected is DictValue)
                {
                    return ((DictValue)expected).value.has(actualText);
                }
                return false;
            }
            if (opLower == "not in")
            {
                return !matchWhere(actual, "in", expected);
            }
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_WHERE_OPERATOR_UNSUPPORTED", $"collections.Where does not support operator '{op}'");
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Params.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Template_runtimeHelpers.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Template_evaluation_paramSemantics.__tsonic_module_init();
            Template_evaluation_scalarSemantics.__tsonic_module_init();
            pageWeight = (PageContext page) =>
            {
                ParamValue? value = Template_evaluation_paramSemantics.findParam(page.Params, "weight");
                if (value is null)
                {
                    return 0;
                }
                if (value.kind != ParamKind.Number)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_PAGE_WEIGHT_INVALID", "Page weight requires a 32-bit integer front-matter value");
                }
                return value.numberValue;
            };
            pageDateMilliseconds = (PageContext page) =>
            {
                double parsed = Tsonic.CSharp.Js.Date.parse(page.date);
                return Tsonic.CSharp.Js.Number.isNaN(parsed) ? 0 : parsed;
            };
            compareValues = (TemplateValue a, TemplateValue b) =>
            {
                if ((object?)a is StringValue && (object?)b is StringValue)
                {
                    string aStr = ((StringValue)a).value;
                    string bStr = ((StringValue)b).value;
                    return Utils_strings.compareText(aStr, bStr);
                }
                if ((object?)a is NumberValue && (object?)b is NumberValue)
                {
                    int aNum = ((NumberValue)a).value;
                    int bNum = ((NumberValue)b).value;
                    if (aNum < bNum)
                    {
                        return -1;
                    }
                    if (aNum > bNum)
                    {
                        return 1;
                    }
                    return 0;
                }
                string aPlain = Template_runtimeHelpers.toPlainString(a);
                string bPlain = Template_runtimeHelpers.toPlainString(b);
                return Utils_strings.compareText(aPlain, bPlain);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class RelatedPageCandidate
    {
        public PageContext page;
        public int score;
        public RelatedPageCandidate(PageContext page, int score)
        {
            this.page = page;
            this.score = score;
        }
    }
    public class PageGroupBuild
    {
        public TemplateValue key;
        public Tsonic.CSharp.Js.JSArray<PageContext> pages;
        public PageGroupBuild(TemplateValue key)
        {
            this.key = key;
            this.pages = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
        }
    }
}
