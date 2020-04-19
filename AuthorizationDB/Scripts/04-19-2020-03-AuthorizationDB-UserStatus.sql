
--  dbo.user_status
--  Created by Jose Polanco
--  Use: Have a record of different user statuses (better than have an Active,Inactive boolean field in dbo.User)
USE AuthenticationDB;
GO

IF OBJECT_ID(N'dbo.user_status', N'U') IS NOT NULL
    BEGIN
        TRUNCATE TABLE dbo.user_status
        DROP TABLE dbo.user_status
    END
GO

CREATE TABLE dbo.user_status(
    id INT,
    status VARCHAR(20) NOT NULL
)

GO

--  UserStatus.enum in the AuthorizationService needs to match this data or viceversa

INSERT INTO user_status(status)
VALUES
(1,'Active'),
(2,'Inactive')