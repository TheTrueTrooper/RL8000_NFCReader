using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL8000_NFCReader
{
    public class NFCCardInfo
    {
        public UInt32 AirProtocalID { private set; get; } = 0;
        public UInt32 TagID { private set; get; } = 0;
        public UInt32 AntennaID { private set; get; } = 0;
        public Byte DSFID { private set; get; } = 0;
        public Byte UIDlen { private set; get; } = 0;
        public Byte[] UID { private set; get; } = new Byte[16];

        public NFCCardInfo()
        { }

        public NFCCardInfo(UInt32 aip_id, UInt32 tag_id, UInt32 ant_id, Byte dsfid, Byte uidlen, Byte[] uid)
        {
            AirProtocalID = aip_id;
            TagID = tag_id;
            AntennaID = ant_id;
            DSFID = dsfid;
            UIDlen = uidlen;
            UID = uid;
        }
    }
}
