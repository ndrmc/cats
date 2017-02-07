use [Cats-v-1-3-2]
go

declare @ProcessTemplateID  int;
declare @draft int;
declare @edited int;
declare @approved int;
declare @rejected int;
declare @signed int;
declare @failed int;
declare @closed int;

IF EXISTS (SELECT * FROM [dbo].[ApplicationSetting] WHERE [SettingName] = 'TransportOrderWorkflow')
BEGIN    
 select top 1 @ProcessTemplateID = SettingValue FROM [dbo].[ApplicationSetting] WHERE [SettingName] = 'TransportOrderWorkflow'
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
   select top 1 @rejected = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Rejected'
   select top 1 @signed = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Signed'
   select top 1 @closed = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Closed'
   select top 1 @failed = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Failed'
   select top 1 @edited = StateTemplateId from StateTemplate where ParentProcessTemplateID = @ProcessTemplateID and Name = 'Edited'

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
		   (@ProcessTemplateID ,@approved,@rejected,'Reject'),
		   (@ProcessTemplateID ,@approved,@signed,'Sign'),
		   (@ProcessTemplateID ,@approved,@failed,'Fail'),
		   (@ProcessTemplateID ,@rejected,@approved,'Approve'),
		    (@ProcessTemplateID ,@rejected,@edited,'Edit'),
		   (@ProcessTemplateID ,@signed,@closed,'Close'),
		   (@ProcessTemplateID ,@edited,@approved,'Approve'),
		    (@ProcessTemplateID ,@edited,@edited,'Edit')
		  
END


