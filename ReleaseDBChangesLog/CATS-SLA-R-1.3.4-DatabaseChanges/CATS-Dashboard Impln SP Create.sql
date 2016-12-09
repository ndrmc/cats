USE [Cats-v-1-3-1]
GO
/****** Object:  StoredProcedure [dbo].[GenericDashboardDataProvider]    Script Date: 12/9/2016 9:50:57 AM ******/
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
SET @StartDate = Convert(DATE, @StartDate);
SET @EndDate = Convert(DATE, @EndDate);

SELECT  Row_Number() OVER(ORDER BY PerformedBy ASC) AS Row,        
			StateTemplate.Name AS ActivityName, --	
			0 AS ActivityCount, -- Frequency 	
			--Count(StateTemplate.Name) AS ActivityCount, -- Frequency 
			BusinessProcessState.PerformedBy, -- 
			ApplicationSetting.SettingName,
			ProcessTemplate.ProcessTemplateID,
			BusinessProcess.ProcessTypeID,
			StateTemplate.StateTemplateID,
			BusinessProcess.BusinessProcessID,
			BusinessProcessState.DatePerformed

FROM            BusinessProcessState INNER JOIN
                ProcessTemplate INNER JOIN
                FlowTemplate ON ProcessTemplate.ProcessTemplateID = FlowTemplate.ParentProcessTemplateID INNER JOIN
                BusinessProcess ON ProcessTemplate.ProcessTemplateID = BusinessProcess.ProcessTypeID INNER JOIN
                StateTemplate ON ProcessTemplate.ProcessTemplateID = StateTemplate.ParentProcessTemplateID INNER JOIN
                ApplicationSetting ON ProcessTemplate.ProcessTemplateID = ApplicationSetting.SettingValue ON BusinessProcessState.StateID = StateTemplate.StateTemplateID AND 
                BusinessProcessState.ParentBusinessProcessID = BusinessProcess.BusinessProcessID
						 
WHERE			--(BusinessProcessState.DatePerformed >= @StartDate AND BusinessProcessState.DatePerformed <= @EndDate) AND	
				ApplicationSetting.SettingName IN (SELECT * FROM @WorkflowName_Array) AND
				BusinessProcessState.PerformedBy IN (SELECT * FROM @User_Array) AND
				StateTemplate.Name In (SELECT * FROM @Activity_Array)

	GROUP BY		StateTemplate.Name, 
					BusinessProcessState.PerformedBy,
					ApplicationSetting.SettingName,
					ProcessTemplate.ProcessTemplateID,
					BusinessProcess.ProcessTypeID,
					StateTemplate.StateTemplateID,
					BusinessProcess.BusinessProcessID,
					BusinessProcessState.DatePerformed

	ORDER BY		BusinessProcessState.PerformedBy,
					StateTemplate.Name,
					BusinessProcessState.DatePerformed

END

