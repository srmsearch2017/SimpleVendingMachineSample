using System;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleVendingMachine.UnitTests
{
    public static class ExceptionExtensions
    {    
        public static void AssertException(this Exception ex, string expectedMessage = null)
        {
            Assert.IsNotNull(ex, "Expected Exception was null.");

            if (expectedMessage != null)
            {
                AssertExceptionMessage(ex, expectedMessage);
            }
        }

        public static void AssertArgumentException(this Exception ex, string expectedMessage, string expectedParamName = null)
        {
            AssertException(ex, null);

            AssertExceptionType<ArgumentException>(ex);

            ArgumentException argEx = ex as ArgumentException;

            StringBuilder exExpectedMessage = new StringBuilder();
            exExpectedMessage.Append(expectedMessage);

            if (expectedParamName != null)
            {
                Assert.IsTrue(argEx.ParamName == expectedParamName, "Expected Argument Exception Parameter was not {0}.", expectedParamName);
                exExpectedMessage.Append(Environment.NewLine).Append("Parameter name: ").Append(expectedParamName);
            }

            AssertExceptionMessage(argEx, exExpectedMessage.ToString());
        }

        public static void AssertExceptionType<T>(this Exception ex) where T : Exception
        {
            Assert.IsTrue(ex.GetType() == typeof(T), "Expected Exception was not of type {0}.", typeof(T));
        }

        public static void AssertExceptionMessage(this Exception ex, string expectedMessage)
        {
            Assert.IsTrue(ex.Message.Equals(expectedMessage, StringComparison.InvariantCulture), "Expected Exception Message was not {0}.", expectedMessage);
        }
    }
}
