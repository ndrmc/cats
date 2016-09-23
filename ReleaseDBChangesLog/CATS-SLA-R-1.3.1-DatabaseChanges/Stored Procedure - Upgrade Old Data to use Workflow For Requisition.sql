use [CatsDRMFSS];

/********************************************************************************************************
********************************************************************************************************
--	Author: Nathnael Getahun (Senior Software Developer @ Neuronet)
--	Type: Stored Procedure
--	Name: Upgrade Old Data
--	Description: Make old data of Relief Requisition table support the new workflow implementation
--	What it does:
----	> Add BusinessProcessID column in Relief Requisition table 
----	> Iterate through each Relief Requisition entry and create them a business process and 
----	  business process state for their current status only
********************************************************************************************************
********************************************************************************************************/


IF COL_LENGTH('EarlyWarning.ReliefRequisition','BusinessProcessID') IS NULL
BEGIN
	ALTER TABLE [EarlyWarning].[ReliefRequisition]
	ADD BusinessProcessID INT NOT NULL
	CONSTRAINT BP_Default_0 DEFAULT 0 WITH VALUES;
END

IF EXISTS(
    SELECT *
    FROM sys.procedures 
    WHERE Object_ID = Object_ID(N'upgrade_old_Requisition_data'))
BEGIN
    DROP PROCEDURE upgrade_old_Requisition_data  
END
GO
CREATE PROCEDURE upgrade_old_Requisition_data  @xmldoc	int        
AS  

/* Begin a transaction that will hold all the statements executed in this procedure */
BEGIN TRANSACTION 

/* Handle new workflow backward compatibility on old document data */
DECLARE @rowid int, @processid int;
--SET @documentname = (SELECT Name FROM OPENXML(@xmldoc, '//ProcessTemplate') WITH ProcessTemplate)
--SET @documnetname = SELECT REPLACE(@documentname, ' ', '');

SET @processid = (SELECT SettingValue FROM ApplicationSetting WHERE SettingName LIKE '%' + (SELECT SettingName FROM OPENXML(@xmldoc, '//ApplicationSetting') WITH ApplicationSetting) + '%')

/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)
	ROLLBACK /* Rollback of the transaction */

DECLARE document_table CURSOR FOR  
    SELECT RequisitionID FROM [EarlyWarning].[ReliefRequisition] 
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
			SET @status = ( SELECT [Status] FROM [EarlyWarning].[ReliefRequisition] WHERE RequisitionID = @rowid );
			IF (@status = 1)  
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Draft' );
			ELSE IF (@status = 2)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Approved' );
			ELSE IF (@status = 3)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Hub Assigned' );
			ELSE IF (@status = 4)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Project Code Assigned' );
			ELSE IF (@status = 5)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Transport Requisition Created' );
			ELSE IF (@status = 6)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Transport Order Created' );
			ELSE IF (@status = 7)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Rejected' );
			ELSE IF (@status = 8)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'SiPc Allocation Approved' );
			

			INSERT INTO BusinessProcessState 
			VALUES ( @processid, @stateid, 'System: Data Migration', '2016-06-06', 'Relief Requisition business process created by data migrator', NULL, NULL);
			SET @businessprocessstateid = SCOPE_IDENTITY();			

			INSERT INTO BusinessProcess 
			VALUES ( @processid, 0, 'ReliefRequisition', @businessprocessstateid, NULL);
			SET @businessprocessid = SCOPE_IDENTITY();

			UPDATE [EarlyWarning].[ReliefRequisition]
			SET BusinessProcessID=@businessprocessid
			WHERE RequisitionID = @rowid;
				
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
Go 

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="Relief Requisition" Description="Workflow for relief requisition" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="ReliefRequisitionWorkflow" SettingValue="" />

	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved" Actions="Approve" />
	<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Rejected,Hub Assigned" Actions="Reject,Assign Hub" /> 
	<StateTemplate Name="Rejected" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="Approved" Actions="Re-Approve" />
	<StateTemplate Name="Hub Assigned" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="Project Code Assigned" Actions="Assign Project Code" />
	<StateTemplate Name="Project Code Assigned" AllowedAccessLevel="0" StateNo="4" StateType="1" FinalStates="Approved,SiPc Allocation Approved" Actions="Uncommit,Approve SI/PC Allocation" />
	<StateTemplate Name="SiPc Allocation Approved" AllowedAccessLevel="0" StateNo="5" StateType="1" FinalStates="Transport Requisition Created" Actions="Create Transport Requisition" />
	<StateTemplate Name="Transport Requisition Created" AllowedAccessLevel="0" StateNo="6" StateType="1" FinalStates="Transport Order Created" Actions="Create Transport Order" />
	<StateTemplate Name="Transport Order Created" AllowedAccessLevel="0" StateNo="7" StateType="1" FinalStates="" Actions="" />   
</Data>'



EXECUTE upgrade_old_data @h
