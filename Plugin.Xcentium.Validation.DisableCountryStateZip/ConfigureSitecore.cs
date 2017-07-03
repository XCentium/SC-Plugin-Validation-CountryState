using System;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Plugin.Xcentium.Validation.DisableCountryStateZip.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Plugin.Xcentium.Validation.DisableCountryStateZip
{
    /// <summary>
    /// The Mayo configure class.
    /// </summary>
    /// <seealso cref="Sitecore.Framework.Configuration.IConfigureSitecore" />
    public class ConfigureSitecore : IConfigureSitecore
    {

        /// <summary>The configure services.</summary>
        /// <param name="services">The services.</param>

        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            Action<SitecorePipelinesConfigBuilder> actionDelegate = c => c
                .ConfigurePipeline<IValidatePartyPipeline>(
                    d =>
                    {
                        d.Replace<ValidatePartyBlock, DisableContryStateZipValidationBlock>();
                    });

            services.Sitecore().Pipelines(actionDelegate);

        }


    }


}
