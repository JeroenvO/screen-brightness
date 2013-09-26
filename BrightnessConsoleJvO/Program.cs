/*Code used from http://www.codeproject.com/Articles/236898/Screen-Brightness-Control-for-Laptops-and-Tablets
 and that code is partly stolen from Samuel Lai http://edgylogic.com/projects/display-brightness-vista-gadget/ :)
 * 
 * Created by Jeroen van Oorschot, januari 2013
 * free to use, but not for sale.
 * Console application to set, increase or decrease the screen brightness of a laptop or tablet
 * You can set the desired level as argument, e.g. 55%  - the level is set to the next higher available level, if the argument is not in the level array
 * To increase use <increase percentage>+ e.g. 10+ . To decrease use <decrease percentage>- e.g. 20-
 * 
 * 
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BrightnessConsoleJvO
{
    static class Program
    {
        static void Main(string[] args)
        {
            Class1 ClassInstance = new Class1(args);
        }
    }
        
}
