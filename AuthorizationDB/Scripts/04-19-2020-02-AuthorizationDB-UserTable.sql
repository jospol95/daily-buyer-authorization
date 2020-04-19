
--  dbo.user
--  Created by Jose Polanco
--  Use: Store user information such as username and password,
--  probably this fields will be divided among CustomerDB.customer later
USE AuthenticationDB;
GO

-- IF OBJECT_ID(N' dbo.[user]',N'U') IS NOT NULL
--      BEGIN
--         DROP TABLE dbo.[user]
--     END
-- GO

CREATE TABLE dbo.[user](
    id INT PRIMARY KEY IDENTITY (1,1),
    authentication_type_id INT NOT NULL,
    username VARCHAR(100) NOT NULL, --should be unique
    firstname VARCHAR(50) NOT NULL,
    lastname VARCHAR(50) NOT NULL,
    email VARCHAR(100) NULL,
    password_hash VARBINARY(MAX) NOT NULL,
    password_salt VARBINARY(MAX) NOT NULL,
    created_date DATETIME2 NULL,
    modified_date DATETIME2 NULL,
    user_status_id INT NOT NULL,
)