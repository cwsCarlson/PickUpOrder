/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

MERGE INTO Orders AS Target 
USING (VALUES (1, 0, null, 0))
AS Source (OrderID, NumContents, OrderContents, RawCost) 
ON Target.OrderID = Source.OrderID 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (NumContents, OrderContents, RawCost) VALUES (NumContents, OrderContents, RawCost);