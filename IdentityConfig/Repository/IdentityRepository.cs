namespace Id_And_Access_Processor.IdentityConfig.Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly AppIdentityDbContext context;

        public IdentityRepository(AppIdentityDbContext context )
        {
            this.context = context;
        }

        public bool CreateClaim(ClaimsStore claimsStore)
        {
            context.ClaimsStore.Add(claimsStore);
            return context.SaveChanges()>0;    
        }

        public ClaimsStore GetClaim(Int64 claimId)
        {
            return context.ClaimsStore.Where(i=>i.Id == claimId).FirstOrDefault();
        }

        public List<ClaimsStore> GetClaims()
        {
            return context.ClaimsStore.ToList();

        }

        public bool UpdateClaim(ClaimsStore claimsStore)
        {
            context.Entry<ClaimsStore>(claimsStore).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
           
            return context.SaveChanges()>0;
        }
    }
}
