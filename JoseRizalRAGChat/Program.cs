using JoseRizalRAGChat.Data;
using JoseRizalRAGChat.Extensions;
using JoseRizalRAGChat.Utilities;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

var builder = Kernel.CreateBuilder();

builder.AddAI();

Kernel kernel = builder.Build();

var chat = kernel.Services.GetRequiredService<IChatCompletionService>();
var embedder = kernel.Services.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

Console.WriteLine("Initializing vector store with Jose Rizal's information...");
var inMemoryVector = new InMemoryVector(embedder);
var collection = await inMemoryVector.Store();
Console.WriteLine("Vector store initialized successfully!\n");

Console.WriteLine("Type any question about Rizal!\nThe AI is trained to specifically know the life of Jose Rizal\n\n" +
    "Type 'exit' to close this program.\n\n");

while (true)
{
    Console.WriteLine("\n----------\n");
    Console.Write("Me: ");
    string? question = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(question))
    {
        Console.WriteLine("Please enter your question");
        continue;
    }
    else if (question.Length > 100)
    {
        Console.WriteLine("Please enter a shorter question (max 100 characters)");
        continue;
    }
    else if (question.ToUpper() == "EXIT")
        break;
    else
    {
        var aiReply = new AIReply(chat, embedder);
        string? reply = await aiReply.Build(question!, collection);

        Console.WriteLine($"Answer: {reply}");
        Console.WriteLine("\n----------\n");
    }
}