using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TestClassAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestMethodAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestInitializeAttribute : Attribute { }

    public static class Assert
    {
        public static void IsTrue(bool condition)
        {
            if (!condition) throw new Exception("Assert.IsTrue failed");
        }

        public static void IsTrue(bool condition, string message)
        {
            if (!condition) throw new Exception(message);
        }

        public static void IsFalse(bool condition)
        {
            if (condition) throw new Exception("Assert.IsFalse failed");
        }

        public static void AreEqual(object expected, object actual)
        {
            if (!object.Equals(expected, actual)) throw new Exception($"Assert.AreEqual failed. Expected: {expected}, Actual: {actual}");
        }

        public static void Fail(string message)
        {
            throw new Exception(message ?? "Assert.Fail");
        }
    }
}
