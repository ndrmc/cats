 BEGIN TRANSACTION
IF   EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[EarlyWarning].[RegionalRequest]') 
         AND name = 'BusinessProcessID'
)
 
 BEGIN
 
declare  @constraintName varchar(60);
SET @constraintName =   (Select top 1   SysObjects.[Name] As [Constraint Name] 
       
From SysObjects Inner Join 
(Select [Name],[ID] From SysObjects) As Tab
On Tab.[ID] = Sysobjects.[Parent_Obj] 
Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID] 
Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID]
where [Tab].[Name] = 'RegionalRequest' and SysObjects.[Name] like 'DF__RegionalR__Busin%'
order by [Tab].[Name]) 
EXEC('ALTER TABLE [EarlyWarning].[RegionalRequest] DROP CONSTRAINT ' + @constraintName)

alter table 
[EarlyWarning].[RegionalRequest]
DROP COLUMN   [BusinessProcessID]
 END
 IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */
 declare @ProcessTemplateID  int;
IF  EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'RegionalRequest') 
BEGIN
Select @ProcessTemplateID =  ProcessTemplateID  FROM [dbo].[ProcessTemplate]
where [Name] = 'RegionalRequest'
Delete from [dbo].[BusinessProcess]
where [ProcessTypeID] = @ProcessTemplateID
delete from [dbo].[BusinessProcessState]
where [ParentBusinessProcessID]  =  @ProcessTemplateID
Delete from [dbo].[ProcessTemplate]
WHere [dbo].[ProcessTemplate].ProcessTemplateID = @ProcessTemplateID
IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */
Delete from [dbo].[StateTemplate]
WHere [dbo].[StateTemplate].ParentProcessTemplateID = @ProcessTemplateID

Delete from [dbo].[FlowTemplate]
WHere [dbo].[FlowTemplate].ParentProcessTemplateID = @ProcessTemplateID

Delete from [dbo].[ApplicationSetting]
WHere [dbo].[ApplicationSetting].SettingValue = @ProcessTemplateID
IF(@@error <> 0)
ROLLBACK /* Rollback of the transaction */
ENd
COMMIT