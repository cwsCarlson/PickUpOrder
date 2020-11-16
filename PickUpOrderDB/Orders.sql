CREATE TABLE [dbo].[Orders]
(
    /* Order ID - A unique ID for this specific order. */
    [OrderID]          INT IDENTITY (1, 1) NOT NULL,

    /* OrderContents - The contents of the order as a comma-separated string
                       of item IDs. */
    [OrderContents]     VARCHAR(8000)       NULL,

    /* RawCost - The total cost before tax. */
    [RawCost]           INT                 NOT NULL,

    /* OrderStatus - An enum value flagging the order as recieved, done, etc.
                     Enums are not supported, so use:
                     NotSubmitted = 0, Received = 1, Preparing = 2, Done = 3, PickedUp = 4*/
    [OrderStatus]       INT                 NOT NULL,

    PRIMARY KEY CLUSTERED ([OrderID] ASC)
)