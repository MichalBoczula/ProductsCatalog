using Microsoft.AspNetCore.Mvc.Testing;
using ProductCatalog.Api;
using Reqnroll;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Common
{
    public sealed class ScenarioApiContext
    {
        private ScenarioHttpEvidence? _evidence;
        public ApplicationFactory? Factory { get; set; }
        public HttpClient? Client { get; set; }
        public JsonDocument? OpenApi { get; set; }

        public void InitializeEvidence(ScenarioInfo scenario, string feature)
            => _evidence = ScenarioHttpEvidence.From(scenario, feature);

        public void PublishEvidenceIdentity() => _evidence!.PublishIdentity();
        public void FlushEvidence() => _evidence!.Flush();

        public HttpClient CreateClient(WebApplicationFactory<Program> factory, bool validateContract = true)
        {
            var capture = new HttpEvidenceHandler(_evidence ?? throw new InvalidOperationException("Scenario evidence is not initialized."));
            return validateContract
                ? factory.CreateDefaultClient(new OpenApiResponseHandler(OpenApi!, _evidence), capture)
                : factory.CreateDefaultClient(capture);
        }
    }
}
