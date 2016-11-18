use [Cats-v-1-3-1]

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="Donation Plan Header" Description="Workflow for Donation Plan Header" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="DonationPlanHeaderWorkflow" SettingValue="" />

	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Sent to hub" Actions="Send to hub" />
	<StateTemplate Name="Sent to hub" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Draft" Actions="Revert" />
</Data>'

EXECUTE define_document_workflow @h