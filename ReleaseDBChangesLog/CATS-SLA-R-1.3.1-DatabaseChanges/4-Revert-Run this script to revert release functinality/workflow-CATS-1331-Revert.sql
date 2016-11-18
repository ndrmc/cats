 BEGIN TRANSACTION
IF   EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[Procurement].[TransportBidPlan]') 
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
where [Tab].[Name] = 'TransportBidPlan' and SysObjects.[Name] like 'DF__Transport__Busin%'
order by [Tab].[Name]) 
EXEC('ALTER TABLE [Procurement].[TransportBidPlan] DROP CONSTRAINT ' + @constraintName)
--ALTER TABLE [EarlyWarning].[RegionalRequest] DROP CONSTRAINT [DF__RegionalR__Busin__07FB56C9]
alter table 
[Procurement].[TransportBidPlan]
DROP COLUMN   [BusinessProcessID]
 END
 IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */
 declare @ProcessTemplateID  int;
IF  EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'Bid Plan') 
BEGIN
Select @ProcessTemplateID =  ProcessTemplateID  FROM [dbo].[ProcessTemplate]
where [Name] = 'Bid Plan'
Delete from [dbo].[BusinessProcess]
where [ProcessTypeID] = @ProcessTemplateID
IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */
delete from [dbo].[BusinessProcessState]
where [ParentBusinessProcessID]  =  @ProcessTemplateID
IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */
Delete from [dbo].[ProcessTemplate]
WHere [dbo].[ProcessTemplate].ProcessTemplateID = @ProcessTemplateID
IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */
Delete from [dbo].[StateTemplate]
WHere [dbo].[StateTemplate].ParentProcessTemplateID = @ProcessTemplateID
IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */
Delete from [dbo].[FlowTemplate]
WHere [dbo].[FlowTemplate].ParentProcessTemplateID = @ProcessTemplateID
IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */
Delete from [dbo].[ApplicationSetting]
WHere [dbo].[ApplicationSetting].SettingValue = @ProcessTemplateID
IF(@@error <> 0) 
ROLLBACK /* Rollback of the transaction */
ENd

COMMIT

