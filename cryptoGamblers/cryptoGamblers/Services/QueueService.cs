using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cryptoGamblers.Services
{
    public class QueueService : IQueueService
    {
        public Guid findOpponent(string userName)
        {
            return Guid.NewGuid();   

        }
    }
}