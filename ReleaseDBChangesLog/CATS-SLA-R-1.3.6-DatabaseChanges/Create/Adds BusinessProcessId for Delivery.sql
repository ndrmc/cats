use [Cats-v-1-3-2]
go

ALTER TABLE Delivery
ADD BusinessProcessID int null

Go

Update Delivery
Set BusinessProcessId = 0