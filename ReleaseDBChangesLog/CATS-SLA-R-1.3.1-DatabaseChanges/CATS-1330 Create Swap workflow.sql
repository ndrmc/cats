--Create CATS-1315

use 
CatsDRMFSS
go
IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[Transfer]') 
         AND name = 'BusinessProcessID'
)
 --IF COL_LENGTH('[EarlyWarning].[RegionalRequest]','[BusinessProcessID]') IS NULL --check if BusinessProcessID exist
 BEGIN
alter table 
[dbo].[Transfer]
add   [BusinessProcessID] [int] Default NULL
 END

declare @ProcessTemplateID table (id int);
IF Not EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'Swap')
BEGIN
 
   INSERT INTO [dbo].[ProcessTemplate]
           ([Name]
           ,[Description]
           ) Output Inserted.ProcessTemplateID INTO @ProcessTemplateID
     VALUES
           ('Swap',
            'Swap Plan Processing'
		   )
		declare @id int 
		declare @draft int 
		declare @approved int		
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
		'SwapWorkflow', -- SettingName - varchar
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
	
	INSERT INTO [dbo].[FlowTemplate]
           ([ParentProcessTemplateID]
           ,[InitialStateID]
           ,[FinalStateID]
           ,[Name]
           )
     VALUES
           (@id ,@draft,@approved,'Approve')		   
		  
END
GO
GO
