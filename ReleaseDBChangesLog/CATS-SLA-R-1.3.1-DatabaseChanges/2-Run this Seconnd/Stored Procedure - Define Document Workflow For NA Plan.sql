

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="Needs Assessment Plan" Description="Workflow for plan of needs assessment" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="NeedAssessmentPlanWorkflow" SettingValue="" />

	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved" Actions="Approve" />
	<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="AssessmentCreated" Actions="Create assessment" /> 
	<StateTemplate Name="AssessmentCreated" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="HRDCreated" Actions="Create HRD" />	
	<StateTemplate Name="HRDCreated" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="PSNPCreated" Actions="Create PSNP" />
	<StateTemplate Name="PSNPCreated" AllowedAccessLevel="0" StateNo="4" StateType="1" FinalStates="Closed" Actions="Close" /> 
	<StateTemplate Name="Closed" AllowedAccessLevel="0" StateNo="5" StateType="1" FinalStates="" Actions="" />   
</Data>'

EXECUTE define_document_workflow @h