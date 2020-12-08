namespace Library

module Say =
    let hello name =
        sprintf "Hello, \"%s\"! Your name is %i characters long." name (String.length name)
