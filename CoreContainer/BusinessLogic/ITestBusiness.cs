using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface ITestBusiness
    {
        public Task BusinessTransform(long id, CancellationToken token);
    }
}