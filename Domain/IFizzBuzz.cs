using System.Threading.Tasks;

namespace Domain
{
    public interface IFizzBuzz
    {
        Task<string> CalculateResultAsync();
    }
}
