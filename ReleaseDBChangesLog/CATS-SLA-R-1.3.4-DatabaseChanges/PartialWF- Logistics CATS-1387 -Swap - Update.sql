
    DECLARE @h int
    EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
    <ProcessTemplate Name="SwapWorkflow" Description="Workflow for Swap receipt plan in logistics." GraphicsData="NULL" PartitionId="0" />
    <ApplicationSetting SettingName="SwapWorkflow" SettingValue="" />
    <StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Edited,Approved" Actions="Edit,Approve" />
    <StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Edited,Approved" Actions="Edit,Approve" />
		
    </Data>'
    EXECUTE update_action_workflow @h