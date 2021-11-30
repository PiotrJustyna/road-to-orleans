using System;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IHelloWorld : Orleans.IGrainWithIntegerKey
    {
        Task<Int64> Factorial(int n);
    }
}
