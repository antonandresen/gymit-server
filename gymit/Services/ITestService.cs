using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using gymit.Models;

namespace gymit.Services
{
    public interface ITestService
    {
        Task<List<Test>> GetTestsAsync();
        Task<Test> GetTestByIDAsync(Guid testID);
        Task<bool> CreateTestAsync(Test test);
        Task<bool> UpdateTestAsync(Test testToUpdate);
        Task<bool> DeleteTestAsync(Guid testID);
        Task<bool> UserOwnsTestAsync(Guid testID, string userID);
        Task<List<Tag>> GetAllTagsAsync();
    }
}
