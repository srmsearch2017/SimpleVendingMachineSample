using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleVendingMachine.UnitTests
{
    public static class ExceptionAssert
    {
        public static void Throws<TException>(Action action) where TException : Exception
        {
            Throws<TException>(action, null, null);
        }
        
        public static void Throws<TException>(Action action, string message) where TException : Exception
        {
            Throws<TException>(action, message, null);
        }

        public static void Throws<TException>(Action action, string message, string paramName) where TException : Exception
        {
            Exception expected = null;

            try
            {
                action();

                Assert.Fail("Expected exception type {0} was not thrown.", typeof(TException));
            }
            catch (Exception ex)
            {
                expected = ex;

                if (typeof(TException).Equals(typeof(ArgumentException)))
                {
                    expected.AssertArgumentException(message, paramName);
                }
                else
                {
                    expected.AssertException(message);
                }

                expected.AssertExceptionType<TException>();
            }            
        }
    }
}
