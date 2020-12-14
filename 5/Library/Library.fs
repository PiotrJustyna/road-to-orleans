namespace Library

module Say =
    let hello name =
        let _input = 
            match (System.String.IsNullOrWhiteSpace name) with
            |true -> invalidArg "name" "string cannot be null or whitespace"
            |false -> name
        sprintf "Hello, \"%s\"! Your name is %i characters long." _input (String.length _input)


        
