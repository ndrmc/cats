

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="Global Workflow" Description="Global action workflow definition" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="GlobalWorkflow" SettingValue="" />
	<StateTemplate Name="Created" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="" Actions="" />
	<StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="" Actions="" /> 
	<StateTemplate Name="Deleted" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="" Actions="" />
	<StateTemplate Name="Printed" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="" Actions="" />
	<StateTemplate Name="Exported" AllowedAccessLevel="0" StateNo="4" StateType="1" FinalStates="" Actions="" />
	
</Data>'

EXECUTE update_action_workflow @h