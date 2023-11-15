using Autofac;
using EmpowerIdMicroservice.Application.CQRS.Commands.Comment;
using EmpowerIdMicroservice.Application.CQRS.Commands.Post;
using EmpowerIdMicroservice.Application.CQRS.Commands.Post.UpdatePostCommand;
using EmpowerIdMicroservice.Application.CQRS.Queries.Comment;
using EmpowerIdMicroservice.Application.CQRS.Queries.Post;
using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate;
using EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate;
using EmpowerIdMicroservice.Infrastructure.Persistence.Repositories;

namespace EmpowerIdMicroservice.Application.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; set; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Register MediatR handlers
            builder.RegisterType<CreatePostCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<UpdatePostCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<UpdateCommentCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<CreateCommentCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetCommentByIdQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetCommentByPostIdQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetPostListQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetPostByIdQueryHandler>().AsImplementedInterfaces();

            builder.RegisterType<TokenService>().AsSelf().SingleInstance();
            builder.RegisterType<AuthService>().AsSelf().SingleInstance();

            builder.RegisterType<CommentRepository>()
                .As<ICommentRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PostRepository>()
               .As<IPostRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<CacheService>()
                .As<ICacheService>()
                .InstancePerLifetimeScope();
        }
    }
}
