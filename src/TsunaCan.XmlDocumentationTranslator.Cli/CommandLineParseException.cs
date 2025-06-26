namespace TsunaCan.XmlDocumentationTranslator.Cli;

/// <summary>
///  The exception that is thrown when the command line argument is invalid.
/// </summary>
internal class CommandLineParseException : Exception
{
    /// <summary>
    ///  Initialize a new instance of the <see cref="CommandLineParseException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    internal CommandLineParseException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///  Initialize a new instance of the <see cref="CommandLineParseException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="innerException">Inner exception.</param>
    internal CommandLineParseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
