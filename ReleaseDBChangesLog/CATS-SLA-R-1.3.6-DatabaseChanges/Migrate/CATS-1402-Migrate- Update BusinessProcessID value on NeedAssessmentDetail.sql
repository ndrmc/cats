

DECLARE @tempTable TABLE (ID int);
Insert into @tempTable select NAId from EarlyWarning.NeedAssessmentDetail
            where BusinessProcessID =  0 
If EXISTS (select * from ApplicationSetting where SettingName = 'GlobalWorkflow')
Begin
declare @globalSettingValue int;
select top(1) @globalSettingValue =   SettingValue from ApplicationSetting where SettingName = 'GlobalWorkflow'
declare @createdStateId int;
select top(1) @createdStateId = StateTemplateID from StateTemplate where ParentProcessTemplateID = @globalSettingValue
begin
declare @id int;

declare @businessProcessId int;
declare @businessProcessStateId int;
declare @datePerformed datetime = GETDATE();
While (Select Count(*) From @tempTable) > 0
  Begin
       select top(1) @id = ID from @tempTable
	  -- insert busines process 
	  insert into BusinessProcess(ProcessTypeID,DocumentID,DocumentType,CurrentStateID,PartitionId)
	  values(@globalSettingValue,0,'Need Assessment Detail',0,NULL) select @businessProcessId = Scope_Identity()

	  -- insert business process state value
	  insert into BusinessProcessState (ParentBusinessProcessID,StateID,PerformedBy,DatePerformed,Comment,PartitionId,AttachmentFile)
	         values(@businessProcessId,@createdStateId,'System',@datePerformed,'System migration',NULL,NULL) set @businessProcessStateId = Scope_Identity();
        -- update the business process current stateId
	   update BusinessProcess set CurrentStateID = @businessProcessStateId where BusinessProcessID = @businessProcessId
	   -- Update the object BusinessProcesessID values
	   update EarlyWarning.NeedAssessmentDetail set BusinessProcessID = @businessProcessId where NAId = @id

	  delete @tempTable where ID = @id
  end

end
end