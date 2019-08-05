using System;

namespace Helseboka.Core.Common.Interfaces
{
    public interface IContainer
    {
        void RegisterType<TFrom, TTo>(params object[] constructorParameters) where TTo : TFrom;
        T Resolve<T>();
        void RegisterInstance<TInterface>(TInterface instance);
        void RegisterSingletonType<TInterface, TImpl>() where TImpl : TInterface;
    }
}