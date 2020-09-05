namespace Domain

open Utils

module Order =
    type Item = { Name: string; Amount: int }

    type Unvalidated =
        { Id: string
          ShippingAddress: string
          Items: Item list }

    type Validated =
        { Id: Validation.OrderId
          ShippingAddress: string
          Items: Item list }

    type Priced =
        { Id: Validation.OrderId
          ShippingAddress: string
          Items: Item list
          Price: int }

    type Order =
        | UnvalidatedOrder of Unvalidated
        | ValidatedOrder of Validated
        | PricedOrder of Priced

    type ValidationError = { Id: string; Reason: string }
    type PricingError = { Id: Validation.OrderId; Reason: string }

    type PlaceOrderError = 
        | Validation of ValidationError
        | Pricing of PricingError

    type Validate = Unvalidated -> Result<Validated, ValidationError>
    type Price = Validated -> Result<Priced, PricingError>

