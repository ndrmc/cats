USE [CatsV6]
GO
/****** Object:  StoredProcedure [dbo].[GenericDashboardDataProvider]    Script Date: 12/14/2016 4:06:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Yonathan>
-- Create date: <Fri, Dec 2 2016>
-- Description:	<This proc accepts five parameters and outputs aggregated data which will be used for dashboard stat. info..>
-- =============================================
ALTER PROCEDURE [dbo].[GenericDashboardDataProvider] 
	-- Add the parameters for the stored procedure here

@StartDate DATE,
@EndDate DATE,
@WorkflowName_Array AS dbo.FilterArray READONLY,
@User_Array AS dbo.FilterArray READONLY,
@Activity_Array AS dbo.FilterArray READONLY

AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET DATEFORMAT dmy;

SET @StartDate = CAST(@StartDate as DATE);
SET @EndDate = CAST(@EndDate as DATE);

SELECT  Row_Number() OVER(ORDER BY PerformedBy ASC) AS Row,        
			StateTemplate.Name AS ActivityName, --	
			0 AS ActivityCount, -- Frequency 	
			--Count(StateTemplate.Name) AS ActivityCount, -- Frequency 
			BusinessProcessState.PerformedBy, -- 
			ApplicationSetting.SettingName,
			Convert(INT, ApplicationSetting.SettingValue) As ProcessTemplateID,
			BusinessProcess.ProcessTypeID,
			StateTemplate.StateTemplateID,
			BusinessProcess.BusinessProcessID,			
			CAST(BusinessProcessState.DatePerformed as DATE) AS DatePerformed

FROM            
				--BusinessProcessState INNER JOIN
    --            ProcessTemplate INNER JOIN
    --            FlowTemplate ON ProcessTemplate.ProcessTemplateID = FlowTemplate.ParentProcessTemplateID INNER JOIN
    --            BusinessProcess ON ProcessTemplate.ProcessTemplateID = BusinessProcess.ProcessTypeID INNER JOIN
    --            StateTemplate ON ProcessTemplate.ProcessTemplateID = StateTemplate.ParentProcessTemplateID INNER JOIN
    --            ApplicationSetting ON ProcessTemplate.ProcessTemplateID = ApplicationSetting.SettingValue ON BusinessProcessState.StateID = StateTemplate.StateTemplateID AND 
    --            BusinessProcessState.ParentBusinessProcessID = BusinessProcess.BusinessProcessID
	StateTemplate INNER JOIN
                         BusinessProcessState ON StateTemplate.StateTemplateID = BusinessProcessState.StateID INNER JOIN
                         BusinessProcess INNER JOIN
                         TransporterCheque ON BusinessProcess.BusinessProcessID = TransporterCheque.BusinessProcessID ON BusinessProcessState.ParentBusinessProcessID = BusinessProcess.BusinessProcessID INNER JOIN
                         ApplicationSetting ON StateTemplate.ParentProcessTemplateID = ApplicationSetting.SettingValue
						 
WHERE			--(BusinessProcessState.DatePerformed >= @StartDate AND BusinessProcessState.DatePerformed <= @EndDate) AND	
				ApplicationSetting.SettingName IN (SELECT * FROM @WorkflowName_Array) AND
				BusinessProcessState.PerformedBy IN (SELECT * FROM @User_Array) AND
				StateTemplate.Name In (SELECT * FROM @Activity_Array)

	GROUP BY		StateTemplate.Name, 
					BusinessProcessState.PerformedBy,
					ApplicationSetting.SettingName,
					ApplicationSetting.SettingValue,
					BusinessProcess.ProcessTypeID,
					StateTemplate.StateTemplateID,
					BusinessProcess.BusinessProcessID,
					BusinessProcessState.DatePerformed

	ORDER BY		BusinessProcessState.PerformedBy,
					StateTemplate.Name,
					BusinessProcessState.DatePerformed

END