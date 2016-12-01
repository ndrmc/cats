
    DECLARE @h int
    EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
    <ProcessTemplate Name="RegionalRequestWorkflow" Description="Workflow for Regional Request." GraphicsData="NULL" PartitionId="0" />
    <ApplicationSetting SettingName="RegionalRequestWorkflow" SettingValue="" />
    <StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved,Edited,Deleted" Actions="Approve,Edit,Delete" />
    <StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Edited,Approved,Deleted" Actions="Edit,Approve,Delete" /> 
	<StateTemplate Name="Closed" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Deleted" Actions="Delete" />   
	<StateTemplate Name="FederalApproved" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Deleted" Actions="Delete" /> 
	<StateTemplate Name="Rejected" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Deleted" Actions="Delete" /> 
	<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Deleted" Actions="Delete" />
    </Data>'
    EXECUTE define_document_action_workflow @h