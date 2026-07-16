using System.ComponentModel;
using ModelContextProtocol.Server;

namespace McpServer.Food
{
    [McpServerToolType]
    public class FoodMcpTool
    {
        [McpServerTool, Description("Creates a food recommendation tool")]
        public async Task<string> FoodRecommendation(
            [Description("Description of the taste you want to satisfy")] string taste
            )
        {
            if (taste.Contains("sour", StringComparison.OrdinalIgnoreCase))
            {
                return "pickled lasagna";
            }

            if (taste.Contains("sweet", StringComparison.OrdinalIgnoreCase))
            {
                return "chocolate dumplings";
            }

            if (taste.Contains("savory", StringComparison.OrdinalIgnoreCase))
            {
                return "biscuits and gravy";
            }

            return "rice cakes";
        }
    }
}