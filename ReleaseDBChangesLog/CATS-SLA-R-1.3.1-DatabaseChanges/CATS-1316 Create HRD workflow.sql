--Create CATS-1315

use 
CatsMaster
go
IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[HRD]') 
         AND name = 'BusinessProcessId'
)
 --IF COL_LENGTH('[EarlyWarning].[RegionalRequest]','[BusinessProcessID]') IS NULL --check if BusinessProcessID exist
 BEGIN
alter table 
[dbo].[HRD]
add   [BusinessProcessId] [int] Default NULL
 END

declare @ProcessTemplateID table (id int);
IF Not EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'HRD')
BEGIN
 
   INSERT INTO [dbo].[ProcessTemplate]
           ([Name]
           ,[Description]
           ) Output Inserted.ProcessTemplateID INTO @ProcessTemplateID
     VALUES
           ('HRD',
            'HRD Processing'
		   )
		declare @id int 
		declare @draft int 
		declare @approved int
		declare @published int
		declare @deleted int 
		declare @closed int   
		select @id = Scope_Identity()  

    -- Insert Application Setting Value
	   INSERT INTO dbo.ApplicationSetting
	   (
	   --SettingID - this column value is auto-generated
	   SettingName,
	   SettingValue,
	   PartitionId)
	   VALUES
	   (
		-- SettingID - int
		'HRDWorkflow', -- SettingName - varchar
		@id, -- SettingValue - varchar
		null -- PartitionId - int
	   );
     
	INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
           ( @id,'Draft',0,0,1 )  select @draft = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Approved',0,1,1 )  select @approved = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Published',0,2,1 ) select @published = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Deleted',0,3,1 ) select @deleted = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Closed',0,4,1 )
		   select @closed = Scope_Identity() 
		   INSERT INTO [dbo].[FlowTemplate]
           ([ParentProcessTemplateID]
           ,[InitialStateID]
           ,[FinalStateID]
           ,[Name]
           )
     VALUES
           (@id ,@draft,@approved,'Approve'),
		   (@id ,@draft,@deleted,'Delete'),
		   (@id ,@approved,@published,'Publish'),
		   (@id ,@Published,@Closed,'Close')
		  
END
GO
GO

SELECT * FROM  Applicationsetting
wh
SELECT * FROM ProcessTemplate 
SELECT * FROM StateTemplate AS t WHERE t.ParentProcessTemplateId = 1004

BEGIN TRANSACTION DELETE ProcessTemplate WHERE ProcessTemplateId = 1004 ROLLBACK TRANSACTION