using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using F10.Models;
using FA1.DbContext;
using FA1.Entities;
using Microsoft.EntityFrameworkCore;

namespace F10.DataAccess;

public sealed class F10Repository : IF10Repository
{
    private readonly AppDbContext _appContext;

    public F10Repository(AppDbContext context)
    {
        _appContext = context;
    }

    public Task<bool> DoesTodoTaskListExistAsync(long listId, CancellationToken ct)
    {
        return _appContext.Set<TodoTaskListEntity>().AnyAsync(entity => entity.Id == listId, ct);
    }

    public async Task<IEnumerable<F10TodoTaskListModel>> GetTodoTaskListAsync(
        long todoTaskListId,
        int numberOfRecord,
        CancellationToken ct
    )
    {
        return await _appContext
            .Set<TodoTaskListEntity>()
            .AsNoTracking()
            .Where(entity => entity.Id >= todoTaskListId)
            .Select(entity => new F10TodoTaskListModel { Id = entity.Id, Name = entity.Name })
            .OrderBy(entity => entity.Id)
            .Take(numberOfRecord + 1)
            .ToListAsync(ct);
    }
}
