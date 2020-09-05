namespace Utils

module Result =

    let bind switchFn =
        fun twoTrackInput ->
            match twoTrackInput with
            | Ok success -> switchFn success
            | Error failure -> Error failure

    let map f aResult =
        match aResult with
        | Ok success -> Ok(f success)
        | Error failure -> Error failure

module Validation =

    let ToNonEmptyList l =
        match l with
        | [] -> failwith "Item list must not be empty"
        | _ -> l

    type OrderId = private OrderId of string

    module OrderId =
        let create str =
            if str.Equals("1") then failwith "Id must not be one" else OrderId str

        let value (OrderId str) = str
