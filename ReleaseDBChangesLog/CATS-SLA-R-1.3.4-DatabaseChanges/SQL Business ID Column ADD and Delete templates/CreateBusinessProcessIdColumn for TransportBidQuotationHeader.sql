IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[Procurement].[TransportBidQuotationHeader]') 
         AND name = 'BusinessProcessID'
)
 
 BEGIN
alter table 
[Procurement].[TransportBidQuotationHeader]
add   [BusinessProcessID] [int] Default NULL
 END
