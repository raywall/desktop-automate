using System;

namespace DesktopAutomateConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var automate = new DesktopCalculatorAutomate();

            automate.OnError += ((Exception ex) => {
                Console.WriteLine(ex.ToString());
            });
            
            try
            {
                automate.OpenApplication($"{Environment.CurrentDirectory}\\Source\\calc.exe");
                automate.PressButton(@"5+3-6/2=");

                Console.ReadLine();
                automate.CloseApplication();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
