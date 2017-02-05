use WorkflowDef
go

declare @ProcessTemplateID  int;

declare @draft int;
declare @edited int;
declare @sentToHub int;
declare @deleted int;
declare @reverted int;

IF EXISTS (SELECT * FROM [dbo].[ApplicationSetting] WHERE [SettingName] = 'DonationPlanHeaderWorkflow')
BEGIN    
 select top 1 @ProcessTemplateID = SettingValue FROM [dbo].[ApplicationSetting] WHERE [SettingName] = 'DonationPlanHeaderWorkflow'
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

			----

			if(not exists(select * from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Deleted'))
   begin 
	INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
           ( @ProcessTemplateID,'Deleted',0,4,1)  select @deleted = Scope_Identity()		   
   End
   else
      begin 
           select top 1 @deleted = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Deleted'
      end

	 
   select top 1 @draft = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Draft'
   select top 1 @sentToHub = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Sent to hub'
   --select top 1 @reverted = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Reverted'
   --select top 1 @deleted = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Deleted'
  -- select top 1 @signed = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Signed'
  -- select top 1 @closed = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Closed'

	delete from FlowTemplate where ParentProcessTemplateID = @ProcessTemplateID
    
		 INSERT INTO [dbo].[FlowTemplate]
           ([ParentProcessTemplateID]
           ,[InitialStateID]
           ,[FinalStateID]
           ,[Name]
           )
     VALUES
       (@ProcessTemplateID ,@draft,@sentToHub,'Send to hub'),
		   (@ProcessTemplateID ,@draft,@edited,'Edit'),
			 (@ProcessTemplateID ,@draft,@deleted,'Delete'),
		   --(@ProcessTemplateID ,@draft,@deleted,'Delete'),
		   (@ProcessTemplateID ,@sentToHub,@draft,'Revert'),
			 (@ProcessTemplateID ,@sentToHub,@edited,'Revert'),
		   --(@ProcessTemplateID ,@approved,@rejected,'Reject'),
		   --(@ProcessTemplateID ,@signed,@closed,'Close'),
		   (@ProcessTemplateID ,@edited,@edited,'Edit'),
		   (@ProcessTemplateID ,@edited,@sentToHub,'Send to hub'),
			 (@ProcessTemplateID ,@edited,@deleted,'Delete'),
			 (@ProcessTemplateID ,@deleted,@deleted,'Delete')
			 --(@ProcessTemplateID ,@rejected,@approved,'Approve'),
			 --(@ProcessTemplateID ,@rejected,@edited,'Edit')
		  
END


