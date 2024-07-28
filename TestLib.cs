public static class TestLib
{
    #region Public Delegates
    public delegate object ObjectExpression();
    public delegate T Expression<T>();
    public delegate T[] ArrayExpression<T>();
    public delegate void VoidExpression();
    #endregion
    #region Public Methods
    public static void AssertNoErr(ObjectExpression expression, string testName = null)
    {
        AssertNoErr(() => { _ = expression.Invoke(); }, testName);
    }
    public static void AssertNoErr<T>(Expression<T> expression, string testName = null) where T : struct
    {
        AssertNoErr(() => { _ = expression.Invoke(); }, testName);
    }
    public static void AssertNoErr<T>(ArrayExpression<T> expression, string testName = null) where T : struct
    {
        AssertNoErr(() => { _ = expression.Invoke(); }, testName);
    }
    public static void AssertNoErr(VoidExpression expression, string testName = null)
    {
        if (expression is null)
        {
            throw new System.Exception($"{nameof(expression)} cannot be null.");
        }
        try
        {
            expression.Invoke();
            LogAssert(true, testName);
        }
        catch (System.Exception ex)
        {
            LogAssert(false, testName, ex.Message);
        }
    }

    public static void AssertValue<T>(Expression<T> expression, T expectedReturn, string testName = null) where T : struct
    {
        if (expression is null)
        {
            throw new System.Exception($"{nameof(expression)} cannot be null.");
        }
        try
        {
            T returnValue = expression.Invoke();
            if (expectedReturn.Equals(returnValue))
            {
                LogAssert(true, testName);
            }
            else
            {
                LogAssert(true, testName, $"Expected {expectedReturn} got {returnValue}");
            }
        }
        catch (System.Exception ex)
        {
            LogAssert(false, testName, ex.Message);
        }
    }

    public static void AssertArray<T>(ArrayExpression<T> expression, T[] expectedReturn, string testName = null) where T : struct
    {
        if (expression is null)
        {
            throw new System.Exception($"{nameof(expression)} cannot be null.");
        }
        try
        {
            T[] returnValue = expression.Invoke();
            if (expectedReturn is null)
            {
                if (returnValue is null)
                {
                    LogAssert(true, testName);
                }
                else
                {
                    LogAssert(true, testName, $"Expected null got {returnValue}");
                }
            }
            else
            {
                if (returnValue.LongLength == expectedReturn.LongLength)
                {
                    long length = expectedReturn.LongLength;
                    for (long i = 0; i < length; i++)
                    {
                        if (expectedReturn[i].Equals(returnValue[i]))
                        {
                            LogAssert(true, testName, $"Expected {expectedReturn[i]} at position {i} got {returnValue[i]}");
                            return;
                        }
                    }
                    LogAssert(true, testName);
                }
                else
                {
                    LogAssert(true, testName, $"Expected length {expectedReturn.Length} got {returnValue.Length}");
                }
            }
        }
        catch (System.Exception ex)
        {
            LogAssert(false, testName, ex.Message);
        }
    }
    #endregion
    #region Private Methods
    private static void LogAssert(bool succeeded, string testName = null, string failureInfo = null)
    {
        if (testName is null || testName is "")
        {
            testName = GetCalleeName();
        }
        System.ConsoleColor originalConsoleColor = System.Console.ForegroundColor;
        if (succeeded)
        {
            System.Console.ForegroundColor = System.ConsoleColor.Green;
            if (testName is null)
            {
                System.Console.WriteLine("Assertation Succeeded");
            }
            else
            {
                System.Console.WriteLine($"Assertation Succeeded - {testName}");
            }
        }
        else
        {
            System.Console.ForegroundColor = System.ConsoleColor.Red;
            if (testName is null)
            {
                if (failureInfo is null || failureInfo is "")
                {
                    System.Console.WriteLine("Assertation Failed");
                }
                else
                {
                    System.Console.WriteLine($"Assertation Succeeded - {failureInfo}");
                }
            }
            else
            {
                if (failureInfo is null || failureInfo is "")
                {
                    System.Console.WriteLine($"Assertation Failed - {testName}");
                }
                else
                {
                    System.Console.WriteLine($"Assertation Succeeded - {testName} - {failureInfo}");
                }
            }
        }
        System.Console.ForegroundColor = originalConsoleColor;
    }

    private static string GetCalleeName()
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        int currentFrame = 0;
        while (currentFrame < stackTrace.FrameCount)
        {
            System.Diagnostics.StackFrame frame = stackTrace.GetFrame(1);
            if (!(frame is null))
            {
                System.Reflection.MethodBase method = frame.GetMethod();
                if (method.DeclaringType != typeof(TestLib))
                {
                    return method.Name;
                }
            }
        }
        return null;
    }
    #endregion
}