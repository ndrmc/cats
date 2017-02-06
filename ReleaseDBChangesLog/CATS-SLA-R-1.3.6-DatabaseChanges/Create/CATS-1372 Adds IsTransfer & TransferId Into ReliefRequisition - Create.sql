
ALTER TABLE  Earlywarning.ReliefRequisition
ADD IsTransfer bit NULL
go

ALTER TABLE  Earlywarning.ReliefRequisition
ADD TransferId int NULL
go

Update EarlyWarning.ReliefRequisition
Set IsTransfer = 0
Go