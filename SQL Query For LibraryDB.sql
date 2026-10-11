-- ============================================
-- CLEANUP: drop existing DB so we start fresh
-- ============================================
USE master;
GO
IF DB_ID('LibraryDB') IS NOT NULL
BEGIN
    ALTER DATABASE [LibraryDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [LibraryDB];
END
GO

-- ============================================
-- Create database
-- ============================================
CREATE DATABASE LibraryDB;
GO

USE LibraryDB;
GO

-- Create the Books table
CREATE TABLE Books
(
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    Genre NVARCHAR(50),
    TotalCopies INT NOT NULL CHECK (TotalCopies > 0)
);
GO

-- Create the Loans table
CREATE TABLE Loans
(
    LoanID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Phone NVARCHAR(20),
    BookID INT NOT NULL,
    LoanDate DATE NOT NULL,
    DueDate DATE NOT NULL,

    CONSTRAINT FK_Loans_Books
        FOREIGN KEY (BookID)
        REFERENCES Books(BookID),

    CONSTRAINT CK_Loans_DueDate
        CHECK (DueDate > LoanDate)
);
GO


-- ============================================
-- Insert 3 sample books
-- ============================================
INSERT INTO [dbo].[Books]
           ([Title], [Author], [Genre], [TotalCopies])
     VALUES
           (N'TGG',        N'Ruan', N'Romance',   7),
           (N'Water',      N'John', N'Discovery', 4),
           (N'Laughing',   N'Dona', N'Comedy',    2)
GO

-- ============================================
-- Insert 3 sample loans
-- ============================================
INSERT INTO [dbo].[Loans]
           ([FirstName], [LastName], [Phone], [BookID], [LoanDate], [DueDate])
     VALUES
           (N'A', N'a', N'01234567', 1, GETDATE(), '2026-10-20'),
           (N'B', N'b', N'01234567', 1, GETDATE(), '2026-10-21'),
           (N'C', N'c', N'01234567', 1, GETDATE(), '2026-10-24')
GO

-- =====================================================
-- Stored procedures: Books

--ADD
CREATE PROCEDURE sp_AddBook
	@pTitle       NVARCHAR(150),
	@pAuthor      NVARCHAR(100),
	@pGenre       NVARCHAR(50),
	@pTotalCopies INT
AS
BEGIN
	INSERT INTO [dbo].[Books] ([Title], [Author], [Genre], [TotalCopies])
	VALUES (@pTitle, @pAuthor, @pGenre, @pTotalCopies)
END
GO

--Get
CREATE PROCEDURE sp_GetAllBooks
AS
BEGIN
	SELECT * FROM [dbo].[Books]
END
GO

--Update
CREATE PROCEDURE sp_UpdateBook
	@pTitle       NVARCHAR(150),
	@pAuthor      NVARCHAR(100),
	@pGenre       NVARCHAR(50),
	@pTotalCopies INT,
	@pBookID      INT
AS
BEGIN
	UPDATE [dbo].[Books]
	SET [Title]       = @pTitle,
	    [Author]      = @pAuthor,
	    [Genre]       = @pGenre,
	    [TotalCopies] = @pTotalCopies
	WHERE [BookID] = @pBookID
END
GO

--Delete
CREATE PROCEDURE sp_DeleteBook
	@pBookID INT
AS
BEGIN
	DELETE FROM [dbo].[Books]
	WHERE [BookID] = @pBookID
END
GO

-- ============================================
-- Stored procedures: Loans

--Add
CREATE PROCEDURE sp_AddLoan
	@pFirstName NVARCHAR(50),
	@pLastName  NVARCHAR(50),
	@pPhone     NVARCHAR(20),
	@pBookID    INT,
	@pLoanDate  DATE,
	@pDueDate   DATE
AS
BEGIN
	INSERT INTO [dbo].[Loans]
           ([FirstName], [LastName], [Phone], [BookID], [LoanDate], [DueDate])
	VALUES
           (@pFirstName, @pLastName, @pPhone, @pBookID, @pLoanDate, @pDueDate)
END
GO

--Get
CREATE PROCEDURE sp_GetAllLoans
AS
BEGIN
	SELECT L.LoanID, L.FirstName, L.LastName, L.Phone,
	       L.BookID, B.Title AS BookTitle,
	       L.LoanDate, L.DueDate
	FROM [dbo].[Loans] AS L
	INNER JOIN [dbo].[Books] AS B ON B.BookID = L.BookID
END
GO

--Update
CREATE PROCEDURE sp_UpdateLoan
	@pFirstName NVARCHAR(50),
	@pLastName  NVARCHAR(50),
	@pPhone     NVARCHAR(20),
	@pBookID    INT,
	@pLoanDate  DATE,
	@pDueDate   DATE,
	@pLoanID    INT
AS
BEGIN
	UPDATE [dbo].[Loans]
	SET [FirstName] = @pFirstName,
	    [LastName]  = @pLastName,
	    [Phone]     = @pPhone,
	    [BookID]    = @pBookID,
	    [LoanDate]  = @pLoanDate,
	    [DueDate]   = @pDueDate
	WHERE [LoanID] = @pLoanID
END
GO

--Delete
CREATE PROCEDURE sp_DeleteLoan
	@pLoanID INT
AS
BEGIN
	DELETE FROM [dbo].[Loans]
	WHERE [LoanID] = @pLoanID
END
GO

--Search
CREATE PROCEDURE sp_SearchLoans
    @pSearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT L.LoanID, L.FirstName, L.LastName, L.Phone,
           L.BookID, B.Title AS BookTitle,
           L.LoanDate, L.DueDate
    FROM [dbo].[Loans] AS L
    INNER JOIN [dbo].[Books] AS B ON B.BookID = L.BookID
    WHERE L.FirstName LIKE '%' + @pSearchTerm + '%'
       OR L.LastName  LIKE '%' + @pSearchTerm + '%'
       OR CAST(L.LoanID AS NVARCHAR(20)) = @pSearchTerm
END
GO