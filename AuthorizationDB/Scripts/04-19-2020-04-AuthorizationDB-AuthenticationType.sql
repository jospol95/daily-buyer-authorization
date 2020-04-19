
--  dbo.authentication_type
--  created by Jose Polanco
--  Use: differentiate authentication type for users

USE AuthenticationDB;
GO

IF OBJECT_ID(N'dbo.authentication_type', N'U') IS NOT NULL
    BEGIN
        TRUNCATE TABLE dbo.authentication_type
        DROP TABLE dbo.authentication_type
    END
GO

CREATE TABLE dbo.authentication_type(
    id INT,
    authentication_name VARCHAR(20)
)

GO

--  AuthenticationType.enum in the AuthorizationService needs to match this data or vice versa
INSERT INTO user_status(status)
VALUES
(1,'AuthorizationAPI'),
(2,'GoogleAuthAPI'),
(3,'FacebookAuthAPI'),
(4,'AppleAuthAPI')