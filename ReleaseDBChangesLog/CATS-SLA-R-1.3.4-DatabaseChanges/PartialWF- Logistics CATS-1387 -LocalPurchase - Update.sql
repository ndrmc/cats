 DECLARE @h int
    EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
    <ProcessTemplate Name="LocalPurchaseReceiptPlanWorkflow" Description="Workflow for LocalPurchase in Logistics." GraphicsData="NULL" PartitionId="0" />
    <ApplicationSetting SettingName="LocalPurchaseReceiptPlanWorkflow" SettingValue="" />
    <StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Edited,Approved" Actions="Edit,Approve" />
    <StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Edited,Approved" Actions="Edit,Approve" />
		<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="Reverted" Actions="Reject" />
		<StateTemplate Name="Reverted" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="Approved" Actions="Approve" />		
    </Data>'
    EXECUTE update_action_workflow @h