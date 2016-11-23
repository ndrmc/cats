IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[DispatchAllocation]') 
         AND name = 'BusinessProcessID'
)
 
 BEGIN
alter table 
[dbo].[DispatchAllocation]
add   [BusinessProcessID] [int] Default NULL
 END
