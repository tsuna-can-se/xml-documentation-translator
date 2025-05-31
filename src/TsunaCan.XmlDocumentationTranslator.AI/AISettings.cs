namespace TsunaCan.XmlDocumentationTranslator.AI
{
    /// <summary>
    ///  Represents the AI translator application settings.
    /// </summary>
    public class AISettings
    {
        /// <summary>
        ///  Gets or sets the authentication token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///  Gets or sets the chat endpoint URL.
        ///  Default is "https://models.inference.ai.azure.com".
        /// </summary>
        public Uri ChatEndPointUrl { get; set; } = new Uri("https://models.inference.ai.azure.com");

        /// <summary>
        ///  Gets or sets the model ID to be used.
        ///  Default is "gpt-4.1-mini".
        /// </summary>
        public string ModelId { get; set; } = "gpt-4.1-mini";

        /// <summary>
        ///  Gets or sets the chunk size for the translation process.
        ///  Default is 4000.
        /// </summary>
        public int ChunkSize { get; set; } = 4000;

        /// <summary>
        ///  Gets or sets the maximum number of concurrent requests for AI translation.
        ///  Default is 5.
        /// </summary>
        public int MaxConcurrentRequests { get; set; } = 5;

        /// <inheritdoc/>
        public override string ToString()
        {
            string tokenDisplay = this.Token.Length > 10
                ? $"{new string('*', this.Token.Length - 5)}{this.Token.Substring(this.Token.Length - 5)}"
                : new string('*', this.Token.Length);
            return $"{nameof(this.Token)}: {tokenDisplay}, " +
                $"{nameof(this.ChatEndPointUrl)}: {this.ChatEndPointUrl}, " +
                $"{nameof(this.ModelId)}: {this.ModelId}, " +
                $"{nameof(this.ChunkSize)}: {this.ChunkSize}, " +
                $"{nameof(this.MaxConcurrentRequests)}: {this.MaxConcurrentRequests}";
        }
    }
}
