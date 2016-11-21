use [CatsDRMFSS];


ALTER TABLE Procurement.TransportBidPlanDetail
ADD BusinessProcessID int NULL 

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="Bid Plan detail workflow" Description="Action Workflow for Bid Plan detail" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="BidPlanDetailActionWorkflow" SettingValue="" />
	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Edited" Actions="Edit" />
	<StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="" Actions="" />
	<StateTemplate Name="Deleted" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="" Actions="" />
</Data>'

EXECUTE update_action_workflow @h