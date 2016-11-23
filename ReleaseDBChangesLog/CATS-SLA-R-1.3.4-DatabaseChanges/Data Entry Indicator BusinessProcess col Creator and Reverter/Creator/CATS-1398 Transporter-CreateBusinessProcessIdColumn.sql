IF  NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[Procurement].[Transporter]') 
         AND name = 'BusinessProcessID'
)
 
 BEGIN
alter table 
Procurement.Transporter
add   [BusinessProcessID] [int] Default NULL
 END
