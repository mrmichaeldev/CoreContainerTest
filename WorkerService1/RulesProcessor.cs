using Database.Models;
using RulesEngine.Models;

internal class RulesProcessor
{
    private readonly RulesEngine.RulesEngine rulesEngine;
    private readonly TestObject input;

    public RulesProcessor(RulesEngine.RulesEngine rulesEngine, TestObject input)
    {
        this.rulesEngine = rulesEngine;
        this.input = input;
    }

    public async Task<List<RuleResultTree>> ExecuteRules(string rulesName, params object[] inputs)
    {
        var result = await rulesEngine.ExecuteAllRulesAsync("TestRules", input);

        result.ForEach(r =>
        {
            if (r.IsSuccess)
            {

            }
        });

        return result;
    }
}