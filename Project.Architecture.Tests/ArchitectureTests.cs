using Microsoft.AspNetCore.Mvc;
using NetArchTest.Rules;
using Project.Domain;
using static Project.Architecture.Tests.Assemblies;

namespace Project.Architecture.Tests;

public class ArchitectureTests 
{
    [Test]
    public void DomainShouldNotReferenceOtherProjects()
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(ApplicationAssemblyName, InfrastructureAssemblyName, APIAssemblyName, AuthAssemblyName)
            .GetResult();

        Assert.That(result.IsSuccessful);
    }

    [Test]
    public void ApplicationShouldOnlyReferenceDomain()
    {
        var result = Types
              .InAssembly(InfrastructureAssembly)
              .Should()
              .NotHaveDependencyOn(ApplicationAssemblyName)
              .GetResult();

        Assert.That(result.IsSuccessful);
    }

    [Test]
    public void InfrastructureShouldOnlyReferenceDomain()
    {
        var result = Types
                  .InAssembly(InfrastructureAssembly)
                  .That()
                  .HaveNameEndingWith("Repository")
                  .Should()
                  .HaveDependencyOn(DomainAssemblyName)
                  .GetResult();

        Assert.That(result.IsSuccessful);
    }

    [Test]
    public void MappersShouldEndWithMapper()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IModelMapper<,>))
            .Should()
            .HaveNameEndingWith("Mapper")
            .GetResult();

        Assert.That(result.IsSuccessful);
    }

    [Test]
    public void ControllersShouldNotUseRepositories()
    {
        var result = Types.InAssembly(APIAssembly)
            .That()
            .Inherit(typeof(ControllerBase))
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureAssemblyName + ".Repositories")
            .GetResult();

        Assert.That(result.IsSuccessful);
    }

    [Test]
    public void ControllersShouldEndWithController()
    {
        var result = Types.InAssembly(APIAssembly)
            .That()
            .Inherit(typeof(ControllerBase))
            .Should()
            .HaveNameEndingWith("Controller")
            .GetResult();

        Assert.That(result.IsSuccessful);
    }
}