# BulkUpdateWithTVP
This project demonstrates how to do Bulk Update with read-back in Sql Server, using Table valued Parameter.

The repo contains 'db.sql' file, which creates 'School' database and relevant tables and stored procedure for testing.

Important points:
1. I am creating a user-defined table type with name 'dbo.PersonType'.
2. and a Stored Proc 'dbo.BulkUpdatePerson', which accepts this table type as a parameter. 

For the UI, I have a simple Window Form,  which has a datagridview to populated and edit Person data.

Before executing this project:
1. Execute db.sql file on a database
2. Correct the sql server connection string maintained in PersonBulkDataAdapter.cs file.



