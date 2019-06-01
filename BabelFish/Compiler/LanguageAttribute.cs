using System;
namespace BabelFish.Compiler
{
    public class LanguageAttribute : Attribute
    {
        public Language Language { get; }
        public string Type { get; }

        public LanguageAttribute(Language language, string type)
        {
            this.Language = language;
            this.Type = type;
        }
    }
}
