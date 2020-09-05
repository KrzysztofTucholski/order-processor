namespace Parsing

open Thoth.Json.Net
open Domain

module Parser =
    let itemDecoder: Decoder<Order.Item list> =
        Decode.list
            (Decode.object (fun get ->
                { Name = get.Required.At [ "name" ] Decode.string
                  Amount = get.Required.At [ "amount" ] Decode.int }))

    let ordersDecoder: Decoder<Order.Unvalidated list> =
        Decode.list
            (Decode.object (fun get ->
                { Id = get.Required.At [ "id" ] Decode.string
                  ShippingAddress = get.Required.At [ "shipTo" ] Decode.string
                  Items = get.Required.At [ "items" ] itemDecoder }))

    let parse json = Decode.fromString ordersDecoder json

    