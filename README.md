## Azure App Service MCP features

These sample applications are provided to test usage of [built-in MCP server authorization](https://learn.microsoft.com/azure/app-service/configure-authentication-mcp) and [built-in MCP](https://learn.microsoft.com/azure/app-service/configure-mcp-built-in?tabs=portal) for Azure App Service. These features are separate from each other, but can be used together.

- The BYO MCP Web App example is provided to test built-in MCP server authorization. This sample app includes two ModelContextProtocol.AspNetCore-based MCP tools, named FoodRecommendation and PetDamage. For the purpose of the MCP server authorization testing, the language framework or tool implementation doesn't matter. The purpose is to test the ability to access the tools in App Service. The ARM template uses WEBSITE_RUN_FROM_PACKAGE to load a pre-built zip of this code so that you don't have to deploy the code separately.

- The Built-in MCP Web App example is provided to test the built-in MCP feature for App Service. This feature reads a provided spec of a non-MCP API so that the non-MCP API endpoints can be read as MCP tools at runtime. The code consists of a non-MCP Microsoft.AspNetCore.Mvc.ApiController-based API. The ARM template uses WEBSITE_RUN_FROM_PACKAGE to load a pre-built zip of this code and API spec so that you don't have to deploy the code or download and provide the API spec separately.  You can optionally test this feature in combination with the built-in MCP server authorization feature.

> Note: How to download an API spec varies according to framework. In this case, a route to the OpenAPI endpoint /openapi/v1.json is exposed via the Program.cs. For convenience, this step is already applied and you just need to deploy the provided ARM template and the spec is automatically included as part of the deployed zip package and read via the aiIntegration.ApiSpecPath property that is set by the ARM template.

## Deploying, configuring, and testing the examples
In both cases, if you want to test MCP authentication, you will need to configure MCP authentication separately since the auth provider (e.g. Entra) can't be configured directly via ARM/Bicep. The most straightforward approach would be to deploy the ARM template and then configure the authentication. However, if you choose to configure the authentication first, provide the same region, App Service Plan name, and Web App name as what you configured authentication on, when you deploy the ARM template. The ARM templates are configured to preserve the existing app settings in addition to the ones provided in the ARM template, if you deploy to an existing Web App.


### BYO MCP Web App
  1. Deploy the App Service Plan, Web App, and code via [![Deploy the BYO MCP Web App to Azure](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.svg?sanitize=true)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fgabesmsft%2FWebAppMCPServers%2Fmaster%2FBYOMCPServer%2Fdeploy%2Fazuredeploy.json)

  2. Follow the steps in [Secure Model Context Protocol calls to Azure App Service from Visual Studio Code with Microsoft Entra authentication](https://learn.microsoft.com/azure/app-service/configure-authentication-mcp-server-vscode) <u>**except**</u> the Prerequisites section.
     > Note: The deployed sample app uses **/api/mcp** as the API endpoint, which matches what is in [Connect from Visual Studio Code](https://learn.microsoft.com/azure/app-service/configure-authentication-mcp-server-vscode#connect-from-visual-studio-code).

  3. Sample questions to ask:
     - Using the discovered tools, what food would you recommend that is sour?
       
       The returned answer should include "pickled lasagna".

     - Using the discovered tools, what pet would you recommend that would stomp my furniture into the ground?

       The returned answer should include "gorilla".

These are not conventional answers, but are used to verify that the model used these MCP tools rather than another source to provide the answers.


### Built-in MCP Web App
  1. Deploy the App Service Plan, Web App, and code and API spec (spec.json) via [![Deploy the Built-in MCP Web App to Azure](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.svg?sanitize=true)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fgabesmsft%2FWebAppMCPServers%2Fmaster%2FAPIForBuiltInMCPServer%2Fdeploy%2Fazuredeploy.json)

  2. [Connect from Visual Studio Code](https://learn.microsoft.com/azure/app-service/configure-authentication-mcp-server-vscode#connect-from-visual-studio-code). Use /fruit-mcp-server instead of /api/mcp as the MCP endpoint configured as part of the URL.

  3. Optional: Follow the steps in configuring authentication in [Secure Model Context Protocol calls to Azure App Service from Visual Studio Code with Microsoft Entra authentication](https://learn.microsoft.com/azure/app-service/configure-authentication-mcp-server-vscode), up to the Connect from Visual Studio Code section. Do not do the steps in the Prerequisites section.

  4. Sample questions to ask:
     - Using the discovered tools, what items did George Washington order?

       The returned answer should include a list of fruit and quantities.

     - Using the discovered tools, who ordered mangoes in syrup?

        The returned answer should be King Kong.
