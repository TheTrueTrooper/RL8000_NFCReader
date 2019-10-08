using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL8000_NFCReader;

namespace RL8000_NFCReader.MifareClassicEvents
{
    public class MifareClassicISO1443ACardDetectedEventArg : EventArgs
    {
        public RL8000_NFC Reader { private set; get; }

        public NFCCardInfo CardInfo { private set; get; } = new NFCCardInfo();

        internal MifareClassicISO1443ACardDetectedEventArg(RL8000_NFC Reader, UInt32 aip_id, UInt32 tag_id, UInt32 ant_id, Byte dsfid, Byte uidlen, Byte[] uid)
        {
            this.Reader = Reader;
            CardInfo = new NFCCardInfo(aip_id, tag_id, ant_id, dsfid, uidlen, uid);
        }
    }
}
