using cloudscribe.DateTimeUtils;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using cloudscribe.SimpleContent.Web;
using cloudscribe.SimpleContent.Web.Design;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.TagHelpers;
using cloudscribe.SimpleContent.Web.Templating;
using cloudscribe.Versioning;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.SiteMap;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
       
        public static IServiceCollection AddSimpleContentMvc(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.TryAddScoped<IPageRoutes, DefaultPageRoutes>();
            services.TryAddScoped<IBlogRoutes, DefaultBlogRoutes>();
            services.TryAddScoped<IBlogService, BlogService>();
            services.TryAddScoped<IAuthorNameResolver, DefaultAuthorNameResolver>();
            services.TryAddScoped<PageEvents, PageEvents>();
            services.TryAddScoped<PostEvents, PostEvents>();

            services.TryAddScoped<IPageService, PageService>();
            services.AddScoped<DraftPublishService>();
            services.TryAddScoped<IProjectService, ProjectService>();
            services.TryAddScoped<IProjectSettingsResolver, DefaultProjectSettingsResolver>();
            services.TryAddScoped<IMediaProcessor, FileSystemMediaProcessor>();

            services.TryAddScoped<IMarkdownProcessor, MarkdownProcessor>();
            services.TryAddScoped<IContentProcessor, ContentProcessor>();

            services.TryAddScoped<TeaserCache>();
            services.TryAddScoped<ITeaserService, TeaserService>();
            
            services.TryAddScoped<IProjectEmailService, ProjectEmailService>();
            services.TryAddScoped<ViewRenderer, ViewRenderer>();
            
            //services.TryAddScoped<IPageNavigationCacheKeys, PageNavigationCacheKeys>();
            services.AddScoped<INavigationTreeBuilder, PagesNavigationTreeBuilder>();
            services.AddScoped<ISiteMapNodeService, BlogSiteMapNodeService>();
            services.AddScoped<IFindCurrentNode, NavigationBlogNodeFinder>();

            services.TryAddScoped<IRoleSelectorProperties, NotImplementedRoleSelectorProperties>();

            services.AddScoped<IVersionProvider, VersionInfo>();
            services.AddScoped<IVersionProvider, DataStorageVersionInfo>();

            if (configuration != null)
            {   
                services.Configure<IconCssClasses>(configuration.GetSection("IconCssClasses"));
                services.Configure<PageEditOptions>(configuration.GetSection("PageEditOptions"));
                services.Configure<BlogEditOptions>(configuration.GetSection("BlogEditOptions"));
                services.Configure<SimpleContentIconConfig>(configuration.GetSection("SimpleContentIconConfig"));
                services.Configure<SimpleContentThemeConfig>(configuration.GetSection("SimpleContentThemeConfig"));
                services.Configure<ContentTemplateConfig>(configuration.GetSection("ContentTemplateConfig"));
                services.Configure<ContentLocalizationOptions>(configuration.GetSection("ContentLocalizationOptions"));

                
            }
            else
            {
                // not doing anything just configuring the default
                services.Configure<IconCssClasses>(c => {  });
                services.Configure<PageEditOptions>(c => { });
                services.Configure<BlogEditOptions>(c => { });
                services.Configure<SimpleContentIconConfig>(c => { });
                services.Configure<SimpleContentThemeConfig>(c => {  });
                services.Configure<ContentTemplateConfig>(c => { });
                services.Configure<ContentLocalizationOptions>(c => { });
            }
            
            services.TryAddScoped<ISimpleContentThemeHelper, DefaultSimpleContentThemeHelper>();
            
            // jk breaking change in Mediatr v12
            // services.AddMediatR(typeof(PageService).Assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddScoped<IParseModelFromForm, DefaultModelFormParser>();
            services.AddScoped<IValidateTemplateModel, DefaultTemplateModelValidator>();
            
            services.TryAddScoped<IPageUrlResolver, PageUrlResolver>();
            services.TryAddScoped<IBlogUrlResolver, BlogUrlResolver>();

            services.AddSingleton<IModelSerializer, JsonModelSerializer>();
            services.AddSingleton<IContentTemplateProvider, ConfigContentTemplateProvider>();
            services.TryAddSingleton<IContentTemplateService, ContentTemplateService>();

            services.TryAddScoped<ITimeZoneIdResolver, DefaultTimeZoneIdResolver>();
            
            services.AddScoped<CultureHelper>();

            return services;
        }

        

        


    }
}
