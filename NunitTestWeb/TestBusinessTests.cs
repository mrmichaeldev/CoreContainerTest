using BusinessLogic;
using Database;
using Database.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NunitTestWeb
{
    public class TestBusinessTests
    {
        Mock<ITestData> testData;

        [SetUp]
        public void Setup()
        {
            testData = new Mock<ITestData>();
            testData.Setup(
                d => d.GetTestObject(It.IsAny<long>()))
                .Returns((long i) =>
                {
                    if (i > 0)
                    {
                        return Task.FromResult(new TestObject
                        {
                            Id = i,
                            Value = "test"
                        });
                    }
                    else
                    {
                        throw new Exception("Not Found");
                    }
                });
        }

        [Test]
        public void TestData()
        {
            testData.Setup(d => d.UpdateTestObject(It.IsAny<TestObject>(), It.IsAny<CancellationToken>()))
                .Callback((TestObject obj, CancellationToken token) =>
                {
                    Assert.AreEqual("TEST", obj.Value);
                });

            var testBusiness = new TestBusiness(testData.Object);
            testBusiness.BusinessTransform(7, CancellationToken.None).Wait();
        }

        [Test]
        public void TestNull()
        {
            testData.Setup(d => d.UpdateTestObject(It.IsAny<TestObject>(), It.IsAny<CancellationToken>()))
                .Callback((TestObject obj, CancellationToken token) =>
                {
                    Assert.AreEqual("TEST", obj.Value);
                });

            var testBusiness = new TestBusiness(testData.Object);

            Assert.Throws<AggregateException>(() =>
            {
                testBusiness.BusinessTransform(0, CancellationToken.None).Wait();
            });
        }
    }
}
