<Query Kind="SQL" />

SET ANSI_NULLS ON;

go

SET QUOTED_IDENTIFIER ON;

go
--This function splits names.
CREATE FUNCTION [dbo].[Surname] (@Surnames VARCHAR(35), @ReturnPortion INT)
RETURNS VARCHAR(35)
AS
  BEGIN
      DECLARE @Return     VARCHAR(35),
              @part1      VARCHAR(35),
              @part2      VARCHAR(35),
              @part3      VARCHAR(35),
              @part4      VARCHAR(35),
              @isComplex  INT = ( CHARINDEX(SPACE(1), TRIM(@Surnames), (
                                 CHARINDEX(SPACE(1), TRIM(@Surnames), 1) ) + 1) ),
              @isComplete INT = 0,
              @p          INT,
              @t          INT;

      SET @Surnames = REPLACE(REPLACE(REPLACE(REPLACE(TRIM(@Surnames), SPACE(3),
                                              SPACE(1))
                                      , '.', SPACE(1)), SPACE(2),
                                              SPACE(1)), SPACE(2), SPACE(1));

      IF @isComplex = 0
		BEGIN
			SET @Return = (SELECT item
							FROM   dbo.Splitstring(@Surnames, SPACE(1))
							WHERE  rn = @ReturnPortion);
		END;
      ELSE
        BEGIN
            SET @p = 1;

            WHILE @isComplete = 0
              BEGIN
                  SET @part1 = (SELECT item
                                FROM   dbo.Splitstring(@Surnames, SPACE(1))
                                WHERE  rn = @p);
                  SET @part2 = (SELECT item
                                FROM   dbo.Splitstring(@Surnames, SPACE(1))
                                WHERE  rn = @p + 1);

                  IF LEN(@part1) = 2
                     AND LEN(@part2) = 2
                    BEGIN
                        SET @part4 = CONCAT(@part4, SPACE(1), @part1, SPACE(1), @part2);
                        SET @p = @p + 2;
                    END;
                  ELSE IF LEN(@part1) BETWEEN 1 AND 3
                     AND @p = 1
                    BEGIN
                        SET @part4 = CONCAT(@part4, SPACE(1), @part1);
                        SET @p = @p + 1;
                    END;
                  ELSE IF LEN(@part4) = 0
                    BEGIN
                        SET @part4 = @part1;
                        SET @p = @p + 1;
                        SET @isComplete = 1;
                    END;
                  ELSE
                    BEGIN
                        SET @part4 = CONCAT(@part4, SPACE(1), @part1);
                        SET @p = @p + 1;
                        SET @isComplete = 1;
                    END;

                  IF @part2 IN ( 'I', 'II', 'III', 'IV' )
                    BEGIN
                        SET @part4 = CONCAT(@part4, SPACE(1), @part2);
                        SET @p = @p + 1;
                        SET @isComplete = 1;
                    END;
                  ELSE IF LEN(@part2) = 1
                    BEGIN
                        SET @part4 = CONCAT(@part4, SPACE(1), @part2);
                        SET @p = @p + 1;
                        SET @isComplete = 1;
                    END;
              END;

            SET @Return = TRIM(@part4);

            IF @ReturnPortion = 2
              BEGIN
                  SET @t = @p;
                  SET @part1 = SPACE(0);
                  SET @part2 = SPACE(0);
                  SET @part3 = SPACE(0);
                  SET @part4 = SPACE(0);
                  SET @isComplete = 0;

                  WHILE @isComplete = 0
                    BEGIN
                        SET @part1 = (SELECT item
                                      FROM   dbo.Splitstring(@Surnames, SPACE(1))
                                      WHERE  rn = @p);
                        SET @part2 = (SELECT item
                                      FROM   dbo.Splitstring(@Surnames, SPACE(1))
                                      WHERE  rn = @p + 1);

                        IF LEN(@part1) = 2
                           AND LEN(@part2) = 2
                          BEGIN
                              SET @part4 = CONCAT(@part4, SPACE(1), @part1, SPACE(1),
                                           @part2
                                           );
                              SET @p = @p + 2;
                          END;
                        ELSE IF LEN(@part1) BETWEEN 1 AND 3
                           AND @t = @p
                          BEGIN
                              SET @part4 = CONCAT(@part4, SPACE(1), @part1);
                              SET @p = @p + 1;
                          END;
                        ELSE IF LEN(@part4) = 0
                          BEGIN
                              SET @part4 = @part1;
                              SET @p = @p + 1;
                              SET @isComplete = 1;
                          END;
                        ELSE
                          BEGIN
                              SET @part4 = CONCAT(@part4, SPACE(1), @part1);
                              SET @p = @p + 1;
                              SET @isComplete = 1;
                          END;

                        IF @part2 IN ( 'I', 'II', 'III', 'IV' )
                          BEGIN
                              SET @part4 = CONCAT(@part4, SPACE(1), @part2);
                              SET @p = @p + 1;
                              SET @isComplete = 1;
                          END;
                        ELSE IF LEN(@part2) = 1
                          BEGIN
                              SET @part4 = CONCAT(@part4, SPACE(1), @part2);
                              SET @p = @p + 1;
                              SET @isComplete = 1;
                          END;

                        IF @part2 != SPACE(0)
                          SET @isComplete = 0;
                    END;--end while
                  SET @Return = TRIM(@part4);
              END;
        END;

      RETURN @Return;
  END;