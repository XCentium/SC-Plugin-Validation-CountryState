using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Xcentium.Validation.DisableCountryStateZip.Pipelines.Blocks
{

    /// <summary>
    /// 
    /// </summary>
    public class DisableContryStateZipValidationBlock : PipelineBlock<Party, Party, CommercePipelineExecutionContext>
    {

        /// <summary>
        /// 
        /// </summary>
        public DisableContryStateZipValidationBlock() : base((string)null)
        {
            
        }

        /// <summary>
        /// Task
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Party> Run(Party arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires<Party>(arg).IsNotNull<Party>($"{(object) this.Name}: The argument cannot be null.");
            var party = this.ValidateParty(arg, context);
            if (party == null)
                await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "AddressNotValid",
                    new object[1]
                    {
                        (object) arg.AddressName
                    }, $"Address {(object) arg.AddressName} is not valid.");
            return party;
        }

        private Party ValidateParty(Party party, CommercePipelineExecutionContext context)
        {
            Condition.Requires<Party>(party).IsNotNull<Party>($"{(object) this.Name}: The party can not be null");
            var success = true;
            success = this.ValidateProperty("AddressName", party.AddressName, context).Result;
            success &= this.ValidateProperty("Address1", party.Address1, context).Result;
            success &= this.ValidateProperty("City", party.City, context).Result;
            return !success ? null : party;
        }

        private async Task<bool> ValidateProperty(string propertyName, string propertyValue, CommercePipelineExecutionContext context)
        {
            Condition.Requires<string>(propertyName).IsNotNull<string>(
                $"{(object) this.Name}: The propertyName can not be null");
            if (!string.IsNullOrEmpty(propertyValue))
                return true;
           await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "InvalidOrMissingPropertyValue", new object[1]
            {
        (object) propertyName
            }, $"Invalid or missing value for property ' {(object) propertyName}'.");
            return false;
        }



    }
}
