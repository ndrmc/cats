use [Cats-v-1-3-2]
go

    DECLARE @h int
    EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data> 
    <ProcessTemplate Name="DeliveryWorkflow" Description="Workflow for Delivery in Logistics." GraphicsData="NULL" PartitionId="0" />
    <ApplicationSetting SettingName="DeliveryWorkflow" SettingValue="" />
    <StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Edited" Actions="Edit" />
    <StateTemplate Name="Edited" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Edited" Actions="" />
	
    </Data>'
    EXECUTE update_action_workflow @h