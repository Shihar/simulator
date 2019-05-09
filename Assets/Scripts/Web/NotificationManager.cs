/**
 * Copyright (c) 2019 LG Electronics, Inc.
 *
 * This software contains code licensed as described in LICENSE.
 *
 */

using Nancy.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Web
{
    public class NotificationManager
    {
        public struct Message
        {
            public string Event;
            public string Data;
        }

        public static HashSet<NotificationManager> Clients = new HashSet<NotificationManager>();

        public BlockingCollection<Message> Queue = new BlockingCollection<Message>();

        static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public static void SendNotification(string @event, object obj)
        {
            var msg = new Message() { Event = @event, Data = Serializer.Serialize(obj) };

            lock (Clients)
            {
                foreach (var client in Clients)
                {
                    client.Queue.Add(msg);
                }
            }
        }
    }
}
