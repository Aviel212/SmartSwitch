using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fleck;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Autofac;

namespace BackEnd.Models
{
    public class WebsocketsServer : IWebsocketsServer
    {
        private WebSocketServer _server;
        private SynchronizedCollection<MacConnectionPair> _macConnectionPairs;

        public WebsocketsServer()
        {
            _server = new WebSocketServer("ws://0.0.0.0:8181");
            _macConnectionPairs = new SynchronizedCollection<MacConnectionPair>();
        }

        // Turns the plug with mac MAC address on or off, on if op is "on" or off if op is "off".
        // Returns true is successful, false otherwise
        private async Task<bool> Turn(string op, string mac)
        {
            try
            {
                await GetSocket(mac).Send("turn-load-" + op);
            }
            catch (NullReferenceException)
            {
                throw new PlugNotConnectedException();
            }
            return true;
        }

        // Turns the plug with mac MAC address on; returns true is successful, false otherwise
        public async Task<bool> TurnOn(string mac) => await Turn("on", mac);

        // Turns the plug with mac MAC address off; returns true is successful, false otherwise
        public async Task<bool> TurnOff(string mac) => await Turn("off", mac);

        public void Start()
        {
            _server.Start(socket =>
            {
                socket.OnOpen = async () =>
                {
                    await socket.Send("who-are-you");
                };
                socket.OnClose = () =>
                {
                    RemovePair(socket);
                };
                socket.OnError = error =>
                {
                    RemovePair(socket);
                };
                socket.OnMessage = message =>
                {
                    string[] messageWords = message.Split(" ");

                    switch (messageWords[0])
                    {
                        case "i-am": HandleIAmMessage(socket, messageWords);
                            break;
                        case "on": HandleOnMessage(socket, messageWords);
                            break;
                        case "sample": HandleSampleMessage(socket, messageWords);
                            break;
                    }
                };
            });
        }

        private IWebSocketConnection GetSocket(string mac) => _macConnectionPairs.FirstOrDefault(x => x.Mac == mac).Socket;

        private string GetMac(IWebSocketConnection socket) => _macConnectionPairs.FirstOrDefault(x => x.Socket == socket).Mac;

        private bool RemovePair(IWebSocketConnection socket) => _macConnectionPairs.Remove(_macConnectionPairs.FirstOrDefault(x => x.Socket == socket));

        private async void HandleIAmMessage(IWebSocketConnection socket, string[] messageWords)
        {
            string currentMac = messageWords[1];
            string ownerUsername = messageWords[2];
            _macConnectionPairs.Add(new MacConnectionPair()
            {
                Mac = currentMac,
                Socket = socket
            });

            // TODO - add error handling in case user doesn't exist
            using (ILifetimeScope scope = Program.Container.BeginLifetimeScope())
            {
                SmartSwitchDbContext context = scope.Resolve<SmartSwitchDbContext>();

                Plug currentPlug = await context.Plugs.FindAsync(currentMac);
                if (currentPlug == null) // if the plug is brand new (not in the system), we'll add it to the owner
                {
                    User owner = await context.Users.FindAsync(ownerUsername);
                    owner.Plugs.Add(new Plug(currentMac));
                    await context.SaveChangesAsync();
                }
                else // otherwise, if the plug belongs to another user -- we'll transfer ownership
                {
                    var currentPlugEntry = context.Entry(currentPlug);
                    if (currentPlugEntry.CurrentValues["Username"].ToString() != ownerUsername)
                    {
                        currentPlugEntry.CurrentValues["Username"] = ownerUsername;
                        await context.SaveChangesAsync();
                    }
                }
            }

            await socket.Send("are-you-on");
        }

        private async void HandleOnMessage(IWebSocketConnection socket, string[] messageWords)
        {
            using (ILifetimeScope scope = Program.Container.BeginLifetimeScope())
            {
                SmartSwitchDbContext context = scope.Resolve<SmartSwitchDbContext>();
                // get plug by mac and update its IsOn property
                Plug currentPlug = await context.Plugs.FindAsync(GetMac(socket));
                currentPlug.IsOn = messageWords[1] == "yes";
                await context.SaveChangesAsync();
            }
        }

        private async void HandleSampleMessage(IWebSocketConnection socket, string[] messageWords)
        {
            PowerUsageSample newSample = new PowerUsageSample(Convert.ToDouble(messageWords[1]), Convert.ToDouble(messageWords[2]));

            // get plug by mac and add the new sample
            using (ILifetimeScope scope = Program.Container.BeginLifetimeScope())
            {
                SmartSwitchDbContext context = scope.Resolve<SmartSwitchDbContext>();
                Plug currentPlug = await context.Plugs.FindAsync(GetMac(socket));
                currentPlug.Samples.Add(newSample);
                await context.SaveChangesAsync();
            }
        }
    }
}
