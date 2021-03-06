﻿using System;
using System.Runtime.Serialization;

namespace DuelMastersModels.Exceptions
{
    /// <summary>
    /// Class for parsing exceptions.
    /// </summary>
    [Serializable]
    public class ParseException : DuelMastersException
    {
        /// <summary>
        /// Creates a parse exception.
        /// </summary>
        public ParseException()
        {
        }

        /// <summary>
        /// Creates a parse exception.
        /// </summary>
        /// <param name="message"></param>
        public ParseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a parse exception. 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a parse exception.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
