using System.Diagnostics.CodeAnalysis;
using TsunaCan.XmlDocumentationTranslator.Cli.Resources;

namespace TsunaCan.XmlDocumentationTranslator.Cli;

/// <summary>
///  ParameterNotSetException is thrown when a required parameter is not set.
/// </summary>
public class ParameterNotSetException : Exception
{
    /// <summary>
    ///  Initializes a new instance of the <see cref="ParameterNotSetException"/> class.
    /// </summary>
    /// <param name="parameterName">parameter name.</param>
    public ParameterNotSetException(string parameterName)
        : base(string.Format(Messages.ParameterNotSet, parameterName))
    {
        this.ParameterName = parameterName;
    }

    /// <summary>
    ///  Get parameter name.
    /// </summary>
    public string ParameterName { get; }

    /// <summary>
    ///  Throws a <see cref="ParameterNotSetException"/> if the specified parameter is not set.
    /// </summary>
    /// <param name="parameterValue">parameter value.</param>
    /// <param name="parameterName">parameter name.</param>
    /// <exception cref="ParameterNotSetException">
    ///  <list type="bullet">
    ///   <item><paramref name="parameterValue"/> is null or empty.</item>
    ///  </list>
    /// </exception>
    public static void ThrowIfNotSet([NotNull] string? parameterValue, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(parameterValue))
        {
            throw new ParameterNotSetException(parameterName);
        }
    }
}
