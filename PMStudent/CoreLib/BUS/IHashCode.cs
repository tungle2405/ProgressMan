using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.BUS
{
    public interface IHashCode
    {
        string Encrypt(string key);

        string Decrypt(string key);
    }
}
