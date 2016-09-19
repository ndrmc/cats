use [CatsDRMFSS];

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="Transport Requisition" Description="Workflow for transport requisition" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="TransportRequisitionWorkflow" SettingValue="" />

	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved" Actions="Approve" />
	<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Closed" Actions="Close" /> 
	<StateTemplate Name="Closed" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="" Actions="" />   
</Data>'

EXECUTE define_document_workflow @h