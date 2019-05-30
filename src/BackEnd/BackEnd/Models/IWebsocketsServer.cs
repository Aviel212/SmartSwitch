using System.Threading.Tasks;

namespace BackEnd.Models
{
    public interface IWebsocketsServer
    {
        void Start();
        Task<bool> TurnOff(string mac);
        Task<bool> TurnOn(string mac);
    }
}