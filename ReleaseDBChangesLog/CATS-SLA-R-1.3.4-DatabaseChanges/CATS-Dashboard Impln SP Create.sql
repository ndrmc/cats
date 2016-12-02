USE [Cats-v-1-3-1]
GO

/****** Object:  StoredProcedure [dbo].[GenericDashboardDataProvider]    Script Date: 12/2/2016 3:45:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Yonathan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GenericDashboardDataProvider] 
	-- Add the parameters for the stored procedure here

@StartDate DateTime,
@EndDate DateTime,
@WorkflowName_Array AS dbo.FilterArray READONLY,
@User_Array AS dbo.FilterArray READONLY,
@Activity_Array AS dbo.FilterArray READONLY

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT  Row_Number() OVER(ORDER BY PerformedBy ASC) AS Row, 
		StateTemplate.Name AS ActivityName, --
		Count(StateTemplate.Name) AS ActivityCount, -- Frequency 
		BusinessProcessState.PerformedBy, -- 
		ApplicationSetting.SettingName

FROM        BusinessProcessState INNER JOIN 
			BusinessProcess ON BusinessProcessState.ParentBusinessProcessID = BusinessProcess.BusinessProcessID INNER JOIN 
			ProcessTemplate ON BusinessProcess.ProcessTypeID = ProcessTemplate.ProcessTemplateID INNER JOIN
            StateTemplate ON ProcessTemplate.ProcessTemplateID = StateTemplate.ParentProcessTemplateID INNER JOIN
            ApplicationSetting ON ProcessTemplate.ProcessTemplateID = ApplicationSetting.SettingValue

WHERE			ApplicationSetting.SettingName IN (SELECT * FROM @WorkflowName_Array) AND
				BusinessProcessState.PerformedBy IN (SELECT * FROM @User_Array) AND
				StateTemplate.Name In (SELECT * FROM @Activity_Array)

GROUP BY		StateTemplate.Name, 
				BusinessProcessState.PerformedBy,
				ApplicationSetting.SettingName
ORDER BY		PerformedBy
END

GO


