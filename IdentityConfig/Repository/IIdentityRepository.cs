namespace Id_And_Access_Processor.IdentityConfig.Repository
{
    public interface IIdentityRepository
    {
        List<ClaimsStore> GetClaims();

        ClaimsStore GetClaim(Int64 claimId);
        bool CreateClaim(ClaimsStore claimsStore);
        bool UpdateClaim(ClaimsStore claimsStore);
    }
}
