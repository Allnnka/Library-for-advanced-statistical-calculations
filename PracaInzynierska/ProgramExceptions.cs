using System;

namespace PracaInzynierska
{

    [Serializable]
    public class EmptyListException : Exception
    {
        public EmptyListException():base(String.Format("The list does not contain items")) { }
    }

    [Serializable]
    public class NotTheSameLengthException : Exception
    {
        public NotTheSameLengthException() : base(String.Format("The collection must be the same length")) { }
    }

    [Serializable]
    public class NotTheRightSizeException : Exception
    {
        public NotTheRightSizeException() : base(String.Format("The collection for which the correlation coefficient is calculated is too small or too large")) { }
    }

    [Serializable]
    public class InvalidArgument : Exception
    {
        public InvalidArgument() : base(String.Format("Invalid argument.")) { }
        public InvalidArgument(string type) : base(String.Format("Invalid {0} argument.",type)) { }

    }
}
