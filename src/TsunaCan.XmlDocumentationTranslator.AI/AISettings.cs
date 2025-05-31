using System;
using TsunaCan.XmlDocumentationTranslator.AI.Resources;

namespace TsunaCan.XmlDocumentationTranslator.AI;

/// <summary>
///  Represents the AI translator application settings.
/// </summary>
public class AISettings
{
    private string token = string.Empty;
    private Uri chatEndPointUrl = new Uri("https://models.inference.ai.azure.com");
    private string modelId = "gpt-4.1-mini";
    private int chunkSize = 4000;
    private int maxConcurrentRequests = 5;

    /// <summary>
    ///  Gets or sets the authentication token.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item>Thrown when the value is null.</item>
    ///  </list>
    /// </exception>
    public string Token
    {
        get => this.token;
        set => this.token = value ?? throw new ArgumentNullException(nameof(this.Token));
    }

    /// <summary>
    ///  Gets or sets the chat endpoint URL.
    ///  Default is "https://models.inference.ai.azure.com".
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item>Thrown when the value is null.</item>
    ///  </list>
    /// </exception>
    public Uri ChatEndPointUrl
    {
        get => this.chatEndPointUrl;
        set => this.chatEndPointUrl = value ?? throw new ArgumentNullException(nameof(this.ChatEndPointUrl));
    }

    /// <summary>
    ///  Gets or sets the model ID to be used.
    ///  Default is "gpt-4.1-mini".
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item>Thrown when the value is null.</item>
    ///  </list>
    /// </exception>
    public string ModelId
    {
        get => this.modelId;
        set => this.modelId = value ?? throw new ArgumentNullException(nameof(this.ModelId));
    }

    /// <summary>
    ///  Gets or sets the chunk size for the translation process.
    ///  Default is 4000.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  <list type="bullet">
    ///   <item>Chunk size must be greater than zero.</item>
    ///  </list>
    /// </exception>
    public int ChunkSize
    {
        get => this.chunkSize;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(this.ChunkSize), Messages.ChunkSizeMustBeGreaterThanZero);
            }

            this.chunkSize = value;
        }
    }

    /// <summary>
    ///  Gets or sets the maximum number of concurrent requests for AI translation.
    ///  Default is 5.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  <list type="bullet">
    ///   <item>Max concurrent requests must be greater than zero.</item>
    ///  </list>
    /// </exception>
    public int MaxConcurrentRequests
    {
        get => this.maxConcurrentRequests;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(this.MaxConcurrentRequests), Messages.MaxConcurrentRequestsMustBeGreaterThanZero);
            }

            this.maxConcurrentRequests = value;
        }
    }

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
