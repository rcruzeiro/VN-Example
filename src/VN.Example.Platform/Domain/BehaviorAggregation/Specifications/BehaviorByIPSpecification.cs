namespace VN.Example.Platform.Domain.BehaviorAggregation.Specifications
{
    public sealed class BehaviorByIPSpecification : BaseSpecification<Behavior>
    {
        public BehaviorByIPSpecification(string ip)
            : base(behavior => behavior.IP == ip)
        { }
    }
}
