using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RL8000_NFCReader.MifareClassicEvents;
using RL8000_NFCReader.NFCCardTypes;

namespace RL8000_NFCReader
{
    public class RL8000_NFC : IDisposable
    {
        const string CONNSTR_NAME_DeviceName = "RL8000";

        internal UIntPtr ReaderHandle { get => _ReaderHandle; }
        UIntPtr _ReaderHandle;
        internal UIntPtr CardTnventoryListHandle { get=> _CardTnventoryListHandle; }
        UIntPtr _CardTnventoryListHandle;

        ISO1443A_MifareClassic_NFCCard Card;

        Thread Listener;

        public MifareClassicISO1443ACardDetected MifareClassicISO1443ACardDetectedEvent;

        public bool Disposed { private set; get; } = false;
        bool ThreadEnd = false;

        static RL8000_NFC()
        {
            int result = rfidlib_reader.RDR_LoadReaderDrivers("\\Drivers");
            if (result != 0)
                throw new Exception($"Driver failed to load with result:{result}");
        }

        public RL8000_NFC()
        {
            string ReaderConnectionString = $"{rfidlib_def.CONNSTR_NAME_RDTYPE}={CONNSTR_NAME_DeviceName};{rfidlib_def.CONNSTR_NAME_COMMTYPE}={rfidlib_def.CONNSTR_NAME_COMMTYPE_USB};{rfidlib_def.CONNSTR_NAME_HIDADDRMODE}={rfidlib_def.CONNSTR_Addressing_NoneAddressed};{rfidlib_def.CONNSTR_NAME_HIDSERNUM}=";

            int result = rfidlib_reader.RDR_Open(ReaderConnectionString, ref _ReaderHandle);
            if (result != 0)
                throw new Exception($"Device failed to open with result:{result}");

            rfidlib_aip_iso14443A.ISO14443A_CreateInvenParam(_CardTnventoryListHandle, 0);

            Listener = new Thread(new ParameterizedThreadStart(ListenerLoop));
            Listener.Start(this);
        }

        void ListenerLoop(object ThisIn)
        {
            int result;
            RL8000_NFC This = (RL8000_NFC)ThisIn;

            while (!ThreadEnd)
            {
                UIntPtr TagDataReport;
                result = rfidlib_reader.RDR_TagInventory(ReaderHandle, rfidlib_def.AI_TYPE_CONTINUE, 0, new byte[15], _CardTnventoryListHandle);
                if (result != 0)
                    throw new Exception($"Failed to tag inventory with result:{result}");

                TagDataReport = rfidlib_reader.RDR_GetTagDataReport(This.ReaderHandle, rfidlib_def.RFID_SEEK_FIRST);

                while (TagDataReport.ToUInt64() > 0)
                {
                    UInt32 aip_id = 0;
                    UInt32 tag_id = 0;
                    UInt32 ant_id = 0;
                    Byte dsfid = 0;
                    Byte uidlen = 0;
                    Byte[] uid = new Byte[16];

                    result = rfidlib_aip_iso14443A.ISO14443A_ParseTagDataReport(TagDataReport, ref aip_id, ref tag_id, ref ant_id, uid, ref uidlen);
                    if (result != 0)
                        throw new Exception($"Failed to parse tag inventory for ISO1443A S50/70 Mifare Classic cards or tags with result:{result}");

                    MifareClassicISO1443ACardDetectedEvent?.Invoke(This, new MifareClassicISO1443ACardDetectedEventArg(This, aip_id = 0, tag_id, ant_id, dsfid, uidlen, uid));

                    TagDataReport = UIntPtr.Zero;
                }
            }
        }

        public ISO1443A_MifareClassic_NFCCard ConnectAs_ISO14443A_MifareClassic_NFC(NFCCardInfo CardInfo, byte CardType = 0)
        {
            UIntPtr CardHandle = UIntPtr.Zero;
            int result = rfidlib_aip_iso14443A.MFCL_Connect(ReaderHandle, CardType, CardInfo.UID, ref CardHandle);
            if (result != 0)
                throw new Exception($"Failed to parse tag inventory for ISO1443A S50/70 Mifare Classic cards or tags with result:{result}");
            Card = new ISO1443A_MifareClassic_NFCCard(this, CardHandle, CardInfo);
            return Card;
        }

        public void DisconnectCard()
        {
            if (!Card.Disposed)
                Card.Dispose();
            Card = null;
        }

        public void Dispose()
        {
            DisconnectCard();
            ThreadEnd = true;
            Disposed = true;
            rfidlib_reader.DNODE_Destroy(CardTnventoryListHandle);
            rfidlib_reader.RDR_Close(ReaderHandle);
        }

        ~RL8000_NFC()
        {
            DisconnectCard();
            ThreadEnd = true;
            if (!Disposed)
            {
                rfidlib_reader.DNODE_Destroy(CardTnventoryListHandle);
                rfidlib_reader.RDR_Close(ReaderHandle);
            }
        }
    }
}
