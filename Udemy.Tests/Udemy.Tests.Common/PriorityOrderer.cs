using Xunit.Abstractions;
using Xunit.Sdk;

namespace Udemy.Tests.Common;

public class PriorityOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        var order = testCases.OrderBy(tc =>
            tc.TestMethod.Method
                .GetCustomAttributes(typeof(PriorityAttribute).FullName)
                .FirstOrDefault()
                ?.GetNamedArgument<int>("Priority") ?? 0);

        Console.WriteLine("Test cases ordered by priority:");
        foreach (var testCase in order)
        {
            Console.WriteLine(testCase.TestMethod.Method.Name);
        }

        return order;
    }
}