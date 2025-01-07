# Semantic Kernel Streaming API Sample

This provides sample app that shows the ASP.NET Core streaming API capability with Semantic Kernel.

## Prerequisites

- [.NET 9+ SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/) with [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- [GitHub CLI](https://github.com/cli/cli#installation)
- [Azure OpenAI](https://learn.microsoft.com/azure/ai-services/openai/overview)

## Getting Started

1. Get the Azure OpenAI access endpoint and key.

1. Clone this repository.

    ```bash
    gh repo fork devkimchi/semantic-kernel-streaming-api-sample --clone
    ```

1. Change the directory to the repository.

    ```bash
    cd semantic-kernel-streaming-api-sample
    ```

1. Open `src/SKStreaming.ApiApp/appsettings.json` and update either the `Azure:OpenAI` section with your Azure OpenAI access endpoint and key or the `GitHub:Models` section with your GitHub PAT and Model ID.

    ```json
    {
      "Azure": {
        "OpenAI": {
          "Endpoint": "{{AZURE_OPENAI_ENDPOINT}}",
          "ApiKey": "{{AZURE_OPENAI_API_KEY}}",
          "DeploymentName": "{{AZURE_OPENAI_DEPLOYMENT_NAME}}"
        }
      },

      "GitHub": {
        "Models": {
          "Endpoint": "https://models.inference.ai.azure.com",
          "GitHubToken": "{{GITHUB_PAT}}",
          "ModelId": "{{GITHUB_MODEL_ID}}"
        }
      }
    }
    ```

   > **NOTE**: DO NOT commit this file back to the repository.

1. In the same `appsettings.json` file, choose which LLM you will use. You can choose either `aoai` or `github`. As a default, it's set to `github`.

    ```json
    {
      "SemanticKernel": {
        "ServiceId": "github"
      }
    }
    ```

1. Build the sample apps.

    ```bash
    dotnet restore && dotnet build
    ```

1. Run the API app.

    ```bash
    dotnet run --project ./src/SKStreaming.ApiApp
    ```

1. Open a new terminal and run the console app.

    ```bash
    # Simple chat completion
    dotnet run --project ./src/SKStreaming.ConsoleApp -- -t chat

    # Streaming chat completion
    dotnet run --project ./src/SKStreaming.ConsoleApp -- -t chat-streaming
    ```

   When you're asked, enter any prompt you want to chat, and see the difference between simple chat completion and streaming chat completion.

1. For auto function calling, run the console app with the bookings option.

    ```bash
    # Book a meeting room
    dotnet run --project ./src/SKStreaming.ConsoleApp -- -t bookings
    ```

   When you're asked, enter a prompt like "Please book a meeting room", and see the auto function calling in action.
