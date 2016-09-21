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
SELECT hrdID FROM [dbo].[HRD] 
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
SET @status = ( SELECT [Status] FROM [dbo].[HRD] WHERE HRDID = @rowid );
IF (@status = 1)
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Draft' );
ELSE IF (@status = 2)
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Approved' );
ELSE IF (@status = 3)
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Published' );
ELSE IF (@status = 4)
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Closed' );
ELSE IF (@status = -1)
SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Deleted' );

ELSE

SET IDENTITY_INSERT BusinessProcessState ON;
INSERT INTO BusinessProcessState
VALUES ( @processid, @stateid, 'System: Data Migration', '2016-06-06', 'HRD business process created by data migrator', NULL);
SET IDENTITY_INSERT BusinessProcessState OFF;
SET @businessprocessstateid = SCOPE_IDENTITY();

INSERT INTO BusinessProcess
VALUES ( @processid, 0, 'HRD', @businessprocessstateid, NULL);
SET @businessprocessid = SCOPE_IDENTITY();

UPDATE [dbo].[HRD]
SET BusinessProcessId=@businessprocessid
WHERE HRDID = @rowid;

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
<ProcessTemplate Name="HRD" Description="HRD Processing" GraphicsData="NULL" PartitionId="0" />
<ApplicationSetting SettingName="HRDWorkflow" SettingValue="" />

<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved,Deleted" Actions="Approve,Delete" />
<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Published" Actions="Publish" /> 
<StateTemplate Name="Published" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="Closed" Actions="Close" />
<StateTemplate Name="Closed" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="" Actions="" />
<StateTemplate Name="Deleted" AllowedAccessLevel="0" StateNo="4" StateType="1" FinalStates="" Actions="" /> 
</Data>'

EXECUTE upgrade_old_data @h