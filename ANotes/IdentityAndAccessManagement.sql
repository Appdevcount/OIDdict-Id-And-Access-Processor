select * from [OID].[OpenIddictApplications]
select * from [OID].[OpenIddictAuthorizations]
select * from [OID].[OpenIddictScopes]
select * from [OID].[OpenIddictTokens]

delete from [OID].[OpenIddictAuthorizations]
delete from [OID].[OpenIddictApplications]
delete from [OID].[OpenIddictScopes]
delete from [OID].[OpenIddictTokens]

select * from [Identity].[Role]
select * from [Identity].[RoleClaims]
select * from [Identity].[User]
select * from [Identity].[UserLogins]
select * from [Identity].[UserClaims]
select * from [Identity].[UserTokens]
select * from [Identity].[UserRoles]
select * from [Identity].[Claims]


delete from [Identity].[Role]
delete from [Identity].[RoleClaims]
delete from [Identity].[User]
delete from [Identity].[UserTokens]
delete from [Identity].[UserClaims]
delete from [Identity].[UserLogins]
delete from [Identity].[UserRoles]
delete from [Identity].[Claims]