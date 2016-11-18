  --Execute the above stored procedure
    DECLARE @h int
    EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
    <ProcessTemplate Name="GiftCertificate" Description="Workflow for Gift Certificate" GraphicsData="NULL" PartitionId="0" />
    <ApplicationSetting SettingName="GiftCertificateWorkflow" SettingValue="" />
    <StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved" Actions="Approve" />
    <StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Rejected" Actions="Reject" /> 
    <StateTemplate Name="Rejected" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="Approved" Actions="Approve" />
    </Data>'
    EXECUTE define_document_workflow @h