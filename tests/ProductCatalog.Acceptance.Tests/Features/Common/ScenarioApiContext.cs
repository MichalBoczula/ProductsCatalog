namespace ProductCatalog.Acceptance.Tests.Features.Common
{
    public sealed class ScenarioApiContext
    {
        public ApplicationFactory? Factory { get; set; }
        public HttpClient? Client { get; set; }
    }
}
