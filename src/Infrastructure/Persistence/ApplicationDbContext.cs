using System.Reflection;
using App.Application.Common.Interfaces;
using App.Domain.Entities;
using App.Infrastructure.Identity;
using Duende.IdentityServer.EntityFramework.Options;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace App.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IMediator mediator

    ) 
        : base(options, operationalStoreOptions)
    {
        _mediator = mediator;
    }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
