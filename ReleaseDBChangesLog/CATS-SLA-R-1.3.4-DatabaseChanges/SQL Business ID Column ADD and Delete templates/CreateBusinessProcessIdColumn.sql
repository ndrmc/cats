IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[TableName]') 
         AND name = 'BusinessProcessID'
)
 
 BEGIN
alter table 
[dbo].[TableName]
add   [BusinessProcessID] [int] Default NULL
 END
