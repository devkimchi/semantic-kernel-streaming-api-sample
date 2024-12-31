namespace SKStreaming.ConsoleApp.Models;

public class PromptRequest(string prompt)
{
    public string? Prompt { get; set; } = prompt;
}
