using BLL;
using DAL;
using IBLL;
using IDAL;
using Models;
using System;

using Unity;

namespace RepositorySysUI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            //注册用户表的DAL和BLL 相当于new一个对象
            container.RegisterType<IUserInfoDAL, UserInfoDAL>();
            container.RegisterType<IUserInfoBLL, UserInfoBLL>();
            container.RegisterType<IDpartmentInfo, DepartmentInfoDAL>();
            container.RegisterType<IDepartmentInfoBLL, DepartmentInfoBLL>();


        }
    }
}