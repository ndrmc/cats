DROP PROCEDURE upgrade_old_data
GO
CREATE PROCEDURE upgrade_old_data @xmldoc int
AS

/* Begin a transaction that will hold all the statements executed in this procedure */
BEGIN TRANSACTION

/* Handle new workflow backward compatibility on old document data */
DECLARE @rowid int, @processid int;
--SET @documentname = (SELECT Name FROM OPENXML(@xmldoc, '//ProcessTemplate') WITH ProcessTemplate)
--SET @documnetname = SELECT REPLACE(@documentname, ' ', '');

SET @processid = (SELECT SettingValue 
FROM ApplicationSetting WHERE SettingName LIKE '%' + 
(SELECT SettingName FROM OPENXML(@xmldoc, '//ApplicationSetting') WITH ApplicationSetting) + '%')

--ALTER TABLE [EarlyWarning].[ReliefRequisition]
--ADD BusinessProcessID INT NULL;
--CONSTRAINT BP_Default_0 DEFAULT 0 WITH VALUES
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */

DECLARE document_table CURSOR FOR
SELECT TransferID FROM [dbo].[Transfer] where CommoditySourceID = 9  -- it is swap
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
/* Trigged the insert_flow_templates procedure for each entries fetched from the cursor */
--DECLARE @bpid int;
--SET @bpid = ( SELECT BusinessProcessID FROM [EarlyWarning].[ReliefRequisition] WHERE RequisitionID = @rowid );
--IF (@bpid = 0)
--BEGIN
DECLARE @status int, @businessprocessid int, @businessprocessstateid int, @stateid int;
SET @status = ( SELECT [StatusID] FROM [dbo].[Transfer] WHERE TransferID = @rowid );
IF (@status = 1)
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Draft' );
ELSE IF (@status = 2)
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Approved' );

ELSE

SET IDENTITY_INSERT BusinessProcessState ON;
INSERT INTO BusinessProcessState
VALUES ( @processid, @stateid, 'System: Data Migration', '2016-06-06', 'Swap business process created by data migrator', NULL,NULL);
SET IDENTITY_INSERT BusinessProcessState OFF;
SET @businessprocessstateid = SCOPE_IDENTITY();

INSERT INTO BusinessProcess
VALUES ( @processid, 0, 'Swap', @businessprocessstateid, NULL);
SET @businessprocessid = SCOPE_IDENTITY();

UPDATE dbo.BusinessProcessState
SET   ParentBusinessProcessID = @businessprocessid
Where BusinessProcessStateID = @businessprocessstateid

UPDATE [dbo].[Transfer]
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
GO

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
<ProcessTemplate Name="Swap" Description="Transfer Plan Processing" GraphicsData="NULL" PartitionId="0" />
<ApplicationSetting SettingName="SwapWorkflow" SettingValue="" />

<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved" Actions="Approve" />
<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="" Actions="" /> 
</Data>'

EXECUTE upgrade_old_data @h