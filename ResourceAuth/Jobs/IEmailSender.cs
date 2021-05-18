using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Jobs
{
    public interface IEmailSender
    {
        Task Send();
    }
}
