using System;
using Helseboka.Core.Common.Interfaces;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace Helseboka.Core.Common.CommonImpl
{
    public class UnityContainerImpl : IContainer
    {
        private UnityContainer container; 

        public UnityContainerImpl()
        {
            container = new UnityContainer();
        }

        public void RegisterType<TFrom, TTo>(params object[] constructorParameters) where TTo : TFrom
        {
            if(constructorParameters != null && constructorParameters.Length > 0)
            {
                container.RegisterType<TFrom, TTo>(new InjectionConstructor(constructorParameters));
            }
            else
            {
                container.RegisterType<TFrom, TTo>();
            }
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public void RegisterInstance<TInterface>(TInterface instance)
        {
            container.RegisterInstance<TInterface>(instance);
        }

        public void RegisterSingletonType<TInterface, TImpl>() where TImpl : TInterface
        {
            container.RegisterType<TInterface, TImpl>(new SingletonLifetimeManager());
        }
    }
}
