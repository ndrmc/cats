

declare @ProcessTemplateID  int;
declare @draft int;
declare @edited int;
declare @approved int;
declare @reverted int;
declare @published int;
declare @deleted int;
declare @closed int
IF EXISTS (SELECT * FROM [dbo].[ApplicationSetting] WHERE [SettingName] = 'HRDWorkflow')
BEGIN    
 select top 1 @ProcessTemplateID = SettingValue FROM [dbo].[ApplicationSetting] WHERE [SettingName] = 'HRDWorkflow'
 if(not exists(select * from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Edited'))
   begin 
	INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
           ( @ProcessTemplateID,'Edited',0,2,1)  select @edited = Scope_Identity()		   
   End
   else
      begin 
           select top 1 @edited = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Edited'
      end
	 
   select top 1 @draft = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Draft'
   select top 1 @approved = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Approved'
   select top 1 @deleted = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Deleted'
   select top 1 @reverted = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Reverted'
    select top 1 @published = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Published'
     select top 1 @closed = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Closed'

	delete from FlowTemplate where ParentProcessTemplateID = @ProcessTemplateID
     INSERT INTO [dbo].[FlowTemplate]
           ([ParentProcessTemplateID]
           ,[InitialStateID]
           ,[FinalStateID]
           ,[Name]
           )
     VALUES
             (@ProcessTemplateID ,@draft,@approved,'Approve'),
		   (@ProcessTemplateID ,@draft,@edited,'Edit'),
		   (@ProcessTemplateID ,@draft,@deleted,'Delete'),
		   (@ProcessTemplateID ,@approved,@published,'Publish'),
		   (@ProcessTemplateID ,@approved,@reverted,'Revert'),
		   (@ProcessTemplateID ,@Published,@Closed,'Close'),
		   (@ProcessTemplateID ,@edited,@deleted,'Delete'),
		   (@ProcessTemplateID ,@edited,@approved,'Approve'),
		    (@ProcessTemplateID ,@reverted,@deleted,'Delete'),
		   (@ProcessTemplateID ,@reverted,@approved,'Approve')
		  
END


