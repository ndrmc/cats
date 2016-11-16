use [Cats-v-1-3-1]

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="Needs Assessment" Description="Workflow for needs assessment processing" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="NeedAssessmentWorkflow" SettingValue="" />

	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Submitted to EW" Actions="Submit to EW" />
	<StateTemplate Name="Submitted to EW" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Reversed,Approved" Actions="Reverse,Approve" /> 
	<StateTemplate Name="Reversed" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="Submitted to EW" Actions="Re-Submit to EW" />	
	<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="" Actions="" />
</Data>'

EXECUTE define_document_workflow @h