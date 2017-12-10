using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cryptoGamblers.Models
{
    public class ActionResult<T> where T : class
    {
        public T Object{ get; set; }
        public HttpStatusCodeResult Status { get; set; }
    }
}