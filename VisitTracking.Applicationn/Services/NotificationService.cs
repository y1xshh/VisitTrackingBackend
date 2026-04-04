using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitTracking.Application.Services
{
    public class NotificationService
    {
        public Task send(string Message)
        {
            Console.WriteLine(Message);
            return Task.CompletedTask;
        }

    }
}

    