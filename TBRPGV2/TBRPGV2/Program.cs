namespace TBRPGV2
{
    internal class Program
    {
        static generalClass genClass = new generalClass();
        static void Main(string[] args)
        {
            Console.Title = "TBRPG";
            genClass.Roll();
            Console.ReadKey();
            
        }
    }
}