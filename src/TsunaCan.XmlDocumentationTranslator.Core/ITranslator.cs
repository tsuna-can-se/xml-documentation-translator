using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TsunaCan.XmlDocumentationTranslator.IntelliSense;

namespace TsunaCan.XmlDocumentationTranslator
{
/// <summary>
///  Interface for translating XML documentation files.
/// </summary>
public interface ITranslator : IDisposable
{
    /// <summary>
    ///  Translates the XML documentation file to a different languages.
    /// </summary>
    /// <param name="document">Source IntelliSense document accessor.</param>
    /// <param name="sourceLanguage">Source document language.</param>
    /// <param name="targetLanguages"> Target document languages.</param>
    /// <returns>Translated IntelliSense documents.</returns>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="targetLanguages"/> is empty.</item>
    ///  </list>
    /// </exception>
    Task<Dictionary<CultureInfo, IntelliSenseDocument>> TranslateAsync(IntelliSenseDocumentAccessor document, CultureInfo? sourceLanguage, IEnumerable<CultureInfo> targetLanguages);
    }
}
