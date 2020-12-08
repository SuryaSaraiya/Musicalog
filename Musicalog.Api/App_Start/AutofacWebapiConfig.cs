using Autofac;
using Autofac.Integration.WebApi;
using MediatR;
using Musicalog.Common.Data;
using Musicalog.Common.Infrastructure.RequestHandlers.Albums;
using Musicalog.Common.Infrastructure.Services;
using Musicalog.Common.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;

namespace Musicalog.Api
{
    public class AutofacWebapiConfig
    {

        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {

            builder.Register<ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.RollingFile(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/Log-{Date}.txt")
                    .CreateLogger();
            }).SingleInstance();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(typeof(AlbumQueryHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(AlbumListQueryHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(CreateAlbumCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(UpdateAlbumCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(DeleteAlbumCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterType<AlbumService>()
                .As<IAlbumService>()
                .InstancePerRequest();

            Container = builder.Build();

            return Container;
        }

    }
}