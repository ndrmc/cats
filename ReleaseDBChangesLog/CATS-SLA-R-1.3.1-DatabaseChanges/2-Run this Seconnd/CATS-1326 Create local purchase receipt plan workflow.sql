--Create CATS-1315

use 
[Cats-v-1-3-1]
go
IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[LocalPurchase]') 
         AND name = 'BusinessProcessID'
)
 --IF COL_LENGTH('[EarlyWarning].[RegionalRequest]','[BusinessProcessID]') IS NULL --check if BusinessProcessID exist
 BEGIN
alter table 
[dbo].[LocalPurchase]
add   [BusinessProcessID] [int] Default NULL
 END

declare @ProcessTemplateID table (id int);
IF Not EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'Local Purchase Receipt Plan')
BEGIN
 
   INSERT INTO [dbo].[ProcessTemplate]
           ([Name]
           ,[Description]
           ) Output Inserted.ProcessTemplateID INTO @ProcessTemplateID
     VALUES
           ('Local Purchase Receipt Plan',
            'Local Purchase Receipt Plan Processing'
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
		'LocalPurchaseReceiptPlanWorkflow', -- SettingName - varchar
		@id, -- SettingValue - varchar
		null -- PartitionId - int
	   );
     
	INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
           ( @id,'Draft',0,0,0)  select @draft = Scope_Identity()
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
