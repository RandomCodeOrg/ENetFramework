using System;

namespace RandomCodeOrg.ENetFramework.Container {
    

    /// <summary>
    /// This attribute can be used on fields or properties to request dependency injection of the required resource.
    /// 
    /// Implementations relying on dependency injections have to be managed resources as well (see <see cref="ManagedAttribute"/>).
    /// Note that a resource has to be requested by its interface definition (except for application scoped resources).
    /// </summary>
    /// <example>
    /// [Managed]
    /// [RequestScoped]
    /// public class HomeController : IHomeController {
    /// 
    ///     [Inject]
    ///     private readonly ISessionController controller;
    /// 
    /// }
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class InjectAttribute : Attribute {

        public Type Implementation { get; set; }

    }


}
