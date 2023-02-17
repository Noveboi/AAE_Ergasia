using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Thema3
{
    internal static class ValidRequests
    {
        public static List<string> GetRequests()
        {
            using (StreamReader reader = new StreamReader("requests.txt"))
            {
                List<string> reqs = new List<string>();
                while (!reader.EndOfStream)
                {
                    reqs.Add(reader.ReadLine());
                }
                return reqs;
            }
        }

        public static bool IsValid(string request)
        {
            foreach (string validRequest in GetRequests())
            {
                if (request == validRequest)
                    return true;
            }

            return false;
        }
    }
}
