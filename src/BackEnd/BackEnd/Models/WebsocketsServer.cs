﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fleck;

namespace BackEnd.Models
{
    public class WebsocketsServer
    {
        private static WebsocketsServer _instance = null;

        private WebSocketServer _server;
        private Dictionary<IWebSocketConnection, string> _macConnections;

        private WebsocketsServer()
        {
            _server = new WebSocketServer("ws://0.0.0.0:8181");
            _macConnections = new Dictionary<IWebSocketConnection, string>();
        }

        public static WebsocketsServer GetInstance()
        {
            if (_instance == null) _instance = new WebsocketsServer();
            return _instance;
        }

        // Turns the plug with mac MAC address on or off, on if op is "on" or off if op is "off".
        // Returns true is successful, false otherwise
        private bool Turn(string op, string mac)
        {
            try
            {
                _macConnections.FirstOrDefault(x => x.Value == mac).Key.Send("turn-load-" + op);
            }
            catch (NullReferenceException)
            {
                throw new PlugNotConnectedException();
            }
            return true;
        }

        // Turns the plug with mac MAC address on; returns true is successful, false otherwise
        public bool TurnOn(string mac)
        {
            return Turn("on", mac);
        }

        // Turns the plug with mac MAC address off; returns true is successful, false otherwise
        public bool TurnOff(string mac)
        {
            return Turn("off", mac);
        }

        public void Start()
        {
            _server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    _macConnections.Add(socket, null);
                    socket.Send("who-are-you");
                };
                socket.OnClose = () =>
                {
                    _macConnections.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    if (message.StartsWith("i-am-")) _macConnections[socket] = message.Substring(5);
                };
            });
        }

        public void TurnAll(string op)
        {
            foreach (KeyValuePair<IWebSocketConnection, string> keyValue in _macConnections)
            {
                Turn(op, keyValue.Value);
            }
        }
    }
}
