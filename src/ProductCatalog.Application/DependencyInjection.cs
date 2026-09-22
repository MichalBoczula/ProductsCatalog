using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Common.Behaviors;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones;
using ProductCatalog.Application.Mapping;

namespace ProductCatalog.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            MappingConfig.RegisterMappings();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));







            services.AddScoped<CreateMobilePhoneCommandFlowDescribtor>();
            services.AddScoped<IFlowDescriber<CreateMobilePhoneCommand>, CreateMobilePhoneCommandFlowDescribtor>();

            services.AddScoped<UpdateMobilePhoneCommandFlowDescribtor>();
            services.AddScoped<IFlowDescriber<UpdateMobilePhoneCommand>, UpdateMobilePhoneCommandFlowDescribtor>();

            services.AddScoped<DeleteMobilePhoneCommandFlowDescribtor>();
            services.AddScoped<IFlowDescriber<DeleteMobilePhoneCommand>, DeleteMobilePhoneCommandFlowDescribtor>();

            services.AddScoped<GetMobilePhoneByIdQueryFlowDescribtor>();
            services.AddScoped<IFlowDescriber<GetMobilePhoneByIdQuery>, GetMobilePhoneByIdQueryFlowDescribtor>();

            services.AddScoped<GetMobilePhoneByIdsQueryFlowDescribtor>();
            services.AddScoped<IFlowDescriber<GetMobilePhoneByIdsQuery>, GetMobilePhoneByIdsQueryFlowDescribtor>();

            services.AddScoped<GetMobilePhoneHistoryQueryFlowDescribtor>();
            services.AddScoped<IFlowDescriber<GetMobilePhoneHistoryQuery>, GetMobilePhoneHistoryQueryFlowDescribtor>();

            services.AddScoped<GetMobilePhonesQueryFlowDescribtor>();
            services.AddScoped<IFlowDescriber<GetMobilePhonesQuery>, GetMobilePhonesQueryFlowDescribtor>();

            services.AddScoped<GetFilteredMobilePhonesQueryFlowDescribtor>();
            services.AddScoped<IFlowDescriber<GetFilteredMobilePhonesQuery>, GetFilteredMobilePhonesQueryFlowDescribtor>();

            services.AddScoped<GetTopMobilePhonesQueryFlowDescribtor>();
            services.AddScoped<IFlowDescriber<GetTopMobilePhonesQuery>, GetTopMobilePhonesQueryFlowDescribtor>();




            return services;
        }
    }
}
