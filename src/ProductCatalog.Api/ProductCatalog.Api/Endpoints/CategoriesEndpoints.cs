
namespace ProductCatalog.Api.Endpoints
{
    public static class CategoriesEndpoints
    {
        public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
        {
            MapCategoriesQueries(app);
            MapCategoriesCommands(app);

            return app;
        }

        private static void MapCategoriesCommands(IEndpointRouteBuilder app)
        {
        }

        private static void MapCategoriesQueries(IEndpointRouteBuilder app)
        {
        }
    }
}
