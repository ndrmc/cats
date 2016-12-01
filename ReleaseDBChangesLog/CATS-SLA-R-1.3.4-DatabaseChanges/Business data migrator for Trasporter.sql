 
 
     DECLARE @businessprocessstateid int, @businessprocessid int;
	DECLARE @Id int;
	DECLARE @rowIds TABLE (primaryid int);
	DECLARE @processTemplateId int;
	DECLARE @stateId int;

	IF EXISTS (SELECT SettingValue FROM ApplicationSetting WHERE 
	dbo.ApplicationSetting.SettingName = 'GlobalWorkflow')
	BEGIN
	SET @processTemplateId = (SELECT SettingValue FROM ApplicationSetting WHERE 
	dbo.ApplicationSetting.SettingName = 'GlobalWorkflow')
	SET @stateId = (SELECT StateTemplateID from StateTemplate where ParentProcessTemplateID = @processTemplateId
	                 and Name = 'Created')
      insert into @rowIds SELECT t.TransporterID -- change it by the primary key of the table 
	 FROM [Procurement].[Transporter] t  -- Change the transporter table by your table
      WHERE t.BusinessProcessID IS NULL OR   t.BusinessProcessID =0

	 
	  While (Select Count(*) From @rowIds) > 0
	  BEGIN

	  Select Top 1 @Id = primaryid From @rowIds

	    INSERT INTO BusinessProcess 
			VALUES (@processTemplateId, 0, 'Transporter', @businessprocessstateid, NULL); -- Change here
			SET @businessprocessid = SCOPE_IDENTITY();

	     INSERT INTO BusinessProcessState 
			VALUES ( @businessprocessid, @stateId, 'System: Data Migration', 
			GETDATE(), 'Transporter Created', NULL, NULL);  --here
			SET @businessprocessstateid = SCOPE_IDENTITY();			

		  UPDATE BusinessProcess
		  SET CurrentStateID = @businessprocessstateid
		  WHERE BusinessProcessID = @businessprocessid

		  -- change the following statement by the table you want to update
		  UPDATE Procurement.Transporter
		  SET
		      Procurement.Transporter.BusinessProcessID= @businessprocessid
		  WHERE Procurement.Transporter.TransporterID = @Id

		  -- Delete the used row id
		   DELETE @rowIds WHERE [@rowIds].primaryid = @Id

	   END
	   End