use [Cats-v-1-3-2]
go

ALTER TABLE Receive
ADD BusinessProcessID int null

Go

Update Receive
Set BusinessProcessId = 0