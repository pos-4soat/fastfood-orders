namespace fastfood_orders.Domain.Enum
{
    public enum OrderStatus
    {
        AwaitingPayment = 1,
        Received = 2,
        InPreparation = 3,
        Ready = 4,
        Finished = 5,
        Canceled = 6
    }
}
