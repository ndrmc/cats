
use [CatsDRMFSS];

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="Bid Plan" Description="Workflow for Bid Plan" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="BidPlanWorkflow" SettingValue="" />

	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved" Actions="Approve,Edit" />
	<StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Approved" Actions="Approve" />
	<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="" Actions="" />
	<StateTemplate Name="Printed" AllowedAccessLevel="0" StateNo="4" StateType="1" FinalStates="" Actions="" />
	<StateTemplate Name="Exported" AllowedAccessLevel="0" StateNo="5" StateType="1" FinalStates="" Actions="" />
	<StateTemplate Name="Deleted" AllowedAccessLevel="0" StateNo="6" StateType="1" FinalStates="" Actions="" />
</Data>'

EXECUTE update_action_workflow @h