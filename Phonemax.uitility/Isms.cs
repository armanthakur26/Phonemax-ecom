using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.uitility
{
    public interface Isms
    {
        Task SendSmsAsync(string number, string message);
    }
}
