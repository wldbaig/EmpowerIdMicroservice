using Autofac;
using MediatR;
using System.Reflection;

namespace EmpowerIdMicroservice.Application.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces(); 
        }
    }
}
