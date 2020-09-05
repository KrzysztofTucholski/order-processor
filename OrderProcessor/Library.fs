namespace OrderProcessor

open Domain
open Parsing
open Utils

module OrderService =
    let validate: Order.Validate =
        fun o ->
            try
                Ok
                    { Id = o.Id |> Validation.OrderId.create
                      ShippingAddress = o.ShippingAddress
                      Items = o.Items |> Validation.ToNonEmptyList }

            with Failure r -> Error { Id = o.Id; Reason = r }

    let price: Order.Price =
        fun o ->
            if o.Id |> Validation.OrderId.value = "4" then
                Ok
                    { Id = o.Id
                      ShippingAddress = o.ShippingAddress
                      Items = o.Items
                      Price = 1 }
            else
                Error
                    { Id = o.Id
                      Reason = "Pricing random error" }

    let priceOrderAdapted input =
        input
        |> price
        |> Result.mapError Order.PlaceOrderError.Pricing

    let validateOrderAdapted input =
        input
        |> validate
        |> Result.mapError Order.PlaceOrderError.Validation

    let printResult (r: Result<Order.Priced, Order.PlaceOrderError>): unit =
        match r with
        | Ok o -> printfn "Order priced %s" (o.Id |> Validation.OrderId.value)
        | Error e ->
            match e with
            | Order.PlaceOrderError.Pricing pe -> printfn "pricing error %s" pe.Reason
            | Order.PlaceOrderError.Validation ve -> printfn "validation error %s" ve.Reason


    let processOrder (o: Order.Unvalidated) =
        o
        |> validateOrderAdapted
        |> Result.bind priceOrderAdapted
        |> printResult

    let main json =
        let parsed = json |> Parser.parse

        match parsed with
        | Ok list -> list |> List.iter (processOrder)
        | Error e -> printfn "JSON error %s" e

        0
