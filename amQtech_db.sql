USE [amQtech_db]
GO
/****** Object:  UserDefinedFunction [dbo].[amQt_fnDepreciationScheduleStraightLineActualDays]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[amQt_fnDepreciationScheduleStraightLineActualDays]
(	
	@id INT = 5,
	@year SMALLINT = 2023
)
RETURNS @table_dates TABLE(start_date DATE, end_date DATE, year INT, beginning_book_value DECIMAL(19,2), depreciation_rate DECIMAL(18,0), beginning DECIMAL(18,2), depreciation_expense DECIMAL(18,2), accumulated_depreciation DECIMAL(18,2), ending_book_value DECIMAL(18,2))
AS
BEGIN
	DECLARE @purchase_date DATE,
			@end_depreciation_date DATE,
			@start_date DATE,
			@end_date DATE,
			@purchase_cost DECIMAL(18,2),
			@residual_value DECIMAL(18,2),
			@useful_life_years INT

	SELECT @purchase_date = purchase_date,
		@purchase_cost = purchase_price,
		@residual_value = residual_value,
		@useful_life_years = useful_life_years
	FROM amQt_FixedAsset
	WHERE id = @id
		
	DECLARE @depcreciation_rate FLOAT,--float so that the aboslute value of depreciation expense is 100% accurate
		@depcreciation_rate_yearly FLOAT = 100.0 / @useful_life_years,
		@depreciation_expense DECIMAL(18,2)

	SET @end_depreciation_date = DATEADD(D, -1, DATEADD(YEAR, @useful_life_years, @purchase_date))
	SET @start_date = @purchase_date

	DECLARE @start_date_constant DATE = @start_date
	DECLARE @days_in_year INT = DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), DATEFROMPARTS(YEAR(@start_date), 12, 31)) + 1

	DECLARE @accumulated_depreciation DECIMAL(18,2) = 0,
			@beginning_book_value DECIMAL(18,2) = @purchase_cost,
			@ending_book_value DECIMAL(18,2) = @purchase_cost		

	WHILE @start_date <= @end_depreciation_date
	BEGIN

		IF @start_date > @purchase_date --second row
			SET @start_date = DATEFROMPARTS(YEAR(@start_date), 1, 1) --always start and jan 1
	
		SET @end_date = DATEFROMPARTS(YEAR(@start_date), 12, 31)
		SET @depcreciation_rate = 100.0 / @useful_life_years / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1) / 100.0
	
		IF YEAR(@end_date) = YEAR(@end_depreciation_date) --last row
		BEGIN		
			SET @end_date = @end_depreciation_date
			IF @end_depreciation_date != DATEFROMPARTS(YEAR(@end_depreciation_date), 12, 31)
			BEGIN 
				IF MONTH(@purchase_date) != 1 
				BEGIN		
					IF DAY(@purchase_date) = 1 --The asset was acquired on the 1st day of any month, other than January
						SET @depcreciation_rate = @depcreciation_rate / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1)
					ELSE 
						SET @depcreciation_rate = @depcreciation_rate / @days_in_year * DATEDIFF(D, @start_date, @end_date)	
				END
				ELSE --The asset was acquired on the any day on January
					SET @depcreciation_rate = @depcreciation_rate / @days_in_year * (DATEDIFF(D, @start_date, @end_date) + 1)						
			END
		END
		ELSE
		BEGIN
			IF DAY(@start_date) != 1
				SET @depcreciation_rate = @depcreciation_rate_yearly / @days_in_year * (DATEDIFF(D, @start_date, @end_date) + 1) / 100.0
		END
		SET @depreciation_expense = @depcreciation_rate * (@purchase_cost - @residual_value)
		SET @ending_book_value = @ending_book_value - @depreciation_expense
	
		IF @accumulated_depreciation IS NULL SET @accumulated_depreciation = @depreciation_expense --first row
		ELSE SET @accumulated_depreciation = @accumulated_depreciation + @depreciation_expense
	
		INSERT INTO @table_dates (start_date, end_date, year, beginning_book_value, depreciation_rate, beginning, depreciation_expense, accumulated_depreciation, ending_book_value) 
		VALUES (@start_date, @end_date, YEAR(@start_date), @beginning_book_value, @depcreciation_rate * 100.00, @accumulated_depreciation - @depreciation_expense, @depreciation_expense, @accumulated_depreciation, @ending_book_value)
	
		SET @beginning_book_value = @beginning_book_value - @depreciation_expense --second row for beginning_book_value
		SET @start_date = DATEADD(YEAR, 1, @start_date)
	END


	RETURN
END

GO
/****** Object:  UserDefinedFunction [dbo].[amQt_fnDepreciationScheduleStraightLineFullMonth]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[amQt_fnDepreciationScheduleStraightLineFullMonth]
(	
	@id INT = 5,
	@year SMALLINT = 2023
)
RETURNS @table_dates TABLE(start_date DATE, end_date DATE, year INT, beginning_book_value DECIMAL(19,2), depreciation_rate DECIMAL(18,0), beginning DECIMAL(18,2), depreciation_expense DECIMAL(18,2), accumulated_depreciation DECIMAL(18,2), ending_book_value DECIMAL(18,2),
	[Jan] DECIMAL(18,2), [Feb] DECIMAL(18,2), [Mar] DECIMAL(18,2), [Apr] DECIMAL(18,2), [May] DECIMAL(18,2), [Jun] DECIMAL(18,2), [Jul] DECIMAL(18,2), [Aug] DECIMAL(18,2), [Sep] DECIMAL(18,2), [Oct] DECIMAL(18,2), [Nov] DECIMAL(18,2), [Dec] DECIMAL(18,2))
AS
BEGIN
	DECLARE @purchase_date DATE,
			@end_depreciation_date DATE,
			@start_date DATE,
			@end_date DATE,
			@purchase_cost DECIMAL(18,2),
			@residual_value DECIMAL(18,2),
			@useful_life_years INT
		
	SELECT @purchase_date = purchase_date,
		@purchase_cost = purchase_price,
		@residual_value = residual_value,
		@useful_life_years = useful_life_years
	FROM amQt_FixedAsset
	WHERE id = @id
		
	DECLARE @depcreciation_rate FLOAT,--float so that the aboslute value of depreciation expense is 100% accurate
		@depcreciation_rate_yearly FLOAT = 100.0 / @useful_life_years,
		@depreciation_expense DECIMAL(18,2)
	SET @end_depreciation_date = DATEADD(D, -1, DATEADD(YEAR, @useful_life_years, DATEFROMPARTS(YEAR(@purchase_date), MONTH(@purchase_date), 1)))
	SET @start_date = @purchase_date--DATEFROMPARTS(YEAR(@purchase_date), MONTH(@purchase_date), 1)

	--IF DAY(@purchase_date) > 15 --16thday onwards
	--	SET @start_date = DATEADD(M, 1, @start_date)

	DECLARE @start_date_constant DATE = @start_date
	DECLARE @days_in_year INT = DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), DATEFROMPARTS(YEAR(@start_date), 12, 31)) + 1

	DECLARE @accumulated_depreciation DECIMAL(18,2) = 0,
			@beginning_book_value DECIMAL(18,2) = @purchase_cost,
			@ending_book_value DECIMAL(18,2) = @purchase_cost
		

	WHILE @start_date <= @end_depreciation_date
	BEGIN

		IF @start_date > @purchase_date --second row
			SET @start_date = DATEFROMPARTS(YEAR(@start_date), 1, 1) --always start and jan 1
	
		SET @end_date = DATEFROMPARTS(YEAR(@start_date), 12, 31)
		SET @depcreciation_rate = 100.0 / @useful_life_years / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1) / 100.0
	
		IF YEAR(@end_date) = YEAR(@end_depreciation_date) --last row
		BEGIN		
			SET @end_date = @end_depreciation_date
			IF @end_depreciation_date != DATEFROMPARTS(YEAR(@end_depreciation_date), 12, 31)
			BEGIN 
				IF MONTH(@purchase_date) != 1 
					SET @depcreciation_rate = @depcreciation_rate / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1)
				ELSE --The asset was acquired on the any day on January
					SET @depcreciation_rate = @depcreciation_rate / @days_in_year * (DATEDIFF(D, @start_date, @end_date) + 1)						
			END
		END

		SET @depreciation_expense = @depcreciation_rate * (@purchase_cost - @residual_value)
		SET @ending_book_value = @ending_book_value - @depreciation_expense
	
		IF @accumulated_depreciation IS NULL SET @accumulated_depreciation = @depreciation_expense --first row
		ELSE SET @accumulated_depreciation = @accumulated_depreciation + @depreciation_expense
	
		INSERT INTO @table_dates (start_date, end_date, year, beginning_book_value, depreciation_rate, beginning, depreciation_expense, accumulated_depreciation, ending_book_value) 
		VALUES (@start_date, @end_date, YEAR(@start_date), @beginning_book_value, @depcreciation_rate * 100.00, @accumulated_depreciation - @depreciation_expense, @depreciation_expense, @accumulated_depreciation, @ending_book_value)
	
		SET @beginning_book_value = @beginning_book_value - @depreciation_expense --second row for beginning_book_value
		SET @start_date = DATEADD(YEAR, 1, @start_date)
	END

	RETURN
END

GO
/****** Object:  UserDefinedFunction [dbo].[amQt_fnDepreciationScheduleSYDActualDays]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[amQt_fnDepreciationScheduleSYDActualDays]
(	
	@id INT = 5,
	@year SMALLINT = 2023
)
RETURNS @table_dates TABLE(start_date DATE, end_date DATE, year INT, beginning_book_value DECIMAL(19,2), beginning DECIMAL(18,2), depreciation_expense DECIMAL(18,2), accumulated_depreciation DECIMAL(18,2), ending_book_value DECIMAL(18,2))
AS
BEGIN
	DECLARE @purchase_date DATE,
			@end_depreciation_date DATE,
			@start_date DATE,
			@end_date DATE,
			@purchase_cost DECIMAL(18,2),
			@residual_value DECIMAL(18,2),
			@useful_life_years INT

	SELECT @purchase_date = purchase_date,
		@purchase_cost = purchase_price,
		@residual_value = residual_value,
		@useful_life_years = useful_life_years
	FROM amQt_FixedAsset
	WHERE id = @id
		
	DECLARE @depreciation_expense FLOAT
	SET @end_depreciation_date = DATEADD(D, -1, DATEADD(YEAR, @useful_life_years, @purchase_date))

	SET @start_date = @purchase_date--DATEFROMPARTS(YEAR(@purchase_date), MONTH(@purchase_date), 1)

	--IF DAY(@purchase_date) > 15 --16thday onwards
	--	SET @start_date = DATEADD(M, 1, @start_date)

	DECLARE @start_date_constant DATE = @start_date
	DECLARE @days_in_year FLOAT = DATEPART(dy,DATEFROMPARTS(YEAR(@start_date),12,31))

	DECLARE @accumulated_depreciation FLOAT = 0,
			@beginning_book_value FLOAT = @purchase_cost,
			@ending_book_value FLOAT = @purchase_cost,
			@total_months FLOAT,
			@estimated_life_remaining INT = @useful_life_years,
			@dep_prev FLOAT = 0,
			@dep FLOAT = 0,
			@a FLOAT = 0,
			@b FLOAT
		

	WHILE @start_date <= @end_depreciation_date
	BEGIN

		SET @dep = (@purchase_cost - @residual_value) * (@estimated_life_remaining / (@useful_life_years * (@useful_life_years + 1) / 2.0))

		IF @start_date > @purchase_date --second row
			SET @start_date = DATEFROMPARTS(YEAR(@start_date), 1, 1) --always start and jan 1
	
		IF YEAR(@start_date) = YEAR(@end_depreciation_date) --last row
			SET @end_date = @end_depreciation_date
		ELSE
			SET @end_date = DATEFROMPARTS(YEAR(@start_date), 12, 31)
	
		SET @total_months = DATEDIFF(M, @start_date, @end_date) + 1
		IF YEAR(@start_date) = YEAR(@purchase_date) --first row
		BEGIN 
			IF DAY(@start_date) != 1--The asset was acquired on the any day of the month
			BEGIN
				SET @a = DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), @start_date) / @days_in_year
				SET @b = (@days_in_year - DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), @start_date)) / @days_in_year
			END
			ELSE
			BEGIN
				SET @a = (12 - @total_months) / 12.0
				SET @b = @total_months / 12.0		
			END
		END		
	
		SET @depreciation_expense = @dep_prev * @a + @dep * @b
		SET @ending_book_value = @ending_book_value - @depreciation_expense
	
		IF @accumulated_depreciation IS NULL SET @accumulated_depreciation = @depreciation_expense --first row
		ELSE SET @accumulated_depreciation = @accumulated_depreciation + @depreciation_expense
	
		INSERT INTO @table_dates (start_date, end_date, year, beginning_book_value, beginning, depreciation_expense, accumulated_depreciation, ending_book_value) 
		VALUES (@start_date, @end_date, YEAR(@start_date), @beginning_book_value, @accumulated_depreciation - @depreciation_expense, @depreciation_expense, @accumulated_depreciation, @ending_book_value)

		SET @dep_prev = @dep
		SET @beginning_book_value = @beginning_book_value - @depreciation_expense --second row for beginning_book_value
		SET @start_date = DATEADD(YEAR, 1, @start_date)
		SET @estimated_life_remaining = @estimated_life_remaining - 1
	END


	RETURN
END


GO
/****** Object:  UserDefinedFunction [dbo].[amQt_fnDepreciationScheduleSYDFullMonth]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[amQt_fnDepreciationScheduleSYDFullMonth]
(	
	@id INT = 5,
	@year SMALLINT = 2023
)
RETURNS @table_dates TABLE(start_date DATE, end_date DATE, year INT, beginning_book_value DECIMAL(19,2), depreciation_rate DECIMAL(18,0), beginning DECIMAL(18,2), depreciation_expense DECIMAL(18,2), accumulated_depreciation DECIMAL(18,2), ending_book_value DECIMAL(18,2),
	[Jan] FLOAT, [Feb] FLOAT, [Mar] FLOAT, [Apr] FLOAT, [May] FLOAT, [Jun] FLOAT, [Jul] FLOAT, [Aug] FLOAT, [Sep] FLOAT, [Oct] FLOAT, [Nov] FLOAT, [Dec] FLOAT)
AS
BEGIN
	DECLARE @purchase_date DATE,
			@end_depreciation_date DATE,
			@start_date DATE,
			@end_date DATE,
			@purchase_cost DECIMAL(18,2),
			@residual_value DECIMAL(18,2),
			@useful_life_years INT
		
	SELECT @purchase_date = purchase_date,
		@purchase_cost = purchase_price,
		@residual_value = residual_value,
		@useful_life_years = useful_life_years
	FROM amQt_FixedAsset
	WHERE id = @id
		
	DECLARE @depreciation_expense FLOAT
	SET @end_depreciation_date = DATEADD(D, -1, DATEADD(YEAR, @useful_life_years, @purchase_date))
	SET @start_date = @purchase_date--DATEFROMPARTS(YEAR(@purchase_date), MONTH(@purchase_date), 1)

	DECLARE @start_date_constant DATE = @start_date
	DECLARE @days_in_year INT = DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), DATEFROMPARTS(YEAR(@start_date), 12, 31)) + 1

	DECLARE @accumulated_depreciation FLOAT = 0,
			@beginning_book_value FLOAT = @purchase_cost,
			@ending_book_value FLOAT = @purchase_cost,
			@total_months FLOAT,
			@estimated_life_remaining INT = @useful_life_years,
			@dep_prev FLOAT = 0,
			@dep FLOAT = 0,
			@a FLOAT = 0,
			@b FLOAT = 0

	WHILE @start_date <= @end_depreciation_date
	BEGIN

		SET @dep = (@purchase_cost - @residual_value) * (@estimated_life_remaining / (@useful_life_years * (@useful_life_years + 1) / 2.0))

		IF @start_date > @purchase_date --second row
			SET @start_date = DATEFROMPARTS(YEAR(@start_date), 1, 1) --always start and jan 1
	
		IF YEAR(@start_date) = YEAR(@end_depreciation_date) --last row
			SET @end_date = @end_depreciation_date
		ELSE
			SET @end_date = DATEFROMPARTS(YEAR(@start_date), 12, 31)
	
		SET @total_months = DATEDIFF(M, @start_date, @end_date) + 1
		IF YEAR(@start_date) = YEAR(@purchase_date) --first row
		BEGIN
			SET @a = (12 - @total_months) / 12.0
			SET @b = @total_months / 12.0		
		END		
	
		SET @depreciation_expense = @dep_prev * @a + @dep * @b
		SET @ending_book_value = @ending_book_value - @depreciation_expense
	
		IF @accumulated_depreciation IS NULL SET @accumulated_depreciation = @depreciation_expense --first row
		ELSE SET @accumulated_depreciation = @accumulated_depreciation + @depreciation_expense
	
		INSERT INTO @table_dates (start_date, end_date, year, beginning_book_value, beginning, depreciation_expense, accumulated_depreciation, ending_book_value) 
		VALUES (@start_date, @end_date, YEAR(@start_date), @beginning_book_value, @accumulated_depreciation - @depreciation_expense, @depreciation_expense, @accumulated_depreciation, @ending_book_value)
	
		SET @dep_prev = @dep
		SET @beginning_book_value = @beginning_book_value - @depreciation_expense --second row for beginning_book_value
		SET @start_date = DATEADD(YEAR, 1, @start_date)
		SET @estimated_life_remaining = @estimated_life_remaining - 1
	END

	RETURN
END


GO
/****** Object:  UserDefinedFunction [dbo].[amQt_fnTransactionNo]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[amQt_fnTransactionNo]
(
	@date	DATETIME = NULL,
	@id		INT = 0
)
RETURNS VARCHAR(50)
AS
BEGIN
	
	RETURN ISNULL(CAST(YEAR(@date) AS VARCHAR) + '-' + REPLACE(STR(@id, 6), SPACE(1), '0'), '')

END


GO
/****** Object:  Table [dbo].[amQt_AccountClassification]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_AccountClassification](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AccountClassification_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AccountClassification_name]  DEFAULT (''),
	[post] [bit] NOT NULL CONSTRAINT [DF_amQt_AccountClassification_post]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_AccountClassification_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_AccountClassification] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_AccountGroup]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_AccountGroup](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AccountGroup_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AccountGroup_name]  DEFAULT (''),
	[post] [bit] NOT NULL CONSTRAINT [DF_amQt_AccountGroup_post]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_AccountGroup_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_AccountGroup] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_AccountType]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_AccountType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AccountType_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AccountType_name]  DEFAULT (''),
	[post] [bit] NOT NULL CONSTRAINT [DF_amQt_AccountType_post]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_AccountType_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_AccountType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_AccumulatedDepreciationAccount]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_AccumulatedDepreciationAccount](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AccumulatedDepreciationAccount_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AccumulatedDepreciationAccount_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_AccumulatedDepreciationAccount_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_AccumulatedDepreciationAccount] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_AssetAccount]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_AssetAccount](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AssetAccount_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AssetAccount_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_AssetAccount_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_AssetAccount] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_AssetCategory]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_AssetCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AssetCategory_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AssetCategory_name]  DEFAULT (''),
	[post] [bit] NOT NULL CONSTRAINT [DF_amQt_AssetCategory_post]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_AssetCategory_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_AssetCategory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_AssetClass]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_AssetClass](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AssetClass_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AssetClass_name]  DEFAULT (''),
	[post] [bit] NOT NULL CONSTRAINT [DF_amQt_AssetClass_post]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_AssetClass_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_AssetClass] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_AssetType]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_AssetType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AssetType_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AssetType_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_AssetType_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_AssetType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_AveragingMethod]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_AveragingMethod](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AveragingMethod_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_AveragingMethod_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_AveragingMethod_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_AveragingMethod] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_ChartOfAccount]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_ChartOfAccount](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_ChartOfAccount_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_ChartOfAccount_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_ChartOfAccount_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_ChartOfAccount] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_CompanyProfile]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_CompanyProfile](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](150) NOT NULL CONSTRAINT [DF_amQt_CompanyProfile_name]  DEFAULT (''),
	[address] [varchar](1500) NOT NULL CONSTRAINT [DF_amQt_CompanyProfile_address]  DEFAULT (''),
	[report_logo] [varbinary](max) NULL,
	[width] [int] NOT NULL CONSTRAINT [DF_amQt_CompanyProfile_width]  DEFAULT ((0)),
	[height] [int] NOT NULL CONSTRAINT [DF_amQt_CompanyProfile_height]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_CompanyProfile_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_CompanyProfile] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_Currency]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_Currency](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Currency_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Currency_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_Currency_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_Currency] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_DepreciationExpenseAccount]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_DepreciationExpenseAccount](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_DepreciationExpenseAccount_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_DepreciationExpenseAccount_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_DepreciationExpenseAccount_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_DepreciationExpenseAccount] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_DepreciationJournal]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_DepreciationJournal](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fixed_asset_id] [int] NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_fixed_asset_id]  DEFAULT ((0)),
	[year] [smallint] NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_year]  DEFAULT ((0)),
	[month] [tinyint] NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_month]  DEFAULT ((0)),
	[depreciation_expense_account_id] [int] NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_depreciation_expense_account_id]  DEFAULT ((0)),
	[depreciation_expense_account_debit_credit] [bit] NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_debit_credit]  DEFAULT ((0)),
	[accumulated_depreciation_account_id] [int] NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_accumulated_depreciation_account_id]  DEFAULT ((0)),
	[accumulated_depreciation_account_debit_credit] [bit] NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_accumulated_depreciation_account_debit_credit]  DEFAULT ((0)),
	[amount] [decimal](18, 2) NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_amount]  DEFAULT ((0)),
	[description] [varchar](500) NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_description]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_DepreciationJournal_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_DepreciationJournal] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_DepreciationJournalDetail]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_DepreciationJournalDetail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[depreciation_journal_id] [int] NOT NULL,
	[depreciation_expense_account_id] [int] NOT NULL,
	[accumulated_depreciation_account_id] [int] NOT NULL,
	[debit_credit] [bit] NOT NULL,
	[amount] [decimal](18, 2) NOT NULL,
	[disable] [bit] NOT NULL,
 CONSTRAINT [PK_amQt_DepreciationJournalDetail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_DepreciationMethod]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_DepreciationMethod](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_DepreciationMethod_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_DepreciationMethod_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_DepreciationMethod_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_DepreciationMethod] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_ExpenseCategory]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_ExpenseCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_ExpenseCategory_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_ExpenseCategory_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_ExpenseCategory_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_ExpenseCategory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_FaultArea]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_FaultArea](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FaultArea_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FaultArea_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_FaultArea_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_FaultArea] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_FaultSymptoms]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_FaultSymptoms](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FaultSymptoms_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FaultSymptoms_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_FaultSymptoms_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_FaultSymptoms] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_FixedAsset]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_FixedAsset](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date_filed] [datetime] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_date_filed]  DEFAULT (getdate()),
	[number] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_number]  DEFAULT ((0)),
	[asset_no] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FixedAsset_asset_no]  DEFAULT (''),
	[product_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_product_id]  DEFAULT ((0)),
	[receiving_detail_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_receiving_detail_id]  DEFAULT ((0)),
	[asset_type_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_asset_type_id]  DEFAULT ((0)),
	[functional_location_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_functional_location_id]  DEFAULT ((0)),
	[description] [varchar](150) NOT NULL CONSTRAINT [DF_amQt_FixedAsset_description]  DEFAULT (''),
	[purchase_date] [date] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_purchase_date]  DEFAULT (getdate()),
	[purchase_price] [decimal](18, 4) NOT NULL CONSTRAINT [DF_amQt_FixedAsset_purchase_price]  DEFAULT ((0)),
	[warranty_expiry] [date] NULL,
	[serial_no] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FixedAsset_serial_no]  DEFAULT (''),
	[model] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FixedAsset_model]  DEFAULT (''),
	[depreciation_start_date] [date] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_depreciation_start_date]  DEFAULT (getdate()),
	[depreciation_method_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_depreciation_method_id]  DEFAULT ((0)),
	[averaging_method_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_averaging_method_id]  DEFAULT ((0)),
	[residual_value] [decimal](18, 4) NOT NULL CONSTRAINT [DF_amQt_FixedAsset_residual_value]  DEFAULT ((0)),
	[useful_life_years] [smallint] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_useful_life_years]  DEFAULT ((0)),
	[is_draft] [bit] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_is_draft]  DEFAULT ((0)),
	[is_registered] [bit] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_is_registered]  DEFAULT ((0)),
	[is_disposed] [bit] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_is_disposed]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_FixedAsset_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_FixedAsset] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_FixedAssetDispose]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_FixedAssetDispose](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fixed_asset_id] [int] NOT NULL,
	[date_disposed] [date] NOT NULL,
	[sale_proceeds] [decimal](18, 4) NOT NULL,
	[cash_account_id] [int] NOT NULL,
	[gain_loss_account_id] [int] NOT NULL,
	[depreciation_financial_year] [varchar](50) NOT NULL,
	[depreciation_date] [date] NOT NULL,
	[disable] [bit] NOT NULL,
 CONSTRAINT [PK_amQt_FixedAssetDispose] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_FixedAssetSetting]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_FixedAssetSetting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[asset_type_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSetting_asset_type_id]  DEFAULT ((0)),
	[asset_class_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSetting_asset_class_id]  DEFAULT ((0)),
	[chart_of_account_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSetting_chart_of_account_id]  DEFAULT ((0)),
	[accumulated_depreciation_account_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSetting_accumulated_depreciation_account_id]  DEFAULT ((0)),
	[depreciation_expense_account_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSetting_depreciation_expense_account_id]  DEFAULT ((0)),
	[depreciation_method_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSetting_depreciation_method_id]  DEFAULT ((0)),
	[averaging_method_id] [int] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSetting_averaging_method_id]  DEFAULT ((0)),
	[useful_life_years] [decimal](18, 4) NOT NULL CONSTRAINT [DF_amQt_FixedAssetSetting_useful_life_years]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSetting_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_FixedAssetSetting] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_FixedAssetSettingDate]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_FixedAssetSettingDate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSettingDate_date]  DEFAULT (getdate()),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_FixedAssetSettingDate_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_FixedAssetSettingDate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_FunctionalLocation]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_FunctionalLocation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FuntionalLocation_fl_id]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FuntionalLocation_name]  DEFAULT (''),
	[parent_fl_id] [int] NOT NULL CONSTRAINT [DF_amQt_FuntionalLocation_parent_fl_id]  DEFAULT (''),
	[fl_status] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FuntionalLocation_fl_status]  DEFAULT (''),
	[address_name] [varchar](150) NOT NULL CONSTRAINT [DF_amQt_FuntionalLocation_address_name]  DEFAULT (''),
	[street] [varchar](150) NOT NULL CONSTRAINT [DF_Table_1_address_street]  DEFAULT (''),
	[city] [varchar](150) NOT NULL CONSTRAINT [DF_Table_1_address_city]  DEFAULT (''),
	[province] [varchar](150) NOT NULL CONSTRAINT [DF_Table_1_address_province]  DEFAULT (''),
	[country] [varchar](150) NOT NULL CONSTRAINT [DF_Table_1_address_country]  DEFAULT (''),
	[zip_code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_FuntionalLocation_zip_code]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_FuntionalLocation_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_FuntionalLocation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_MaintenanceJobType]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_MaintenanceJobType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_MaintenanceJobType_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_MaintenanceJobType_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_MaintenanceJobType_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_MaintenanceJobType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_MaintenanceJobTypeVariant]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_MaintenanceJobTypeVariant](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[maintenance_job_type_id] [int] NOT NULL CONSTRAINT [DF_amQt_MaintenanceJobTypeVariant_maintenance_job_type_id]  DEFAULT ((0)),
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_MaintenanceJobTypeVariant_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_MaintenanceJobTypeVariant_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_MaintenanceJobTypeVariant_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_MaintenanceJobTypeVariant] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_MaintenanceRequest]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_MaintenanceRequest](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_date]  DEFAULT (getdate()),
	[number] [int] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_number]  DEFAULT ((0)),
	[start_date] [date] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_start_date]  DEFAULT (getdate()),
	[end_date] [date] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_end_date]  DEFAULT (getdate()),
	[maintenance_request_type_id] [int] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_maintenance_request_type_id]  DEFAULT ((0)),
	[service_level_id] [int] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_service_level_id]  DEFAULT ((0)),
	[requested_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_requested_by_id]  DEFAULT ((0)),
	[functional_location_id] [int] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_functional_location_id]  DEFAULT ((0)),
	[fixed_asset_id] [int] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_fixed_asset_id]  DEFAULT ((0)),
	[fault_symptoms_id] [int] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_fault_symptoms_id]  DEFAULT ((0)),
	[fault_area_id] [int] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_fault_area_id]  DEFAULT ((0)),
	[description] [varchar](250) NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_description]  DEFAULT (''),
	[status] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_status]  DEFAULT ('NEW'),
	[active] [bit] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_active]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequest_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_MaintenanceRequest] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_MaintenanceRequestType]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_MaintenanceRequestType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequestType_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequestType_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_MaintenanceRequestType_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_MaintenanceRequestType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_Module]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_Module](
	[id] [smallint] NOT NULL,
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Module_name]  DEFAULT (''),
	[key] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Module_key]  DEFAULT (''),
	[group] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Module_group]  DEFAULT (''),
 CONSTRAINT [PK_amQt_Module] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_PaymentMode]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_PaymentMode](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_PaymentMode_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_PaymentMode_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_PaymentMode_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_PaymentMode] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_PaymentTerms]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_PaymentTerms](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_PaymentTerms_name]  DEFAULT (''),
	[remarks] [varchar](250) NOT NULL CONSTRAINT [DF_amQt_PaymentTerms_remarks]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_PaymentTerms_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_PaymentTerms] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_Personnel]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_Personnel](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Personnel_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Personnel_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_Personnel_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_Personnel] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_Product]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_Product](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Product_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Product_name]  DEFAULT (''),
	[unit_id] [int] NOT NULL CONSTRAINT [DF_amQt_Product_unit_id]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_Product_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_Product] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_PurchaseOrder]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_PurchaseOrder](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrder_date]  DEFAULT (getdate()),
	[date_of_delivery] [date] NULL,
	[number] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrder_number]  DEFAULT ((0)),
	[quotation_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrder_quotation_id]  DEFAULT ((0)),
	[terms] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_PurchaseOrder_terms]  DEFAULT (''),
	[prepared_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrder_prepared_by_id]  DEFAULT ((0)),
	[noted_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrder_noted_by_id]  DEFAULT ((0)),
	[approved_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrder_approved_by_id]  DEFAULT ((0)),
	[revised] [bit] NOT NULL CONSTRAINT [DF_Table_1_is_revised]  DEFAULT ((0)),
	[cancelled] [bit] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrder_cancelled]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrder_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_PurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_PurchaseOrderDetail]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_PurchaseOrderDetail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[purchase_order_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrderDetail_purchase_order_id]  DEFAULT ((0)),
	[quotation_detail_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrderDetail_quotation_detail_id]  DEFAULT ((0)),
	[quantity] [decimal](18, 4) NOT NULL CONSTRAINT [DF_amQt_PurchaseOrderDetail_quantity]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_PurchaseOrderDetail_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_PurchaseOrderDetail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_PurchaseRequest]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_PurchaseRequest](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_date]  DEFAULT (getdate()),
	[number] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_number]  DEFAULT ((0)),
	[requested_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_requested_by_id]  DEFAULT ((0)),
	[date_required] [date] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_date_required]  DEFAULT (getdate()),
	[supplier_1_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_supplier_id_1]  DEFAULT ((0)),
	[supplier_2_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_supplier_id_2]  DEFAULT ((0)),
	[supplier_3_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_supplier_id_3]  DEFAULT ((0)),
	[remarks] [varchar](500) NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_remarks]  DEFAULT (''),
	[approved_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_approved_by_id]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequest_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_PurchaseRequest] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_PurchaseRequestDetail]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_PurchaseRequestDetail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[purchase_request_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequestDetail_purchase_request_id]  DEFAULT ((0)),
	[product_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequestDetail_product_id]  DEFAULT ((0)),
	[quantity] [decimal](9, 2) NOT NULL CONSTRAINT [DF_amQt_PurchaseRequestDetail_quantity]  DEFAULT ((0)),
	[cost] [decimal](18, 2) NOT NULL CONSTRAINT [DF_amQt_PurchaseRequestDetail_cost]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_PurchaseRequestDetail_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_PurchaseRequestDetail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_PurchaseVoucher]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_PurchaseVoucher](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL CONSTRAINT [DF_amQt_PurchaseVoucher_date]  DEFAULT (getdate()),
	[number] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseVoucher_number]  DEFAULT ((0)),
	[receiving_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseVoucher_receiving_id]  DEFAULT ((0)),
	[payment_mode_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseVoucher_payment_mode_id]  DEFAULT ((0)),
	[prepared_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseVoucher_prepared_by_id]  DEFAULT ((0)),
	[checked_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseVoucher_checked_by_id]  DEFAULT ((0)),
	[approved_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_PurchaseVoucher_approved_by_id]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_PurchaseVoucher_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_PurchaseVoucher] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_Quotation]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_Quotation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL CONSTRAINT [DF_amQt_Quotation_date]  DEFAULT (getdate()),
	[number] [int] NOT NULL CONSTRAINT [DF_amQt_Quotation_number]  DEFAULT ((0)),
	[purchase_request_id] [int] NOT NULL CONSTRAINT [DF_amQt_Quotation_purchase_request_id]  DEFAULT ((0)),
	[prepared_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_Quotation_prepared_by_id]  DEFAULT ((0)),
	[approved_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_Quotation_approved_by_id]  DEFAULT ((0)),
	[supplier_no] [tinyint] NOT NULL CONSTRAINT [DF_amQt_Quotation_supplier_no]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_Quotation_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_Quotation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_QuotationDetail]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_QuotationDetail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[quotation_id] [int] NOT NULL,
	[purchase_request_detail_id] [int] NOT NULL CONSTRAINT [DF_Table_1_purchase_request_detail_id]  DEFAULT ((0)),
	[cost1] [decimal](18, 4) NOT NULL CONSTRAINT [DF_amQt_QuotationDetail_price1]  DEFAULT ((0)),
	[cost2] [decimal](18, 4) NOT NULL CONSTRAINT [DF_Table_1_price11]  DEFAULT ((0)),
	[cost3] [decimal](18, 4) NOT NULL CONSTRAINT [DF_Table_1_price12]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_QuotationDetail_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_QuotationDetail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_Receiving]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_Receiving](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL CONSTRAINT [DF_amQt_Receiving_date]  DEFAULT (getdate()),
	[number] [int] NOT NULL CONSTRAINT [DF_amQt_Receiving_number]  DEFAULT ((0)),
	[purchase_order_id] [int] NOT NULL CONSTRAINT [DF_amQt_Receiving_purchase_order_id]  DEFAULT ((0)),
	[prepared_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_Receiving_prepared_by_id]  DEFAULT ((0)),
	[checked_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_Receiving_checked_by_id]  DEFAULT ((0)),
	[approved_by_id] [int] NOT NULL CONSTRAINT [DF_amQt_Receiving_approved_by_id]  DEFAULT ((0)),
	[invoice_no] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Receiving_invoice_no]  DEFAULT (''),
	[dr_no] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Receiving_dr_no]  DEFAULT (''),
	[amount] [decimal](18, 2) NOT NULL CONSTRAINT [DF_amQt_Receiving_amount]  DEFAULT ((0)),
	[remarks] [varchar](500) NOT NULL CONSTRAINT [DF_amQt_Receiving_remarks]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_Receiving_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_Receiving] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_ReceivingDetail]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_ReceivingDetail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[receiving_id] [int] NOT NULL CONSTRAINT [DF_amQt_ReceivingDetail_receiving_id]  DEFAULT ((0)),
	[purchase_order_detail_id] [int] NOT NULL CONSTRAINT [DF_amQt_ReceivingDetail_purchase_order_detail_id]  DEFAULT ((0)),
	[quantity] [decimal](18, 4) NOT NULL CONSTRAINT [DF_amQt_ReceivingDetail_quantity]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_ReceivingDetail_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_ReceivingDetail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_ServiceLevel]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_ServiceLevel](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_ServiceLevel_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_ServiceLevel_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_ServiceLevel_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_ServiceLevel] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_Supplier]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_Supplier](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Supplier_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Supplier_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_Supplier_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_Supplier] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_Trade]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_Trade](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Trade_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Trade_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_Trade_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_Trade] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_Unit]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_Unit](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Unit_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Unit_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_Unit_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_Unit] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_UserAccess]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_UserAccess](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL CONSTRAINT [DF_amQt_UserAccess_user_id]  DEFAULT ((0)),
	[module_id] [smallint] NOT NULL CONSTRAINT [DF_amQt_UserAccess_module_id]  DEFAULT ((0)),
	[select] [bit] NOT NULL CONSTRAINT [DF_amQt_UserAccess_select]  DEFAULT ((0)),
	[insert] [bit] NOT NULL CONSTRAINT [DF_amQt_UserAccess_insert]  DEFAULT ((0)),
	[update] [bit] NOT NULL CONSTRAINT [DF_amQt_UserAccess_update]  DEFAULT ((0)),
	[delete] [bit] NOT NULL CONSTRAINT [DF_amQt_UserAccess_delete]  DEFAULT ((0)),
	[print] [bit] NOT NULL CONSTRAINT [DF_amQt_UserAccess_print]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_UserAccess] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_Users]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_Users_username]  DEFAULT (''),
	[hash] [varchar](max) NOT NULL CONSTRAINT [DF_amQt_Users_hash]  DEFAULT (''),
	[salt] [varbinary](512) NOT NULL,
	[photo] [varbinary](max) NULL,
	[personnel_id] [int] NOT NULL CONSTRAINT [DF_amQt_Users_personnel_id]  DEFAULT ((0)),
	[allow_no_schedule] [bit] NOT NULL CONSTRAINT [DF_amQt_Users_allow_no_schedule]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_Users_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[amQt_WorkOrder]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_WorkOrder](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL CONSTRAINT [DF_amQt_WorkOrder_date]  DEFAULT (getdate()),
	[expected_start_date] [date] NOT NULL CONSTRAINT [DF_amQt_WorkOrder_start_date]  DEFAULT (getdate()),
	[expected_end_date] [date] NOT NULL CONSTRAINT [DF_amQt_WorkOrder_end_date]  DEFAULT (getdate()),
	[number] [int] NOT NULL CONSTRAINT [DF_amQt_WorkOrder_number]  DEFAULT ((0)),
	[maintenance_request_id] [int] NOT NULL CONSTRAINT [DF_amQt_WorkOrder_maintenance_request_id]  DEFAULT ((0)),
	[work_order_type_id] [int] NOT NULL CONSTRAINT [DF_amQt_WorkOrder_work_order_type_id]  DEFAULT ((0)),
	[maintenance_job_type_variant_id] [int] NOT NULL CONSTRAINT [DF_amQt_WorkOrder_maintenance_job_type_variant_id]  DEFAULT ((0)),
	[trade_id] [int] NOT NULL CONSTRAINT [DF_amQt_WorkOrder_trade_id]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_WorkOrder_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_WorkOrder] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_WorkOrderHours]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_WorkOrderHours](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[work_order_id] [int] NOT NULL CONSTRAINT [DF_amQt_WorkOrderHours_work_order_id]  DEFAULT ((0)),
	[expense_category_id] [int] NOT NULL CONSTRAINT [DF_amQt_WorkOrderHours_expense_category_id]  DEFAULT ((0)),
	[hours] [decimal](9, 2) NOT NULL CONSTRAINT [DF_amQt_WorkOrderHours_hours]  DEFAULT ((0)),
	[rate_per_hour] [decimal](9, 2) NOT NULL CONSTRAINT [DF_amQt_WorkOrderHours_rate_per_hour]  DEFAULT ((0)),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_WorkOrderHours_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_WorkOrderHours] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_WorkOrderProducts]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[amQt_WorkOrderProducts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[work_order_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[unit_id] [int] NOT NULL,
	[price] [decimal](18, 4) NOT NULL,
	[currency_id] [int] NOT NULL,
	[disable] [bit] NOT NULL,
 CONSTRAINT [PK_amQt_WorkOrderProducts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[amQt_WorkOrderType]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[amQt_WorkOrderType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_WorkOrderType_code]  DEFAULT (''),
	[name] [varchar](50) NOT NULL CONSTRAINT [DF_amQt_WorkOrderType_name]  DEFAULT (''),
	[disable] [bit] NOT NULL CONSTRAINT [DF_amQt_WorkOrderType_disable]  DEFAULT ((0)),
 CONSTRAINT [PK_amQt_WorkOrderType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[amQt_DepreciationJournalDetail] ADD  CONSTRAINT [DF_amQt_DepreciationJournalDetail_depreciation_journal_id]  DEFAULT ((0)) FOR [depreciation_journal_id]
GO
ALTER TABLE [dbo].[amQt_DepreciationJournalDetail] ADD  CONSTRAINT [DF_amQt_DepreciationJournalDetail_depreciation_expense_account_id]  DEFAULT ((0)) FOR [depreciation_expense_account_id]
GO
ALTER TABLE [dbo].[amQt_DepreciationJournalDetail] ADD  CONSTRAINT [DF_amQt_DepreciationJournalDetail_accumulated_depreciation_account_id]  DEFAULT ((0)) FOR [accumulated_depreciation_account_id]
GO
ALTER TABLE [dbo].[amQt_DepreciationJournalDetail] ADD  CONSTRAINT [DF_amQt_DepreciationJournalDetail_debit_credit]  DEFAULT ((0)) FOR [debit_credit]
GO
ALTER TABLE [dbo].[amQt_DepreciationJournalDetail] ADD  CONSTRAINT [DF_amQt_DepreciationJournalDetail_amount]  DEFAULT ((0)) FOR [amount]
GO
ALTER TABLE [dbo].[amQt_DepreciationJournalDetail] ADD  CONSTRAINT [DF_amQt_DepreciationJournalDetail_disable]  DEFAULT ((0)) FOR [disable]
GO
ALTER TABLE [dbo].[amQt_FixedAssetDispose] ADD  CONSTRAINT [DF_amQt_FixedAssetDispose_fixed_asset_id]  DEFAULT ((0)) FOR [fixed_asset_id]
GO
ALTER TABLE [dbo].[amQt_FixedAssetDispose] ADD  CONSTRAINT [DF_Table_1_date]  DEFAULT (getdate()) FOR [date_disposed]
GO
ALTER TABLE [dbo].[amQt_FixedAssetDispose] ADD  CONSTRAINT [DF_amQt_FixedAssetDispose_sale_proceeds]  DEFAULT ((0)) FOR [sale_proceeds]
GO
ALTER TABLE [dbo].[amQt_FixedAssetDispose] ADD  CONSTRAINT [DF_Table_1_chart_of_account_id]  DEFAULT ((0)) FOR [cash_account_id]
GO
ALTER TABLE [dbo].[amQt_FixedAssetDispose] ADD  CONSTRAINT [DF_amQt_FixedAssetDispose_gain_loss_account_id]  DEFAULT ((0)) FOR [gain_loss_account_id]
GO
ALTER TABLE [dbo].[amQt_FixedAssetDispose] ADD  CONSTRAINT [DF_amQt_FixedAssetDispose_depreciation_financial_year]  DEFAULT ('') FOR [depreciation_financial_year]
GO
ALTER TABLE [dbo].[amQt_FixedAssetDispose] ADD  CONSTRAINT [DF_amQt_FixedAssetDispose_depreciation_date]  DEFAULT (getdate()) FOR [depreciation_date]
GO
ALTER TABLE [dbo].[amQt_FixedAssetDispose] ADD  CONSTRAINT [DF_amQt_FixedAssetDispose_disable]  DEFAULT ((0)) FOR [disable]
GO
ALTER TABLE [dbo].[amQt_WorkOrderProducts] ADD  CONSTRAINT [DF_amQt_WorkOrderProducts_work_order_id]  DEFAULT ((0)) FOR [work_order_id]
GO
ALTER TABLE [dbo].[amQt_WorkOrderProducts] ADD  CONSTRAINT [DF_amQt_WorkOrderProducts_product_id]  DEFAULT ((0)) FOR [product_id]
GO
ALTER TABLE [dbo].[amQt_WorkOrderProducts] ADD  CONSTRAINT [DF_amQt_WorkOrderProducts_unit_id]  DEFAULT ((0)) FOR [unit_id]
GO
ALTER TABLE [dbo].[amQt_WorkOrderProducts] ADD  CONSTRAINT [DF_amQt_WorkOrderProducts_price]  DEFAULT ((0)) FOR [price]
GO
ALTER TABLE [dbo].[amQt_WorkOrderProducts] ADD  CONSTRAINT [DF_amQt_WorkOrderProducts_currency_id]  DEFAULT ((0)) FOR [currency_id]
GO
ALTER TABLE [dbo].[amQt_WorkOrderProducts] ADD  CONSTRAINT [DF_amQt_WorkOrderProducts_disable]  DEFAULT ((0)) FOR [disable]
GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountClassificationDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountClassificationDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_AccountClassification SET disable = 1 WHERE id = @id

END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountClassificationInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountClassificationInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50),
	@post BIT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_AccountClassification
		(
			code,
			name,
			post

		)
		VALUES
		(
			@code,
			@name,
			@post
		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_AccountClassification SET
			code = @code,
			name = @name,
			post = @post

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END





GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountClassificationSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountClassificationSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_AccountClassification
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_AccountClassification p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountClassificationSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountClassificationSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_AccountClassification p
	WHERE p.id = @id
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountGroupDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountGroupDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_AccountGroup SET disable = 1 WHERE id = @id

END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountGroupInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountGroupInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50),
	@post BIT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_AccountGroup
		(
			code,
			name,
			post

		)
		VALUES
		(
			@code,
			@name,
			@post
		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_AccountGroup SET
			code = @code,
			name = @name,
			post = @post

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END





GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountGroupSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountGroupSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_AccountGroup
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_AccountGroup p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountGroupSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountGroupSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_AccountGroup p
	WHERE p.id = @id
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountTypeDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountTypeDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_AccountType SET disable = 1 WHERE id = @id

END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountTypeInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountTypeInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50),
	@post BIT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_AccountType
		(
			code,
			name,
			post

		)
		VALUES
		(
			@code,
			@name,
			@post
		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_AccountType SET
			code = @code,
			name = @name,
			post = @post

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END





GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountTypeSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountTypeSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_AccountType
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_AccountType p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccountTypeSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccountTypeSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_AccountType p
	WHERE p.id = @id
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccumulatedDepreciationAccountDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccumulatedDepreciationAccountDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_AccumulatedDepreciationAccount SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccumulatedDepreciationAccountInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccumulatedDepreciationAccountInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_AccumulatedDepreciationAccount
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_AccumulatedDepreciationAccount SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccumulatedDepreciationAccountSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccumulatedDepreciationAccountSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_AccumulatedDepreciationAccount
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_AccumulatedDepreciationAccount p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spAccumulatedDepreciationAccountSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAccumulatedDepreciationAccountSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_AccumulatedDepreciationAccount p
	WHERE p.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetAccountDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetAccountDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_AssetAccount SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetAccountInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetAccountInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_AssetAccount
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_AssetAccount SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetAccountSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetAccountSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_AssetAccount
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_AssetAccount p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetAccountSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetAccountSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_AssetAccount p
	WHERE p.id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetCategoryDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetCategoryDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_AssetCategory SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetCategoryInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetCategoryInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50),
	@post BIT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_AssetCategory
		(
			code,
			name,
			post

		)
		VALUES
		(
			@code,
			@name,
			@post
		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_AssetCategory SET
			code = @code,
			name = @name,
			post = @post

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetCategorySearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetCategorySearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_AssetCategory
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_AssetCategory p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetCategorySelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetCategorySelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_AssetCategory p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetClassDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetClassDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_AssetClass SET disable = 1 WHERE id = @id

END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetClassInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetClassInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50),
	@post BIT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_AssetClass
		(
			code,
			name,
			post

		)
		VALUES
		(
			@code,
			@name,
			@post
		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_AssetClass SET
			code = @code,
			name = @name,
			post = @post

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END





GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetClassSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetClassSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_AssetClass
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_AssetClass p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetClassSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetClassSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_AssetClass p
	WHERE p.id = @id
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetTypeDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetTypeDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_AssetType SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetTypeInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetTypeInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_AssetType
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_AssetType SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetTypeSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetTypeSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_AssetType
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_AssetType p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spAssetTypeSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAssetTypeSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_AssetType p
	WHERE p.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spAveragingMethodDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAveragingMethodDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_AveragingMethod SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spAveragingMethodInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAveragingMethodInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_AveragingMethod
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_AveragingMethod SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spAveragingMethodSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAveragingMethodSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_AveragingMethod
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_AveragingMethod p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spAveragingMethodSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spAveragingMethodSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_AveragingMethod p
	WHERE p.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spChartOfAccountDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spChartOfAccountDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_ChartOfAccount SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spChartOfAccountInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spChartOfAccountInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_ChartOfAccount
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_ChartOfAccount SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spChartOfAccountSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spChartOfAccountSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_ChartOfAccount
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_ChartOfAccount p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spChartOfAccountSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spChartOfAccountSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_ChartOfAccount p
	WHERE p.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spCompanyProfileDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spCompanyProfileDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_CompanyProfile SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spCompanyProfileInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spCompanyProfileInsertUpdateSingleItem]
	@id INT OUTPUT,
	@name VARCHAR(150),
	@address VARCHAR(1500),
	@report_logo VARBINARY(MAX) = NULL,
	@width INT,
	@height INT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_CompanyProfile
		(
			name,
			address,
			report_logo,
			width,
			height

		)
		VALUES
		(
			@name,
			@address,
			@report_logo,
			@width,
			@height

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_CompanyProfile SET
			name = @name,
			address = @address,
			report_logo = @report_logo,
			width = @width,
			height = @height

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spCompanyProfileSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spCompanyProfileSearchList]	
	@record_count int = NULL OUTPUT,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_CompanyProfile
			WHERE disable = 0
			)
		RETURN
	END

	SELECT *
	FROM amQt_CompanyProfile
	WHERE disable = 0
END







GO
/****** Object:  StoredProcedure [dbo].[amQt_spCompanyProfileSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spCompanyProfileSelectSingleItem]	
	@id INT = 0
AS
BEGIN
	
	SELECT *
	FROM amQt_CompanyProfile
	WHERE id = @id
END







GO
/****** Object:  StoredProcedure [dbo].[amQt_spCurrencyDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spCurrencyDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_Currency SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spCurrencyInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spCurrencyInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_Currency
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_Currency SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spCurrencySearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spCurrencySearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_Currency
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_Currency p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spCurrencySelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spCurrencySelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_Currency p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationExpenseAccountDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationExpenseAccountDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_DepreciationExpenseAccount SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationExpenseAccountInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationExpenseAccountInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_DepreciationExpenseAccount
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_DepreciationExpenseAccount SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationExpenseAccountSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationExpenseAccountSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_DepreciationExpenseAccount
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_DepreciationExpenseAccount p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationExpenseAccountSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationExpenseAccountSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_DepreciationExpenseAccount p
	WHERE p.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationJournalDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationJournalDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_DepreciationJournal SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationJournalDetailSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spDepreciationJournalDetailSearchList]	
	@record_count int = NULL OUTPUT,
	@depreciation_journal_id INT
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_DepreciationJournalDetail
			WHERE disable = 0
			AND depreciation_journal_id = (CASE @depreciation_journal_id WHEN 0 THEN depreciation_journal_id ELSE @depreciation_journal_id END)
			)
		RETURN
	END

	SELECT jd.*,
		dea.name AS depreciation_expense_account_name,
		ada.name AS accumulated_depreciation_account_name
	FROM amQt_DepreciationJournalDetail jd JOIN amQt_DepreciationExpenseAccount dea
	ON dea.id = jd.depreciation_expense_account_id JOIN amQt_AccumulatedDepreciationAccount ada
	ON ada.id = jd.accumulated_depreciation_account_id
	WHERE jd.disable = 0
	AND jd.depreciation_journal_id = (CASE @depreciation_journal_id WHEN 0 THEN jd.depreciation_journal_id ELSE @depreciation_journal_id END)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationJournalDetailSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spDepreciationJournalDetailSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT jd.*,
		dea.name AS depreciation_expense_account_name,
		ada.name AS accumulated_depreciation_account_name
	FROM amQt_DepreciationJournalDetail jd JOIN amQt_DepreciationExpenseAccount dea
	ON dea.id = jd.depreciation_expense_account_id JOIN amQt_AccumulatedDepreciationAccount ada
	ON ada.id = jd.accumulated_depreciation_account_id
	WHERE jd.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationJournalInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationJournalInsertUpdateSingleItem]
	@id INT OUTPUT,
	@fixed_asset_id INT,
	@year SMALLINT,
	@month TINYINT,
	@depreciation_expense_account_id INT,
	@depreciation_expense_account_debit_credit BIT,
	@accumulated_depreciation_account_id INT,
	@accumulated_depreciation_account_debit_credit BIT,
	@amount DECIMAL(18,2),
	@description VARCHAR(500)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_DepreciationJournal
		(
			fixed_asset_id,
			year,
			month,
			depreciation_expense_account_id,
			depreciation_expense_account_debit_credit,
			accumulated_depreciation_account_id,
			accumulated_depreciation_account_debit_credit,
			amount,
			description

		)
		VALUES
		(
			@fixed_asset_id,
			@year,
			@month,
			@depreciation_expense_account_id,
			@depreciation_expense_account_debit_credit,
			@accumulated_depreciation_account_id,
			@accumulated_depreciation_account_debit_credit,
			@amount,
			@description

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_DepreciationJournal SET
			fixed_asset_id = @fixed_asset_id,
			year = @year,
			month = @month,
			depreciation_expense_account_id = @depreciation_expense_account_id,
			depreciation_expense_account_debit_credit = @depreciation_expense_account_debit_credit,
			accumulated_depreciation_account_id = @accumulated_depreciation_account_id,
			accumulated_depreciation_account_debit_credit = @accumulated_depreciation_account_debit_credit,
			amount = @amount,
			description = @description

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationJournalSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spDepreciationJournalSearchList]	
	@record_count int = NULL OUTPUT,
	@fixed_asset_id INT,
	@year SMALLINT = 0,
	@month TINYINT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_DepreciationJournal
			WHERE disable = 0
			AND fixed_asset_id = (CASE @fixed_asset_id WHEN 0 THEN fixed_asset_id ELSE @fixed_asset_id END)
			AND year = (CASE @year WHEN 0 THEN year ELSE @year END)
			AND month = (CASE @month WHEN 0 THEN month ELSE @month END)
			)
		RETURN
	END

	SELECT j.*,
		dea.name AS depreciation_expense_account_name,
		ada.name AS accumulated_depreciation_account_name
	FROM amQt_DepreciationJournal j JOIN amQt_ChartOfAccount dea
	ON dea.id = j.depreciation_expense_account_id JOIN amQt_ChartOfAccount ada
	ON ada.id = j.accumulated_depreciation_account_id
	WHERE j.disable = 0
	AND j.fixed_asset_id = (CASE @fixed_asset_id WHEN 0 THEN j.fixed_asset_id ELSE @fixed_asset_id END)
	AND j.year = (CASE @year WHEN 0 THEN j.year ELSE @year END)
	AND j.month = (CASE @month WHEN 0 THEN j.month ELSE @month END)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationJournalSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spDepreciationJournalSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT j.*,
		dea.name AS depreciation_expense_account_name,
		ada.name AS accumulated_depreciation_account_name
	FROM amQt_DepreciationJournal j JOIN amQt_ChartOfAccount dea
	ON dea.id = j.depreciation_expense_account_id JOIN amQt_ChartOfAccount ada
	ON ada.id = j.accumulated_depreciation_account_id
	WHERE j.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationMethodDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationMethodDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_DepreciationMethod SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationMethodInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationMethodInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_DepreciationMethod
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_DepreciationMethod SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationMethodSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationMethodSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_DepreciationMethod
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_DepreciationMethod p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationMethodSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spDepreciationMethodSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_DepreciationMethod p
	WHERE p.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spDepreciationStraightLineFullMonth]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spDepreciationStraightLineFullMonth]
	@purchase_date DATE = '1/19/2019',
	@purchase_cost DECIMAL(18,2) = 4985300,
	@residual_value DECIMAL(18,2) = 85000,
	@useful_life_years INT = 25,
	@year SMALLINT = 2019
AS
BEGIN
	DECLARE @start_date DATE,
			@end_date DATE,
			@end_depreciation_date DATE
		
	DECLARE @depcreciation_rate FLOAT,--float so that the aboslute value of depreciation expense is 100% accurate
		@depcreciation_rate_yearly FLOAT = 100.0 / @useful_life_years,
		@depcreciation_expense DECIMAL(18,2)

	SET @start_date = @purchase_date--DATEFROMPARTS(YEAR(@purchase_date), MONTH(@purchase_date), 1)
	SET @end_depreciation_date = DATEADD(D, -1, DATEADD(YEAR, @useful_life_years, @purchase_date))

	--IF DAY(@purchase_date) > 15 --16thday onwards
	--	SET @start_date = DATEADD(M, 1, @start_date)

	DECLARE @start_date_constant DATE = @start_date
	DECLARE @days_in_year INT = DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), DATEFROMPARTS(YEAR(@start_date), 12, 31)) + 1

	DECLARE @table_dates TABLE(start_date DATE, end_date DATE, year INT, beginning_book_value DECIMAL(19,2), depreciation_rate DECIMAL(18,0), depreciation_expense DECIMAL(18,2), accumulated_depreciation DECIMAL(18,2), ending_book_value DECIMAL(18,2))
	DECLARE @accumulated_depreciation DECIMAL(18,2),
			@beginning_book_value DECIMAL(18,2) = @purchase_cost,
			@ending_book_value DECIMAL(18,2) = @purchase_cost
		

	WHILE @start_date <= @end_depreciation_date
	BEGIN

		IF @start_date > @purchase_date --second row
			SET @start_date = DATEFROMPARTS(YEAR(@start_date), 1, 1) --always start and jan 1
	
		SET @end_date = DATEFROMPARTS(YEAR(@start_date), 12, 31)
		SET @depcreciation_rate = 100.0 / @useful_life_years / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1) / 100.0
	
		IF YEAR(@end_date) = YEAR(@end_depreciation_date) --last row
		BEGIN		
			SET @end_date = @end_depreciation_date
			IF @end_depreciation_date != DATEFROMPARTS(YEAR(@end_depreciation_date), 12, 31)
			BEGIN 
				IF MONTH(@purchase_date) != 1 
					SET @depcreciation_rate = @depcreciation_rate / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1)
				ELSE --The asset was acquired on the any day on January
					SET @depcreciation_rate = @depcreciation_rate / @days_in_year * (DATEDIFF(D, @start_date, @end_date) + 1)						
			END
		END

		SET @depcreciation_expense = @depcreciation_rate * (@purchase_cost - @residual_value)
		SET @ending_book_value = @ending_book_value - @depcreciation_expense
	
		IF @accumulated_depreciation IS NULL SET @accumulated_depreciation = @depcreciation_expense --first row
		ELSE SET @accumulated_depreciation = @accumulated_depreciation + @depcreciation_expense
	
		INSERT INTO @table_dates VALUES (@start_date, @end_date, YEAR(@start_date), @beginning_book_value, @depcreciation_rate * 100.00, @depcreciation_expense, @accumulated_depreciation, @ending_book_value)
	
		SET @beginning_book_value = @beginning_book_value - @depcreciation_expense --second row for beginning_book_value
		SET @start_date = DATEADD(YEAR, 1, @start_date)
	END

	SELECT * FROM @table_dates WHERE year = @year OR @year = 0
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spExpenseCategoryDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spExpenseCategoryDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_ExpenseCategory SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spExpenseCategoryInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spExpenseCategoryInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_ExpenseCategory
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_ExpenseCategory SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spExpenseCategorySearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spExpenseCategorySearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_ExpenseCategory
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_ExpenseCategory p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spExpenseCategorySelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spExpenseCategorySelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_ExpenseCategory p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFaultAreaDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFaultAreaDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_FaultArea SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFaultAreaInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFaultAreaInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_FaultArea
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_FaultArea SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spFaultAreaSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFaultAreaSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_FaultArea
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_FaultArea p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFaultAreaSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFaultAreaSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_FaultArea p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFaultSymptomsDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFaultSymptomsDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_FaultSymptoms SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFaultSymptomsInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFaultSymptomsInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_FaultSymptoms
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_FaultSymptoms SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spFaultSymptomsSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFaultSymptomsSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_FaultSymptoms
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_FaultSymptoms p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFaultSymptomsSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFaultSymptomsSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_FaultSymptoms p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFixedAssetDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_FixedAsset SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFixedAssetInsertUpdateSingleItem]
	@id INT OUTPUT,
	@asset_no VARCHAR(50),
	@product_id INT,
	@receiving_detail_id INT,
	@asset_type_id INT,
	@functional_location_id INT,
	@description VARCHAR(150),
	@purchase_date DATE,
	@purchase_price DECIMAL(18,2),
	@warranty_expiry DATE,
	@serial_no VARCHAR(50),
	@model VARCHAR(50),
	@depreciation_start_date DATE,
	@depreciation_method_id INT,
	@averaging_method_id INT,
	@residual_value DECIMAL(18,2),
	@useful_life_years SMALLINT,
	@is_draft BIT,
	@is_registered BIT,
	@is_disposed BIT
AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_FixedAsset
		(
			asset_no,
			product_id,
			receiving_detail_id,
			asset_type_id,
			functional_location_id,
			description,
			purchase_date,
			purchase_price,
			warranty_expiry,
			serial_no,
			model,
			depreciation_start_date,
			depreciation_method_id,
			averaging_method_id,
			residual_value,
			useful_life_years,
			is_draft,
			is_registered,
			is_disposed
		)
		VALUES
		(
			@asset_no,
			@product_id,
			@receiving_detail_id,
			@asset_type_id,
			@functional_location_id,
			@description,
			@purchase_date,
			@purchase_price,
			@warranty_expiry,
			@serial_no,
			@model,
			@depreciation_start_date,
			@depreciation_method_id,
			@averaging_method_id,
			@residual_value,
			@useful_life_years,
			@is_draft,
			@is_registered,
			@is_disposed

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_FixedAsset SET
			asset_no = @asset_no,
			asset_type_id = @asset_type_id,
			functional_location_id = @functional_location_id,
			description = @description,
			purchase_date = @purchase_date,
			purchase_price = @purchase_price,
			warranty_expiry = @warranty_expiry,
			serial_no = @serial_no,
			model = @model,
			depreciation_start_date = @depreciation_start_date,
			depreciation_method_id = @depreciation_method_id,
			averaging_method_id = @averaging_method_id,
			residual_value = @residual_value,
			useful_life_years = @useful_life_years,
			is_draft = @is_draft,
			is_registered = @is_registered,
			is_disposed = @is_disposed

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFixedAssetSearchList]	
	@record_count int = NULL OUTPUT
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_FixedAsset
			WHERE disable = 0
			)
		RETURN
	END

	SELECT a.*,
		p.code AS product_code,
		p.name AS product_name,
		t.name AS asset_type_name,
		l.name AS functional_location_name,
		ISNULL(dm.name, '') AS depreciation_method_name,
		ISNULL(am.name, '') AS averaging_method_name
	FROM amQt_FixedAsset a JOIN amQt_AssetType t 
	ON t.id = a.asset_type_id JOIN amQt_Product p
	ON p.id = a.product_id JOIN amQt_FunctionalLocation l
	ON l.id = a.functional_location_id LEFT JOIN amQt_DepreciationMethod dm
	ON dm.id = a.depreciation_method_id LEFT JOIN amQt_AveragingMethod am
	ON am.id = a.averaging_method_id
	WHERE a.disable = 0
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFixedAssetSelectSingleItem]	
	@id INT = 0
AS
BEGIN
	
	SELECT a.*,
		p.code AS product_code,
		p.name AS product_name,
		t.name AS asset_type_name,
		l.name AS functional_location_name,
		ISNULL(dm.name, '') AS depreciation_method_name,
		ISNULL(am.name, '') AS averaging_method_name
	FROM amQt_FixedAsset a JOIN amQt_AssetType t 
	ON t.id = a.asset_type_id JOIN amQt_Product p
	ON p.id = a.product_id JOIN amQt_FunctionalLocation l
	ON l.id = a.functional_location_id LEFT JOIN amQt_DepreciationMethod dm
	ON dm.id = a.depreciation_method_id LEFT JOIN amQt_AveragingMethod am
	ON am.id = a.averaging_method_id
	WHERE a.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSettingDateDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFixedAssetSettingDateDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_FixedAssetSettingDate SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSettingDateInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFixedAssetSettingDateInsertUpdateSingleItem]
	@id INT OUTPUT,
	@date DATE

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_FixedAssetSettingDate
		(
			date

		)
		VALUES
		(
			@date

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_FixedAssetSettingDate SET
			date = @date

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSettingDateSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spFixedAssetSettingDateSearchList]	
	@record_count int = NULL OUTPUT
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_FixedAssetSettingDate
			WHERE disable = 0
			)
		RETURN
	END

	SELECT *
	FROM amQt_FixedAssetSettingDate
	WHERE disable = 0
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSettingDateSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spFixedAssetSettingDateSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT *
	FROM amQt_FixedAssetSettingDate
	WHERE id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSettingDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFixedAssetSettingDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_FixedAssetSetting SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSettingInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFixedAssetSettingInsertUpdateSingleItem]
	@id INT OUTPUT,
	@asset_type_id INT,
	@asset_class_id INT,
	@chart_of_account_id INT,
	@accumulated_depreciation_account_id INT,
	@depreciation_expense_account_id INT,
	@depreciation_method_id INT,
	@averaging_method_id INT,
	@useful_life_years DECIMAL(18,2)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_FixedAssetSetting
		(
			asset_type_id,
			asset_class_id,
			chart_of_account_id,
			accumulated_depreciation_account_id,
			depreciation_expense_account_id,
			depreciation_method_id,
			averaging_method_id,
			useful_life_years

		)
		VALUES
		(
			@asset_type_id,
			@asset_class_id,
			@chart_of_account_id,
			@accumulated_depreciation_account_id,
			@depreciation_expense_account_id,
			@depreciation_method_id,
			@averaging_method_id,
			@useful_life_years

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_FixedAssetSetting SET
			asset_type_id = @asset_type_id,
			asset_class_id = @asset_class_id,
			chart_of_account_id = @chart_of_account_id,
			accumulated_depreciation_account_id = @accumulated_depreciation_account_id,
			depreciation_expense_account_id = @depreciation_expense_account_id,
			depreciation_method_id = @depreciation_method_id,
			averaging_method_id = @averaging_method_id,
			useful_life_years = @useful_life_years

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSettingSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spFixedAssetSettingSearchList]	
	@record_count int = NULL OUTPUT
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_FixedAssetSetting
			WHERE disable = 0
			)
		RETURN
	END

	SELECT s.*,
		t.name AS asset_type_name,

		ISNULL(ac.code, '') AS asset_class_code,
		ISNULL(ac.name, '') AS asset_class_name,

		ISNULL(aa.code, '') AS chart_of_account_code,
		ISNULL(aa.name, '') AS chart_of_account_name,

		ISNULL(ada.code, '') AS accumulated_depreciation_account_code,
		ISNULL(ada.name, '') AS accumulated_depreciation_account_name,
		
		ISNULL(dea.code, '') AS depreciation_expense_account_code,
		ISNULL(dea.name, '') AS depreciation_expense_account_name,
		
		ISNULL(dm.code, '') AS depreciation_method_code,
		ISNULL(dm.name, '') AS depreciation_method_name,
		
		ISNULL(am.code, '') AS averaging_method_code,
		ISNULL(am.name, '') AS averaging_method_name
	FROM amQt_FixedAssetSetting s JOIN amQt_AssetType t
	ON t.id = s.asset_type_id LEFT JOIN amQt_ChartOfAccount aa
	ON aa.id = s.chart_of_account_id LEFT JOIN amQt_ChartOfAccount ada
	ON ada.id = s.accumulated_depreciation_account_id LEFT JOIN amQt_ChartOfAccount dea
	ON dea.id = s.depreciation_expense_account_id LEFT JOIN amQt_DepreciationMethod dm
	ON dm.id = s.depreciation_method_id LEFT JOIN amQt_AveragingMethod am
	ON am.id = s.averaging_method_id LEFT JOIN amQt_AssetClass ac
	ON ac.id = s.asset_class_id
	WHERE s.disable = 0
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spFixedAssetSettingSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spFixedAssetSettingSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT s.*,
		t.name AS asset_type_name,

		ISNULL(ac.code, '') AS asset_class_code,
		ISNULL(ac.name, '') AS asset_class_name,

		ISNULL(aa.code, '') AS chart_of_account_code,
		ISNULL(aa.name, '') AS chart_of_account_name,

		ISNULL(ada.code, '') AS accumulated_depreciation_account_code,
		ISNULL(ada.name, '') AS accumulated_depreciation_account_name,
		
		ISNULL(dea.code, '') AS depreciation_expense_account_code,
		ISNULL(dea.name, '') AS depreciation_expense_account_name,
		
		ISNULL(dm.code, '') AS depreciation_method_code,
		ISNULL(dm.name, '') AS depreciation_method_name,
		
		ISNULL(am.code, '') AS averaging_method_code,
		ISNULL(am.name, '') AS averaging_method_name
	FROM amQt_FixedAssetSetting s JOIN amQt_AssetType t
	ON t.id = s.asset_type_id LEFT JOIN amQt_ChartOfAccount aa
	ON aa.id = s.chart_of_account_id LEFT JOIN amQt_ChartOfAccount ada
	ON ada.id = s.accumulated_depreciation_account_id LEFT JOIN amQt_ChartOfAccount dea
	ON dea.id = s.depreciation_expense_account_id LEFT JOIN amQt_DepreciationMethod dm
	ON dm.id = s.depreciation_method_id LEFT JOIN amQt_AveragingMethod am
	ON am.id = s.averaging_method_id LEFT JOIN amQt_AssetClass ac
	ON ac.id = s.asset_class_id
	WHERE s.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spFunctionalLocationDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFunctionalLocationDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_FunctionalLocation SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spFunctionalLocationInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFunctionalLocationInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50),
	@parent_fl_id INT,
	@fl_status VARCHAR(50),
	@address_name VARCHAR(150),
	@street VARCHAR(150),
	@city VARCHAR(150),
	@province VARCHAR(150),
	@country VARCHAR(150),
	@zip_code VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_FunctionalLocation
		(
			code,
			name,
			parent_fl_id,
			fl_status,
			address_name,
			street,
			city,
			province,
			country,
			zip_code

		)
		VALUES
		(
			@code,
			@name,
			@parent_fl_id,
			@fl_status,
			@address_name,
			@street,
			@city,
			@province,
			@country,
			@zip_code

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_FunctionalLocation SET
			code = @code,
			name = @name,
			parent_fl_id = @parent_fl_id,
			fl_status = @fl_status,
			address_name = @address_name,
			street = @street,
			city = @city,
			province = @province,
			country = @country,
			zip_code = @zip_code

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spFunctionalLocationSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFunctionalLocationSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_FunctionalLocation
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT f.*,
		ISNULL(f2.name, '') AS parent_fl_name
	FROM amQt_FunctionalLocation f LEFT JOIN amQt_FunctionalLocation f2
	ON f.id = f2.parent_fl_id
	WHERE f.disable = 0
	AND (f.code = @code AND f.id != @id OR @code IS NULL)
	AND (f.name = @name AND f.id != @id OR @name IS NULL)
END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spFunctionalLocationSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spFunctionalLocationSelectSingleItem]	
	@id INT = 0
AS
BEGIN
	SELECT f.*,
		ISNULL(f2.name, '') AS parent_fl_name
	FROM amQt_FunctionalLocation f LEFT JOIN amQt_FunctionalLocation f2
	ON f.id = f2.parent_fl_id
	WHERE f.id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spGetNewTrasactionNo]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spGetNewTrasactionNo]
	@table VARCHAR(50)
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX) = N'
	DECLARE @number INT
	SET @number = ISNULL((SELECT MAX(number) FROM amQt_' + @table + ' WHERE YEAR(date) = YEAR(GETDATE())), 0) + 1

	SELECT dbo.amQt_fnTransactionNo(GETDATE(), @number)
	'

	EXEC(@sql)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceJobTypeDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceJobTypeDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_MaintenanceJobType SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceJobTypeInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceJobTypeInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_MaintenanceJobType
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_MaintenanceJobType SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceJobTypeSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceJobTypeSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_MaintenanceJobType
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_MaintenanceJobType p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceJobTypeSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceJobTypeSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_MaintenanceJobType p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceJobTypeVariantDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceJobTypeVariantDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_MaintenanceJobTypeVariant SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceJobTypeVariantInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceJobTypeVariantInsertUpdateSingleItem]
	@id INT OUTPUT,
	@maintenance_job_type_id INT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_MaintenanceJobTypeVariant
		(
			maintenance_job_type_id,
			code,
			name

		)
		VALUES
		(
			@maintenance_job_type_id,
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_MaintenanceJobTypeVariant SET
			maintenance_job_type_id = @maintenance_job_type_id,
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceJobTypeVariantSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceJobTypeVariantSearchList]	
	@record_count int = NULL OUTPUT,
	@maintenance_job_type_id INT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_MaintenanceJobTypeVariant
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			AND maintenance_job_type_id = (CASE @maintenance_job_type_id WHEN 0 THEN maintenance_job_type_id ELSE @maintenance_job_type_id END)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_MaintenanceJobTypeVariant p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
	AND maintenance_job_type_id = (CASE @maintenance_job_type_id WHEN 0 THEN maintenance_job_type_id ELSE @maintenance_job_type_id END)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceJobTypeVariantSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceJobTypeVariantSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_MaintenanceJobTypeVariant p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceRequestDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceRequestDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_MaintenanceRequest SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceRequestInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceRequestInsertUpdateSingleItem]
	@id INT OUTPUT,
	@start_date DATE,
	@end_date DATE,
	@maintenance_request_type_id INT,
	@service_level_id INT,
	@requested_by_id INT,
	@functional_location_id INT,
	@fixed_asset_id INT,
	@fault_symptoms_id INT,
	@fault_area_id INT,
	@description VARCHAR(250),
	@status VARCHAR(50),
	@active BIT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		DECLARE @number INT
		SET @number = ISNULL((SELECT MAX(number) FROM amQt_MaintenanceRequest WHERE YEAR(date) = YEAR(GETDATE())), 0) + 1

		INSERT INTO amQt_MaintenanceRequest
		(
			number,
			start_date,
			end_date,
			maintenance_request_type_id,
			service_level_id,
			requested_by_id,
			functional_location_id,
			fixed_asset_id,
			fault_symptoms_id,
			fault_area_id,
			description,
			status,
			active

		)
		VALUES
		(
			@number,
			@start_date,
			@end_date,
			@maintenance_request_type_id,
			@service_level_id,
			@requested_by_id,
			@functional_location_id,
			@fixed_asset_id,
			@fault_symptoms_id,
			@fault_area_id,
			@description,
			@status,
			@active

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_MaintenanceRequest SET
			start_date = @start_date,
			end_date = @end_date,
			maintenance_request_type_id = @maintenance_request_type_id,
			service_level_id = @service_level_id,
			requested_by_id = @requested_by_id,
			functional_location_id = @functional_location_id,
			fixed_asset_id = @fixed_asset_id,
			fault_symptoms_id = @fault_symptoms_id,
			fault_area_id = @fault_area_id,
			description = @description,
			status = @status,
			active = @active

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceRequestSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[amQt_spMaintenanceRequestSearchList]	
	@record_count int = NULL OUTPUT,
	@start_date DATE = NULL,
	@end_date DATE = NULL
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(mr.id)
			FROM amQt_MaintenanceRequest mr
			WHERE mr.disable = 0
			AND (mr.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)			
			)
		RETURN
	END

	SELECT mr.*,
		dbo.amQt_fnTransactionNo(mr.date, mr.number) AS maintenance_request_no,
		mrt.name AS maintenance_request_type_name,
		sl.name AS service_level_name,
		p.name AS requested_by_name,
		fl.name AS functional_location_name,
		p2.name AS fixed_asset_name,
		fs.name AS fault_symptoms_name,
		fa2.name AS fault_area_name
	FROM amQt_MaintenanceRequest mr JOIN amQt_MaintenanceRequestType mrt
	ON mrt.id = mr.maintenance_request_type_id JOIN amQt_ServiceLevel sl
	ON sl.id = mr.service_level_id JOIN amQt_Personnel p
	ON p.id = mr.requested_by_id JOIN amQt_FunctionalLocation fl
	ON fl.id = mr.functional_location_id JOIN amQt_FixedAsset fa
	ON fa.id = mr.fixed_asset_id JOIN amQt_Product p2
	ON p2.id = fa.product_id JOIN amQt_FaultSymptoms fs
	ON fs.id = mr.fault_symptoms_id JOIN amQt_FaultArea fa2
	ON fa2.id = mr.fault_area_id
	WHERE mr.disable = 0
	AND (mr.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceRequestSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[amQt_spMaintenanceRequestSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT mr.*,
		dbo.amQt_fnTransactionNo(mr.date, mr.number) AS maintenance_request_no,
		mrt.name AS maintenance_request_type_name,
		sl.name AS service_level_name,
		p.name AS requested_by_name,
		fl.name AS functional_location_name,
		p2.name AS fixed_asset_name,
		fs.name AS fault_symptoms_name,
		fa2.name AS fault_area_name
	FROM amQt_MaintenanceRequest mr JOIN amQt_MaintenanceRequestType mrt
	ON mrt.id = mr.maintenance_request_type_id JOIN amQt_ServiceLevel sl
	ON sl.id = mr.service_level_id JOIN amQt_Personnel p
	ON p.id = mr.requested_by_id JOIN amQt_FunctionalLocation fl
	ON fl.id = mr.functional_location_id JOIN amQt_FixedAsset fa
	ON fa.id = mr.fixed_asset_id JOIN amQt_Product p2
	ON p2.id = fa.product_id JOIN amQt_FaultSymptoms fs
	ON fs.id = mr.fault_symptoms_id JOIN amQt_FaultArea fa2
	ON fa2.id = mr.fault_area_id
	WHERE mr.id = @id
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceRequestTypeDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceRequestTypeDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_MaintenanceRequestType SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceRequestTypeInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceRequestTypeInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_MaintenanceRequestType
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_MaintenanceRequestType SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceRequestTypeSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceRequestTypeSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_MaintenanceRequestType
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_MaintenanceRequestType p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spMaintenanceRequestTypeSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spMaintenanceRequestTypeSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_MaintenanceRequestType p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPaymentModeDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spPaymentModeDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_PaymentMode SET disable = 1 WHERE id = @id

END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spPaymentModeInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spPaymentModeInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_PaymentMode
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_PaymentMode SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END





GO
/****** Object:  StoredProcedure [dbo].[amQt_spPaymentModeSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spPaymentModeSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_PaymentMode
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_PaymentMode p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spPaymentModeSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spPaymentModeSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_PaymentMode p
	WHERE p.id = @id
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spPaymentTermsDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spPaymentTermsDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_PaymentTerms SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPaymentTermsInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spPaymentTermsInsertUpdateSingleItem]
	@id INT OUTPUT,
	@name VARCHAR(50),
	@remarks VARCHAR(250)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_PaymentTerms
		(
			name,
			remarks

		)
		VALUES
		(
			@name,
			@remarks

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_PaymentTerms SET
			name = @name,
			remarks = @remarks
		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spPaymentTermsSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spPaymentTermsSearchList]	
	@record_count int = NULL OUTPUT,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_PaymentTerms
			WHERE disable = 0
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_PaymentTerms p
	WHERE p.disable = 0
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPaymentTermsSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spPaymentTermsSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_PaymentTerms p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPersonnelDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPersonnelDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_Personnel SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spPersonnelInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPersonnelInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_Personnel
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_Personnel SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spPersonnelSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPersonnelSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_Personnel
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_Personnel p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spPersonnelSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPersonnelSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_Personnel p
	WHERE p.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spProductDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spProductDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_Product SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spProductInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spProductInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50),
	@unit_id INT = 0

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_Product
		(
			code,
			name,
			unit_id

		)
		VALUES
		(
			@code,
			@name,
			@unit_id

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_Product SET
			code = @code,
			name = @name,
			unit_id = @unit_id
		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spProductSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spProductSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_Product
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*,
		ISNULL(u.name, '') AS unit_name
	FROM amQt_Product p LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spProductSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spProductSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*,
		ISNULL(u.name, '') AS unit_name
	FROM amQt_Product p LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE p.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseOrderDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spPurchaseOrderDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_PurchaseOrder SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseOrderDetailDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseOrderDetailDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_PurchaseOrderDetail SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseOrderDetailInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseOrderDetailInsertUpdateSingleItem]
	@id INT OUTPUT,
	@purchase_order_id INT,
	@quotation_detail_id INT,
	@quantity DECIMAL(18,2)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_PurchaseOrderDetail
		(
			purchase_order_id,
			quotation_detail_id,
			quantity

		)
		VALUES
		(
			@purchase_order_id,
			@quotation_detail_id,
			@quantity

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_PurchaseOrderDetail SET

			quantity = @quantity

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseOrderDetailSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spPurchaseOrderDetailSearchList]	
	@record_count int = NULL OUTPUT,
	@purchase_order_id INT = 0,
	@quotation_detail_id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_PurchaseOrderDetail
			WHERE disable = 0
			AND purchase_order_id = (CASE @purchase_order_id WHEN 0 THEN purchase_order_id ELSE @purchase_order_id END)
			AND quotation_detail_id = (CASE @quotation_detail_id WHEN 0 THEN quotation_detail_id ELSE @quotation_detail_id END)
			)
		RETURN
	END

	SELECT pod.*,
		p.name AS product_name,
		ISNULL(u.name, '') AS unit_name,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) AS cost,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) * pod.quantity AS total_cost
	FROM amQt_PurchaseOrderDetail pod JOIN amQt_QuotationDetail qd 
	ON qd.id = pod.quotation_detail_id JOIN amQt_PurchaseRequestDetail pd
	ON pd.id = qd.purchase_request_detail_id JOIN amQt_Product p
	ON p.id = pd.product_id JOIN amQt_Quotation q
	ON q.id = qd.quotation_id LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE pod.disable = 0
	AND pod.purchase_order_id = (CASE @purchase_order_id WHEN 0 THEN pod.purchase_order_id ELSE @purchase_order_id END)
	AND pod.quotation_detail_id = (CASE @quotation_detail_id WHEN 0 THEN pod.quotation_detail_id ELSE @quotation_detail_id END)	
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseOrderDetailSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spPurchaseOrderDetailSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT pod.*,
		p.name AS product_name,
		ISNULL(u.name, '') AS unit_name,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) AS cost,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) * pod.quantity AS total_cost
	FROM amQt_PurchaseOrderDetail pod JOIN amQt_QuotationDetail qd 
	ON qd.id = pod.quotation_detail_id JOIN amQt_PurchaseRequestDetail pd
	ON pd.id = qd.purchase_request_detail_id JOIN amQt_Product p
	ON p.id = pd.product_id JOIN amQt_Quotation q
	ON q.id = qd.quotation_id LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE pod.id = @id
	
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseOrderInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseOrderInsertUpdateSingleItem]
	@id INT OUTPUT,
	@date_of_delivery DATE = NULL,
	@quotation_id INT,
	@terms VARCHAR(50),
	@prepared_by_id INT,
	@noted_by_id INT,
	@approved_by_id INT,
	@revised BIT,
	@cancelled BIT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN

		DECLARE @number INT
		SET @number = ISNULL((SELECT MAX(number) FROM amQt_PurchaseOrder WHERE YEAR(date) = YEAR(GETDATE())), 0) + 1

		INSERT INTO amQt_PurchaseOrder
		(
			date_of_delivery,
			number,
			quotation_id,
			terms,
			prepared_by_id,
			noted_by_id,
			approved_by_id,
			revised,
			cancelled

		)
		VALUES
		(
			@date_of_delivery,
			@number,
			@quotation_id,
			@terms,
			@prepared_by_id,
			@noted_by_id,
			@approved_by_id,
			@revised,
			@cancelled

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_PurchaseOrder SET

			date_of_delivery = @date_of_delivery,
			terms = @terms,
			prepared_by_id = @prepared_by_id,
			noted_by_id = @noted_by_id,
			approved_by_id = @approved_by_id,
			revised = @revised,
			cancelled = @cancelled

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
	RETURN 0
		END
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseOrderSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spPurchaseOrderSearchList]	
	@record_count int = NULL OUTPUT,
	@start_date DATE = NULL,
	@end_date DATE = NULL,
	@purchase_request_id INT = 0,
	@quotation_id INT = 0,
	@for_approval BIT = 0,
	@for_receiving BIT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(po.id)
			FROM amQt_PurchaseOrder po JOIN amQt_Quotation q
			ON q.id = po.quotation_id
			WHERE po.disable = 0
			AND (po.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
			AND po.quotation_id = (CASE @quotation_id WHEN 0 THEN po.quotation_id ELSE @quotation_id END)
			AND q.purchase_request_id = (CASE @purchase_request_id WHEN 0 THEN q.purchase_request_id ELSE @purchase_request_id END)
			AND (
				po.id IN (SELECT po.id
						FROM amQt_PurchaseOrderDetail pod JOIN amQt_QuotationDetail qd
						ON qd.id = pod.quotation_detail_id JOIN amQt_PurchaseRequestDetail pd
						ON pd.id = qd.purchase_request_detail_id JOIN amQt_PurchaseOrder po
						ON po.id = pod.purchase_order_id
						WHERE qd.disable = 0
						AND pd.disable = 0
						AND pod.quantity - ISNULL((SELECT SUM(quantity) FROM amQt_ReceivingDetail WHERE disable = 0 AND purchase_order_detail_id = pod.id), 0) > 0
						AND po.approved_by_id > 0
						)
				OR @for_receiving = 0
			)
			AND (po.approved_by_id = 0 OR @for_approval = 0)
			)
		RETURN
	END

	SELECT po.*,
		p.name AS prepared_by_name,
		ISNULL(p2.name, '') AS noted_by_name,
		ISNULL(p3.name, '') AS approved_by_name,
		(CASE q.supplier_no WHEN 1 THEN s1.name WHEN 2 THEN s2.name WHEN 3 THEN s3.name ELSE '' END) AS supplier_name,
		(CASE q.supplier_no WHEN 1 THEN s1.id WHEN 2 THEN s2.id WHEN 3 THEN s3.id ELSE 0 END) AS supplier_id,
		dbo.amQt_fnTransactionNo(pr.date, pr.number) AS purchase_request_no,
		dbo.amQt_fnTransactionNo(q.date, q.number) AS quotation_no,
		dbo.amQt_fnTransactionNo(po.date, po.number) AS purchase_order_no
	FROM amQt_PurchaseOrder po JOIN amQt_Quotation q
	ON q.id = po.quotation_id JOIN amQt_Personnel p
	ON p.id = po.prepared_by_id JOIN amQt_PurchaseRequest pr
	ON pr.id = q.purchase_request_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Personnel p2
	ON p2.id = po.noted_by_id LEFT JOIN amQt_Users u
	ON u.id = po.approved_by_id LEFT JOIN amQt_Personnel p3
	ON p3.id = u.personnel_id 
	WHERE po.disable = 0
	AND (po.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
	AND po.quotation_id = (CASE @quotation_id WHEN 0 THEN po.quotation_id ELSE @quotation_id END)
	AND q.purchase_request_id = (CASE @purchase_request_id WHEN 0 THEN q.purchase_request_id ELSE @purchase_request_id END)
	AND (
			po.id IN (SELECT po.id
					FROM amQt_PurchaseOrderDetail pod JOIN amQt_QuotationDetail qd
					ON qd.id = pod.quotation_detail_id JOIN amQt_PurchaseRequestDetail pd
					ON pd.id = qd.purchase_request_detail_id JOIN amQt_PurchaseOrder po
					ON po.id = pod.purchase_order_id
					WHERE qd.disable = 0
					AND pd.disable = 0
					AND pod.quantity - ISNULL((SELECT SUM(quantity) FROM amQt_ReceivingDetail WHERE disable = 0 AND purchase_order_detail_id = pod.id), 0) > 0
					AND po.approved_by_id > 0
					)
			OR @for_receiving = 0
		)
	AND (po.approved_by_id = 0 OR @for_approval = 0)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseOrderSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spPurchaseOrderSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT po.*,
		p.name AS prepared_by_name,
		ISNULL(p2.name, '') AS noted_by_name,
		ISNULL(p3.name, '') AS approved_by_name,
		(CASE q.supplier_no WHEN 1 THEN s1.name WHEN 2 THEN s2.name WHEN 3 THEN s3.name ELSE '' END) AS supplier_name,
		(CASE q.supplier_no WHEN 1 THEN s1.id WHEN 2 THEN s2.id WHEN 3 THEN s3.id ELSE 0 END) AS supplier_id,
		dbo.amQt_fnTransactionNo(pr.date, pr.number) AS purchase_request_no,
		dbo.amQt_fnTransactionNo(q.date, q.number) AS quotation_no,
		dbo.amQt_fnTransactionNo(po.date, po.number) AS purchase_order_no
	FROM amQt_PurchaseOrder po JOIN amQt_Quotation q
	ON q.id = po.quotation_id JOIN amQt_Personnel p
	ON p.id = po.prepared_by_id JOIN amQt_PurchaseRequest pr
	ON pr.id = q.purchase_request_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Personnel p2
	ON p2.id = po.noted_by_id LEFT JOIN amQt_Users u
	ON u.id = po.approved_by_id LEFT JOIN amQt_Personnel p3
	ON p3.id = u.personnel_id 
	WHERE po.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseRequestDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseRequestDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_PurchaseRequest SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseRequestDetailDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseRequestDetailDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_PurchaseRequestDetail SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseRequestDetailInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseRequestDetailInsertUpdateSingleItem]
	@id INT OUTPUT,
	@purchase_request_id INT,
	@product_id INT,
	@quantity DECIMAL(18,2),
	@cost DECIMAL(18,2)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_PurchaseRequestDetail
		(
			purchase_request_id,
			product_id,
			quantity,
			cost

		)
		VALUES
		(
			@purchase_request_id,
			@product_id,
			@quantity,
			@cost

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_PurchaseRequestDetail SET
			purchase_request_id = @purchase_request_id,
			product_id = @product_id,
			quantity = @quantity,
			cost = @cost

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseRequestDetailSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseRequestDetailSearchList]	
	@record_count int = NULL OUTPUT,
	@purchase_request_id INT
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_PurchaseRequestDetail
			WHERE disable = 0
			AND purchase_request_id = (CASE @purchase_request_id WHEN 0 THEN purchase_request_id ELSE @purchase_request_id END)
			)
		RETURN
	END

	SELECT prd.*,
		p.name AS product_name,
		ISNULL(u.name, '') AS unit_name
	FROM amQt_PurchaseRequestDetail prd JOIN amQt_Product p
	ON p.id = prd.product_id LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE prd.disable = 0
	AND prd.purchase_request_id = (CASE @purchase_request_id WHEN 0 THEN prd.purchase_request_id ELSE @purchase_request_id END)
	
END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseRequestDetailSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseRequestDetailSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT prd.*,
		p.name AS product_name,
		ISNULL(u.name, '') AS unit_name
	FROM amQt_PurchaseRequestDetail prd JOIN amQt_Product p
	ON p.id = prd.product_id LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE prd.id = @id
	
END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseRequestInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseRequestInsertUpdateSingleItem]
	@id INT OUTPUT,
	@requested_by_id INT,
	@date_required DATE,
	@supplier_1_id INT,
	@supplier_2_id INT,
	@supplier_3_id INT,
	@remarks VARCHAR(500),
	@approved_by_id INT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		DECLARE @number INT
		SET @number = ISNULL((SELECT MAX(number) FROM amQt_PurchaseRequest WHERE YEAR(date) = YEAR(GETDATE())), 0) + 1
		INSERT INTO amQt_PurchaseRequest
		(
			number,
			requested_by_id,
			date_required,
			supplier_1_id,
			supplier_2_id,
			supplier_3_id,
			remarks,
			approved_by_id

		)
		VALUES
		(
			@number,
			@requested_by_id,
			@date_required,
			@supplier_1_id,
			@supplier_2_id,
			@supplier_3_id,
			@remarks,
			@approved_by_id

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_PurchaseRequest SET

			requested_by_id = @requested_by_id,
			date_required = @date_required,
			supplier_1_id = @supplier_1_id,
			supplier_2_id = @supplier_2_id,
			supplier_3_id = @supplier_3_id,
			remarks = @remarks,
			approved_by_id = @approved_by_id

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseRequestSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseRequestSearchList]	
	@record_count int = NULL OUTPUT,
	@start_date DATE = NULL,
	@end_date DATE = NULL,
	@for_approval BIT = 0,
	@for_quotation BIT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_PurchaseRequest
			WHERE disable = 0
			AND (date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
			AND (id NOT IN (SELECT purchase_request_id FROM amQt_Quotation WHERE disable = 0)  AND approved_by_id > 0 OR @for_quotation = 0)
			AND (approved_by_id = 0 OR @for_approval = 0)
			)
		RETURN
	END

	SELECT pr.*,
		p.name AS requested_by_name,
		ISNULL(s1.name, '') AS supplier_1_name,
		ISNULL(s2.name, '') AS supplier_2_name,
		ISNULL(s3.name, '') AS supplier_3_name,
		ISNULL(p2.name, '') AS approved_by_name,
		dbo.amQt_fnTransactionNo(pr.date, pr.number) AS purchase_request_no
	FROM amQt_PurchaseRequest pr JOIN amQt_Personnel p
	ON p.id = pr.requested_by_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Users u
	ON u.id = pr.approved_by_id LEFT JOIN amQt_Personnel p2
	ON p2.id = u.personnel_id
	WHERE pr.disable = 0
	AND (pr.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
	AND (pr.id NOT IN (SELECT purchase_request_id FROM amQt_Quotation WHERE disable = 0 ) AND pr.approved_by_id > 0 OR @for_quotation = 0)
	AND (pr.approved_by_id = 0 OR @for_approval = 0)
END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseRequestSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseRequestSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT pr.*,
		p.name AS requested_by_name,
		ISNULL(s1.name, '') AS supplier_1_name,
		ISNULL(s2.name, '') AS supplier_2_name,
		ISNULL(s3.name, '') AS supplier_3_name,
		ISNULL(p2.name, '') AS approved_by_name,
		dbo.amQt_fnTransactionNo(pr.date, pr.number) AS purchase_request_no
	FROM amQt_PurchaseRequest pr JOIN amQt_Personnel p
	ON p.id = pr.requested_by_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Users u
	ON u.id = pr.approved_by_id LEFT JOIN amQt_Personnel p2
	ON p2.id = u.personnel_id
	WHERE pr.id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseVoucherDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseVoucherDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_PurchaseVoucher SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseVoucherInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spPurchaseVoucherInsertUpdateSingleItem]
	@id INT OUTPUT,
	@receiving_id INT,
	@payment_mode_id INT,
	@prepared_by_id INT,
	@checked_by_id INT,
	@approved_by_id INT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		
		DECLARE @number INT
		SET @number = ISNULL((SELECT MAX(number) FROM amQt_PurchaseVoucher WHERE YEAR(date) = YEAR(GETDATE())), 0) + 1

		INSERT INTO amQt_PurchaseVoucher
		(
			number,
			receiving_id,
			payment_mode_id,
			prepared_by_id,
			checked_by_id,
			approved_by_id

		)
		VALUES
		(
			@number,
			@receiving_id,
			@payment_mode_id,
			@prepared_by_id,
			@checked_by_id,
			@approved_by_id

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_PurchaseVoucher SET

			receiving_id = @receiving_id,
			payment_mode_id = @payment_mode_id,
			prepared_by_id = @prepared_by_id,
			checked_by_id = @checked_by_id,
			approved_by_id = @approved_by_id

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseVoucherSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[amQt_spPurchaseVoucherSearchList]	
	@record_count int = NULL OUTPUT,
	@start_date DATE = NULL,
	@end_date DATE = NULL,
	@for_fixed_asset BIT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(v.id)
			FROM amQt_PurchaseVoucher v JOIN amQt_Receiving r
			ON r.id = v.receiving_id
			WHERE v.disable = 0
			AND (v.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
			AND (r.id IN (
					SELECT rd.receiving_id
					FROM amQt_Receiving r JOIN amQt_ReceivingDetail rd
					ON r.id = rd.receiving_id LEFT JOIN amQt_FixedAsset fa
					ON rd.id = fa.receiving_detail_id AND fa.disable = 0
					WHERE r.disable = 0
					AND rd.disable = 0
					AND r.approved_by_id > 0					
					GROUP BY rd.receiving_id, rd.quantity
					HAVING rd.quantity != COUNT(fa.id)
				)
				OR @for_fixed_asset = 0
			)
		)
		RETURN
	END

	SELECT v.*,
		dbo.amQt_fnTransactionNo(v.date, v.number) AS transaction_no,
		r.invoice_no,
		dbo.amQt_fnTransactionNo(r.date, r.number) AS receiving_no,
		dbo.amQt_fnTransactionNo(po.date, po.number) AS purchase_order_no,
		(CASE q.supplier_no WHEN 1 THEN s1.name WHEN 2 THEN s2.name WHEN 3 THEN s3.name ELSE '' END) AS supplier_name,
		r.amount,
		p.name AS prepared_by_name,
		ISNULL(p2.name, '') AS checked_by_name,
		ISNULL(p3.name, '') AS approved_by_name		
	FROM amQt_PurchaseVoucher v JOIN amQt_Receiving r
	ON r.id = v.receiving_id JOIN amQt_PurchaseOrder po
	ON po.id = r.purchase_order_id JOIN amQt_Quotation q
	ON q.id = po.quotation_id JOIN amQt_Personnel p
	ON p.id = v.prepared_by_id JOIN amQt_PurchaseRequest pr
	ON pr.id = q.purchase_request_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Personnel p2
	ON p2.id = v.checked_by_id LEFT JOIN amQt_Users u
	ON u.id = v.approved_by_id LEFT JOIN amQt_Personnel p3
	ON p3.id = u.personnel_id 
	WHERE v.disable = 0
	AND (v.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
	AND (r.id IN (
			SELECT rd.receiving_id
			FROM amQt_Receiving r JOIN amQt_ReceivingDetail rd
			ON r.id = rd.receiving_id LEFT JOIN amQt_FixedAsset fa
			ON rd.id = fa.receiving_detail_id AND fa.disable = 0
			WHERE r.disable = 0
			AND rd.disable = 0
			AND r.approved_by_id > 0		
			GROUP BY rd.receiving_id, rd.quantity
			HAVING rd.quantity != COUNT(fa.id)
		)
		OR @for_fixed_asset = 0
	)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spPurchaseVoucherSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[amQt_spPurchaseVoucherSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT v.*,
		dbo.amQt_fnTransactionNo(v.date, v.number) AS transaction_no,
		r.invoice_no,
		dbo.amQt_fnTransactionNo(r.date, r.number) AS receiving_no,
		dbo.amQt_fnTransactionNo(po.date, po.number) AS purchase_order_no,
		(CASE q.supplier_no WHEN 1 THEN s1.name WHEN 2 THEN s2.name WHEN 3 THEN s3.name ELSE '' END) AS supplier_name,
		r.amount,
		p.name AS prepared_by_name,
		ISNULL(p2.name, '') AS checked_by_name,
		ISNULL(p3.name, '') AS approved_by_name		
	FROM amQt_PurchaseVoucher v JOIN amQt_Receiving r
	ON r.id = v.receiving_id JOIN amQt_PurchaseOrder po
	ON po.id = r.purchase_order_id JOIN amQt_Quotation q
	ON q.id = po.quotation_id JOIN amQt_Personnel p
	ON p.id = v.prepared_by_id JOIN amQt_PurchaseRequest pr
	ON pr.id = q.purchase_request_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Personnel p2
	ON p2.id = v.checked_by_id LEFT JOIN amQt_Users u
	ON u.id = v.approved_by_id LEFT JOIN amQt_Personnel p3
	ON p3.id = u.personnel_id 
	WHERE v.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spQuotationDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spQuotationDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_Quotation SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spQuotationDetailDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spQuotationDetailDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_QuotationDetail SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spQuotationDetailInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spQuotationDetailInsertUpdateSingleItem]
	@id INT OUTPUT,
	@quotation_id INT,
	@purchase_request_detail_id INT,
	@cost1 DECIMAL(18,2),
	@cost2 DECIMAL(18,2),
	@cost3 DECIMAL(18,2)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_QuotationDetail
		(
			quotation_id,
			purchase_request_detail_id,
			cost1,
			cost2,
			cost3

		)
		VALUES
		(
			@quotation_id,
			@purchase_request_detail_id,
			@cost1,
			@cost2,
			@cost3

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_QuotationDetail SET
			cost1 = @cost1,
			cost2 = @cost2,
			cost3 = @cost3

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spQuotationDetailSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spQuotationDetailSearchList]	
	@record_count int = NULL OUTPUT,
	@quotation_id INT
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_QuotationDetail
			WHERE disable = 0
			AND quotation_id = (CASE @quotation_id WHEN 0 THEN quotation_id ELSE @quotation_id END)
			)
		RETURN
	END

	SELECT qd.*,
		p.name AS product_name,
		ISNULL(u.name, '') AS unit_name,
		pd.quantity,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) AS cost,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) * pd.quantity AS total_cost
	FROM amQt_QuotationDetail qd JOIN amQt_PurchaseRequestDetail pd
	ON pd.id = qd.purchase_request_detail_id JOIN amQt_Product p
	ON p.id = pd.product_id JOIN amQt_Quotation q
	ON q.id = qd.quotation_id LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE qd.disable = 0
	AND qd.quotation_id = (CASE @quotation_id WHEN 0 THEN qd.quotation_id ELSE @quotation_id END)
	
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spQuotationDetailSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spQuotationDetailSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT qd.*,
		p.name AS product_name,
		ISNULL(u.name, '') AS unit_name,
		pd.quantity,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) AS cost,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) * pd.quantity AS total_cost
	FROM amQt_QuotationDetail qd JOIN amQt_PurchaseRequestDetail pd
	ON pd.id = qd.purchase_request_detail_id JOIN amQt_Product p
	ON p.id = pd.product_id JOIN amQt_Quotation q
	ON q.id = qd.quotation_id LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE qd.id = @id
	
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spQuotationInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spQuotationInsertUpdateSingleItem]
	@id INT OUTPUT,
	@purchase_request_id INT,
	@prepared_by_id INT,
	@approved_by_id INT,
	@supplier_no TINYINT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		DECLARE @number INT
		SET @number = ISNULL((SELECT MAX(number) FROM amQt_Quotation WHERE YEAR(date) = YEAR(GETDATE())), 0) + 1

		INSERT INTO amQt_Quotation
		(
			number,
			purchase_request_id,
			prepared_by_id,
			approved_by_id,
			supplier_no

		)
		VALUES
		(
			@number,
			@purchase_request_id,
			@prepared_by_id,
			@approved_by_id,
			@supplier_no

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_Quotation SET
			purchase_request_id = @purchase_request_id,
			prepared_by_id = @prepared_by_id,
			approved_by_id = @approved_by_id,
			supplier_no = @supplier_no

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spQuotationSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spQuotationSearchList]	
	@record_count int = NULL OUTPUT,
	@start_date DATE = NULL,
	@end_date DATE = NULL,
	@purchase_request_id INT = 0,
	@for_approval BIT = 0,
	@for_po BIT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_Quotation
			WHERE disable = 0
			AND (date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
			AND purchase_request_id = (CASE @purchase_request_id WHEN 0 THEN purchase_request_id ELSE @purchase_request_id END)
			AND (
				id IN (SELECT q.id
						FROM amQt_QuotationDetail qd JOIN amQt_PurchaseRequestDetail pd
						ON pd.id = qd.purchase_request_detail_id JOIN amQt_Quotation q
						ON q.id = qd.quotation_id
						WHERE qd.disable = 0
						AND pd.disable = 0
						AND pd.quantity - ISNULL((SELECT SUM(quantity) FROM amQt_PurchaseOrderDetail WHERE disable = 0 AND quotation_detail_id = qd.id), 0) > 0
						AND q.approved_by_id > 0
						)
				OR @for_po = 0
			)
			AND (approved_by_id = 0 OR @for_approval = 0)
			)
		RETURN
	END

	SELECT q.*,
		p.name AS prepared_by_name,
		ISNULL(s1.name, '') AS supplier_1_name,
		ISNULL(s2.name, '') AS supplier_2_name,
		ISNULL(s3.name, '') AS supplier_3_name,
		ISNULL(p2.name, '') AS approved_by_name,
		dbo.amQt_fnTransactionNo(pr.date, pr.number) AS purchase_request_no,
		dbo.amQt_fnTransactionNo(q.date, q.number) AS quotation_no
	FROM amQt_Quotation q JOIN amQt_Personnel p
	ON p.id = q.prepared_by_id JOIN amQt_PurchaseRequest pr
	ON pr.id = q.purchase_request_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Users u
	ON u.id = q.approved_by_id LEFT JOIN amQt_Personnel p2
	ON p2.id = u.personnel_id
	WHERE q.disable = 0
	AND (q.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
	AND q.purchase_request_id = (CASE @purchase_request_id WHEN 0 THEN q.purchase_request_id ELSE @purchase_request_id END)
	AND (
			q.id IN (SELECT qd.id
					FROM amQt_QuotationDetail qd JOIN amQt_PurchaseRequestDetail pd
					ON pd.id = qd.purchase_request_detail_id JOIN amQt_Quotation q
					ON q.id = qd.quotation_id
					WHERE qd.disable = 0
					AND pd.disable = 0
					AND pd.quantity - ISNULL((SELECT SUM(quantity) FROM amQt_PurchaseOrderDetail WHERE disable = 0 AND quotation_detail_id = qd.id), 0) > 0
					AND q.approved_by_id > 0
					)
			OR @for_po = 0
		)
	AND (q.approved_by_id = 0 OR @for_approval = 0)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spQuotationSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spQuotationSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT q.*,
		p.name AS prepared_by_name,
		ISNULL(s1.name, '') AS supplier_1_name,
		ISNULL(s2.name, '') AS supplier_2_name,
		ISNULL(s3.name, '') AS supplier_3_name,
		ISNULL(p2.name, '') AS approved_by_name,
		dbo.amQt_fnTransactionNo(pr.date, pr.number) AS purchase_request_no,
		dbo.amQt_fnTransactionNo(q.date, q.number) AS quotation_no
	FROM amQt_Quotation q JOIN amQt_Personnel p
	ON p.id = q.prepared_by_id JOIN amQt_PurchaseRequest pr
	ON pr.id = q.purchase_request_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Users u
	ON u.id = q.approved_by_id LEFT JOIN amQt_Personnel p2
	ON p2.id = u.personnel_id
	WHERE q.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spReceivingDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spReceivingDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_Receiving SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spReceivingDetailDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spReceivingDetailDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_ReceivingDetail SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spReceivingDetailInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spReceivingDetailInsertUpdateSingleItem]
	@id INT OUTPUT,
	@receiving_id INT,
	@purchase_order_detail_id INT,
	@quantity DECIMAL(18,2)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_ReceivingDetail
		(
			receiving_id,
			purchase_order_detail_id,
			quantity

		)
		VALUES
		(
			@receiving_id,
			@purchase_order_detail_id,
			@quantity

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_ReceivingDetail SET

			quantity = @quantity

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spReceivingDetailSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[amQt_spReceivingDetailSearchList]	
	@record_count int = NULL OUTPUT,
	@receiving_id INT = 0,
	@purchase_order_detail_id INT = 0,
	@for_fixed_asset BIT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_ReceivingDetail
			WHERE disable = 0
			AND receiving_id = (CASE @receiving_id WHEN 0 THEN receiving_id ELSE @receiving_id END)
			AND purchase_order_detail_id = (CASE @purchase_order_detail_id WHEN 0 THEN purchase_order_detail_id ELSE @purchase_order_detail_id END)
			AND (id IN (
					SELECT rd.id
					FROM amQt_Receiving r JOIN amQt_ReceivingDetail rd
					ON r.id = rd.receiving_id LEFT JOIN amQt_FixedAsset fa
					ON rd.id = fa.receiving_detail_id AND fa.disable = 0
					WHERE r.disable = 0
					AND rd.disable = 0
					AND r.approved_by_id > 0					
					GROUP BY rd.id, rd.quantity
					HAVING rd.quantity != COUNT(fa.id)
				)
				OR @for_fixed_asset = 0
			)
		)
		RETURN
	END

	SELECT rd.*,
		p.name AS product_name,
		ISNULL(u.name, '') AS unit_name,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) AS cost,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) * rd.quantity AS total_cost
	FROM amQt_ReceivingDetail rd JOIN amQt_PurchaseOrderDetail pod
	ON pod.id = rd.purchase_order_detail_id JOIN amQt_QuotationDetail qd 
	ON qd.id = pod.quotation_detail_id JOIN amQt_PurchaseRequestDetail pd
	ON pd.id = qd.purchase_request_detail_id JOIN amQt_Product p
	ON p.id = pd.product_id JOIN amQt_Quotation q
	ON q.id = qd.quotation_id LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE rd.disable = 0
	AND rd.receiving_id = (CASE @receiving_id WHEN 0 THEN rd.receiving_id ELSE @receiving_id END)
	AND rd.purchase_order_detail_id = (CASE @purchase_order_detail_id WHEN 0 THEN rd.purchase_order_detail_id ELSE @purchase_order_detail_id END)
	AND (rd.id IN (
			SELECT rd.id
			FROM amQt_Receiving r JOIN amQt_ReceivingDetail rd
			ON r.id = rd.receiving_id LEFT JOIN amQt_FixedAsset fa
			ON rd.id = fa.receiving_detail_id AND fa.disable = 0
			WHERE r.disable = 0
			AND rd.disable = 0
			AND r.approved_by_id > 0					
			GROUP BY rd.id, rd.quantity
			HAVING rd.quantity != COUNT(fa.id)
		)
		OR @for_fixed_asset = 0
	)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spReceivingDetailSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[amQt_spReceivingDetailSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT rd.*,
		p.name AS product_name,
		ISNULL(u.name, '') AS unit_name,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) AS cost,
		(CASE q.supplier_no WHEN 1 THEN qd.cost1 WHEN 2 THEN qd.cost2 WHEN 3 THEN qd.cost3 ELSE 0 END) * rd.quantity AS total_cost
	FROM amQt_ReceivingDetail rd JOIN amQt_PurchaseOrderDetail pod
	ON pod.id = rd.purchase_order_detail_id JOIN amQt_QuotationDetail qd 
	ON qd.id = pod.quotation_detail_id JOIN amQt_PurchaseRequestDetail pd
	ON pd.id = qd.purchase_request_detail_id JOIN amQt_Product p
	ON p.id = pd.product_id JOIN amQt_Quotation q
	ON q.id = qd.quotation_id LEFT JOIN amQt_Unit u
	ON u.id = p.unit_id
	WHERE rd.id = @id
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spReceivingInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spReceivingInsertUpdateSingleItem]
	@id INT OUTPUT,
	@purchase_order_id INT,
	@prepared_by_id INT,
	@checked_by_id INT,
	@approved_by_id INT,
	@invoice_no VARCHAR(50),
	@dr_no VARCHAR(50),
	@amount DECIMAL(18,2),
	@remarks VARCHAR(500) = ''

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN

		DECLARE @number INT
		SET @number = ISNULL((SELECT MAX(number) FROM amQt_Receiving WHERE YEAR(date) = YEAR(GETDATE())), 0) + 1

		INSERT INTO amQt_Receiving
		(
			number,
			purchase_order_id,
			prepared_by_id,
			checked_by_id,
			approved_by_id,
			invoice_no,
			dr_no,
			amount,
			remarks

		)
		VALUES
		(
			@number,
			@purchase_order_id,
			@prepared_by_id,
			@checked_by_id,
			@approved_by_id,
			@invoice_no,
			@dr_no,
			@amount,
			@remarks

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_Receiving SET

			purchase_order_id = @purchase_order_id,
			prepared_by_id = @prepared_by_id,
			checked_by_id = @checked_by_id,
			approved_by_id = @approved_by_id,
			invoice_no = @invoice_no,
			dr_no = @dr_no,
			amount = @amount,
			remarks = @remarks

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spReceivingSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[amQt_spReceivingSearchList]	
	@record_count int = NULL OUTPUT,
	@start_date DATE = NULL,
	@end_date DATE = NULL,
	@quotation_id INT = 0,
	@purchase_order_id INT = 0,
	@for_approval BIT = 0,
	@for_purchase_voucher BIT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(po.id)
			FROM amQt_Receiving r JOIN amQt_PurchaseOrder po
			ON po.id = r.purchase_order_id
			WHERE r.disable = 0
			AND (r.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
			AND r.purchase_order_id = (CASE @purchase_order_id WHEN 0 THEN r.purchase_order_id ELSE @purchase_order_id END)
			AND po.quotation_id = (CASE @quotation_id WHEN 0 THEN po.quotation_id ELSE @quotation_id END)			
			AND (r.id NOT IN (SELECT receiving_id FROM amQt_PurchaseVoucher WHERE disable = 0) AND r.approved_by_id > 0 OR @for_purchase_voucher = 0)
			AND (r.approved_by_id = 0 OR @for_approval = 0)
			)
		RETURN
	END

	SELECT r.*,
		p.name AS prepared_by_name,
		ISNULL(p2.name, '') AS checked_by_name,
		ISNULL(p3.name, '') AS approved_by_name,
		(CASE q.supplier_no WHEN 1 THEN s1.name WHEN 2 THEN s2.name WHEN 3 THEN s3.name ELSE '' END) AS supplier_name,
		(CASE q.supplier_no WHEN 1 THEN s1.id WHEN 2 THEN s2.id WHEN 3 THEN s3.id ELSE 0 END) AS supplier_id,
		dbo.amQt_fnTransactionNo(pr.date, pr.number) AS purchase_request_no,
		dbo.amQt_fnTransactionNo(q.date, q.number) AS quotation_no,
		dbo.amQt_fnTransactionNo(po.date, po.number) AS purchase_order_no,
		dbo.amQt_fnTransactionNo(r.date, r.number) AS receiving_no
	FROM amQt_Receiving r JOIN amQt_PurchaseOrder po
	ON po.id = r.purchase_order_id JOIN amQt_Quotation q
	ON q.id = po.quotation_id JOIN amQt_Personnel p
	ON p.id = r.prepared_by_id JOIN amQt_PurchaseRequest pr
	ON pr.id = q.purchase_request_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Personnel p2
	ON p2.id = r.checked_by_id LEFT JOIN amQt_Users u
	ON u.id = r.approved_by_id LEFT JOIN amQt_Personnel p3
	ON p3.id = u.personnel_id 
	WHERE r.disable = 0
	AND (r.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
	AND r.purchase_order_id = (CASE @purchase_order_id WHEN 0 THEN r.purchase_order_id ELSE @purchase_order_id END)
	AND po.quotation_id = (CASE @quotation_id WHEN 0 THEN po.quotation_id ELSE @quotation_id END)			
	AND (r.id NOT IN (SELECT receiving_id FROM amQt_PurchaseVoucher WHERE disable = 0) AND r.approved_by_id > 0 OR @for_purchase_voucher = 0)
	AND (r.approved_by_id = 0 OR @for_approval = 0)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spReceivingSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[amQt_spReceivingSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT r.*,
		p.name AS prepared_by_name,
		ISNULL(p2.name, '') AS checked_by_name,
		ISNULL(p3.name, '') AS approved_by_name,
		(CASE q.supplier_no WHEN 1 THEN s1.name WHEN 2 THEN s2.name WHEN 3 THEN s3.name ELSE '' END) AS supplier_name,
		(CASE q.supplier_no WHEN 1 THEN s1.id WHEN 2 THEN s2.id WHEN 3 THEN s3.id ELSE 0 END) AS supplier_id,
		dbo.amQt_fnTransactionNo(pr.date, pr.number) AS purchase_request_no,
		dbo.amQt_fnTransactionNo(q.date, q.number) AS quotation_no,
		dbo.amQt_fnTransactionNo(po.date, po.number) AS purchase_order_no,
		dbo.amQt_fnTransactionNo(r.date, r.number) AS receiving_no
	FROM amQt_Receiving r JOIN amQt_PurchaseOrder po
	ON po.id = r.purchase_order_id JOIN amQt_Quotation q
	ON q.id = po.quotation_id JOIN amQt_Personnel p
	ON p.id = r.prepared_by_id JOIN amQt_PurchaseRequest pr
	ON pr.id = q.purchase_request_id LEFT JOIN amQt_Supplier s1
	ON s1.id = pr.supplier_1_id LEFT JOIN amQt_Supplier s2
	ON s2.id = pr.supplier_2_id LEFT JOIN amQt_Supplier s3
	ON s3.id = pr.supplier_3_id LEFT JOIN amQt_Personnel p2
	ON p2.id = r.checked_by_id LEFT JOIN amQt_Users u
	ON u.id = r.approved_by_id LEFT JOIN amQt_Personnel p3
	ON p3.id = u.personnel_id 
	WHERE r.id = @id		
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spReportDepreciationScheduleStraightLineActualDaysAnnually]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spReportDepreciationScheduleStraightLineActualDaysAnnually]
	@id INT = 5,
	@asset_type_id INT = 0,
	@year SMALLINT = 2023
AS
BEGIN

	DECLARE @table TABLE(StartDate DATE, EndDate DATE, AssetNo VARCHAR(50), AssetName VARCHAR(250), UsefulLifeYear INT, PurchaseDate DATE, EndDepreciationDate DATE, PurchaseCost DECIMAL(19,2), ResidualValue DECIMAL(18,0), Beginning DECIMAL(18,2), DepreciationExpense DECIMAL(18,2), Ending DECIMAL(18,2), BookValueEnd DECIMAL(18,2))


	DECLARE my_cursor CURSOR FOR
	SELECT id
	FROM amQt_FixedAsset
	WHERE id = (CASE @id WHEN 0 THEN id ELSE @id END)
	AND asset_type_id = (CASE @asset_type_id WHEN 0 THEN asset_type_id ELSE @asset_type_id END)
	AND disable = 0
	OPEN my_cursor

	FETCH NEXT FROM my_cursor
	INTO @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		INSERT INTO @table
		SELECT t.start_date AS StartDate,
			t.end_date AS EndDate,
			fa.asset_no AS AssetNo,
			p.name AS AssetName,
			fa.useful_life_years AS UsefulLifeYears,
			fa.purchase_date AS PurchaseDate,
			DATEADD(D, -1, DATEADD(YEAR, fa.useful_life_years, fa.purchase_date)) AS EndDepreciationDate,
			fa.purchase_price AS PurchaseCost,
			fa.residual_value AS ResidualValue,
			t.beginning AS Beginning,
			t.depreciation_expense AS DepreciationExpense,
			t.accumulated_depreciation AS Ending,
			t.ending_book_value AS BookValueEnd
		FROM amQt_fnDepreciationScheduleStraightLineActualDays(@id, @year) t CROSS JOIN amQt_FixedAsset fa JOIN amQt_Product p
		ON p.id = fa.product_id
		WHERE t.year >= @year
		AND fa.id = @id

		FETCH NEXT FROM my_cursor
		INTO @id
	END

	CLOSE my_cursor
	DEALLOCATE my_cursor
	
	SELECT *, YEAR(StartDate) AS Year FROM @table
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spReportDepreciationScheduleStraightLineActualDaysMonthly]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spReportDepreciationScheduleStraightLineActualDaysMonthly]
	@id INT = 5,
	@asset_type_id INT = 0,
	@year SMALLINT = 2023
AS
BEGIN
	DECLARE @purchase_date DATE,
			@end_depreciation_date DATE,
			@start_date DATE,
			@end_date DATE,
			@purchase_cost DECIMAL(18,2),
			@residual_value DECIMAL(18,2),
			@useful_life_years INT

	DECLARE @table TABLE(StartDate DATE, EndDate DATE, AssetNo VARCHAR(50), AssetName VARCHAR(250), UsefulLifeYear INT, PurchaseDate DATE, EndDepreciationDate DATE, PurchaseCost DECIMAL(19,2), ResidualValue DECIMAL(18,0), Beginning DECIMAL(18,2), DepreciationExpense DECIMAL(18,2), Ending DECIMAL(18,2),
			[Jan] DECIMAL(18,2), [Feb] DECIMAL(18,2), [Mar] DECIMAL(18,2), [Apr] DECIMAL(18,2), [May] DECIMAL(18,2), [Jun] DECIMAL(18,2), [Jul] DECIMAL(18,2), [Aug] DECIMAL(18,2), [Sep] DECIMAL(18,2), [Oct] DECIMAL(18,2), [Nov] DECIMAL(18,2), [Dec] DECIMAL(18,2), BookValueEnd DECIMAL(18,2))

	DECLARE my_cursor CURSOR FOR
	SELECT id
	FROM amQt_FixedAsset
	WHERE id = (CASE @id WHEN 0 THEN id ELSE @id END)
	AND asset_type_id = (CASE @asset_type_id WHEN 0 THEN asset_type_id ELSE @asset_type_id END)
	AND disable = 0
	OPEN my_cursor

	FETCH NEXT FROM my_cursor
	INTO @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		SELECT @purchase_date = purchase_date,
			@purchase_cost = purchase_price,
			@residual_value = residual_value,
			@useful_life_years = useful_life_years
		FROM amQt_FixedAsset
		WHERE id = @id
		
		DECLARE @depcreciation_rate FLOAT,--float so that the aboslute value of depreciation expense is 100% accurate
			@depcreciation_rate_yearly FLOAT = 100.0 / @useful_life_years,
			@depreciation_expense DECIMAL(18,2)

		SET @end_depreciation_date = DATEADD(D, -1, DATEADD(YEAR, @useful_life_years, @purchase_date))
		SET @start_date = @purchase_date

		DECLARE @start_date_constant DATE = @start_date
		DECLARE @days_in_year INT = DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), DATEFROMPARTS(YEAR(@start_date), 12, 31)) + 1

		DECLARE @table_dates TABLE(start_date DATE, end_date DATE, year INT, beginning_book_value DECIMAL(19,2), depreciation_rate DECIMAL(18,0), beginning DECIMAL(18,2), depreciation_expense DECIMAL(18,2), accumulated_depreciation DECIMAL(18,2), ending_book_value DECIMAL(18,2),
			[Jan] DECIMAL(18,2), [Feb] DECIMAL(18,2), [Mar] DECIMAL(18,2), [Apr] DECIMAL(18,2), [May] DECIMAL(18,2), [Jun] DECIMAL(18,2), [Jul] DECIMAL(18,2), [Aug] DECIMAL(18,2), [Sep] DECIMAL(18,2), [Oct] DECIMAL(18,2), [Nov] DECIMAL(18,2), [Dec] DECIMAL(18,2))

		DECLARE @accumulated_depreciation DECIMAL(18,2) = 0,
				@beginning_book_value DECIMAL(18,2) = @purchase_cost,
				@ending_book_value DECIMAL(18,2) = @purchase_cost		

		WHILE @start_date <= @end_depreciation_date
		BEGIN

			IF @start_date > @purchase_date --second row
				SET @start_date = DATEFROMPARTS(YEAR(@start_date), 1, 1) --always start and jan 1
	
			SET @end_date = DATEFROMPARTS(YEAR(@start_date), 12, 31)
			SET @depcreciation_rate = 100.0 / @useful_life_years / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1) / 100.0
	
			IF YEAR(@end_date) = YEAR(@end_depreciation_date) --last row
			BEGIN		
				SET @end_date = @end_depreciation_date
				IF @end_depreciation_date != DATEFROMPARTS(YEAR(@end_depreciation_date), 12, 31)
				BEGIN 
					IF MONTH(@purchase_date) != 1 
					BEGIN		
						IF DAY(@purchase_date) = 1 --The asset was acquired on the 1st day of any month, other than January
							SET @depcreciation_rate = @depcreciation_rate / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1)
						ELSE 
							SET @depcreciation_rate = @depcreciation_rate / @days_in_year * DATEDIFF(D, @start_date, @end_date)	
					END
					ELSE --The asset was acquired on the any day on January
						SET @depcreciation_rate = @depcreciation_rate / @days_in_year * (DATEDIFF(D, @start_date, @end_date) + 1)						
				END
			END
			ELSE
			BEGIN
				IF DAY(@start_date) != 1
					SET @depcreciation_rate = @depcreciation_rate_yearly / @days_in_year * (DATEDIFF(D, @start_date, @end_date) + 1) / 100.0
			END
			SET @depreciation_expense = @depcreciation_rate * (@purchase_cost - @residual_value)
			SET @ending_book_value = @ending_book_value - @depreciation_expense
	
			IF @accumulated_depreciation IS NULL SET @accumulated_depreciation = @depreciation_expense --first row
			ELSE SET @accumulated_depreciation = @accumulated_depreciation + @depreciation_expense
	
			INSERT INTO @table_dates (start_date, end_date, year, beginning_book_value, depreciation_rate, beginning, depreciation_expense, accumulated_depreciation, ending_book_value) 
			VALUES (@start_date, @end_date, YEAR(@start_date), @beginning_book_value, @depcreciation_rate * 100.00, @accumulated_depreciation - @depreciation_expense, @depreciation_expense, @accumulated_depreciation, @ending_book_value)
	
			SET @beginning_book_value = @beginning_book_value - @depreciation_expense --second row for beginning_book_value
			SET @start_date = DATEADD(YEAR, 1, @start_date)
		END

		--SELECT *, 
		--	DATEDIFF(D, start_date, end_date) + 1 AS days_in_year,
		--	depreciation_expense / (DATEDIFF(M, start_date, end_date) + 1) AS monthly_depreciation_expense FROM @table_dates WHERE year = @year OR @year = 0


		DECLARE @month_start INT,
				@month_end INT,
				@depreciation_expense_actual_days DECIMAL(18,2)

		DECLARE my_cursor_monthly CURSOR FOR
		SELECT start_date, end_date, DATEDIFF(D, start_date, end_date) + 1 AS days_in_year, depreciation_expense
		FROM @table_dates
		ORDER BY year

		OPEN my_cursor_monthly

		FETCH NEXT FROM my_cursor_monthly
		INTO @start_date, @end_date, @days_in_year, @depreciation_expense
		WHILE @@FETCH_STATUS = 0
		BEGIN
	
			SET @month_start = MONTH(@start_date)
			SET @month_end = MONTH(@end_date)
	
			WHILE @month_start <= @month_end
			BEGIN
				IF YEAR(@purchase_date) = YEAR(@start_date) AND @month_start = MONTH(@purchase_date) --1st year, 1st month
					SET @start_date = DATEFROMPARTS(YEAR(@start_date), @month_start, DAY(@start_date))
				ELSE SET @start_date = DATEFROMPARTS(YEAR(@start_date), @month_start, 1)

				SET @end_date = EOMONTH(@start_date)
				SET @depreciation_expense_actual_days = @depreciation_expense / @days_in_year * (DATEDIFF(D, @start_date, @end_date) + 1)

				IF @month_start = 1 UPDATE @table_dates SET [Jan]  = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 2 UPDATE @table_dates SET [Feb] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 3 UPDATE @table_dates SET [Mar] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 4 UPDATE @table_dates SET [Apr] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 5 UPDATE @table_dates SET [May] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 6 UPDATE @table_dates SET [Jun] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 7 UPDATE @table_dates SET [Jul] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 8 UPDATE @table_dates SET [Aug] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 9 UPDATE @table_dates SET [Sep] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 10 UPDATE @table_dates SET [Oct] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 11 UPDATE @table_dates SET [Nov] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 12 UPDATE @table_dates SET [Dec] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				SET @month_start = @month_start + 1
			END
			FETCH NEXT FROM my_cursor_monthly
			INTO @start_date, @end_date, @days_in_year, @depreciation_expense
		END

		CLOSE my_cursor_monthly
		DEALLOCATE my_cursor_monthly

		INSERT INTO @table
		SELECT t.start_date AS StartDate,
			t.end_date AS EndDate,
			fa.asset_no AS AssetNo,
			p.name AS AssetName,
			fa.useful_life_years AS UsefulLifeYears,
			fa.purchase_date AS PurchaseDate,
			DATEADD(D, -1, DATEADD(YEAR, fa.useful_life_years, fa.purchase_date)) AS EndDepreciationDate,
			fa.purchase_price AS PurchaseCost,
			fa.residual_value AS ResidualValue,
			t.beginning AS Beginning,
			t.depreciation_expense AS DepreciationExpense,
			t.accumulated_depreciation AS Ending,
			t.Jan, t.Feb, t.Mar, t.Apr, t.May, t.Jun, t.Jul, t.Aug, t.Sep, t.Oct, t.Nov, t.Dec,
			t.ending_book_value AS BookValueEnd
		FROM @table_dates t CROSS JOIN amQt_FixedAsset fa JOIN amQt_Product p
		ON p.id = fa.product_id
		WHERE t.year = @year
		AND fa.id = @id

		DELETE FROM @table_dates
		FETCH NEXT FROM my_cursor
		INTO @id
	END

	CLOSE my_cursor
	DEALLOCATE my_cursor

	SELECT * FROM @table
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spReportDepreciationScheduleStraightLineFullMonthAnnually]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spReportDepreciationScheduleStraightLineFullMonthAnnually]
	@id INT = 5,
	@asset_type_id INT = 0,
	@year SMALLINT = 2023
AS
BEGIN

	DECLARE @table TABLE(StartDate DATE, EndDate DATE, AssetNo VARCHAR(50), AssetName VARCHAR(250), UsefulLifeYear INT, PurchaseDate DATE, EndDepreciationDate DATE, PurchaseCost DECIMAL(19,2), ResidualValue DECIMAL(18,0), Beginning DECIMAL(18,2), DepreciationExpense DECIMAL(18,2), Ending DECIMAL(18,2), BookValueEnd DECIMAL(18,2))


	DECLARE my_cursor CURSOR FOR
	SELECT id
	FROM amQt_FixedAsset
	WHERE id = (CASE @id WHEN 0 THEN id ELSE @id END)
	AND asset_type_id = (CASE @asset_type_id WHEN 0 THEN asset_type_id ELSE @asset_type_id END)
	AND disable = 0
	OPEN my_cursor

	FETCH NEXT FROM my_cursor
	INTO @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		INSERT INTO @table
		SELECT t.start_date AS StartDate,
			t.end_date AS EndDate,
			fa.asset_no AS AssetNo,
			p.name AS AssetName,
			fa.useful_life_years AS UsefulLifeYears,
			fa.purchase_date AS PurchaseDate,
			DATEADD(D, -1, DATEADD(YEAR, fa.useful_life_years, fa.purchase_date)) AS EndDepreciationDate,
			fa.purchase_price AS PurchaseCost,
			fa.residual_value AS ResidualValue,
			t.beginning AS Beginning,
			t.depreciation_expense AS DepreciationExpense,
			t.accumulated_depreciation AS Ending,
			t.ending_book_value AS BookValueEnd
		FROM amQt_fnDepreciationScheduleStraightLineFullMonth(@id, @year) t CROSS JOIN amQt_FixedAsset fa JOIN amQt_Product p
		ON p.id = fa.product_id
		WHERE t.year >= @year
		AND fa.id = @id

		FETCH NEXT FROM my_cursor
		INTO @id
	END

	CLOSE my_cursor
	DEALLOCATE my_cursor
	
	SELECT *, YEAR(StartDate) AS Year FROM @table
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spReportDepreciationScheduleStraightLineFullMonthMonthly]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spReportDepreciationScheduleStraightLineFullMonthMonthly]
	@id INT = 5,
	@asset_type_id INT = 0,
	@year SMALLINT = 2023
AS
BEGIN
	DECLARE @purchase_date DATE,
			@end_depreciation_date DATE,
			@start_date DATE,
			@end_date DATE,
			@purchase_cost DECIMAL(18,2),
			@residual_value DECIMAL(18,2),
			@useful_life_years INT

	DECLARE @table TABLE(StartDate DATE, EndDate DATE, AssetNo VARCHAR(50), AssetName VARCHAR(250), UsefulLifeYear INT, PurchaseDate DATE, EndDepreciationDate DATE, PurchaseCost DECIMAL(19,2), ResidualValue DECIMAL(18,0), Beginning DECIMAL(18,2), DepreciationExpense DECIMAL(18,2), Ending DECIMAL(18,2),
			[Jan] DECIMAL(18,2), [Feb] DECIMAL(18,2), [Mar] DECIMAL(18,2), [Apr] DECIMAL(18,2), [May] DECIMAL(18,2), [Jun] DECIMAL(18,2), [Jul] DECIMAL(18,2), [Aug] DECIMAL(18,2), [Sep] DECIMAL(18,2), [Oct] DECIMAL(18,2), [Nov] DECIMAL(18,2), [Dec] DECIMAL(18,2), BookValueEnd DECIMAL(18,2))

	DECLARE my_cursor CURSOR FOR
	SELECT id
	FROM amQt_FixedAsset
	WHERE id = (CASE @id WHEN 0 THEN id ELSE @id END)
	AND asset_type_id = (CASE @asset_type_id WHEN 0 THEN asset_type_id ELSE @asset_type_id END)
	AND disable = 0
	OPEN my_cursor

	FETCH NEXT FROM my_cursor
	INTO @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		SELECT @purchase_date = purchase_date,
			@purchase_cost = purchase_price,
			@residual_value = residual_value,
			@useful_life_years = useful_life_years
		FROM amQt_FixedAsset
		WHERE id = @id
		
		DECLARE @depcreciation_rate FLOAT,--float so that the aboslute value of depreciation expense is 100% accurate
			@depcreciation_rate_yearly FLOAT = 100.0 / @useful_life_years,
			@depreciation_expense DECIMAL(18,2)
		SET @end_depreciation_date = DATEADD(D, -1, DATEADD(YEAR, @useful_life_years, @purchase_date))
		SET @start_date = @purchase_date--DATEFROMPARTS(YEAR(@purchase_date), MONTH(@purchase_date), 1)

		--IF DAY(@purchase_date) > 15 --16thday onwards
		--	SET @start_date = DATEADD(M, 1, @start_date)

		DECLARE @start_date_constant DATE = @start_date
		DECLARE @days_in_year INT = DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), DATEFROMPARTS(YEAR(@start_date), 12, 31)) + 1

		DECLARE @table_dates TABLE(start_date DATE, end_date DATE, year INT, beginning_book_value DECIMAL(19,2), depreciation_rate DECIMAL(18,0), beginning DECIMAL(18,2), depreciation_expense DECIMAL(18,2), accumulated_depreciation DECIMAL(18,2), ending_book_value DECIMAL(18,2),
			[Jan] DECIMAL(18,2), [Feb] DECIMAL(18,2), [Mar] DECIMAL(18,2), [Apr] DECIMAL(18,2), [May] DECIMAL(18,2), [Jun] DECIMAL(18,2), [Jul] DECIMAL(18,2), [Aug] DECIMAL(18,2), [Sep] DECIMAL(18,2), [Oct] DECIMAL(18,2), [Nov] DECIMAL(18,2), [Dec] DECIMAL(18,2))
		DECLARE @accumulated_depreciation DECIMAL(18,2) = 0,
				@beginning_book_value DECIMAL(18,2) = @purchase_cost,
				@ending_book_value DECIMAL(18,2) = @purchase_cost	
		
		WHILE @start_date <= @end_depreciation_date
		BEGIN

			IF @start_date > @purchase_date --second row
				SET @start_date = DATEFROMPARTS(YEAR(@start_date), 1, 1) --always start and jan 1
	
			SET @end_date = DATEFROMPARTS(YEAR(@start_date), 12, 31)
			SET @depcreciation_rate = 100.0 / @useful_life_years / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1) / 100.0
	
			IF YEAR(@end_date) = YEAR(@end_depreciation_date) --last row
			BEGIN		
				SET @end_date = @end_depreciation_date
				IF @end_depreciation_date != DATEFROMPARTS(YEAR(@end_depreciation_date), 12, 31)
				BEGIN 
					IF MONTH(@purchase_date) != 1 
						SET @depcreciation_rate = @depcreciation_rate / 12.0 * (DATEDIFF(M, @start_date,  @end_date) + 1)
					ELSE --The asset was acquired on the any day on January
						SET @depcreciation_rate = @depcreciation_rate / @days_in_year * (DATEDIFF(D, @start_date, @end_date) + 1)						
				END
			END

			SET @depreciation_expense = @depcreciation_rate * (@purchase_cost - @residual_value)
			SET @ending_book_value = @ending_book_value - @depreciation_expense
	
			IF @accumulated_depreciation IS NULL SET @accumulated_depreciation = @depreciation_expense --first row
			ELSE SET @accumulated_depreciation = @accumulated_depreciation + @depreciation_expense
	
			INSERT INTO @table_dates (start_date, end_date, year, beginning_book_value, depreciation_rate, beginning, depreciation_expense, accumulated_depreciation, ending_book_value) 
			VALUES (@start_date, @end_date, YEAR(@start_date), @beginning_book_value, @depcreciation_rate * 100.00, @accumulated_depreciation - @depreciation_expense, @depreciation_expense, @accumulated_depreciation, @ending_book_value)
	
			SET @beginning_book_value = @beginning_book_value - @depreciation_expense --second row for beginning_book_value
			SET @start_date = DATEADD(YEAR, 1, @start_date)
		END

		--SELECT *, depreciation_expense / (DATEDIFF(M, start_date, end_date) + 1) AS monthly_depreciation_expense FROM @table_dates WHERE year = @year OR @year = 0

		DECLARE @month_start INT,
				@month_end INT,
				@months INT,
				@sql VARCHAR(MAX)
		SELECT @month_start = MONTH(start_date),
			@month_end = MONTH(end_date),
			@depreciation_expense = depreciation_expense
		FROM @table_dates 
		WHERE year = @year
		SET @months = @month_end - @month_start + 1
		SET @depreciation_expense = @depreciation_expense / @months
		WHILE @month_start <= @month_end
		BEGIN
			IF @month_start = 1 UPDATE @table_dates SET [Jan] = @depreciation_expense
			IF @month_start = 2 UPDATE @table_dates SET [Feb] = @depreciation_expense
			IF @month_start = 3 UPDATE @table_dates SET [Mar] = @depreciation_expense
			IF @month_start = 4 UPDATE @table_dates SET [Apr] = @depreciation_expense
			IF @month_start = 5 UPDATE @table_dates SET [May] = @depreciation_expense
			IF @month_start = 6 UPDATE @table_dates SET [Jun] = @depreciation_expense
			IF @month_start = 7 UPDATE @table_dates SET [Jul] = @depreciation_expense
			IF @month_start = 8 UPDATE @table_dates SET [Aug] = @depreciation_expense
			IF @month_start = 9 UPDATE @table_dates SET [Sep] = @depreciation_expense
			IF @month_start = 10 UPDATE @table_dates SET [Oct] = @depreciation_expense
			IF @month_start = 11 UPDATE @table_dates SET [Nov] = @depreciation_expense
			IF @month_start = 12 UPDATE @table_dates SET [Dec] = @depreciation_expense
			SET @month_start = @month_start + 1
		END

		INSERT INTO @table
		SELECT t.start_date AS StartDate,
			t.end_date AS EndDate,
			fa.asset_no AS AssetNo,
			p.name AS AssetName,
			fa.useful_life_years AS UsefulLifeYears,
			fa.purchase_date AS PurchaseDate,
			DATEADD(D, -1, DATEADD(YEAR, fa.useful_life_years, fa.purchase_date)) AS EndDepreciationDate,
			fa.purchase_price AS PurchaseCost,
			fa.residual_value AS ResidualValue,
			t.beginning AS Beginning,
			t.depreciation_expense AS DepreciationExpense,
			t.accumulated_depreciation AS Ending,
			t.Jan, t.Feb, t.Mar, t.Apr, t.May, t.Jun, t.Jul, t.Aug, t.Sep, t.Oct, t.Nov, t.Dec,
			t.ending_book_value AS BookValueEnd
		FROM @table_dates t CROSS JOIN amQt_FixedAsset fa JOIN amQt_Product p
		ON p.id = fa.product_id
		WHERE t.year = @year
		AND fa.id = @id

		DELETE FROM @table_dates
		FETCH NEXT FROM my_cursor
		INTO @id
	END

	CLOSE my_cursor
	DEALLOCATE my_cursor

	SELECT * FROM @table
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spReportDepreciationScheduleSYDActualDaysAnnually]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spReportDepreciationScheduleSYDActualDaysAnnually]
	@id INT = 5,
	@asset_type_id INT = 0,
	@year SMALLINT = 2023
AS
BEGIN

	DECLARE @table TABLE(StartDate DATE, EndDate DATE, AssetNo VARCHAR(50), AssetName VARCHAR(250), UsefulLifeYear INT, PurchaseDate DATE, EndDepreciationDate DATE, PurchaseCost DECIMAL(19,2), ResidualValue DECIMAL(18,0), Beginning DECIMAL(18,2), DepreciationExpense DECIMAL(18,2), Ending DECIMAL(18,2), BookValueEnd DECIMAL(18,2))


	DECLARE my_cursor CURSOR FOR
	SELECT id
	FROM amQt_FixedAsset
	WHERE id = (CASE @id WHEN 0 THEN id ELSE @id END)
	AND asset_type_id = (CASE @asset_type_id WHEN 0 THEN asset_type_id ELSE @asset_type_id END)
	AND disable = 0
	OPEN my_cursor

	FETCH NEXT FROM my_cursor
	INTO @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		INSERT INTO @table
		SELECT t.start_date AS StartDate,
			t.end_date AS EndDate,
			fa.asset_no AS AssetNo,
			p.name AS AssetName,
			fa.useful_life_years AS UsefulLifeYears,
			fa.purchase_date AS PurchaseDate,
			DATEADD(D, -1, DATEADD(YEAR, fa.useful_life_years, fa.purchase_date)) AS EndDepreciationDate,
			fa.purchase_price AS PurchaseCost,
			fa.residual_value AS ResidualValue,
			t.beginning AS Beginning,
			t.depreciation_expense AS DepreciationExpense,
			t.accumulated_depreciation AS Ending,
			t.ending_book_value AS BookValueEnd
		FROM amQt_fnDepreciationScheduleSYDActualDays(@id, @year) t CROSS JOIN amQt_FixedAsset fa JOIN amQt_Product p
		ON p.id = fa.product_id
		WHERE t.year >= @year
		AND fa.id = @id

		FETCH NEXT FROM my_cursor
		INTO @id
	END

	CLOSE my_cursor
	DEALLOCATE my_cursor
	
	SELECT *, YEAR(StartDate) AS Year FROM @table
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spReportDepreciationScheduleSYDActualDaysMonthly]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spReportDepreciationScheduleSYDActualDaysMonthly]
	@id INT = 5,
	@asset_type_id INT = 0,
	@year SMALLINT = 2023
AS
BEGIN
	DECLARE @purchase_date DATE,
			@end_depreciation_date DATE,
			@start_date DATE,
			@end_date DATE,
			@purchase_cost DECIMAL(18,2),
			@residual_value DECIMAL(18,2),
			@useful_life_years INT

	DECLARE @table TABLE(StartDate DATE, EndDate DATE, AssetNo VARCHAR(50), AssetName VARCHAR(250), UsefulLifeYear INT, PurchaseDate DATE, EndDepreciationDate DATE, PurchaseCost DECIMAL(19,2), ResidualValue DECIMAL(18,0), Beginning DECIMAL(18,2), DepreciationExpense DECIMAL(18,2), Ending DECIMAL(18,2),
			[Jan] DECIMAL(18,2), [Feb] DECIMAL(18,2), [Mar] DECIMAL(18,2), [Apr] DECIMAL(18,2), [May] DECIMAL(18,2), [Jun] DECIMAL(18,2), [Jul] DECIMAL(18,2), [Aug] DECIMAL(18,2), [Sep] DECIMAL(18,2), [Oct] DECIMAL(18,2), [Nov] DECIMAL(18,2), [Dec] DECIMAL(18,2), BookValueEnd DECIMAL(18,2))

	DECLARE my_cursor CURSOR FOR
	SELECT id
	FROM amQt_FixedAsset
	WHERE id = (CASE @id WHEN 0 THEN id ELSE @id END)
	AND asset_type_id = (CASE @asset_type_id WHEN 0 THEN asset_type_id ELSE @asset_type_id END)
	AND disable = 0
	OPEN my_cursor

	FETCH NEXT FROM my_cursor
	INTO @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		SELECT @purchase_date = purchase_date,
			@purchase_cost = purchase_price,
			@residual_value = residual_value,
			@useful_life_years = useful_life_years
		FROM amQt_FixedAsset
		WHERE id = @id
		
		DECLARE @depreciation_expense FLOAT
		SET @end_depreciation_date = DATEADD(D, -1, DATEADD(YEAR, @useful_life_years, @purchase_date))

		SET @start_date = @purchase_date--DATEFROMPARTS(YEAR(@purchase_date), MONTH(@purchase_date), 1)

		--IF DAY(@purchase_date) > 15 --16thday onwards
		--	SET @start_date = DATEADD(M, 1, @start_date)

		DECLARE @start_date_constant DATE = @start_date
		DECLARE @days_in_year FLOAT = DATEPART(dy,DATEFROMPARTS(YEAR(@start_date),12,31))

		DECLARE @table_dates TABLE(start_date DATE, end_date DATE, total_months INT, year INT, estimated_life_remaining INT, dep DECIMAL(18,2), a DECIMAL(18,2), b DECIMAL(18,2), beginning_book_value DECIMAL(19,2), beginning DECIMAL(18,2), depreciation_expense FLOAT, accumulated_depreciation DECIMAL(18,2), ending_book_value DECIMAL(18,2),
			[Jan] FLOAT, [Feb] FLOAT, [Mar] FLOAT, [Apr] FLOAT, [May] FLOAT, [Jun] FLOAT, [Jul] FLOAT, [Aug] FLOAT, [Sep] FLOAT, [Oct] FLOAT, [Nov] FLOAT, [Dec] FLOAT)
		DECLARE @accumulated_depreciation FLOAT = 0,
				@beginning_book_value FLOAT = @purchase_cost,
				@ending_book_value FLOAT = @purchase_cost,
				@total_months FLOAT,
				@estimated_life_remaining INT = @useful_life_years,
				@dep_prev FLOAT = 0,
				@dep FLOAT = 0,
				@a FLOAT = 0,
				@b FLOAT
		

		WHILE @start_date <= @end_depreciation_date
		BEGIN

			SET @dep = (@purchase_cost - @residual_value) * (@estimated_life_remaining / (@useful_life_years * (@useful_life_years + 1) / 2.0))

			IF @start_date > @purchase_date --second row
				SET @start_date = DATEFROMPARTS(YEAR(@start_date), 1, 1) --always start and jan 1
	
			IF YEAR(@start_date) = YEAR(@end_depreciation_date) --last row
				SET @end_date = @end_depreciation_date
			ELSE
				SET @end_date = DATEFROMPARTS(YEAR(@start_date), 12, 31)
	
			SET @total_months = DATEDIFF(M, @start_date, @end_date) + 1
			IF YEAR(@start_date) = YEAR(@purchase_date) --first row
			BEGIN 
				IF DAY(@start_date) != 1--The asset was acquired on the any day of the month
				BEGIN
					SET @a = DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), @start_date) / @days_in_year
					SET @b = (@days_in_year - DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), @start_date)) / @days_in_year
				END
				ELSE
				BEGIN
					SET @a = (12 - @total_months) / 12.0
					SET @b = @total_months / 12.0		
				END
			END		
	
			SET @depreciation_expense = @dep_prev * @a + @dep * @b
			SET @ending_book_value = @ending_book_value - @depreciation_expense
	
			IF @accumulated_depreciation IS NULL SET @accumulated_depreciation = @depreciation_expense --first row
			ELSE SET @accumulated_depreciation = @accumulated_depreciation + @depreciation_expense
	
			INSERT INTO @table_dates (start_date, end_date, total_months, estimated_life_remaining, dep, a, b, year, beginning_book_value, beginning, depreciation_expense, accumulated_depreciation, ending_book_value) 
			VALUES (@start_date, @end_date, @total_months, @estimated_life_remaining, @dep, @dep_prev * @a, @dep * @b, YEAR(@start_date), @beginning_book_value, @accumulated_depreciation - @depreciation_expense, @depreciation_expense, @accumulated_depreciation, @ending_book_value)
	
			SET @dep_prev = @dep
			SET @beginning_book_value = @beginning_book_value - @depreciation_expense --second row for beginning_book_value
			SET @start_date = DATEADD(YEAR, 1, @start_date)
			SET @estimated_life_remaining = @estimated_life_remaining - 1
		END

		--SELECT *, depreciation_expense / (DATEDIFF(M, start_date, end_date) + 1) AS monthly_depreciation_expense FROM @table_dates WHERE year = @year OR @year = 0
		DECLARE @month_start INT,
				@month_end INT,
				@depreciation_expense_actual_days DECIMAL(18,2)

		DECLARE my_cursor_monthly CURSOR FOR
		SELECT start_date, end_date, DATEDIFF(D, start_date, end_date) + 1 AS days_in_year, depreciation_expense
		FROM @table_dates
		ORDER BY year

		OPEN my_cursor_monthly

		FETCH NEXT FROM my_cursor_monthly
		INTO @start_date, @end_date, @days_in_year, @depreciation_expense
		WHILE @@FETCH_STATUS = 0
		BEGIN
	
			SET @month_start = MONTH(@start_date)
			SET @month_end = MONTH(@end_date)
	
			WHILE @month_start <= @month_end
			BEGIN
				IF YEAR(@purchase_date) = YEAR(@start_date) AND @month_start = MONTH(@purchase_date) --1st year, 1st month
					SET @start_date = DATEFROMPARTS(YEAR(@start_date), @month_start, DAY(@start_date))
				ELSE SET @start_date = DATEFROMPARTS(YEAR(@start_date), @month_start, 1)

				SET @end_date = EOMONTH(@start_date)
				SET @depreciation_expense_actual_days = @depreciation_expense / @days_in_year * (DATEDIFF(D, @start_date, @end_date) + 1)

				IF @month_start = 1 UPDATE @table_dates SET [Jan]  = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 2 UPDATE @table_dates SET [Feb] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 3 UPDATE @table_dates SET [Mar] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 4 UPDATE @table_dates SET [Apr] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 5 UPDATE @table_dates SET [May] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 6 UPDATE @table_dates SET [Jun] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 7 UPDATE @table_dates SET [Jul] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 8 UPDATE @table_dates SET [Aug] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 9 UPDATE @table_dates SET [Sep] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 10 UPDATE @table_dates SET [Oct] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 11 UPDATE @table_dates SET [Nov] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				IF @month_start = 12 UPDATE @table_dates SET [Dec] = @depreciation_expense_actual_days WHERE year = YEAR(@start_date)
				SET @month_start = @month_start + 1
			END
			FETCH NEXT FROM my_cursor_monthly
			INTO @start_date, @end_date, @days_in_year, @depreciation_expense
		END

		CLOSE my_cursor_monthly
		DEALLOCATE my_cursor_monthly

		INSERT INTO @table
		SELECT t.start_date AS StartDate,
			t.end_date AS EndDate,
			fa.asset_no AS AssetNo,
			p.name AS AssetName,
			fa.useful_life_years AS UsefulLifeYears,
			fa.purchase_date AS PurchaseDate,
			DATEADD(D, -1, DATEADD(YEAR, fa.useful_life_years, fa.purchase_date)) AS EndDepreciationDate,
			fa.purchase_price AS PurchaseCost,
			fa.residual_value AS ResidualValue,
			t.beginning AS Beginning,
			t.depreciation_expense AS DepreciationExpense,
			t.accumulated_depreciation AS Ending,
			t.Jan, t.Feb, t.Mar, t.Apr, t.May, t.Jun, t.Jul, t.Aug, t.Sep, t.Oct, t.Nov, t.Dec,
			t.ending_book_value AS BookValueEnd
		FROM @table_dates t CROSS JOIN amQt_FixedAsset fa JOIN amQt_Product p
		ON p.id = fa.product_id
		WHERE t.year = @year
		AND fa.id = @id

		DELETE FROM @table_dates
		FETCH NEXT FROM my_cursor
		INTO @id
	END

	CLOSE my_cursor
	DEALLOCATE my_cursor

	SELECT * FROM @table
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spReportDepreciationScheduleSYDFullMonthAnnually]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spReportDepreciationScheduleSYDFullMonthAnnually]
	@id INT = 5,
	@asset_type_id INT = 0,
	@year SMALLINT = 2023
AS
BEGIN

	DECLARE @table TABLE(StartDate DATE, EndDate DATE, AssetNo VARCHAR(50), AssetName VARCHAR(250), UsefulLifeYear INT, PurchaseDate DATE, EndDepreciationDate DATE, PurchaseCost DECIMAL(19,2), ResidualValue DECIMAL(18,0), Beginning DECIMAL(18,2), DepreciationExpense DECIMAL(18,2), Ending DECIMAL(18,2), BookValueEnd DECIMAL(18,2))


	DECLARE my_cursor CURSOR FOR
	SELECT id
	FROM amQt_FixedAsset
	WHERE id = (CASE @id WHEN 0 THEN id ELSE @id END)
	AND asset_type_id = (CASE @asset_type_id WHEN 0 THEN asset_type_id ELSE @asset_type_id END)
	AND disable = 0
	OPEN my_cursor

	FETCH NEXT FROM my_cursor
	INTO @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		INSERT INTO @table
		SELECT t.start_date AS StartDate,
			t.end_date AS EndDate,
			fa.asset_no AS AssetNo,
			p.name AS AssetName,
			fa.useful_life_years AS UsefulLifeYears,
			fa.purchase_date AS PurchaseDate,
			DATEADD(D, -1, DATEADD(YEAR, fa.useful_life_years, fa.purchase_date)) AS EndDepreciationDate,
			fa.purchase_price AS PurchaseCost,
			fa.residual_value AS ResidualValue,
			t.beginning AS Beginning,
			t.depreciation_expense AS DepreciationExpense,
			t.accumulated_depreciation AS Ending,
			t.ending_book_value AS BookValueEnd
		FROM amQt_fnDepreciationScheduleSYDFullMonth(@id, @year) t CROSS JOIN amQt_FixedAsset fa JOIN amQt_Product p
		ON p.id = fa.product_id
		WHERE t.year >= @year
		AND fa.id = @id

		FETCH NEXT FROM my_cursor
		INTO @id
	END

	CLOSE my_cursor
	DEALLOCATE my_cursor
	
	SELECT *, YEAR(StartDate) AS Year FROM @table
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spReportDepreciationScheduleSYDFullMonthMonthly]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spReportDepreciationScheduleSYDFullMonthMonthly]
	@id INT = 5,
	@asset_type_id INT = 0,
	@year SMALLINT = 2023
AS
BEGIN
	DECLARE @purchase_date DATE,
			@end_depreciation_date DATE,
			@start_date DATE,
			@end_date DATE,
			@purchase_cost DECIMAL(18,2),
			@residual_value DECIMAL(18,2),
			@useful_life_years INT

	DECLARE @table TABLE(StartDate DATE, EndDate DATE, AssetNo VARCHAR(50), AssetName VARCHAR(250), UsefulLifeYear INT, PurchaseDate DATE, EndDepreciationDate DATE, PurchaseCost DECIMAL(19,2), ResidualValue DECIMAL(18,0), Beginning DECIMAL(18,2), DepreciationExpense DECIMAL(18,2), Ending DECIMAL(18,2),
			[Jan] FLOAT, [Feb] FLOAT, [Mar] FLOAT, [Apr] FLOAT, [May] FLOAT, [Jun] FLOAT, [Jul] FLOAT, [Aug] FLOAT, [Sep] FLOAT, [Oct] FLOAT, [Nov] FLOAT, [Dec] FLOAT, BookValueEnd DECIMAL(18,2))
			
	DECLARE my_cursor CURSOR FOR
	SELECT id
	FROM amQt_FixedAsset
	WHERE id = (CASE @id WHEN 0 THEN id ELSE @id END)
	AND asset_type_id = (CASE @asset_type_id WHEN 0 THEN asset_type_id ELSE @asset_type_id END)
	AND disable = 0
	OPEN my_cursor

	FETCH NEXT FROM my_cursor
	INTO @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		SELECT @purchase_date = purchase_date,
			@purchase_cost = purchase_price,
			@residual_value = residual_value,
			@useful_life_years = useful_life_years
		FROM amQt_FixedAsset
		WHERE id = @id
		
		DECLARE @depreciation_expense FLOAT
		SET @end_depreciation_date = DATEADD(D, -1, DATEADD(YEAR, @useful_life_years, @purchase_date))
		SET @start_date = @purchase_date

		DECLARE @start_date_constant DATE = @start_date
		DECLARE @days_in_year INT = DATEDIFF(D, DATEFROMPARTS(YEAR(@start_date), 1, 1), DATEFROMPARTS(YEAR(@start_date), 12, 31)) + 1

		DECLARE @table_dates TABLE(start_date DATE, end_date DATE, total_months INT, year INT, estimated_life_remaining INT, dep DECIMAL(18,2), a DECIMAL(18,2), b DECIMAL(18,2), beginning_book_value DECIMAL(19,2), beginning DECIMAL(18,2), depreciation_expense FLOAT, accumulated_depreciation DECIMAL(18,2), ending_book_value DECIMAL(18,2),
			[Jan] FLOAT, [Feb] FLOAT, [Mar] FLOAT, [Apr] FLOAT, [May] FLOAT, [Jun] FLOAT, [Jul] FLOAT, [Aug] FLOAT, [Sep] FLOAT, [Oct] FLOAT, [Nov] FLOAT, [Dec] FLOAT)

		DECLARE @accumulated_depreciation FLOAT = 0,
				@beginning_book_value FLOAT = @purchase_cost,
				@ending_book_value FLOAT = @purchase_cost,
				@total_months FLOAT,
				@estimated_life_remaining INT = @useful_life_years,
				@dep_prev FLOAT = 0,
				@dep FLOAT = 0,
				@a FLOAT = 0,
				@b FLOAT = 0

		WHILE @start_date <= @end_depreciation_date
		BEGIN

			SET @dep = (@purchase_cost - @residual_value) * (@estimated_life_remaining / (@useful_life_years * (@useful_life_years + 1) / 2.0))

			IF @start_date > @purchase_date --second row
				SET @start_date = DATEFROMPARTS(YEAR(@start_date), 1, 1) --always start and jan 1
	
			IF YEAR(@start_date) = YEAR(@end_depreciation_date) --last row
				SET @end_date = @end_depreciation_date
			ELSE
				SET @end_date = DATEFROMPARTS(YEAR(@start_date), 12, 31)
	
			SET @total_months = DATEDIFF(M, @start_date, @end_date) + 1
			IF YEAR(@start_date) = YEAR(@purchase_date) --first row
			BEGIN
				SET @a = (12 - @total_months) / 12.0
				SET @b = @total_months / 12.0		
			END		
	
			SET @depreciation_expense = @dep_prev * @a + @dep * @b
			SET @ending_book_value = @ending_book_value - @depreciation_expense
	
			IF @accumulated_depreciation IS NULL SET @accumulated_depreciation = @depreciation_expense --first row
			ELSE SET @accumulated_depreciation = @accumulated_depreciation + @depreciation_expense
	
			INSERT INTO @table_dates (start_date, end_date, total_months, estimated_life_remaining, dep, a, b, year, beginning_book_value, beginning, depreciation_expense, accumulated_depreciation, ending_book_value) 
			VALUES (@start_date, @end_date, @total_months, @estimated_life_remaining, @dep, @dep_prev * @a, @dep * @b, YEAR(@start_date), @beginning_book_value, @accumulated_depreciation - @depreciation_expense, @depreciation_expense, @accumulated_depreciation, @ending_book_value)
	
			SET @dep_prev = @dep
			SET @beginning_book_value = @beginning_book_value - @depreciation_expense --second row for beginning_book_value
			SET @start_date = DATEADD(YEAR, 1, @start_date)
			SET @estimated_life_remaining = @estimated_life_remaining - 1
		END

		DECLARE @month_start INT,
				@month_end INT,
				@months INT,
				@sql VARCHAR(MAX)
		SELECT @month_start = MONTH(start_date),
			@month_end = MONTH(end_date),
			@depreciation_expense = depreciation_expense
		FROM @table_dates 
		WHERE year = @year
		SET @months = @month_end - @month_start + 1
		SET @depreciation_expense = @depreciation_expense / @months
		WHILE @month_start <= @month_end
		BEGIN
			IF @month_start = 1 UPDATE @table_dates SET [Jan] = @depreciation_expense
			IF @month_start = 2 UPDATE @table_dates SET [Feb] = @depreciation_expense
			IF @month_start = 3 UPDATE @table_dates SET [Mar] = @depreciation_expense
			IF @month_start = 4 UPDATE @table_dates SET [Apr] = @depreciation_expense
			IF @month_start = 5 UPDATE @table_dates SET [May] = @depreciation_expense
			IF @month_start = 6 UPDATE @table_dates SET [Jun] = @depreciation_expense
			IF @month_start = 7 UPDATE @table_dates SET [Jul] = @depreciation_expense
			IF @month_start = 8 UPDATE @table_dates SET [Aug] = @depreciation_expense
			IF @month_start = 9 UPDATE @table_dates SET [Sep] = @depreciation_expense
			IF @month_start = 10 UPDATE @table_dates SET [Oct] = @depreciation_expense
			IF @month_start = 11 UPDATE @table_dates SET [Nov] = @depreciation_expense
			IF @month_start = 12 UPDATE @table_dates SET [Dec] = @depreciation_expense
			SET @month_start = @month_start + 1
		END

		INSERT INTO @table
		SELECT t.start_date AS StartDate,
			t.end_date AS EndDate,
			fa.asset_no AS AssetNo,
			p.name AS AssetName,
			fa.useful_life_years AS UsefulLifeYears,
			fa.purchase_date AS PurchaseDate,
			DATEADD(D, -1, DATEADD(YEAR, fa.useful_life_years, fa.purchase_date)) AS EndDepreciationDate,
			fa.purchase_price AS PurchaseCost,
			fa.residual_value AS ResidualValue,
			t.beginning AS Beginning,
			t.depreciation_expense AS DepreciationExpense,
			t.accumulated_depreciation AS Ending,
			t.Jan, t.Feb, t.Mar, t.Apr, t.May, t.Jun, t.Jul, t.Aug, t.Sep, t.Oct, t.Nov, t.Dec,
			t.ending_book_value AS BookValueEnd
		FROM @table_dates t CROSS JOIN amQt_FixedAsset fa JOIN amQt_Product p
		ON p.id = fa.product_id
		WHERE t.year = @year
		AND fa.id = @id

		DELETE FROM @table_dates
		FETCH NEXT FROM my_cursor
		INTO @id
	END

	CLOSE my_cursor
	DEALLOCATE my_cursor

	SELECT * FROM @table
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spServiceLevelDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spServiceLevelDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_ServiceLevel SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spServiceLevelInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spServiceLevelInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_ServiceLevel
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_ServiceLevel SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spServiceLevelSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spServiceLevelSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_ServiceLevel
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_ServiceLevel p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spServiceLevelSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spServiceLevelSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_ServiceLevel p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spSupplierDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spSupplierDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_Supplier SET disable = 1 WHERE id = @id

END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spSupplierInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spSupplierInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_Supplier
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_Supplier SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spSupplierSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spSupplierSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_Supplier
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_Supplier p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spSupplierSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spSupplierSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_Supplier p
	WHERE p.id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[amQt_spTradeDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spTradeDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_Trade SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spTradeInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spTradeInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_Trade
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_Trade SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spTradeSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spTradeSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_Trade
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_Trade p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spTradeSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spTradeSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_Trade p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spUnitDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spUnitDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_Unit SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spUnitInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spUnitInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_Unit
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_Unit SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spUnitSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spUnitSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_Unit
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_Unit p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spUnitSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spUnitSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_Unit p
	WHERE p.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spUserAccessInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spUserAccessInsertUpdateSingleItem]
	@id INT OUTPUT,
	@user_id INT,
	@module_id SMALLINT,
	@select BIT,
	@insert BIT,
	@update BIT,
	@delete BIT,
	@print BIT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_UserAccess
		(
			user_id,
			module_id,
			[select],
			[insert],
			[update],
			[delete],
			[print]

		)
		VALUES
		(
			@user_id,
			@module_id,
			@select,
			@insert,
			@update,
			@delete,
			@print

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_UserAccess SET
			user_id = @user_id,
			module_id = @module_id,
			[select] = @select,
			[insert] = @insert,
			[update] = @update,
			[delete] = @delete,
			[print] = @print

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spUserAccessSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spUserAccessSearchList]
	@user_id INT = 0,
	@module_id SMALLINT = 0,
	@record_count int = NULL OUTPUT
AS
BEGIN

	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_UserAccess
			WHERE module_id = (CASE @module_id WHEN 0 THEN module_id ELSE @module_id END)
			AND user_id = (CASE @user_id WHEN 0 THEN user_id ELSE @user_id END)
			
			)
		RETURN
	END

	SELECT ISNULL(ua.id, 0) AS id,
		ISNULL(ua.user_id, 0) AS user_id,
		m.id AS module_id,
		m.name AS module_name,
		m.[group] AS module_group,
		ISNULL(ua.[select], 0) AS [select],
		ISNULL(ua.[insert], 0) AS [insert],
		ISNULL(ua.[update], 0) AS [update],
		ISNULL(ua.[delete], 0) AS [delete],
		ISNULL(ua.[print], 0) AS [print]
	FROM amQt_Module m LEFT JOIN amQt_UserAccess ua
	ON m.id = ua.module_id AND ua.user_id = @user_id
	WHERE m.id = (CASE @module_id WHEN 0 THEN m.id ELSE @module_id END)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spUserAccessSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spUserAccessSelectSingleItem]
	@id INT
AS
BEGIN
	SELECT ua.*,
		m.name AS module_name,
		m.[group] AS module_group
	FROM amQt_Module m JOIN amQt_UserAccess ua
	ON m.id = ua.module_id
	WHERE ua.id = @id
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spUsersDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spUsersDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_Users SET disable = 1 WHERE id = @id

END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spUsersInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spUsersInsertUpdateSingleItem]
	@id INT OUTPUT,
	@username VARCHAR(50),
	@password VARCHAR(50) = NULL,
	@hash VARCHAR(MAX),
	@salt VARBINARY(512),
	@photo VARBINARY(MAX) = NULL,
	@personnel_id INT

AS 
BEGIN 
	DECLARE @return_value INT 

	IF @id IS NULL 
	BEGIN 
		INSERT INTO amQt_Users
		(
			username,
			hash,
			salt,
			photo,
			personnel_id
		) 
		VALUES 
		( 
			@username,
			@hash,
			@salt,
			@photo,
			@personnel_id
		) 
		SELECT @return_value = SCOPE_IDENTITY() 
	END
	ELSE 
	BEGIN 
		UPDATE amQt_Users SET 
			username = @username,
			hash = (CASE WHEN @password IS NULL THEN hash ELSE @hash END),
			salt = (CASE WHEN @password IS NULL THEN salt ELSE @salt END),
			photo = @photo,
			personnel_id = @personnel_id

		WHERE id = @id 

		SELECT @return_value = @id 
	END

	SET @id = @return_value
	IF (@@ERROR != 0) 
	BEGIN 
		RETURN -1 
	END 
	ELSE 
	BEGIN 
		RETURN 0 
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spUsersSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spUsersSearchList]
	@username VARCHAR(50) = NULL,
	@personnel_id INT = 0,
	@record_count int = NULL OUTPUT
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_Users
			WHERE disable = 0
			AND (username = @username OR @username IS NULL)
			AND personnel_id = (CASE @personnel_id WHEN 0 THEN personnel_id ELSE @personnel_id END)
			)
		RETURN
	END

	SELECT *
	FROM amQt_Users
	WHERE disable = 0
	AND (username = @username OR @username IS NULL)
	AND personnel_id = (CASE @personnel_id WHEN 0 THEN personnel_id ELSE @personnel_id END)
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spUsersSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[amQt_spUsersSelectSingleItem]
	@id INT
AS
BEGIN
	
	SELECT *
	FROM amQt_Users
	WHERE id = @id
END





GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spWorkOrderDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_WorkOrder SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderHoursDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spWorkOrderHoursDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_WorkOrderHours SET disable = 1 WHERE id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderHoursInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spWorkOrderHoursInsertUpdateSingleItem]
	@id INT OUTPUT,
	@work_order_id INT,
	@expense_category_id INT,
	@hours DECIMAL(18,2),
	@rate_per_hour DECIMAL(18,2)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_WorkOrderHours
		(
			work_order_id,
			expense_category_id,
			hours,
			rate_per_hour

		)
		VALUES
		(
			@work_order_id,
			@expense_category_id,
			@hours,
			@rate_per_hour

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_WorkOrderHours SET
			work_order_id = @work_order_id,
			expense_category_id = @expense_category_id,
			hours = @hours,
			rate_per_hour = @rate_per_hour

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderHoursSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spWorkOrderHoursSearchList]	
	@record_count int = NULL OUTPUT,
	@work_order_id INT
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_WorkOrderHours
			WHERE disable = 0
			AND work_order_id = (CASE @work_order_id WHEN 0 THEN work_order_id ELSE @work_order_id END)
			)
		RETURN
	END

	SELECT w.*,
		c.name AS expense_category_name
	FROM amQt_WorkOrderHours w JOIN amQt_ExpenseCategory c
	ON c.id = w.expense_category_id
	WHERE w.disable = 0
	AND w.work_order_id = (CASE @work_order_id WHEN 0 THEN w.work_order_id ELSE @work_order_id END)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderHoursSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[amQt_spWorkOrderHoursSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT w.*,
		c.name AS expense_category_name
	FROM amQt_WorkOrderHours w JOIN amQt_ExpenseCategory c
	ON c.id = w.expense_category_id
	WHERE w.id = @id
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spWorkOrderInsertUpdateSingleItem]
	@id INT OUTPUT,
	@expected_start_date DATE,
	@expected_end_date DATE,
	@maintenance_request_id INT,
	@work_order_type_id INT,
	@maintenance_job_type_variant_id INT,
	@trade_id INT

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		DECLARE @number INT
		SET @number = ISNULL((SELECT MAX(number) FROM amQt_WorkOrder WHERE YEAR(date) = YEAR(GETDATE())), 0) + 1

		INSERT INTO amQt_WorkOrder
		(
			expected_start_date,
			expected_end_date,
			number,
			maintenance_request_id,
			work_order_type_id,
			maintenance_job_type_variant_id,
			trade_id

		)
		VALUES
		(
			@expected_start_date,
			@expected_end_date,
			@number,
			@maintenance_request_id,
			@work_order_type_id,
			@maintenance_job_type_variant_id,
			@trade_id

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_WorkOrder SET
			expected_start_date = @expected_start_date,
			expected_end_date = @expected_end_date,
			maintenance_request_id = @maintenance_request_id,
			work_order_type_id = @work_order_type_id,
			maintenance_job_type_variant_id = @maintenance_job_type_variant_id,
			trade_id = @trade_id

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderProductsSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spWorkOrderProductsSearchList]	
	@record_count int = NULL OUTPUT,
	@work_order_id INT
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_WorkOrderProducts
			WHERE disable = 0
			AND work_order_id = (CASE @work_order_id WHEN 0 THEN work_order_id ELSE @work_order_id END)
			)
		RETURN
	END

	SELECT w.*,
		p.name AS product_name,
		c.name AS currency_name
	FROM amQt_WorkOrderProducts w JOIN amQt_Product p
	ON p.id = w.product_id JOIN amQt_Currency c
	ON c.id = w.currency_id
	WHERE w.disable = 0
	AND w.work_order_id = (CASE @work_order_id WHEN 0 THEN w.work_order_id ELSE @work_order_id END)
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderProductsSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[amQt_spWorkOrderProductsSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT w.*,
		p.name AS product_name,
		c.name AS currency_name
	FROM amQt_WorkOrderProducts w JOIN amQt_Product p
	ON p.id = w.product_id JOIN amQt_Currency c
	ON c.id = w.currency_id
	WHERE w.id = @id
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[amQt_spWorkOrderSearchList]	
	@record_count int = NULL OUTPUT,
	@start_date DATE = NULL,
	@end_date DATE = NULL
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(mr.id)
			FROM amQt_WorkOrder wo JOIN amQt_MaintenanceRequest mr
			ON mr.id = wo.maintenance_request_id
			WHERE wo.disable = 0
			AND (mr.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)			
			)
		RETURN
	END

	SELECT wo.*,
		mr.date,
		dbo.amQt_fnTransactionNo(mr.date, mr.number) AS maintenance_request_no,
		p2.name AS fixed_asset_name,
		fl.name AS functional_location_name,
		dbo.amQt_fnTransactionNo(mr.date, wo.number) AS work_order_no,	
		wt.name AS work_order_type_name,
		mr.description,
		sl.name AS service_level_name,
		mr.status,
		mr.active
	FROM amQt_WorkOrder wo JOIN amQt_MaintenanceRequest mr
	ON mr.id = wo.maintenance_request_id JOIN amQt_WorkOrderType wt
	ON wt.id = wo.work_order_type_id JOIN amQt_MaintenanceJobTypeVariant v
	ON v.id = wo.maintenance_job_type_variant_id JOIN amQt_MaintenanceJobType jt
	ON jt.id = v.maintenance_job_type_id JOIN amQt_ServiceLevel sl
	ON sl.id = mr.service_level_id JOIN amQt_FunctionalLocation fl
	ON fl.id = mr.functional_location_id JOIN amQt_FixedAsset fa
	ON fa.id = mr.fixed_asset_id JOIN amQt_Product p2
	ON p2.id = fa.product_id
	WHERE wo.disable = 0
	AND (mr.date BETWEEN @start_date AND @end_date OR @start_date IS NULL)
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[amQt_spWorkOrderSelectSingleItem]	
	@id INT
AS
BEGIN
	
	SELECT wo.*,
		mr.date,
		dbo.amQt_fnTransactionNo(mr.date, mr.number) AS maintenance_request_no,
		p2.name AS fixed_asset_name,
		fl.name AS functional_location_name,
		dbo.amQt_fnTransactionNo(mr.date, wo.number) AS work_order_no,	
		wt.name AS work_order_type_name,
		mr.description,
		sl.name AS service_level_name,
		mr.status,
		mr.active
	FROM amQt_WorkOrder wo JOIN amQt_MaintenanceRequest mr
	ON mr.id = wo.maintenance_request_id JOIN amQt_WorkOrderType wt
	ON wt.id = wo.work_order_type_id JOIN amQt_MaintenanceJobTypeVariant v
	ON v.id = wo.maintenance_job_type_variant_id JOIN amQt_MaintenanceJobType jt
	ON jt.id = v.maintenance_job_type_id JOIN amQt_ServiceLevel sl
	ON sl.id = mr.service_level_id JOIN amQt_FunctionalLocation fl
	ON fl.id = mr.functional_location_id JOIN amQt_FixedAsset fa
	ON fa.id = mr.fixed_asset_id JOIN amQt_Product p2
	ON p2.id = fa.product_id
	WHERE wo.id = @id
END



GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderTypeDeleteSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spWorkOrderTypeDeleteSingleItem]
	@id INT

AS
BEGIN
	UPDATE amQt_WorkOrderType SET disable = 1 WHERE id = @id

END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderTypeInsertUpdateSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spWorkOrderTypeInsertUpdateSingleItem]
	@id INT OUTPUT,
	@code VARCHAR(50),
	@name VARCHAR(50)

AS
BEGIN
	DECLARE @return_value INT

	IF @id IS NULL
	BEGIN
		INSERT INTO amQt_WorkOrderType
		(
			code,
			name

		)
		VALUES
		(
			@code,
			@name

		)
		SELECT @return_value = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE amQt_WorkOrderType SET
			code = @code,
			name = @name

		WHERE id = @id

		SELECT @return_value = @id
	END

	SET @id = @return_value
	IF (@@ERROR != 0)
	BEGIN
		RETURN -1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END




GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderTypeSearchList]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spWorkOrderTypeSearchList]	
	@record_count int = NULL OUTPUT,
	@code VARCHAR(50) = NULL,
	@name VARCHAR(150) = NULL,
	@id INT = 0
AS
BEGIN
	IF (@record_count IS NOT NULL)
	BEGIN
		SET @record_count = (SELECT COUNT(id)
			FROM amQt_WorkOrderType
			WHERE disable = 0
			AND (code = @code AND id != @id OR @code IS NULL)
			AND (name = @name AND id != @id OR @name IS NULL)
			)
		RETURN
	END

	SELECT p.*
	FROM amQt_WorkOrderType p
	WHERE p.disable = 0
	AND (p.code = @code AND p.id != @id OR @code IS NULL)
	AND (p.name = @name AND p.id != @id OR @name IS NULL)
END


GO
/****** Object:  StoredProcedure [dbo].[amQt_spWorkOrderTypeSelectSingleItem]    Script Date: 11/9/2023 10:53:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[amQt_spWorkOrderTypeSelectSingleItem]	
	@id INT
AS
BEGIN
	SELECT p.*
	FROM amQt_WorkOrderType p
	WHERE p.id = @id
END


GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1=debit, 0=credit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'amQt_DepreciationJournal', @level2type=N'COLUMN',@level2name=N'depreciation_expense_account_debit_credit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1=debit, 0=credit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'amQt_DepreciationJournalDetail', @level2type=N'COLUMN',@level2name=N'debit_credit'
GO
