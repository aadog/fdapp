using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fdapp.Service
{
    public partial class RootShell
    {
        public static partial Task<string> Exec(string command);
    }
}
