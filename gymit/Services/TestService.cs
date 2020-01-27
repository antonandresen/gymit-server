using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using gymit.Models;
using gymit.Models.DBContexts;


namespace gymit.Services
{
    public class TestService : ITestService
    {
        private readonly DataContext _dataContext;

        public TestService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<Test>> GetTestsAsync()
        {
            var queryable = _dataContext.Tests.AsQueryable();
            return await queryable.Include(x => x.Tags).ToListAsync(); 
        }

        public async Task<Test> GetTestByIDAsync(Guid testID)
        {
            return await _dataContext.Tests.SingleOrDefaultAsync(test => test.ID == testID);
        }

        public async Task<bool> CreateTestAsync(Test test)
        {
            test.Tags?.ForEach(tag => tag.TagName = tag.TagName.ToLower());

            await AddNewTags(test);
            await _dataContext.Tests.AddAsync(test);

            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdateTestAsync(Test testToUpdate)
        {
            _dataContext.Tests.Update(testToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteTestAsync(Guid testID)
        {
            var test = await GetTestByIDAsync(testID);
            if (test == null) return false;

            _dataContext.Tests.Remove(test);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> UserOwnsTestAsync(Guid testID, string userID)
        {
            var test = await _dataContext.Tests.AsNoTracking().SingleOrDefaultAsync(tst => tst.ID == testID);
            if(test == null)
            {
                return false;
            }

            if(test.UserId != userID)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _dataContext.Tags.AsNoTracking().ToListAsync();
        }

        private async Task AddNewTags(Test test)
        {
            foreach (var tag in test.Tags)
            {
                var existingTag =
                    await _dataContext.Tags.SingleOrDefaultAsync(x =>
                        x.Name == tag.TagName);
                if (existingTag != null)
                    continue;

                await _dataContext.Tags.AddAsync(new Tag
                { Name = tag.TagName, CreatedOn = DateTime.UtcNow, CreatorId = test.UserId });
            }
        }
    }
}
