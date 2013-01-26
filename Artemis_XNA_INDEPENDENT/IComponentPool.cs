using System;
namespace Artemis
{
    public interface IComponentPool<T>
     where T : ComponentPoolable
    {
        void CleanUp();
        T New();
        void ReturnObject(T obj);
    }
}
