
 declare  @constraintName varchar(60);
SET @constraintName =   (Select top 1   SysObjects.[Name] As [Constraint Name] 
       
From SysObjects Inner Join 
(Select [Name],[ID] From SysObjects) As Tab
On Tab.[ID] = Sysobjects.[Parent_Obj] 
Inner Join sysconstraints On sysconstraints.Constid = Sysobjects.[ID] 
Inner Join SysColumns Col On Col.[ColID] = sysconstraints.[ColID] And Col.[ID] = Tab.[ID]
where [Tab].[Name] = 'NeedAssessmentDetail' and SysObjects.[Name] like 'DF__NeedAsses__Busin%'
order by [Tab].[Name]) 
EXEC('ALTER TABLE [EarlyWarning].[NeedAssessmentDetail] DROP CONSTRAINT ' + @constraintName)
Alter table EarlyWarning.NeedAssessmentDetail
drop column BusinessProcessID