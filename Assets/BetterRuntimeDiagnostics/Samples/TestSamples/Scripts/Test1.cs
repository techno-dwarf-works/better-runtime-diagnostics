using Better.Diagnostics.Runtime.CommandConsoleModule;

namespace Samples.TestSamples.Scripts
{
    public class Test1
    {
        [ConsoleCommand("run")]
        private static int Test(int value)
        {
            return value * value;
        }
    }
}