using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly Context _context;

    private IContactRepository _contacts;
    private ITagRepository _tags;
    private IContactTagRepository _contactsTag;
    public IUserRepository _users;

    public UnitOfWork(Context context)
    {
        _context = context;
    }

	public IContactRepository Contacts
	{
		get
		{
			if (_contacts is null)
				_contacts = new ContactRepository(_context);

			return _contacts;
		}
	}

	public ITagRepository Tags
    {
        get
        {
            if (_tags is null)
                _tags = new TagRepository(_context);

            return _tags;
        }
    }

    public IContactTagRepository ContactsTag
    {
        get
        {
            if (_contactsTag is null)
                _contactsTag = new ContactTagRepository(_context);

            return _contactsTag;
        }
    }

    public IUserRepository Users
    {
        get
        {
            if (_users is null)
                _users = new UserRepository(_context);
            return _users;
        }
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
