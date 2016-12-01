
    DECLARE @h int
    EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
    <ProcessTemplate Name="TransportOrderWorkflow" Description="Workflow for Transport Order." GraphicsData="NULL" PartitionId="0" />
    <ApplicationSetting SettingName="TransportOrderWorkflow" SettingValue="" />
    <StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved,Edited" Actions="Approve,Edit" />
    <StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Edited,Approved" Actions="Edit,Approve" />
	
    </Data>'
    EXECUTE update_action_workflow @h