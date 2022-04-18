using Database.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Database
{
    public interface ITestData
    {
        Task<TestObject> GetTestObject(long id);

        Task UpdateTestObject(TestObject obj, CancellationToken token);

        Task<long> InsertTestObject(string value, CancellationToken token);

        Task DeleteTestObject(long id, CancellationToken token);
    }
}
