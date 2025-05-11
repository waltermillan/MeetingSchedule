using Core.Interfaces;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IContactRepository Contacts { get; }
        ITagRepository Tags { get; }
        IContactTagRepository ContactsTag { get; }

        void Dispose();
        Task<int> SaveAsync();
    }
}
