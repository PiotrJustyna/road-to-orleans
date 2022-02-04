using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using NLua;

namespace Grains
{
    public class HelloWorld : Orleans.Grain, IHelloWorld
    {
        public Task<Int64> Factorial(int n)
        {
            var luaString = File.ReadAllText("HelloWorld.lua");
            Int64 result;

            using (var lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoString(luaString);

                var factorialFunction = lua["factorial"] as LuaFunction;
                result = (Int64)factorialFunction.Call(n).First();
            }

            return Task.FromResult(result);
        }
    }
}