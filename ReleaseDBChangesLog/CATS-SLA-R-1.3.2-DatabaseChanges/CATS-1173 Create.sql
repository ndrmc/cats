
--- Script for issue CATS-1173
ALTER TABLE dbo.Hub ADD HubParentID INT;

UPDATE dbo.Hub SET HubParentID= 1 WHERE HubID = 1
UPDATE dbo.Hub SET HubParentID= 2 WHERE HubID = 2
UPDATE dbo.Hub SET HubParentID= 3 WHERE HubID = 3
UPDATE dbo.Hub SET HubParentID= 1 WHERE HubID = 5
UPDATE dbo.Hub SET HubParentID= 3 WHERE HubID = 6
UPDATE dbo.Hub SET HubParentID= 1 WHERE HubID = 7
UPDATE dbo.Hub SET HubParentID= 2 WHERE HubID = 8
UPDATE dbo.Hub SET HubParentID= 2 WHERE HubID = 9
UPDATE dbo.Hub SET HubParentID= 1 WHERE HubID = 10
UPDATE dbo.Hub SET HubParentID= 11 WHERE HubID = 11
UPDATE dbo.Hub SET HubParentID= 1 WHERE HubID = 12
UPDATE dbo.Hub SET HubParentID= 3 WHERE HubID = 13
UPDATE dbo.Hub SET HubParentID= 1 WHERE HubID = 14
UPDATE dbo.Hub SET HubParentID= 2 WHERE HubID = 15
UPDATE dbo.Hub SET HubParentID= 3 WHERE HubID = 16
UPDATE dbo.Hub SET HubParentID= 11 WHERE HubID = 4015