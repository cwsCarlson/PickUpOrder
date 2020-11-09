CREATE TABLE [dbo].[Orders]
(
    /* Order ID - A unique ID for this specific order. */
    [OrderID]           INT IDENTITY (1, 1) NOT NULL,

    /* AssociatedUserID - The ID of the user that placed the order.
                          It will be added once users are implemented.
    [AssociatedUserID]  INT                 NULL,*/

    /* NumContents - The number of items in the order. */
    [NumContents]       INT                 NOT NULL,

    /* OrderContents - The contents of the order as a comma-separated string
                       of item IDs. */
    [OrderContents]     VARCHAR(8000)       NULL,

    /* RawCost - The total cost before tax. */
    [RawCost]           INT                 NOT NULL,

    /* OrderStatus - An enum value flagging the order as recieved, done, etc.
                     Implement once status is ready.
    [OrderStatus]       INT                 NULL,*/
    PRIMARY KEY CLUSTERED ([OrderID] ASC)
)