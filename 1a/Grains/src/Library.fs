namespace Grains

open System.Threading.Tasks
open Orleans

type IDummyTests =
    inherit IGrainWithIntegerKey
    abstract member DummyTest1 : input: string -> cancellationToken: GrainCancellationToken -> Task<bool>
    abstract member DummyTest2 : input: string -> cancellationToken: GrainCancellationToken -> Task<bool>

type HelloWorld() =
    inherit Grain()

    interface IDummyTests with
        member this.DummyTest1 (input: string) (_: GrainCancellationToken) : Task<bool> = task { return true }
        member this.DummyTest2 (input: string) (_: GrainCancellationToken) : Task<bool> = task { return false }