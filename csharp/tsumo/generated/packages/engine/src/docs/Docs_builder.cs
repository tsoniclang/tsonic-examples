using System;

namespace Tsumo.Engine
{
    public static class Docs_builder
    {
        public static Func<BuildRequest, LoadedDocsConfig, string, int> buildDocsSite
        {
            get;
            private set;
        } = default(Func<BuildRequest, LoadedDocsConfig, string, int>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Build_layout.__tsonic_module_init();
            Utils_urlPath.__tsonic_module_init();
            Build_outputPlan.__tsonic_module_init();
            Config.__tsonic_module_init();
            Diagnostics.__tsonic_module_init();
            Env.__tsonic_module_init();
            Models.__tsonic_module_init();
            Params.__tsonic_module_init();
            Utils_html.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Utils_text.__tsonic_module_init();
            Docs_config.__tsonic_module_init();
            Docs_content.__tsonic_module_init();
            Docs_directoryGraph.__tsonic_module_init();
            Docs_editUrl.__tsonic_module_init();
            Docs_markdown.__tsonic_module_init();
            Docs_models.__tsonic_module_init();
            Docs_nav.__tsonic_module_init();
            Docs_output.__tsonic_module_init();
            Docs_routes.__tsonic_module_init();
            Docs_searchIndex.__tsonic_module_init();
            buildDocsSite = (BuildRequest request, LoadedDocsConfig docsLoaded, string outDir) =>
            {
                string siteDir = Tsonic.CSharp.Node.path.resolve(request.siteDir);
                LoadedConfig loaded = Config_loader.loadSiteConfig(siteDir);
                SiteConfig config = loaded.config;
                string? requestBaseURL = request.baseURL;
                if (requestBaseURL is not null && Tsonic.CSharp.Js.String.trim(requestBaseURL) != "")
                {
                    config.baseURL = Utils_text.ensureTrailingSlash(Tsonic.CSharp.Js.String.trim(requestBaseURL));
                }
                DocsSiteConfig docsConfig = docsLoaded.config;
                if (Tsonic.CSharp.Js.String.trim(docsConfig.siteName) != "")
                {
                    config.title = Tsonic.CSharp.Js.String.trim(docsConfig.siteName);
                }
                string? themeDir = Build_layout.resolveThemeDir(siteDir, config, request.themesDir);
                BuildEnvironment env = new BuildEnvironment(siteDir, themeDir, outDir, new Tsonic.CSharp.Js.JSArray<ModuleMount>(new ModuleMount[] { }), request.buildTime);
                SiteOutputPlan outputPlan = new SiteOutputPlan();
                if (themeDir is not null)
                {
                    outputPlan.addDirectory(Tsonic.CSharp.Node.path.join(themeDir, "static"), "", "theme static files", "theme-static");
                }
                outputPlan.addDirectory(Tsonic.CSharp.Node.path.join(siteDir, "static"), "", "site static files", "site-static");
                Tsonic.CSharp.Js.JSArray<PageContext> emptyPages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                Tsonic.CSharp.Js.JSArray<PageContext> emptyTranslations = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                Tsonic.CSharp.Js.JSArray<string> emptyStrings = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                SiteContext site = new SiteContext(config, emptyPages, null, null);
                site.Sites = new Tsonic.CSharp.Js.JSArray<SiteContext>(new SiteContext[] { site });
                string? baseTpl = Build_layout.selectTemplate(env, new Tsonic.CSharp.Js.JSArray<string>(new string[] { "_default/baseof.html" }));
                string homeTpl = Build_layout.selectTemplate(env, new Tsonic.CSharp.Js.JSArray<string>(new string[] { "index.html", "docs/home.html", "docs/list.html", "_default/list.html" })) ?? "_default/list.html";
                string listTpl = Build_layout.selectTemplate(env, new Tsonic.CSharp.Js.JSArray<string>(new string[] { "docs/list.html", "_default/list.html" })) ?? "_default/list.html";
                string singleTpl = Build_layout.selectTemplate(env, new Tsonic.CSharp.Js.JSArray<string>(new string[] { "docs/single.html", "_default/single.html" })) ?? "_default/single.html";
                Tsonic.CSharp.Js.JSArray<PageContext> mountRootPages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                Tsonic.CSharp.Js.JSArray<PageContext> allPagesForOutput = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                Tsonic.CSharp.Js.JSArray<DocsMountContext> mountContexts = new Tsonic.CSharp.Js.JSArray<DocsMountContext>(new DocsMountContext[] { });
                Tsonic.CSharp.Js.JSArray<SearchDocument> searchDocs = new Tsonic.CSharp.Js.JSArray<SearchDocument>(new SearchDocument[] { });
                DocsOutputClaims outputClaims = new DocsOutputClaims();
                bool rootMountOwnsHome = false;
                Tsonic.CSharp.Js.JSArray<DocsMountConfig> mounts = docsConfig.mounts;
                for (int mountIndex = 0; mountIndex < mounts.length; mountIndex++)
                {
                    DocsMountConfig mount = mounts[mountIndex];
                    DocsMountRoutes discovered = Docs_routes.discoverDocsMountRoutes(mount);
                    DocsContentInventory content = Docs_content.loadDocsContent(discovered.markdown, request.buildDrafts);
                    for (int index = 0; index < discovered.assets.length; index++)
                    {
                        DocsAssetRoute asset = discovered.assets[index];
                        outputClaims.add(asset.outputRelPath, asset.sourcePath);
                        outputPlan.addAsset(asset.outputRelPath, asset.sourcePath, $"docs asset '{asset.sourcePath}'", "docs-asset");
                    }
                    foreach (DocsContentRoute indexed in content.indexByDirectory.values())
                    {
                        outputClaims.add(indexed.route.outputRelPath, indexed.route.sourcePath);
                    }
                    for (int index_1 = 0; index_1 < content.leaves.length; index_1++)
                    {
                        DocsContentRoute leaf = content.leaves[index_1];
                        outputClaims.add(leaf.route.outputRelPath, leaf.route.sourcePath);
                    }
                    Tsonic.CSharp.Js.Map<string, string> routeMap = content.permalinkByRelativePath;
                    mountContexts.push(new DocsMountContext(mount.name, mount.urlPrefix, Docs_nav.loadMountNav(mount, routeMap)));
                    Tsonic.CSharp.Js.JSArray<string> prefixSegs = Docs_routes.docsMountPrefixSegments(mount.urlPrefix);
                    if (prefixSegs.length == 0)
                    {
                        rootMountOwnsHome = true;
                    }
                    string mountSection = prefixSegs.length > 0 ? prefixSegs[0] : mount.name;
                    Tsonic.CSharp.Js.Map<string, DocsContentRoute> indexByDir = content.indexByDirectory;
                    Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> leafPagesByDir = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>();
                    Tsonic.CSharp.Js.JSArray<DocsContentRoute> leafArr = content.leaves;
                    for (int i = 0; i < leafArr.length; i++)
                    {
                        DocsContentRoute source = leafArr[i];
                        DocsMarkdownRoute r = source.route;
                        ParsedContent parsed = source.parsed;
                        FrontMatter fm = parsed.frontMatter;
                        MarkdownResult md = Docs_markdown.renderDocsMarkdown(parsed.body, new DocsLinkRewriteContext(mount, r.sourcePath, r.dirKey, routeMap, docsConfig.strictLinks));
                        HtmlString content_1 = new HtmlString(md.html);
                        HtmlString summary = new HtmlString(md.summaryHtml);
                        string plainText = md.plainText;
                        string baseName = Docs_routes.withoutMarkdownExtension(r.fileName);
                        string title = fm.title ?? Utils_text.humanizeSlug(baseName);
                        Tsonic.CSharp.Js.Date dateUtc = fm.date ?? source.modifiedAt;
                        string dateString = dateUtc.toISOString();
                        string lastmodString = source.modifiedAt.toISOString();
                        PageFile file = new PageFile(Tsonic.CSharp.Node.path.resolve(r.sourcePath), r.dirKey == "" ? "" : r.dirKey + "/", baseName);
                        Tsonic.CSharp.Js.Map<string, ParamValue> @params = fm.Params;
                        @params.set("mount", ParamValue.@string(mount.name));
                        @params.set("mountPrefix", ParamValue.@string(mount.urlPrefix));
                        @params.set("relPath", ParamValue.@string(r.relPath));
                        string? editUrl = Docs_editUrl.createDocsEditUrl(mount, r.relPath);
                        if (editUrl is not null)
                        {
                            @params.set("editURL", ParamValue.@string(editUrl));
                        }
                        PageContext ctx = new PageContext(title, dateString, lastmodString, fm.draft, "page", mountSection, fm.type ?? "docs", baseName, r.relPermalink, plainText, new HtmlString(""), content_1, summary, fm.description ?? "", fm.tags, fm.categories, @params, file, site.Language, emptyTranslations, null, site, emptyPages, null, emptyPages, fm.layout);
                        Tsonic.CSharp.Js.JSArray<PageContext>? list = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<PageContext>>(leafPagesByDir, r.dirKey);
                        if (list is null)
                        {
                            list = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                            leafPagesByDir.set(r.dirKey, list);
                        }
                        list.push(ctx);
                        allPagesForOutput.push(ctx);
                        searchDocs.push(new SearchDocument(title, r.relPermalink, mount.name, plainText));
                    }
                    Tsonic.CSharp.Js.Map<string, bool> dirSet = new Tsonic.CSharp.Js.Map<string, bool>();
                    Docs_directoryGraph.addDocsDirectoryWithParents("", dirSet);
                    foreach (string indexKey in indexByDir.keys())
                    {
                        Docs_directoryGraph.addDocsDirectoryWithParents(indexKey, dirSet);
                    }
                    foreach (string leafKey in leafPagesByDir.keys())
                    {
                        Docs_directoryGraph.addDocsDirectoryWithParents(leafKey, dirSet);
                    }
                    Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<string>> childDirsByDir = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<string>>();
                    foreach (string childDirKey in dirSet.keys())
                    {
                        if (childDirKey == "")
                        {
                            continue;
                        }
                        string parentKey = Docs_directoryGraph.docsParentDirectory(childDirKey);
                        Tsonic.CSharp.Js.JSArray<string>? list_1 = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<string>>(childDirsByDir, parentKey);
                        if (list_1 is null)
                        {
                            list_1 = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                            childDirsByDir.set(parentKey, list_1);
                        }
                        list_1.push(childDirKey);
                    }
                    Tsonic.CSharp.Js.JSArray<string> dirKeys = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                    foreach (string collectedDirKey in dirSet.keys())
                    {
                        dirKeys.push(collectedDirKey);
                    }
                    dirKeys.sort((string a, string b) =>
                    {
                        int depth = Docs_directoryGraph.docsDirectoryDepth(b) - Docs_directoryGraph.docsDirectoryDepth(a);
                        return depth != 0 ? depth : Utils_strings.compareText(a, b);
                    });
                    Tsonic.CSharp.Js.Map<string, PageContext> sectionByDir = new Tsonic.CSharp.Js.Map<string, PageContext>();
                    for (int i_1 = 0; i_1 < dirKeys.length; i_1++)
                    {
                        string dirKey = dirKeys[i_1];
                        Tsonic.CSharp.Js.JSArray<PageContext> childPages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                        Tsonic.CSharp.Js.JSArray<string>? childDirList = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<string>>(childDirsByDir, dirKey);
                        if (childDirList is not null)
                        {
                            childDirList.sort((string a, string b) => Utils_strings.compareText(a, b));
                            Tsonic.CSharp.Js.JSArray<string> childDirKeys = childDirList;
                            for (int j = 0; j < childDirKeys.length; j++)
                            {
                                string childKey = childDirKeys[j];
                                PageContext? childSection = Tsonic.CSharp.Js.Map.getReference<string, PageContext>(sectionByDir, childKey);
                                if (childSection is not null)
                                {
                                    childPages.push(childSection);
                                }
                            }
                        }
                        Tsonic.CSharp.Js.JSArray<PageContext>? leafList = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<PageContext>>(leafPagesByDir, dirKey);
                        if (leafList is not null)
                        {
                            leafList.sort((PageContext a, PageContext b) => Utils_strings.compareText(a.title, b.title));
                            Tsonic.CSharp.Js.JSArray<PageContext> leafPages = leafList;
                            for (int j_1 = 0; j_1 < leafPages.length; j_1++)
                            {
                                childPages.push(leafPages[j_1]);
                            }
                        }
                        Tsonic.CSharp.Js.JSArray<string> routeSegments = dirKey == "" ? emptyStrings : Tsonic.CSharp.Js.String.split(dirKey, "/");
                        Tsonic.CSharp.Js.JSArray<string> urlParts = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                        urlParts.push(mount.urlPrefix);
                        for (int j_2 = 0; j_2 < routeSegments.length; j_2++)
                        {
                            urlParts.push(routeSegments[j_2]);
                        }
                        string relPermalink = Utils_urlPath.combineUrlPath(urlParts);
                        DocsContentRoute? idxRoute = Tsonic.CSharp.Js.Map.getReference<string, DocsContentRoute>(indexByDir, dirKey);
                        if (idxRoute is null)
                        {
                            outputClaims.add(Docs_output.docsOutputPathForPermalink(relPermalink), $"<generated docs section {mount.name}:{dirKey}>");
                        }
                        string dirSlug = dirKey == "" ? mountSection : Docs_directoryGraph.docsDirectoryName(dirKey);
                        string title_1 = dirKey == "" ? mount.name : Utils_text.humanizeSlug(dirSlug);
                        HtmlString content_2 = new HtmlString("");
                        HtmlString summary_1 = new HtmlString("");
                        string plain = "";
                        string description = "";
                        Tsonic.CSharp.Js.Map<string, ParamValue> params_1 = new Tsonic.CSharp.Js.Map<string, ParamValue>();
                        bool draft = false;
                        string dateString_1 = "";
                        string lastmodString_1 = "";
                        PageFile? file_1 = null;
                        string? layout = null;
                        if (idxRoute is not null)
                        {
                            ParsedContent parsed_1 = idxRoute.parsed;
                            DocsMarkdownRoute route = idxRoute.route;
                            FrontMatter fm_1 = parsed_1.frontMatter;
                            draft = fm_1.draft;
                            layout = fm_1.layout;
                            if (draft && !request.buildDrafts)
                            {
                            }
                            else
                            {
                                MarkdownResult md_1 = Docs_markdown.renderDocsMarkdown(parsed_1.body, new DocsLinkRewriteContext(mount, route.sourcePath, dirKey, routeMap, docsConfig.strictLinks));
                                content_2 = new HtmlString(md_1.html);
                                summary_1 = new HtmlString(md_1.summaryHtml);
                                description = fm_1.description ?? "";
                                title_1 = fm_1.title ?? title_1;
                                string plainText_1 = md_1.plainText;
                                plain = plainText_1;
                                searchDocs.push(new SearchDocument(title_1, relPermalink, mount.name, plainText_1));
                                Tsonic.CSharp.Js.Date dateUtc_1 = fm_1.date ?? idxRoute.modifiedAt;
                                dateString_1 = dateUtc_1.toISOString();
                                lastmodString_1 = idxRoute.modifiedAt.toISOString();
                                file_1 = new PageFile(Tsonic.CSharp.Node.path.resolve(route.sourcePath), dirKey == "" ? "" : dirKey + "/", "_index");
                                params_1 = fm_1.Params;
                                params_1.set("relPath", ParamValue.@string(route.relPath));
                                string? editUrl_1 = Docs_editUrl.createDocsEditUrl(mount, route.relPath);
                                if (editUrl_1 is not null)
                                {
                                    params_1.set("editURL", ParamValue.@string(editUrl_1));
                                }
                            }
                        }
                        params_1.set("mount", ParamValue.@string(mount.name));
                        params_1.set("mountPrefix", ParamValue.@string(mount.urlPrefix));
                        params_1.set("dirKey", ParamValue.@string(dirKey));
                        string slug = dirSlug;
                        PageContext sectionCtx = new PageContext(title_1, dateString_1, lastmodString_1, draft, "section", mountSection, "docs", slug, relPermalink, plain, new HtmlString(""), content_2, summary_1, description, emptyStrings, emptyStrings, params_1, file_1, site.Language, emptyTranslations, null, site, childPages, null, emptyPages, layout);
                        sectionByDir.set(dirKey, sectionCtx);
                        allPagesForOutput.push(sectionCtx);
                    }
                    PageContext? mountRoot = Tsonic.CSharp.Js.Map.getReference<string, PageContext>(sectionByDir, "");
                    if (mountRoot is not null)
                    {
                        mountRootPages.push(mountRoot);
                    }
                }
                Tsonic.CSharp.Js.JSArray<PageContext> mountRoots = mountRootPages;
                site.pages = mountRoots;
                site.docsMounts = mountContexts;
                string? homeMount = docsConfig.homeMount;
                string? chosenHome = homeMount is not null && Tsonic.CSharp.Js.String.trim(homeMount) != "" ? Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(homeMount)) : null;
                if (!rootMountOwnsHome)
                {
                    outputClaims.add("index.html", "<generated docs home>");
                }
                HtmlString homeContent = new HtmlString("");
                HtmlString homeSummary = new HtmlString("");
                string homeDescription = "";
                string homeTitle = config.title;
                bool homeMountMatched = chosenHome is null;
                if (chosenHome is not null)
                {
                    for (int i_2 = 0; i_2 < mountRoots.length; i_2++)
                    {
                        PageContext m = mountRoots[i_2];
                        ParamValue mountNameParam = Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(m.Params, "mount") ?? ParamValue.@string("");
                        ParamValue mountPrefixParam = Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(m.Params, "mountPrefix") ?? ParamValue.@string("");
                        string mountName = mountNameParam.stringValue;
                        string mountPrefix = mountPrefixParam.stringValue;
                        if (Tsonic.CSharp.Js.String.toLowerCase(mountName) == chosenHome || Tsonic.CSharp.Js.String.toLowerCase(mountPrefix) == chosenHome)
                        {
                            homeTitle = m.title;
                            homeContent = m.content;
                            homeSummary = m.summary;
                            homeDescription = m.description;
                            homeMountMatched = true;
                            break;
                        }
                    }
                }
                if (!homeMountMatched)
                {
                    throw Diagnostics.createTsumoError("TSUMO_DOCS_HOME_MOUNT_NOT_FOUND", $"Configured homeMount does not match a docs mount: {homeMount ?? ""}", docsLoaded.path);
                }
                PageContext homeCtx = new PageContext(homeTitle, "", "", false, "home", "", "docs", "", "/", "", new HtmlString(""), homeContent, homeSummary, homeDescription, emptyStrings, emptyStrings, new Tsonic.CSharp.Js.Map<string, ParamValue>(), null, site.Language, emptyTranslations, null, site, mountRoots, null, emptyPages, null);
                Docs_directoryGraph.assignDocsPageAncestry(homeCtx, null, emptyPages);
                site.home = homeCtx;
                Tsonic.CSharp.Js.JSArray<PageContext> allSitePages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { homeCtx });
                for (int index_2 = 0; index_2 < allPagesForOutput.length; index_2++)
                {
                    allSitePages.push(allPagesForOutput[index_2]);
                }
                site.allPages = allSitePages;
                string homeHtml = Build_layout.renderWithBase(env, baseTpl, homeTpl, homeCtx);
                outputPlan.addText("index.html", homeHtml, "docs home page");
                Tsonic.CSharp.Js.JSArray<PageContext> allPages = allPagesForOutput;
                for (int i_3 = 0; i_3 < allPages.length; i_3++)
                {
                    PageContext page = allPages[i_3];
                    if (page.relPermalink == "/")
                    {
                        continue;
                    }
                    string tpl = page.kind == "page" ? singleTpl : listTpl;
                    string html = Build_layout.renderWithBase(env, baseTpl, tpl, page);
                    string outputRelPath = Docs_output.docsOutputPathForPermalink(page.relPermalink);
                    outputPlan.addText(outputRelPath, html, $"docs page '{page.relPermalink}'");
                }
                if (docsConfig.generateSearchIndex)
                {
                    string name = Tsonic.CSharp.Js.String.trim(docsConfig.searchIndexFileName);
                    if (name != "")
                    {
                        outputClaims.add(name, "<generated docs search index>");
                        string json = Docs_searchIndex.renderSearchIndexJson(searchDocs);
                        outputPlan.addText(name, json, "docs search index");
                    }
                }
                outputPlan.applyDeferredTemplateResults(env.finalizeDeferredTemplates());
                outputPlan.render(outDir);
                return outputPlan.generatedOutputCount();
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
