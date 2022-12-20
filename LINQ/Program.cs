using CommonFunctions;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace LINQ
{
    enum LINQSections
    {
        Basics = 1,
        MethodSyntax = 2,
        Delegates = 3,
        LambdaExpressions = 4
    }

    class Program
    {
        static void Main()
        {
            switch (InterfaceFunctions.ChooseSection<LINQSections>())
            {
                case LINQSections.Basics:
                    {
                        Basics.BasicLINQSyntax();
                    }
                    break;
                case LINQSections.MethodSyntax:
                    {
                        Methods.MethodLINQSyntax();
                    }
                    break;
                case LINQSections.Delegates:
                    {
                        Delegates.DelegateTests();
                    }
                    break;
                case LINQSections.LambdaExpressions:
                    {
                        LambdaExpressions.LambdaTests();
                    }
                    break;
            }
            Console.ReadLine();
        }
    }
}
