USE CatsDRMFSS

DROP PROCEDURE AddParentHubIDToHub 
GO  
CREATE PROCEDURE AddParentHubIDToHub       
AS 

ALTER TABLE [dbo].[Hub]
ADD HubParentID int 
GO

DECLARE @AdamaHubInsertID INT;
DECLARE @KombolchaHubInsertID INT;
DECLARE @DiredawaHubInsertID INT;
DECLARE @MekeleHubInsertID INT;
DECLARE @AddisAbabaHubInsertID INT;

SET @AdamaHubInsertID = (SELECT HubID FROM Hub WHERE Name = 'Adama');
SET @KombolchaHubInsertID = (SELECT HubID FROM Hub WHERE Name = 'Kombolcha');
SET @DiredawaHubInsertID = (SELECT HubID FROM Hub WHERE Name = 'Diredawa');
SET @MekeleHubInsertID = (SELECT HubID FROM Hub WHERE Name = 'Mekele');
SET @AddisAbabaHubInsertID = (SELECT HubID FROM Hub WHERE Name = 'Addis Ababa');


UPDATE [dbo].[Hub]
	SET HubParentID=@AdamaHubInsertID
	WHERE HubID = @AdamaHubInsertID;


UPDATE [dbo].[Hub]
	SET HubParentID=@KombolchaHubInsertID
	WHERE HubID = @KombolchaHubInsertID;

UPDATE [dbo].[Hub]
	SET HubParentID=@DiredawaHubInsertID
	WHERE HubID = @DiredawaHubInsertID;

UPDATE [dbo].[Hub]
	SET HubParentID=@MekeleHubInsertID
	WHERE HubID = @MekeleHubInsertID;

UPDATE [dbo].[Hub]
	SET HubParentID=@AddisAbabaHubInsertID
	WHERE HubID = @AddisAbabaHubInsertID;

UPDATE [dbo].[Hub]
	SET HubParentID=@AddisAbabaHubInsertID
	WHERE Name = 'Adama (EFSRA)';

UPDATE [dbo].[Hub]
	SET HubParentID=@DiredawaHubInsertID
	WHERE Name = 'Diredawa  (EFSRA)';

UPDATE [dbo].[Hub]
	SET HubParentID=@AdamaHubInsertID
	WHERE Name = 'Nazreth WFP SO';

UPDATE [dbo].[Hub]
	SET HubParentID=@KombolchaHubInsertID
	WHERE Name = 'Kombolcha WFP SO';

UPDATE [dbo].[Hub]
	SET HubParentID=@KombolchaHubInsertID
	WHERE Name = 'Kombolcha  (EFSRA)';

UPDATE [dbo].[Hub]
	SET HubParentID=@AdamaHubInsertID
	WHERE Name = 'Welaita Sodo';

UPDATE [dbo].[Hub]
	SET HubParentID=@AdamaHubInsertID
	WHERE Name = 'Shashemene';

UPDATE [dbo].[Hub]
	SET HubParentID=@DiredawaHubInsertID
	WHERE Name = 'Shenile Diredawa';

UPDATE [dbo].[Hub]
	SET HubParentID=@KombolchaHubInsertID
	WHERE Name = 'Wereta Gondar';
	
UPDATE [dbo].[Hub]
	SET HubParentID=@DiredawaHubInsertID
	WHERE Name = 'jijiga';

UPDATE [dbo].[Hub]
	SET HubParentID=@DiredawaHubInsertID
	WHERE Name = 'jijiga';

UPDATE [dbo].[Hub]
	SET HubParentID=@MekeleHubInsertID
	WHERE Name = 'Mekele WFP SO';


Go


DROP PROCEDURE DropColumnParentHubIDToHub 
GO  
CREATE PROCEDURE DropColumnParentHubIDToHub       
AS 

ALTER TABLE [dbo].[Hub]
DROP COLUMN HubParentID

GO


