﻿using System.Threading.Tasks;
using Interfaces;
using Orleans;
using Library;

namespace Grains
{
    public class HelloWorld : Orleans.Grain, IHelloWorld
    {
        public Task<string> SayHello(string name, GrainCancellationToken grainCancellationToken)
        {
            if (!grainCancellationToken.CancellationToken.IsCancellationRequested)
            {
                return Task.FromResult(Say.hello(name));
            }

            return null;
        }
    }
}