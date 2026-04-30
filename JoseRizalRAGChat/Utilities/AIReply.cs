using JoseRizalRAGChat.Entities;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.InMemory;

namespace JoseRizalRAGChat.Utilities
{
    internal class AIReply
    {
        private readonly IChatCompletionService _chatCompletion;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;

        public AIReply(IChatCompletionService chatCompletion,
            IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _chatCompletion = chatCompletion;
            _embeddingGenerator = embeddingGenerator;
        }

        public async Task<string?> Build(string question,
            InMemoryCollection<string, TextChunkRecord> collection)
        {
            var questionEmbedding = await _embeddingGenerator.GenerateAsync(question);
            
            var hits = new List<TextChunkRecord>();

            await foreach(var hit in collection.SearchAsync(questionEmbedding.Vector.ToArray(), top: 5))
            {
                if (hit.Record is not null)
                {
                    hits.Add(hit.Record);
                }
            }

            var context = string.Join("\n---\n", hits.Select((h, id) => $"[Doc {id + 1}] Source: {h.Source}\n{h.Content}"));
            
            var history = new ChatHistory();

            history.AddSystemMessage("""
                Base your answer with only the provided context.
                If the context doesn't contain the answer, say: "This is out of my information range."
                Keep your answer clear, brief, and concise. Avoid unnecessary explanations.
                """);

            history.AddUserMessage($"""
                Question: {question}
                Context: {context}
                """);

            var reply = await _chatCompletion.GetChatMessageContentAsync(history);

            return reply.Content;
        }
    }
}
