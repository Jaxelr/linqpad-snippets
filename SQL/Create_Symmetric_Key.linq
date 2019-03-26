<Query Kind="SQL" />

--Create master key first for Encryption. 
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'superdupersecurepassgoeshereplease'

--DROP CERTIFICATE CertEnvironment
CREATE CERTIFICATE CertEnvironment
   --ENCRYPTION BY PASSWORD = 'superdupersecurepassgoeshereplease'
   WITH SUBJECT = 'Environment Certificate for Encrypting keys';
GO

--DROP SYMMETRIC KEY ConnectionUserKey
CREATE SYMMETRIC KEY ConnectionUserKey
WITH Algorithm = AES_256
ENCRYPTION BY CERTIFICATE CertEnvironment;
GO

--This is a sample usage of the encryption 
IF OBJECT_ID('usrs') > 0  DROP TABLE usrs;
CREATE TABLE usrs
(
	id INT IDENTITY(1, 1) NOT NULL,
	usr varchar(256) NOT NULL,
	pass varbinary(256) NOT NULL
)

--Open the key to Write.
OPEN SYMMETRIC KEY ConnectionUserKey DECRYPTION BY CERTIFICATE CertEnvironment;

INSERT INTO usrs(usr, pass)
 SELECT 'The User' , EncryptByKey(Key_GUID('ConnectionUserKey'), 'The Secure Password')

CLOSE SYMMETRIC KEY ConnectionUserKey;

SELECT * FROM usrs; --Encrypted

--Open the key to Read.
OPEN SYMMETRIC KEY ConnectionUserKey DECRYPTION BY CERTIFICATE CertEnvironment;

SELECT usr, CONVERT(varchar(256), DecryptByKey(pass)) password FROM usrs

CLOSE SYMMETRIC KEY ConnectionUserKey;
GO
