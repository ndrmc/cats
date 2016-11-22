
SELECT * FROM dbo.ApplicationSetting [as] WHERE [as].SettingName = 'PSNPWorkflow'
SELECT * FROM dbo.ProcessTemplate pt WHERE pt.ProcessTemplateID = 1
SELECT * FROM dbo.StateTemplate st WHERE st.ParentProcessTemplateID = 3048
SELECT * FROM dbo.FlowTemplate ft WHERE ft.ParentProcessTemplateID = 3048


SELECT * FROM dbo.RegionalPSNPPlan rp
SELECT * FROM dbo.StateTemplate st WHERE st.AllowedAccessLevel >=2

SELECT * FROM dbo.RegionalPSNPPlan rp WHERE r