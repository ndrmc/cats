
    DECLARE @h int
    EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
    <ProcessTemplate Name="ReciptPlanForLoanWorkflow" Description="Workflow for Loan - receipt plan in logistics." GraphicsData="NULL" PartitionId="0" />
    <ApplicationSetting SettingName="ReciptPlanForLoanWorkflow" SettingValue="" />
    <StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Edited,Deleted" Actions="Edit" />
    <StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Edited,Deleted" Actions="" />
		
    </Data>'
    EXECUTE update_action_workflow @h