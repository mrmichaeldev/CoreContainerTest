using Database.Models;
using RulesEngine.Models;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RulesEngine.RulesEngine rulesEngine;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            var reSettings = new ReSettings
            {
                CustomTypes = new Type[] { typeof(Utils) },
                CustomActions = new Dictionary<string, Func<RulesEngine.Actions.ActionBase>>
                {
                    {"Remove 01 at end of NM109", () => new CustomAction(logger) }
                }
            };

            var rules = new Rule[]
            {
                new Rule
                {
                    RuleName = "Valley Medical - REC2370 remove trailing 01 from Member ID",
                    Enabled = true,
                    Expression = "provider.NM109 == \"1649209230\" AND recID == \"REC2730\" AND subscriberLoop.NM1 != null AND subscriberLoop.NM1.NM109.Length == 10 AND Utils.RegexMatch(subscriberLoop.NM1.NM109, \".*01$\")",
                    Actions = new RuleActions
                    {
                        OnSuccess = new ActionInfo
                        {
                            Name = "Remove 01 at end of NM109",
                            Context = new Dictionary<string, object>{ }
                        }
                    }
                }
            };

            var workflow = new Workflow()
            {
                Rules = rules,
                WorkflowName = "Test"
            };

            rulesEngine = new RulesEngine.RulesEngine(new Workflow[] { workflow }, logger, reSettings);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    var result = await rulesEngine.ExecuteAllRulesAsync("Test", new RuleParameter("subscriberLoop", new Provider { NM1 = new NM1 { NM109 = "1649209201" } }), new RuleParameter("recID", "REC2730"), new RuleParameter("provider", new { NM109 = "1649209230" }));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
