namespace VN.Example.Platform.Domain.BehaviorAggregation.Specifications
{
    public sealed class BehaviorByPageNameSpecification : BaseSpecification<Behavior>
    {
        public BehaviorByPageNameSpecification(string pageName)
            : base(behavior => behavior.PageName == pageName)
        { }
    }
}
