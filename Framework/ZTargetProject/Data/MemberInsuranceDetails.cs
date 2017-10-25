namespace Framework.ZTargetProject.Data
{
    public class MemberInsuranceDetails
    {
        //Optional
        public string OptionalDeathBenefitAmount { get; set; }
        public string OptionalTPDBenefitAmount { get; set; }

        //Inbuilt
        public string InbuiltDeathInsuranceAmount { get; set; }
        public string InbuiltTIInsuranceAmount { get; set; }

        //Other
        public  string FirstPremiumAmount { get; set; }
    }
}