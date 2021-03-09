﻿using System;

namespace com.drewchaseproject.net.Flexx.Core.Exceptions
{
    public class LibraryNotFoundException : Exception
    {
        public LibraryNotFoundException() : base()
        {
        }
        public LibraryNotFoundException(string message) : base(message)
        {
        }
        public LibraryNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}