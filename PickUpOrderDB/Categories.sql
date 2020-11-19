CREATE TABLE [dbo].[Categories]
(
	/* CategoryID - A unique ID for this specific category. */
    [CategoryID]	INT IDENTITY (1, 1) NOT NULL,

	/* CategoryName - The category name. */
	[CategoryName]	VARCHAR(50)			NOT NULL,

	PRIMARY KEY CLUSTERED ([CategoryID] ASC)
)
