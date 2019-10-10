namespace VN.Example.Platform.Domain.BehaviorAggregation.Specifications
{
    public class BehaviorByUniqueSpecification : BaseSpecification<Behavior>
    {
        public BehaviorByUniqueSpecification(string ip, string pageName, string userAgent)
            : base(behavior => behavior.IP == ip && behavior.PageName
                                                    == pageName && behavior.UserAgent == userAgent)
        { }
    }
}
