--Create CATS-1315
 BEGIN TRANSACTION
use 
CatsMaster
go
IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[EarlyWarning].[RegionalRequest]') 
         AND name = 'BusinessProcessID'
)
 --IF COL_LENGTH('[EarlyWarning].[RegionalRequest]','[BusinessProcessID]') IS NULL --check if BusinessProcessID exist
 BEGIN
alter table 
[EarlyWarning].[RegionalRequest]
add   [BusinessProcessID] [int] Default 0 NOT NULL
 END
 IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */
declare @ProcessTemplateID table (id int);
IF Not EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'Regional Request')
BEGIN
 

   INSERT INTO [dbo].[ProcessTemplate]
           ([Name]
           ,[Description]
           ) Output Inserted.ProcessTemplateID INTO @ProcessTemplateID
     VALUES
           ('Regional Request',
            'Work flow for Regional Request'
		   )
		declare @id int 
		declare @Create int 
		declare @Closed int
		declare @FederalApproved int
		declare @Rejecet int 
		declare @Approve int   
		select @id = Scope_Identity()  
		 INSERT INTO [dbo].[ApplicationSetting]
           ([SettingName]
           ,[SettingValue]
           )
     VALUES
           ('RegionalRequestWorkflow'
           ,@id
           )
	INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
           ( @id,'Draft',0,0,0 )  select @Create = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Closed',0,3,1 )  select @Closed = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )

     VALUES
		   ( @id,'FederalApproved',0,2,1 ) select @FederalApproved = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Rejected',0,2,1 ) select @Rejecet = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Approved',0,1,1 )
		   select @Approve = Scope_Identity() 
		   
		 IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */  

		   INSERT INTO [dbo].[FlowTemplate]
           ([ParentProcessTemplateID]
           ,[InitialStateID]
           ,[FinalStateID]
           ,[Name]
           )
     VALUES
           (@id ,@Create,@Approve,'Approve'),
		   (@id ,@Approve,@Rejecet,'Reject'),
		   (@id ,@Approve,@FederalApproved,'Federal Approve'),
		   (@id ,@Rejecet,@Approve,'Approve'),
		   (@id ,@FederalApproved,@Closed,'Close')
IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */		  
END
GO
GO
COMMIT