using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Architecture.Tests;
public static class Assemblies
{
    public static readonly Assembly APIAssembly = typeof(API.Controllers.GroupsController).Assembly;
    public static readonly Assembly ApplicationAssembly = typeof(Application.Groups.GroupService).Assembly;
    public static readonly Assembly AuthAssembly = typeof(Auth.AuthorizationExtensions).Assembly;
    public static readonly Assembly DomainAssembly = typeof(Domain.Groups.GroupModel).Assembly;
    public static readonly Assembly GenericAssembly = typeof(Generic.Extensions).Assembly;
    public static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.Repositories.GroupRepository).Assembly;

    public static readonly string APIAssemblyName = APIAssembly.GetName().Name!;
    public static readonly string ApplicationAssemblyName = ApplicationAssembly.GetName().Name!;
    public static readonly string AuthAssemblyName = AuthAssembly.GetName().Name!;
    public static readonly string DomainAssemblyName = DomainAssembly.GetName().Name!;
    public static readonly string GenericAssemblyName = GenericAssembly.GetName().Name!;
    public static readonly string InfrastructureAssemblyName = InfrastructureAssembly.GetName().Name!;
}
