use [CatsDRMFSS];

DECLARE @h int
EXECUTE sp_xml_preparedocument @h OUTPUT, N'<Data>  
	<ProcessTemplate Name="Relief Requisition" Description="Workflow for relief requisition" GraphicsData="NULL" PartitionId="0" />
	<ApplicationSetting SettingName="ReliefRequisitionWorkflow" SettingValue="" />

	<StateTemplate Name="Draft" AllowedAccessLevel="0" StateNo="0" StateType="0" FinalStates="Approved" Actions="Approve" />
	<StateTemplate Name="Approved" AllowedAccessLevel="0" StateNo="1" StateType="1" FinalStates="Rejected,Hub Assigned" Actions="Reject,Assign Hub" /> 
	<StateTemplate Name="Rejected" AllowedAccessLevel="0" StateNo="2" StateType="1" FinalStates="Approved" Actions="Re-Approve" />
	<StateTemplate Name="Hub Assigned" AllowedAccessLevel="0" StateNo="3" StateType="1" FinalStates="Project Code Assigned" Actions="Assign Project Code" />
	<StateTemplate Name="Project Code Assigned" AllowedAccessLevel="0" StateNo="4" StateType="1" FinalStates="Approved,SiPc Allocation Approved" Actions="Uncommit,Approve SI/PC Allocation" />
	<StateTemplate Name="SiPc Allocation Approved" AllowedAccessLevel="0" StateNo="5" StateType="1" FinalStates="Transport Requisition Created" Actions="Create Transport Requisition" />
	<StateTemplate Name="Transport Requisition Created" AllowedAccessLevel="0" StateNo="6" StateType="1" FinalStates="Transport Order Created" Actions="Create Transport Order" />
	<StateTemplate Name="Transport Order Created" AllowedAccessLevel="0" StateNo="7" StateType="1" FinalStates="" Actions="" />   
</Data>'

EXECUTE define_document_workflow @h

