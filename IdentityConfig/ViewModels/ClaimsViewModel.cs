namespace Id_And_Access_Processor.IdentityConfig.ViewModels
{
    public class ClaimsViewModel
    {
        public Int64 Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public bool Selected { get; set; }//new
    }
}
