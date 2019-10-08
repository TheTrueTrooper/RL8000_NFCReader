using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL8000_NFCReader
{
    enum RL8000_Errors
    {
        NoError = 0,
        UnknownError = -1,
        IOError = -2,
        ParameterError = -3,
        ParameterValueError = -4,
        ReaderRespondTimeout = -5,
        MemoryAllocationFail = -6,
        ReaderAlreadInUse = -7,
        InvalidMessageSizeFromReader = -12,
        ErrorFromReader_NoRDRGetReaderLastReturnError = -17,
        TimeoutStopTriggerOccur = -21,
        InvalidTagCommand = -22,
        InvalidConfigurationBlockNo = -23,
        TCPSocketError = -25,
        SizeOfInputBufferTooSmall = -26
    }
}
