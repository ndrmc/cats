--Create CATS-1315
--- Created BY Robel Alazar

use 
[Cats-v-1-3-1]
go
IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[LoanReciptPlan]') 
         AND name = 'BusinessProcessID'
)
 --check if BusinessProcessID exist
 BEGIN
alter table 
[dbo].[LoanReciptPlan]
add   [BusinessProcessID] [int] Default 0 NOT NULL
 END

declare @ProcessTemplateID table (id int);
IF Not EXISTS (SELECT * FROM [dbo].[ProcessTemplate] WHERE [Name] = 'Recipt Plan For Loan')
BEGIN
  
   INSERT INTO [dbo].[ProcessTemplate]
           ([Name]
           ,[Description]
           ) Output Inserted.ProcessTemplateID INTO @ProcessTemplateID
     VALUES
           ('Recipt Plan For Loan',
            'Work flow for Recipt Plan For Loan'
		   )
		declare @id int  
		declare @draft int 
		declare @Approve int
		declare @Rejected int 
		declare @signed int 
		declare @closed int  
		select @id = Scope_Identity()  
		 INSERT INTO [dbo].[ApplicationSetting]
           ([SettingName]
           ,[SettingValue]
           )
     VALUES
           ('ReciptPlanForLoanWorkflow'
           ,@id
           )
	INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
           ( @id,'Draft',0,0,0 )  select @draft = Scope_Identity()
		   INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Approved',0,1,1 )  select @Approve = Scope_Identity()
		    INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Rejected',0,2,1 )  select @Rejected = Scope_Identity() 		   
		    INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Signed',0,2,1 )  select @signed = Scope_Identity()   
     INSERT INTO [dbo].[StateTemplate]
           ([ParentProcessTemplateID] ,[Name],[AllowedAccessLevel],[StateNo]
           ,[StateType]
           )
     VALUES
		   ( @id,'Closed',0,3,1 )  select @closed = Scope_Identity() 

		   INSERT INTO [dbo].[FlowTemplate]
           ([ParentProcessTemplateID]
           ,[InitialStateID]
           ,[FinalStateID]
           ,[Name]
           )
     VALUES
           (@id ,@draft,@Approve,'Approve'),
		   (@id ,@Rejected,@Approve,'Approve'),
		   (@id ,@Approve,@Rejected,'Reject'),
		   (@id ,@Approve,@signed,'Sign'),
		   (@id ,@signed,@closed,'Close')
		   
		
		  
END
GO
GO