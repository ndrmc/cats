
GO
/****** Object:  StoredProcedure [dbo].[define_document_action_workflow]    Script Date: 11/11/2016 1:08:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[update_action_workflow]  @xmldoc	int        
AS  

/* Begin a transaction that will hold all the statements executed in this procedure */
BEGIN TRANSACTION 

/* Define the variable that holds the insert_id for the process template */
DECLARE @ProcessTemplateID INT;
/* Define the variables that hold the name, the final states, and actions defined for each states in the xml document */
DECLARE @name varchar(50), @finalstates varchar(50), @actions varchar(50);


IF NOT EXISTS ( SELECT * FROM ApplicationSetting WHERE SettingName in (SELECT SettingName FROM OPENXML(@xmldoc, '//ApplicationSetting') WITH ApplicationSetting))
BEGIN
   INSERT INTO ProcessTemplate SELECT Name, [Description], GraphicsData, PartitionId FROM OPENXML(@xmldoc, '//ProcessTemplate') WITH ProcessTemplate
     IF(@@error <> 0)  
	   ROLLBACK 
   SET @ProcessTemplateID = SCOPE_IDENTITY();
      INSERT INTO ApplicationSetting 
     VALUES ( (SELECT SettingName FROM OPENXML(@xmldoc, '//ApplicationSetting') WITH ApplicationSetting), @ProcessTemplateID, NULL);
     IF(@@error <> 0)  
	 ROLLBACK 
END
Else
 set @ProcessTemplateID = (SELECT SettingValue FROM ApplicationSetting 
  WHERE SettingName like (SELECT SettingName FROM OPENXML(@xmldoc, '//ApplicationSetting') WITH ApplicationSetting))


/* Insert new state templates into the 'StateTemplate' table from the list of <StateTemplate /> definitions in the xml document */
INSERT INTO StateTemplate 
SELECT @ProcessTemplateID, Name, AllowedAccessLevel, StateNo, StateType, NULL FROM OPENXML(@xmldoc, '//StateTemplate') WITH StateTemplate
WHERE name NOT IN (SELECT name FROM dbo.StateTemplate st WHERE st.ParentProcessTemplateID = @ProcessTemplateID)
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)  
	ROLLBACK /* Rollback of the transaction */

--- update state no of all 
declare @stateName varchar(20);
declare @Id int;
DECLARE @stateNo int;
DECLARE @stateType int;

DECLARE @statTable TABLE (StateTemplateID int,Name varchar(30));
insert into @statTable SELECT st.StateTemplateID,st.Name FROM [dbo].[StateTemplate] st WHERE st.ParentProcessTemplateID = @ProcessTemplateID
-- Iterate the existing statetemplate and update with the new stateno and statetype value
While (Select Count(*) From @statTable) > 0
Begin
    Select Top 1 @Id = StateTemplateID, @stateName = Name From @statTable
    SET @stateNo = (SELECT StateNo FROM OPENXML(@xmldoc, '//StateTemplate') WITH StateTemplate WHERE Name = @stateName)
    SET @stateType = (SELECT StateType FROM OPENXML(@xmldoc, '//StateTemplate') WITH StateTemplate WHERE Name = @stateName)
    update dbo.StateTemplate
    SET       
        dbo.StateTemplate.StateNo =  @stateNo,
        dbo.StateTemplate.StateType = @stateType 
	   Where StateTemplateID = @Id 
	   AND ParentProcessTemplateID = @ProcessTemplateID
    DELETE @statTable WHERE [@statTable].StateTemplateID = @Id
End

IF(@@error <> 0)
ROLLBACK 

/* Temporary Edge table: that parsesthe xml document into a table like structure */  
SELECT *   
INTO #TempEdge   
FROM OPENXML(@xmldoc, '//StateTemplate')  
/* Check if the above statement failed to execute and rollback the transcation */
IF(@@error <> 0)  
	ROLLBACK /* Rollback of the transaction */

/*Delete existing flow template*/
delete FROM dbo.FlowTemplate where ParentProcessTemplateID = @ProcessTemplateID
IF(@@error <> 0)  
	ROLLBACK
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

