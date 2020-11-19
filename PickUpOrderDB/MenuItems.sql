CREATE TABLE [dbo].[MenuItems]
(
	/* ItemID - A unique ID for this specific menu item. */
    [ItemID]		INT IDENTITY (1, 1) NOT NULL,

	/* Name - The item name. It is kept even if the item was deleted. */
	[Name]			VARCHAR(50)			NOT NULL,

	/* Description - A string of text that describes the item. */
	[Description]	VARCHAR(240)		NULL,

	/* Price - The item's cost in cents.
			   If null, this means that the item has been removed. */
	[Price]			INT					NULL,

	/* Category - An integer value representing the corresponding category. */
	[Category]		INT					NULL,

	PRIMARY KEY CLUSTERED ([ItemID] ASC)
)
