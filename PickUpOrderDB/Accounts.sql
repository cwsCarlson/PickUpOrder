CREATE TABLE [dbo].[Accounts]
(
	/* UserID - A unique ID for this specific user. */
    [UserID]			INT IDENTITY (1, 1) NOT NULL,

	/* Email - The user's email address. */
	[Email]				VARCHAR(50)			NOT NULL,

	/* PasswordHash - The SHA-256 hash of the user's password. */
	[PasswordHash]		VARCHAR(44)		NOT NULL,

	/* Type - A value (in the AccountType enum) flagging the user type.
              Customer = 1, Employee = 2, Manager = 3 */
    [Type]				INT                 NOT NULL,

	/* Orders - The orders this user placed as a comma-separated string
                of order IDs. */
	[Orders]			VARCHAR(8000)		NULL,

	PRIMARY KEY CLUSTERED ([UserID] ASC)
)