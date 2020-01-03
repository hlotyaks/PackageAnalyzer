using System;


namespace PackageAnalyzer.Exceptions
{
    public class PackageBuilderCycleException : Exception
    {
        public PackageBuilderCycleException()
        {
        }

        public PackageBuilderCycleException(string message) : base(message)
        {
        }

        public PackageBuilderCycleException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}