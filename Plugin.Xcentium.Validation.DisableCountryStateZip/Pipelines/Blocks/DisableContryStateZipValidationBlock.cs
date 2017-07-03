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
            Condition.Requires<Party>(arg).IsNotNull<Party>(string.Format("{0}: The argument cannot be null.", (object)this.Name));
            var party = this.ValidateParty(arg, context);
            if (party == null)
                context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "AddressNotValid", new object[1]
                {
          (object) arg.AddressName
                }, string.Format("Address {0} is not valid.", (object)arg.AddressName));
            await Task.Delay(100);
            return party;
        }

        private Party ValidateParty(Party party, CommercePipelineExecutionContext context)
        {
            Condition.Requires<Party>(party).IsNotNull<Party>(string.Format("{0}: The party can not be null", (object)this.Name));
            var success = true;
            success = this.ValidateProperty("AddressName", party.AddressName, context);
            success &= this.ValidateProperty("Address1", party.Address1, context);
            success &= this.ValidateProperty("City", party.City, context);
            return !success ? null : party;
        }

        private bool ValidateProperty(string propertyName, string propertyValue, CommercePipelineExecutionContext context)
        {
            Condition.Requires<string>(propertyName).IsNotNull<string>(string.Format("{0}: The propertyName can not be null", (object)this.Name));
            if (!string.IsNullOrEmpty(propertyValue))
                return true;
            context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "InvalidOrMissingPropertyValue", new object[1]
            {
        (object) propertyName
            }, string.Format("Invalid or missing value for property ' {0}'.", (object)propertyName));
            return false;
        }



    }
}
