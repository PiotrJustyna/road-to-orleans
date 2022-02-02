namespace Interfaces

open System.Threading.Tasks
open Orleans

type IHelloWorld =
    inherit IGrainWithIntegerKey
    abstract member SayHello : name: string -> cancellationToken: GrainCancellationToken -> Task<string>
