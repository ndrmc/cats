use [CatsDRMFSS];

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="PSNP Plan" Description="Plan to Execution" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="PSNPWorkflow" SettingValue="" />
	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Edited,Submitted for Approval" Actions="Edit,Ask Approval" />
	<StateTemplate Name="Submitted for Approval" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Under Revision,Approved" Actions="Request Revision,Approve" /> 
	<StateTemplate Name="Under Revision" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="Approved,Submitted for Approval" Actions="Re-Approve," />
	<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="Completed" Actions="Completed" />
	<StateTemplate Name="Completed" AllowedAccessLevel="0" StateNo="4" StateType="1" FinalStates="Approved,SiPc Allocation Approved" Actions="Uncommit,Approve SI/PC Allocation" />
	<StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="5" StateType="1" FinalStates="Edited,Submitted for Approval" Actions="Edit,Ask Approval" />
	
</Data>'

EXECUTE update_action_workflow @h