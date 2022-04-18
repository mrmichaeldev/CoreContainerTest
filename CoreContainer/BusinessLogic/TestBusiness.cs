using Database;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class TestBusiness : ITestBusiness
    {
        private readonly ITestData testData;

        public TestBusiness(ITestData testData)
        {
            this.testData = testData;
        }

        public async Task BusinessTransform(long objId, CancellationToken token)
        {
            var testObject = await testData.GetTestObject(objId);
            testObject.Value = testObject.Value.ToUpper();

            await testData.UpdateTestObject(testObject, token);
        }
    }
}
