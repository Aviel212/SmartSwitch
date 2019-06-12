using System.Threading.Tasks;

namespace BackEnd.Models.Websockets
{
    public interface IWebsocketsServer
    {
        void Start();
        Task<bool> TurnOff(string mac);
        Task<bool> TurnOn(string mac);
        void NotifyUserAdded(User newUser);
    }
}