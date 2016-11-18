--Create CATS-1331

use 
CatsMaster
go
BEGIN TRANSACTION
IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[Procurement].[TransportBidPlan]') 
         AND name = 'BusinessProcessID'
)
 --check if BusinessProcessID exist
 BEGIN
alter table 
[Procurement].[TransportBidPlan]
add   [BusinessProcessID] [int] Default 0 NOT NULL
 END

declare @ProcessTemplateID table (id int);
IF Not EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'Bid Plan')
BEGIN
  
   INSERT INTO [dbo].[ProcessTemplate]
           ([Name]
           ,[Description]
           ) Output Inserted.ProcessTemplateID INTO @ProcessTemplateID
     VALUES
           ('Bid Plan',
            'Work flow for Bid Plan'
		   )
		declare @id int 
		declare @draft int 
		
		declare @Approve int   
		select @id = Scope_Identity()
		 INSERT INTO [dbo].[ApplicationSetting]
           ([SettingName]
           ,[SettingValue]
           )
     VALUES
           ('BidPlanWorkflow'
           ,@id
           )  
		 IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */
	INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
           ( @id,'Draft',0,0,0 )  select @draft = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Edited',0,1,1 )  select @Approve = Scope_Identity()
		   		   
		   

		   INSERT INTO [dbo].[FlowTemplate]
           ([ParentProcessTemplateID]
           ,[InitialStateID]
           ,[FinalStateID]
           ,[Name]
           )
     VALUES
           (@id ,@draft,@Approve,'Edit')
		   
	 IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */	
		  
END
GO
COMMIT
GO