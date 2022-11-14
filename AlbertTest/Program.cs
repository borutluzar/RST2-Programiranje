namespace AlbertTest
{
    internal class Program
    {
        enum Testing
        {
            Home,
            Job
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            CommonFunctions.InterfaceFunctions.ChooseSection<Testing>();
        }

    }
}