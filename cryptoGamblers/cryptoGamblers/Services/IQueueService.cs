﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cryptoGamblers.Services
{
    public interface IQueueService
    {
        Guid findOpponent(string userName);
    }
}