using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VN.Example.Platform.Domain
{
    public static class ValidationHelper
    {
        public static void Validate(this ICommand command)
        {
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(command,
                                                       new ValidationContext(command, null, null),
                                                       results,
                                                       false);
            if (!isValid)
            {
                var builder = new StringBuilder();

                builder.AppendJoin('|', results);

                throw new ArgumentException(builder.ToString());
            }
        }
    }
}
