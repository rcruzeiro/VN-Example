namespace VN.Example.Platform.Domain.BehaviorAggregation.Specifications
{
    public class BehaviorByIpAndPageNameSpecification : BaseSpecification<Behavior>
    {
        public BehaviorByIpAndPageNameSpecification(string ip, string pageName)
            : base(behavior => behavior.IP == ip && behavior.PageName == pageName)
        { }
    }
}
