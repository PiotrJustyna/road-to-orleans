module UnitTests.Tests

open Xunit
open Library
open Interfaces
open System.Threading.Tasks
open Foq
open Client

[<Fact>]
let ``Library Valid String Input Test`` () =
    let result = Say.hello("Mereta")
    Assert.Equal("Hello, \"Mereta\"! Your name is 6 characters long.", result)

[<Fact>]
let ``Library Empty String Input Throws Exception`` () =
    let methodCall = (fun () -> Say.hello "" |> ignore)
    Assert.Throws<System.ArgumentException>(methodCall)


[<Fact>]
let ``Library Exception Messgage Test`` () = 
    let methodCall = (fun () -> Say.hello "" |> ignore)
    let ex = Assert.Throws<System.ArgumentException>(methodCall);
    Assert.Equal("string cannot be null or whitespace (Parameter 'name')" , ex.Message);


[<Fact>]
let ``Grains Service Test Returns Correct Output`` () =
    let _grainService = {
        new IHelloWorld with 
            member _this.SayHello(input) = 
                match input with 
                    "Mereta" -> Task.FromResult("Hello, \"Mereta\"! Your name is 6 characters long.") }
    let input, output = "Mereta",  Task.FromResult("Hello, \"Mereta\"! Your name is 6 characters long.")
    let result = _grainService.SayHello(input)
    Assert.Equal(output.Result, result.Result)


[<Fact>]
let ``Hello World Client Returns Task`` () =
    let output = "Hello, \"Mereta\"! Your name is 6 characters long."
    let _clientService = Mock<HelloWorldClientHostedService>().Setup(fun me -> <@ me.StartAsync(any()) @> ).Returns(Task.CompletedTask).Create()
    let xx = _clientService.StartAsync(any())
    Assert.Equal(typeof<Task>, xx.GetType().BaseType)


