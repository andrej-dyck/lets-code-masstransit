# Let's Code MassTransit

- [MassTransit](https://masstransit.io/)
- [Documentation](https://masstransit.io/documentation/concepts)

## Tasks

- An `Invoices` feature that issues invoices when new `Orders` are placed
- Make sure saving an order and `OrderPlaced` events are transactional (
  use [Outbox Pattern](https://masstransit.io/documentation/patterns/in-memory-outbox))
- Prepare `Emails` for `Orders` and `Invoices` to be send (messages should be light-weight;
  use [Claim Check Pattern](https://masstransit.io/documentation/patterns/claim-check))
- Configure a message broker (e.g., [RabbitMQ](https://masstransit.io/documentation/transports/rabbitmq)) for production

## Optional Tasks

- A `Revenues` feature that keeps track of total sale revenues
- A `Sales` feature that has ownership of SKU prices
- Write unit tests for Consumers using MassTransit's [Test Harness](https://masstransit.io/documentation/concepts/testing)

## Advanced Task

- Use the [Saga Pattern](https://masstransit.io/documentation/patterns/saga) to orchestrate the process from an order
  placed, invoice issued, to payment and shipping.
- Orders might be canceled

## Development

Run with development profile:

```shell
dotnet run --launch-profile dev-http --project WebShop.HttpApi
```

