using DotNetEnv;
using Microsoft.SemanticKernel;

namespace JoseRizalRAGChat.Extensions
{
    internal static class RegisterAIExtension
    {
        public static void AddAI(this IKernelBuilder kernelBuilder)
        {
            Env.Load(".env");

            string apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? 
                throw new ArgumentNullException("No AI API found.");

            string chatModel = Environment.GetEnvironmentVariable("CHAT_MODEL") ?? 
                throw new ArgumentNullException("No chat model found.");

            string embeddingModel = Environment.GetEnvironmentVariable("EMBEDDING_MODEL") ?? 
                throw new ArgumentNullException("No embedding model found.");

            kernelBuilder.AddGoogleAIGeminiChatCompletion(chatModel, apiKey);
            kernelBuilder.AddGoogleAIEmbeddingGenerator(embeddingModel, apiKey);
        }
    }
}
