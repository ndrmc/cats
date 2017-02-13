USE [Cats-v-1-3-2]
GO

DECLARE @tempTable TABLE (ID UniqueIdentifier);
Insert into @tempTable select ReceiveID from [Receive]
            where BusinessProcessID =  0
If EXISTS (select * from ApplicationSetting where SettingName = 'ReceiveHubWorkflow')
declare @settingValue int;
select top(1) @settingValue = SettingValue from ApplicationSetting where SettingName = 'ReceiveHubWorkflow'
declare @createdStateId int;
select top(1) @createdStateId = StateTemplateID from StateTemplate where ParentProcessTemplateID = @settingValue And Name = 'Draft'
begin
declare @id uniqueidentifier;

declare @businessProcessId int;
declare @businessProcessStateId int;
declare @datePerformed datetime = GETDATE();
While (Select Count(*) From @tempTable) > 0
  Begin
       select top(1) @id = ID from @tempTable
	  -- insert busines process 
	  insert into BusinessProcess(ProcessTypeID,DocumentID,DocumentType,CurrentStateID,PartitionId)
	  values(@settingValue,0,'Receive Action State',0,NULL) select @businessProcessId = Scope_Identity()

	  -- insert business process state value
	  insert into BusinessProcessState (ParentBusinessProcessID,StateID,PerformedBy,DatePerformed,Comment,PartitionId,AttachmentFile)
	         values(@businessProcessId,@createdStateId,'System',@datePerformed,'System migration',NULL,NULL) set @businessProcessStateId = Scope_Identity();
        -- update the business process current stateId
	   update BusinessProcess set CurrentStateID = @businessProcessStateId where BusinessProcessID = @businessProcessId
	   -- Update the object BusinessProcesessID values
	   update [Receive] set BusinessProcessID = @businessProcessId where ReceiveID = @id

	  delete @tempTable where ID = @id
  end

end
