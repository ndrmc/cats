--Create CATS-1325


 BEGIN TRANSACTION
use 
CatsMaster
go
IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[Procurement].[TransportOrder]') 
         AND name = 'BusinessProcessID'
)
 --IF COL_LENGTH('[EarlyWarning].[RegionalRequest]','[BusinessProcessID]') IS NULL --check if BusinessProcessID exist
 BEGIN
alter table 
[Procurement].[TransportOrder]
add   [BusinessProcessID] [int] Default 0 NOT NULL
 END
 IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */
declare @ProcessTemplateID table (id int);
IF Not EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'TransportOrder')
BEGIN
 

   INSERT INTO [dbo].[ProcessTemplate]
           ([Name]
           ,[Description]
           ) Output Inserted.ProcessTemplateID INTO @ProcessTemplateID
     VALUES
           ('TransportOrder',
            'Work flow for Transport Order'
		   )
		declare @id int 
		declare @draft int 
		declare @Closed int
		declare @Signed int
		declare @Rejected int 
		declare @Approved int   
		select @id = Scope_Identity()  
		 INSERT INTO [dbo].[ApplicationSetting]
           ([SettingName]
           ,[SettingValue]
           )
     VALUES
           ('TransportOrderWorkflow'
           ,@id
           )
	INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
           ( @id,'Draft',0,0,0 )  select @Draft = Scope_Identity()

    INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Approved',0,1,1 )
		   select @Approved = Scope_Identity() 
		    INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Signed',0,2,1 ) select @Signed = Scope_Identity()

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
		   ( @id,'Rejected',0,2,1 ) select @Rejected = Scope_Identity()
		  
		  
		   
		 IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */  

		   INSERT INTO [dbo].[FlowTemplate]
           ([ParentProcessTemplateID]
           ,[InitialStateID]
           ,[FinalStateID]
           ,[Name]
           )
     VALUES
           (@id ,@Draft,@Approved,'Approve'),
		   (@id ,@Approved,@Signed,'Sign'),
		   (@id ,@Approved,@Rejected,'Reject'),
		   (@id ,@Signed,@Closed,'Close')
IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */		  
END
GO
GO
COMMIT