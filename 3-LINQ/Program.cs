using CommonFunctions;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace LINQ
{
    enum LINQSections
    {
        Delegates = 1,
        LambdaExpressions = 2,
        Basics = 3,
        MethodSyntax = 4,
    }

    class Program
    {
        static void Main()
        {
            switch (InterfaceFunctions.ChooseSection<LINQSections>())
            {
                case LINQSections.Delegates:
                    {
                        Delegates.DelegateExamples();
                    }
                    break;
                case LINQSections.LambdaExpressions:
                    {
                        LambdaExpressions.LambdaExpressionExamples();
                    }
                    break;
                case LINQSections.Basics:
                    {
                        Basics.BasicLINQSyntax();
                    }
                    break;
                case LINQSections.MethodSyntax:
                    {
                        Functions.FunctionLINQSyntax();
                    }
                    break;
            }
            Console.ReadLine();
        }
    }
}
