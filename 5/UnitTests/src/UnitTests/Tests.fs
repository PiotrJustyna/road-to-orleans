module UnitTests.Tests

open Xunit
open Library
let func y = fun () -> Say.hello y

[<Fact>]
let ``Library Valid String Input Test`` () =
    let result = Say.hello("Mereta")
    Assert.Equal("Hello, \"Mereta\"! Your name is 6 characters long.", result)

[<Fact>]
let ``Library Empty String Input Throws Exception`` () =
    Assert.Throws<System.ArgumentException>((fun () -> Say.hello "" |> ignore))

[<Fact>]
let ``Library Whitespace String Input Throws Exception`` () =
    Assert.Throws<System.ArgumentException>((fun () -> Say.hello "\t" |> ignore))


[<Fact>]
let ``Library Exception Messgage Test`` () = 
    let ex = Assert.Throws<System.ArgumentException>((fun () -> Say.hello "" |> ignore));
    Assert.Equal("string cannot be null or whitespace (Parameter 'name')" , ex.Message);
    Assert.Same(typeof<System.ArgumentException>, ex.GetType())


