using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Model
{
    public class RoomsModel
    {
        string room;
        public RoomsModel(string room)
        {
            this.room = room;
        }
        public string Room
        {
            get { return room; }
            set { room = value; }
        }
    }
}
