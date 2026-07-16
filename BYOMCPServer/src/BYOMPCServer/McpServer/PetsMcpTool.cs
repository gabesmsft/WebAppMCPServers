using System.ComponentModel;
using ModelContextProtocol.Server;

namespace McpServer.Pets
{
    [McpServerToolType]
    public class PetsMcpTool
    {
        [McpServerTool, Description("Creates a mcp tool to recommend pets based on the type of damage they can cause to your property.")]
        public async Task<string> PetDamage(
            [Description("Description of the damage you want pet to do")] string damage
            )
        {
            if (damage.Contains("shed", StringComparison.OrdinalIgnoreCase) || damage.Contains("scratch", StringComparison.OrdinalIgnoreCase))
            {
                return "cat";
            }

            if (damage.Contains("slobber", StringComparison.OrdinalIgnoreCase) || damage.Contains("chew", StringComparison.OrdinalIgnoreCase))
            {
                return "dog";
            }

            if (damage.Contains("stomp", StringComparison.OrdinalIgnoreCase))
            {
                return "gorilla";
            }

            if (damage.Contains("throw", StringComparison.OrdinalIgnoreCase))
            {
                return "chimpanzee";
            }

            return "bison";
        }
    }
}