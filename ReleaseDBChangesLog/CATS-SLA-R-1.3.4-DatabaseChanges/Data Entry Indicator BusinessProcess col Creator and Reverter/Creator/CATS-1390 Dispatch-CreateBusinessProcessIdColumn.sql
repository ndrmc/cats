IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[Dispatch]') 
         AND name = 'BusinessProcessID'
)
 
 BEGIN
alter table 
[dbo].[Dispatch]
add   [BusinessProcessID] [int] Default NULL
 END
