using System.Globalization;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator.Cli.Resources;

namespace TsunaCan.XmlDocumentationTranslator.Cli;

/// <summary>
///  Parses command-line arguments and applies them to an <see cref="Options"/> instance.
/// </summary>
internal class CommandLineParser
{
    /// <summary>
    ///  Parses the specified command-line arguments and overrides the corresponding properties in the provided <see cref="Options"/> instance.
    /// </summary>
    /// <param name="args">The command-line arguments to parse.</param>
    /// <param name="options">The <see cref="Options"/> instance to override with parsed values.</param>
    /// <returns>The <see cref="Options"/> instance with updated values from the command-line arguments.</returns>
    /// <exception cref="CommandLineParseException">
    ///  <list type="bullet">
    ///   <item>Thrown when an argument is invalid, missing, or cannot be parsed as expected (e.g., invalid culture name, invalid chunk size).</item>
    ///  </list>
    /// </exception>
    internal static Options ParseAndOverride(string[] args, Options options)
    {
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i].StartsWith("--source-document-path") || args[i].StartsWith("-s"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                options.SourceDocumentPath = value;
            }
            else if (args[i].StartsWith("--output-directory-path"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                options.OutputDirectoryPath = value;
            }
            else if (args[i].StartsWith("--source-document-language"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                try
                {
                    options.SourceDocumentLanguage = CultureInfo.GetCultureInfo(value, true);
                }
                catch (Exception ex)
                {
                    throw new CommandLineParseException(string.Format(Messages.InvalidCultureName, value), ex);
                }
            }
            else if (args[i].StartsWith("--output-file-languages") || args[i].StartsWith("-l"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                var cultures = value.Split(',').Select(c => c.Trim()).ToArray();
                options.OutputFileLanguages = cultures.Select(c =>
                {
                    try
                    {
                        return CultureInfo.GetCultureInfo(c, true);
                    }
                    catch (Exception ex)
                    {
                        throw new CommandLineParseException(string.Format(Messages.InvalidCultureName, c), ex);
                    }
                }).ToArray();
            }
            else if (args[i].StartsWith("--token") || args[i].StartsWith("-t"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                options.Token = value;
            }
            else if (args[i].StartsWith("--chat-endpoint-url"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
                {
                    options.ChatEndPointUrl = uri;
                }
                else
                {
                    throw new CommandLineParseException(Messages.InvalidChatEndpointUrl);
                }
            }
            else if (args[i].StartsWith("--model-id"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                options.ModelId = value;
            }
            else if (args[i].StartsWith("--chunk-size"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                if (int.TryParse(value, out var chunkSize) && chunkSize > 0)
                {
                    options.ChunkSize = chunkSize;
                }
                else
                {
                    throw new CommandLineParseException(Messages.InvalidChunkSize);
                }
            }
            else if (args[i].StartsWith("--max-concurrent-requests"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                if (int.TryParse(value, out var maxConcurrentRequests) && maxConcurrentRequests > 0)
                {
                    options.MaxConcurrentRequests = maxConcurrentRequests;
                }
                else
                {
                    throw new CommandLineParseException(Messages.InvalidMaxConcurrentRequests);
                }
            }
            else if (args[i].StartsWith("--log-level"))
            {
                var value = args[++i];
                ThrowIfParameterName(value);
                if (Enum.TryParse<LogLevel>(value, out var logLevel))
                {
                    options.LogLevel = logLevel;
                }
                else
                {
                    throw new CommandLineParseException(Messages.InvalidLogLevel);
                }
            }
            else if (args[i].StartsWith("--help") || args[i].StartsWith("-h"))
            {
                options.IsHelpRequested = true;
            }
        }

        return options;
    }

    /// <summary>
    ///  Throws a <see cref="CommandLineParseException"/> if the argument appears to be a parameter name (starts with '-').
    /// </summary>
    /// <param name="arg">The argument to check.</param>
    /// <exception cref="CommandLineParseException">
    ///  <list type="bullet">
    ///   <item>Thrown if <paramref name="arg"/> starts with '-'.</item>
    ///  </list>
    /// </exception>
    private static void ThrowIfParameterName(string arg)
    {
        if (arg.StartsWith('-'))
        {
            throw new CommandLineParseException(string.Format(Messages.InvalidVariableIsSet, arg));
        }
    }
}
