namespace Interfaces

open System.Threading.Tasks
open Orleans

type IHelloWorld =
    inherit IGrainWithIntegerKey
    abstract member SayHello : name: string -> Task<string>
