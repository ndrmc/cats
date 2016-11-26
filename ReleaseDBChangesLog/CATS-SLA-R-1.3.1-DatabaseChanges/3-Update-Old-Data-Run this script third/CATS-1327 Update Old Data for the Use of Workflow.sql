
 BEGIN TRANSACTION
 declare @ProcessTemplateID  int;
IF  EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'TransferReceiptPlan') 
BEGIN
Select @ProcessTemplateID =  ProcessTemplateID  FROM [dbo].[ProcessTemplate]
where [Name] = 'TransferReceiptPlan'

DECLARE @rowid int, @currentStateID int;

--SET @ProcessTemplateID = (SELECT SettingValue FROM ApplicationSetting WHERE SettingName LIKE '%' + (SELECT SettingName FROM OPENXML(@xmldoc, '//ApplicationSetting') WITH ApplicationSetting) + '%')

IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */
DECLARE document_table CURSOR FOR 
SELECT TransferId FROM Transfer
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */
/* Open the above mapped cursor */
OPEN document_table 
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */
/* Iterate through the above cursor by fetching each row one by one and mapping the row entry to @rowid */
FETCH NEXT FROM document_table INTO @rowid 
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */
/* Check if the current status of the fetch routine is different from -1 and continue fetching */
WHILE (@@FETCH_STATUS <> -1) 
BEGIN 
IF (@@FETCH_STATUS <> -2) 
BEGIN 

DECLARE @status int, @businessprocessid int, @businessprocessstateid int, @stateid int;
SET @status = ( SELECT [StatusID] FROM Transfer WHERE TransferID  = @rowid );
IF (@status = 1) 
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @ProcessTemplateID AND Name = 'Draft' );
ELSE IF (@status = 2)
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @ProcessTemplateID AND Name = 'Approved' );
ELSE IF (@status = 5)
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @ProcessTemplateID AND Name = 'Rejected' );
ELSE
SET IDENTITY_INSERT BusinessProcessState ON;
INSERT INTO BusinessProcessState 
VALUES ( @ProcessTemplateID, @stateid, 'System: Data Migration', '2016-06-06', ' Transfer business process created by data migrator', NULL, NULL) select @currentStateID = Scope_Identity();
--SET IDENTITY_INSERT BusinessProcessState OFF;
--SET @businessprocessstateid = SCOPE_IDENTITY();
--SET IDENTITY_INSERT BusinessProcess ON;
INSERT INTO BusinessProcess ([ProcessTypeID]
           ,[DocumentID]
           ,[DocumentType]
           ,[CurrentStateID])
VALUES ( @ProcessTemplateID, 0, 'TransferReceiptPlan', @currentStateID);
SET IDENTITY_INSERT BusinessProcess OFF;
SET @businessprocessid = SCOPE_IDENTITY();
UPDATE BusinessProcessState
SET [ParentBusinessProcessID]= @BusinessProcessID
WHERE [BusinessProcessStateID] = @currentStateID;
UPDATE Transfer
SET BusinessProcessID=@businessprocessid
WHERE TransferID = @rowid;

--END 
END 
FETCH NEXT FROM document_table INTO @rowid 
END 
/* Close the above mapped cursor */
CLOSE document_table 
/* Deallocate the above mapped cursor */
DEALLOCATE document_table
/* The end of the procedure transaction */
COMMIT
End
Go