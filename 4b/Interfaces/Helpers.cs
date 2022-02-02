using System.Runtime.CompilerServices;

//TODO: Find an appropriate directory for a helper class
namespace Interfaces
{
    public static class Helpers
    {
        public static string CallerName([CallerMemberName]string name = "")
        {
            return name;
        }
    }
}