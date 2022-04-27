using Database.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Database
{
    public class MockTestData : ITestData
    {
        static TestObject obj = new TestObject
        {
            Value = "Test String"
        };

        public Task DeleteTestObject(long id, CancellationToken token)
        {
            obj = null;
            return Task.Delay(0);
        }

        public Task<TestObject> GetTestObject(long id)
        {
            return Task.FromResult(obj);
        }

        public Task<long> InsertTestObject(string value, CancellationToken token)
        {
            obj.Value = value;
            obj.Id++;
            return Task.FromResult(obj.Id);
        }

        public Task UpdateTestObject(TestObject obj, CancellationToken token)
        {
             obj.Value = obj.Value;
            return Task.Delay(0);
        }
    }
}
