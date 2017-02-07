use [Cats-v-1-3-2]
go

/****** Object:  StoredProcedure [dbo].[GenericDashboardDataProvider]    Script Date: 12/16/2016 9:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Yonathan
-- Create date: Fri, Dec 2 2016
-- Description: This proc accepts five parameters and outputs aggregated data which will be used for dashboard stat. info..
-- =============================================
CREATE PROCEDURE [dbo].[GenericDashboardDataProvider]
  -- Add the parameters for the stored procedure here
  @StartDate          DATE,
  @EndDate            DATE,
  @WorkflowName_Array AS dbo.FILTERARRAY readonly,
  @User_Array         AS dbo.FILTERARRAY readonly,
  @Activity_Array     AS dbo.FILTERARRAY readonly
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET nocount ON;
      SET dateformat dmy;
      SET @StartDate = Cast(@StartDate AS DATE);
      SET @EndDate = Cast(@EndDate AS DATE);

      DECLARE @IsAllUser NVARCHAR(50);
      DECLARE @Users TABLE
        (
           NAME NVARCHAR(50) NULL
        )

      SET @IsAllUser = (SELECT *
                        FROM   @User_Array
                        WHERE  filter = 'ALL with Data')

      IF @IsAllUser = 'ALL with Data'
        BEGIN
            INSERT INTO @Users
            SELECT username
            FROM   userprofile
        END
      ELSE
        BEGIN
            INSERT INTO @Users
            SELECT filter
            FROM   @User_Array
        END

      SELECT Row_number()
               OVER(
                 ORDER BY performedby ASC)                    AS Row,
             statetemplate.NAME                               AS ActivityName,
             --  
             0                                                AS ActivityCount,
             -- Frequency   
             --Count(StateTemplate.Name) AS ActivityCount, -- Frequency 
             businessprocessstate.performedby,-- 
             applicationsetting.settingname,
             CONVERT(INT, applicationsetting.settingvalue)    AS
             ProcessTemplateID
             ,
             businessprocess.processtypeid,
             statetemplate.statetemplateid,
             businessprocess.businessprocessid,
             Cast(businessprocessstate.dateperformed AS DATE) AS DatePerformed
      FROM

     statetemplate
      INNER JOIN businessprocessstate
              ON statetemplate.statetemplateid = businessprocessstate.stateid
      INNER JOIN businessprocess
                      
              ON businessprocessstate.parentbusinessprocessid =
                 businessprocess.businessprocessid
      INNER JOIN applicationsetting
              ON statetemplate.parentprocesstemplateid =
                 applicationsetting.settingvalue
      WHERE
        --(BusinessProcessState.DatePerformed >= @StartDate AND BusinessProcessState.DatePerformed <= @EndDate) AND  
        applicationsetting.settingname IN (SELECT *
                                           FROM   @WorkflowName_Array)
        AND businessprocessstate.performedby IN (SELECT *
                                                 FROM   @Users)
        AND statetemplate.NAME IN (SELECT *
                                   FROM   @Activity_Array)
      GROUP  BY statetemplate.NAME,
                businessprocessstate.performedby,
                applicationsetting.settingname,
                applicationsetting.settingvalue,
                businessprocess.processtypeid,
                statetemplate.statetemplateid,
                businessprocess.businessprocessid,
                businessprocessstate.dateperformed
      ORDER  BY businessprocessstate.performedby,
                statetemplate.NAME,
                businessprocessstate.dateperformed
  END  