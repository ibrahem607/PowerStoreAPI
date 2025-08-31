﻿
#nullable enable

namespace PowerStore.Core.Contract.Errors
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(string message, List<string> errors) : base(message)
        {
            Errors = errors;
        }

        public ValidationException(string message) : base(message)
        {
            Errors = new List<string> { message };
        }
    }

    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }

    public class IdentityOperationException : Exception
    {
        public List<string> Errors { get; }

        public IdentityOperationException(string message, List<string> errors) : base(message)
        {
            Errors = errors;
        }

        public IdentityOperationException(string message) : base(message)
        {
            Errors = new List<string> { message };
        }
    }
}
