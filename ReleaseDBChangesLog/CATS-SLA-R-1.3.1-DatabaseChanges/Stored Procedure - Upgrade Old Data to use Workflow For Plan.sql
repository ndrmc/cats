use [CatsDRMFSS];

/********************************************************************************************************
********************************************************************************************************
--	Author: Nathnael Getahun (Senior Software Developer @ Neuronet)
--	Type: Stored Procedure
--	Name: Upgrade Old Data
--	Description: Make old data of Needs Assessment Plan table support the new workflow implementation
--	What it does:
----	> Add BusinessProcessID column in Needs Assessment Plan table 
----	> Iterate through each Needs Assessment Plan entry and create them a business process and 
----	  business process state for their current status only
********************************************************************************************************
********************************************************************************************************/


IF COL_LENGTH('dbo.Plan','BusinessProcessID') IS NULL
BEGIN
	ALTER TABLE [dbo].[Plan]
	ADD BusinessProcessID INT NOT NULL
	CONSTRAINT BP_Default_0 DEFAULT 0 WITH VALUES;
END

IF EXISTS(
    SELECT *
    FROM sys.procedures 
    WHERE Object_ID = Object_ID(N'upgrade_needs_assessment_plan_old_data'))
BEGIN
    DROP PROCEDURE upgrade_needs_assessment_plan_old_data  
END
GO
CREATE PROCEDURE upgrade_needs_assessment_plan_old_data  @xmldoc	int        
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
    SELECT PlanID FROM [dbo].[Plan] 
	WHERE PlanID IN (SELECT PlanID FROM [Earlywarning].[NeedAssessment])
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
		--SET @bpid = ( SELECT BusinessProcessID FROM [Logistics].[TransportRequisition] WHERE RequisitionID = @rowid );
		--IF (@bpid = 0)  
		--BEGIN
			DECLARE @status int, @businessprocessid int, @businessprocessstateid int, @stateid int;
			SET @status = ( SELECT [Status] FROM [dbo].[Plan] WHERE PlanID = @rowid );
			IF (@status = 1)  
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Draft' );
			ELSE IF (@status = 2)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Approved' );
			ELSE IF (@status = 3)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'AssessmentCreated' );
			ELSE IF (@status = 4)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'HRDCreated' );
			ELSE IF (@status = 5)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'PSNPCreated' );
			ELSE IF (@status = 6)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Closed' );


			INSERT INTO BusinessProcessState 
			VALUES ( @processid, @stateid, 'System: Data Migration', '2016-06-06', 'Plan business process created by data migrator', NULL);
			SET @businessprocessstateid = SCOPE_IDENTITY();			

			INSERT INTO BusinessProcess 
			VALUES ( @processid, 0, 'Plan', @businessprocessstateid, NULL);
			SET @businessprocessid = SCOPE_IDENTITY();

			UPDATE [dbo].[Plan]
			SET BusinessProcessID=@businessprocessid
			WHERE PlanID = @rowid;
				
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
	<ProcessTemplate Name="Needs Assessment Plan" Description="Workflow for plan of needs assessment" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="PlanWorkflow" SettingValue="" />
</Data>'



EXECUTE upgrade_needs_assessment_plan_old_data @h
