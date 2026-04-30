using Microsoft.Extensions.VectorData;

namespace JoseRizalRAGChat.Entities
{
    internal class TextChunkRecord
    {
        [VectorStoreKey]
        public string Id { get; set; } = default!;

        [VectorStoreData]
        public string Source { get; set; } = string.Empty;

        [VectorStoreData]
        public string Content { get; set; } = string.Empty;

        [VectorStoreVector(3072)]
        public float[] Embedding { get; set; } = default!;
    }
}
