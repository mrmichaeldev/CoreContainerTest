using Database.Models;
using RulesEngine.Actions;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class CustomAction : ActionBase
    {
        private readonly ILogger<Worker> logger;

        public CustomAction(ILogger<Worker> logger)
        {
            this.logger = logger;
        }

        public override async ValueTask<object> Run(ActionContext context, RuleParameter[] ruleParameters)
        {
            try
            {
                var provider = ruleParameters[0].Value as Provider;
                var nm109 = provider.NM1.NM109;

                provider.NM1.NM109 = nm109.Substring(0, nm109.Length - 2);
                return await ValueTask.FromResult(provider);
            }
            catch(Exception exc)
            {
                logger.LogError(exc, "Error running custom action");
            }

            return null;
        }
    }
}
