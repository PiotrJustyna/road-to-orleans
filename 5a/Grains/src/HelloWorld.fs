namespace Grains

open System.Threading.Tasks
open Interfaces
open Library
open Orleans

type HelloWorld() =
    inherit Grain()

    interface IHelloWorld with
        member this.SayHello (name: string) (_: GrainCancellationToken) : Task<string> =
            task { return Say.hello (name) }
