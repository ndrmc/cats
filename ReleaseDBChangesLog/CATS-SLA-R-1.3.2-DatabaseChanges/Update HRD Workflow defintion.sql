use [CatsDRMFSS];

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="HRD" Description="HRD Processing" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="HRDWorkflow" SettingValue="" />
	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved,Deleted" Actions="Approve,Delete" />
	<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="Published,Reverted" Actions="Publish,Revert" />
	<StateTemplate Name="Reverted" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="Approved,Deleted" Actions="Approve,Delete" />
	<StateTemplate Name="Published" AllowedAccessLevel="0" StateNo="4" StateType="1" FinalStates="Closed" Actions="Close" />
	<StateTemplate Name="Deleted" AllowedAccessLevel="0" StateNo="5" StateType="1" FinalStates="" Actions="" />
	<StateTemplate Name="Closed" AllowedAccessLevel="0" StateNo="6" StateType="1" FinalStates="" Actions="" />
</Data>'

EXECUTE update_action_workflow @h