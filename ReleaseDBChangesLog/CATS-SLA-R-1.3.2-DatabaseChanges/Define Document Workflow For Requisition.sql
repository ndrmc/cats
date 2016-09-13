/********************************************************************************************************
********************************************************************************************************
--	Author: Nathnael Getahun (Senior Software Developer @ Neuronet)
--	Type: Stored Procedure
--	Name: Define Document Workflow
--	Description: A Generic Document Workflow Definition Procedure
--	What it does:
--	> Reads a formally formatted XML document 
--	> Inserts a process template
--	> Uses the above inserted process id to insert application setting
--	> Inserts all the states of the document process defined in the xml document
--	> Iterate through each process states trigger a flow template insert query
********************************************************************************************************
********************************************************************************************************/
DROP PROCEDURE define_document_workflow  
GO  
CREATE PROCEDURE define_document_workflow  @xmldoc	int        
AS  

/* Begin a transaction that will hold all the statements executed in this procedure */
BEGIN TRANSACTION 

/* Define the variable that holds the insert_id for the process template */
DECLARE @ProcessTemplateID INT;
/* Define the variables that hold the name, the final states, and actions defined for each states in the xml document */
DECLARE @name varchar(50), @finalstates varchar(50), @actions varchar(50)

/* Insert a new process template into the 'ProcessTemplate' table from the <ProcessTemplate /> definition in the xml document */
INSERT INTO ProcessTemplate SELECT Name, [Description], GraphicsData, PartitionId FROM OPENXML(@xmldoc, '//ProcessTemplate') WITH ProcessTemplate
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)  
	ROLLBACK /* Rollback of the transaction */
/* Set the insert_id to the above defined variable for later reference */
SET @ProcessTemplateID = SCOPE_IDENTITY();

/* Insert a new application setting into the 'ApplicationSetting' table from the <ApplicationSetting /> definition in the xml document */
INSERT INTO ApplicationSetting 
VALUES ( (SELECT SettingName FROM OPENXML(@xmldoc, '//ApplicationSetting') WITH ApplicationSetting), @ProcessTemplateID, NULL);
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)  
	ROLLBACK /* Rollback of the transaction */

/* Insert new state templates into the 'StateTemplate' table from the list of <StateTemplate /> definitions in the xml document */
INSERT INTO StateTemplate 
SELECT @ProcessTemplateID, Name, AllowedAccessLevel, StateNo, StateType, NULL FROM OPENXML(@xmldoc, '//StateTemplate') WITH StateTemplate
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)  
	ROLLBACK /* Rollback of the transaction */
	  
/* Temporary Edge table: that parsesthe xml document into a table like structure */  
SELECT *   
INTO #TempEdge   
FROM OPENXML(@xmldoc, '//StateTemplate')  
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)  
	ROLLBACK /* Rollback of the transaction */

/* Define a cursor that maps the aforementioned structure into table containing rows of states name, final states, and actions */
DECLARE fillfinalstates_cursor CURSOR FOR  
    SELECT CAST(iv.text AS nvarchar(200)) AS name,  
           CAST(av.text AS nvarchar(4000)) AS finalstates,  
				CAST(bv.text AS nvarchar(4000)) AS actions
    FROM   #TempEdge c, #TempEdge i,  
           #TempEdge iv, #TempEdge a, #TempEdge av,
		   #TempEdge b, #TempEdge bv  
    WHERE  c.id = i.parentid  
    AND    UPPER(i.localname) = UPPER('Name')  
    AND    i.id = iv.parentid  
    AND    c.id = a.parentid  
    AND    UPPER(a.localname) = UPPER('FinalStates')  
    AND    a.id = av.parentid
	AND    c.id = b.parentid  
    AND    UPPER(b.localname) = UPPER('Actions')  
    AND    b.id = bv.parentid
 /* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)  
	ROLLBACK /* Rollback of the transaction */

 /* Open the above mapped cursor */
OPEN fillfinalstates_cursor  
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)  
	ROLLBACK /* Rollback of the transaction */

/* Iterate through the above cursor by fetching each row one by one and mapping the row entries to @name, @finalstates, and @actions variables */
FETCH NEXT FROM fillfinalstates_cursor INTO @name, @finalstates, @actions  
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)  
	ROLLBACK /* Rollback of the transaction */

/* Check if the current status of the fetch routine is different from -1 and continue fetching */
WHILE (@@FETCH_STATUS <> -1)  
BEGIN  
    IF (@@FETCH_STATUS <> -2)  
    BEGIN  
		/* Trigged the insert_flow_templates procedure for each entries fetched from the cursor */
		DECLARE @sp int  
		DECLARE @att varchar(500)  
		DECLARE @sp1 int  
		DECLARE @att1 varchar(500)
		SET @sp = 0 
		SET @sp1 = 0  
		WHILE (LEN(@finalstates) > 1)  
		BEGIN   
			SET @sp = CHARINDEX(',', @finalstates+ ',')  
			SET @att = LEFT(@finalstates, @sp-1)
			SET @sp1 = CHARINDEX(',', @actions+ ',')  
			SET @att1 = LEFT(@actions, @sp1-1)  
			EXEC('INSERT INTO FlowTemplate VALUES ('+@ProcessTemplateID+', (SELECT StateTemplateID FROM StateTemplate WHERE ParentProcessTemplateID = '+@ProcessTemplateID 
				+' AND Name = '''+ @name +'''), (SELECT StateTemplateID FROM StateTemplate WHERE ParentProcessTemplateID = '+@ProcessTemplateID 
				+' AND Name = '''+ @att +'''), '''+ @att1+''', NULL)')
			/* Check if the above statement failed to execute and rollback the transcation */
			IF(@@error <> 0)  
				ROLLBACK /* Rollback of the transaction */
			SET @finalstates = SUBSTRING(@finalstates+ ',', @sp+1, LEN(@finalstates)+1-@sp) 
			SET @actions = SUBSTRING(@actions+ ',', @sp1+1, LEN(@actions)+1-@sp1) 
		END  
    END  
    FETCH NEXT FROM fillfinalstates_cursor INTO @name, @finalstates, @actions  
END  
/* Close the above mapped cursor */
CLOSE fillfinalstates_cursor  
/* Deallocate the above mapped cursor */
DEALLOCATE fillfinalstates_cursor  

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

EXECUTE define_document_workflow @h

