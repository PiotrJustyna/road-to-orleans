namespace Grains

open Interfaces
open Library
open Orleans

//type HelloWorld() =
//    inherit Grain()
//module HelloWorld =
//    let SayHello(name: string) =
//        {new IHelloWorld with
//        member this.SayHello(name: string) =
//            task { return Say.hello (name) } }

type HelloWorld() =
    inherit Grain()

    interface IHelloWorld with
        member this.SayHello(name: string) =
            task { return Say.hello (name) } 