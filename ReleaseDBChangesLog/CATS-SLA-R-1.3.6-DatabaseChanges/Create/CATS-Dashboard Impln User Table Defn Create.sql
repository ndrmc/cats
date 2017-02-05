USE [Cats-v-1-3-1]
GO

/****** Object:  UserDefinedTableType [dbo].[FilterArray]    Script Date: 12/2/2016 3:45:47 PM ******/
CREATE TYPE [dbo].[FilterArray] AS TABLE(
	[Filter] [nvarchar](50) NULL
)
GO


