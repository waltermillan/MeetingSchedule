using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork(Context context) : IUnitOfWork, IDisposable
    {
        private IContactRepository? _contacts;
        private ITagRepository? _tags;
        private IContactTagRepository? _contactsTag;
        public IUserRepository? _users;

        public IContactRepository Contacts
        {
            get
            {
                _contacts ??= new ContactRepository(context);

                return _contacts;
            }
        }

        public ITagRepository Tags
        {
            get
            {
                _tags ??= new TagRepository(context);

                return _tags;
            }
        }

        public IContactTagRepository ContactTags
        {
            get
            {
                _contactsTag ??= new ContactTagRepository(context);

                return _contactsTag;
            }
        }

        public IUserRepository Users
        {
            get
            {
                _users ??= new UserRepository(context);
                return _users;
            }
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
