using JoseRizalRAGChat.Entities;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.InMemory;
using Microsoft.SemanticKernel.Text;

namespace JoseRizalRAGChat.Data
{
    internal class InMemoryVector
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
        private const string knowledgeFile = "jose-rizal-info.txt";

        public InMemoryVector(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _embeddingGenerator = embeddingGenerator;
        }

        public async Task<InMemoryCollection<string, TextChunkRecord>> Store()
        {
            var vectorStore = new InMemoryVectorStore();
            var collection = vectorStore.GetCollection<string, TextChunkRecord>("docs");
            await ReadFile(collection);
            
            return collection;
        }

        private async Task ReadFile(InMemoryCollection<string, TextChunkRecord> collection)
        {
            await collection.EnsureCollectionExistsAsync();

            if(!File.Exists(knowledgeFile))
            {
                throw new FileNotFoundException($"The file '{knowledgeFile}' was not found.");
            }

            var text = await File.ReadAllTextAsync(knowledgeFile);
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            #pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var chunks = TextChunker.SplitMarkdownParagraphs(lines,
                maxTokensPerParagraph: 300,
                overlapTokens: 50);

            int id = 0;

            foreach (var chunk in chunks)
            {
                id++;

                Console.WriteLine($"Chunking... {id}");

                var fileEmbedding = await _embeddingGenerator.GenerateAsync(chunk);

                var record = new TextChunkRecord()
                {
                    Id = $"chunk-{id}",
                    Source = knowledgeFile,
                    Content = chunk,
                    Embedding = fileEmbedding.Vector.ToArray()
                };

                await collection.UpsertAsync(record);
            }
        }
    }
}
