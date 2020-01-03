using System;


namespace PackageAnalyzer.Exceptions
{
    public class GraphBuilderException : Exception
    {
        public GraphBuilderException()
        {
        }

        public GraphBuilderException(string message) : base(message)
        {
        }

        public GraphBuilderException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}