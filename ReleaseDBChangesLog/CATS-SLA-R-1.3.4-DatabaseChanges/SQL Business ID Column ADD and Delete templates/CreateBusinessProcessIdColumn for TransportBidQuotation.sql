IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[Procurement].[TransportBidQuotation]') 
         AND name = 'BusinessProcessID'
)
 
 BEGIN
alter table 
[Procurement].[TransportBidQuotation]
add   [BusinessProcessID] [int] Default NULL
 END
