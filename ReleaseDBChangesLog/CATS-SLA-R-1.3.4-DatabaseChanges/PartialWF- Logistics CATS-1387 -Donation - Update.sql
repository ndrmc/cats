
    DECLARE @h int
    EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
    <ProcessTemplate Name="DonationPlanHeaderWorkflow" Description="Workflow for Donation receipt plan in logistics." GraphicsData="NULL" PartitionId="0" />
    <ApplicationSetting SettingName="DonationPlanHeaderWorkflow" SettingValue="" />
    <StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Edited,Sent to hub,Deleted" Actions="Send to hub,Edit,Delete" />
    <StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Edited,Sent to hub,Reverted,Deleted" Actions="Send to hub,Edit,Revert,Delete" />
	<StateTemplate Name="Deleted" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Deleted" Actions="Delete" />
	
    </Data>'
    EXECUTE define_document_action_workflow @h