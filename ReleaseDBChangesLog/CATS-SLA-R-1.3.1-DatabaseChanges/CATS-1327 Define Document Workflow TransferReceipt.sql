  --Execute the above stored procedure
    DECLARE @h int
    EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
    <ProcessTemplate Name="TransferReceiptPlan" Description="Workflow for Transfer Receipt Plan in Logistics" GraphicsData="NULL" PartitionId="0" />
    <ApplicationSetting SettingName="TranferReceiptPlanWorkflow" SettingValue="" />
    <StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved" Actions="Approve" />
    <StateTemplate Name="Approve" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Approved" /> 
    
    </Data>'
    EXECUTE define_document_workflow @h