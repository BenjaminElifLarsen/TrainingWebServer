using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Pages
    {
        public string Index()
        {
            return @"<!DOCTYPE html>
            <html>
                <body>
                <h1> Index </h1>
                   <p> This is the index page.</p>
                 </body>
            </html> ";
        }
        public string Test()
        {
            return @"<!DOCTYPE html>
            <html>
                <body>
                <h1> Test </h1>
                   <p> This is the test page.</p>
                 </body>
            </html> ";
        }
    }
}
