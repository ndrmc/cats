USE [CatsDRMFSS]
GO
/****** Object:  StoredProcedure [dbo].[SPReceiveAllocationAggr]    Script Date: 7/31/2016 4:59:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nathnael Getahun
-- Create date: July 31, 2016
-- Description:	Stored procedure that selects ReceiveAllocations with their respective aggregated received quantities
-- =============================================
DROP PROCEDURE [dbo].[SPReceiveAllocationAggr] 