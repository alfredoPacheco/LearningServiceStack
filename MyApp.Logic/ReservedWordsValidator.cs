using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Logic
{
    public class ReservedWdordsValidator : AbstractValidator<Document>
    {
        public ReservedWdordsValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .Must(notBeReservedWord);
        }

        bool notBeReservedWord(string word)
        {
            var reservedWords = new List<string> { "abc", "def" };
            return !reservedWords.Contains(word);
        }
    }

    public class Document
    {
        public string Name { get; set; }
    }
}
