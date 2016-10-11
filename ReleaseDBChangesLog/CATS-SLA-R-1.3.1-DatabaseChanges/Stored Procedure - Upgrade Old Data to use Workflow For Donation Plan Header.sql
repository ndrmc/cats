use [CatsDRMFSS];

/********************************************************************************************************
********************************************************************************************************
--	Author: Nathnael Getahun (Senior Software Developer @ Neuronet)
--	Type: Stored Procedure
--	Name: Upgrade Old Data
--	Description: Make old data of Donation Plan Header table support the new workflow implementation
--	What it does:
----	> Add BusinessProcessID column in Donation Plan Header table 
----	> Iterate through each Donation Plan Header entry and create them a business process and 
----	  business process state for their current status only
********************************************************************************************************
********************************************************************************************************/


IF COL_LENGTH('dbo.DonationPlanHeader','BusinessProcessID') IS NULL
BEGIN
	ALTER TABLE [dbo].[DonationPlanHeader]
	ADD BusinessProcessID INT NOT NULL
	CONSTRAINT BP_Default_2 DEFAULT 0 WITH VALUES;
END

IF EXISTS(
    SELECT *
    FROM sys.procedures 
    WHERE Object_ID = Object_ID(N'upgrade_donation_plan_header_old_data'))
BEGIN
    DROP PROCEDURE upgrade_donation_plan_header_old_data  
END
GO
CREATE PROCEDURE upgrade_donation_plan_header_old_data  @xmldoc	int        
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
    SELECT DonationHeaderPlanID FROM [dbo].[DonationPlanHeader] 
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
			SET @status = ( SELECT [Status] FROM [dbo].[DonationPlanHeader] WHERE DonationHeaderPlanID = @rowid );
			IF (@status = 1 OR @status IS NULL)  
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Draft' );
			ELSE IF (@status = 2)
				SET @stateid = ( SELECT [StateTemplateID] FROM [StateTemplate] WHERE ParentProcessTemplateID = @processid AND Name = 'Sent to hub' );


			INSERT INTO BusinessProcessState 
			VALUES ( @processid, @stateid, 'System: Data Migration', '2016-06-06', 'Donation Plan Header business process created by data migrator', NULL);
			SET @businessprocessstateid = SCOPE_IDENTITY();			

			INSERT INTO BusinessProcess 
			VALUES ( @processid, 0, 'DonationPlanHeader', @businessprocessstateid, NULL);
			SET @businessprocessid = SCOPE_IDENTITY();

			UPDATE [dbo].[BusinessProcessState]
			SET ParentBusinessProcessID=@businessprocessid
			WHERE BusinessProcessStateID = @businessprocessstateid;

			UPDATE [dbo].[DonationPlanHeader]
			SET BusinessProcessID=@businessprocessid
			WHERE DonationHeaderPlanID = @rowid;
				
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
	<ProcessTemplate Name="Donation Plan Header" Description="Workflow for Donation Plan Header" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="DonationPlanHeaderWorkflow" SettingValue="" />
</Data>'



EXECUTE upgrade_donation_plan_header_old_data @h
