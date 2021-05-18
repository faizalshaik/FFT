using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftlib
{
    enum Channels
    {
        channel_h2 = 0,
        channel_h1,
        channel_l2,
        channel_l1,
        channel_reserved1,
        channel_reserved2,
        channel_reserved3,
        channel_reserved4,
        channel_max_count
    }

    enum CalcFunctions
    {
        magnitude = 0,
        magnitude_normalized,
        get_intensity,
        get_amplitude,
        get_decibels,
        get_decible10,
        get_power_spectrum,
        get_noise_power,
        calc_reserved_0,
        calc_reserved_1,
        calc_reserved_2,
        calc_reserved_3,
        calc_function_max_count
    }

    public class FILTERSETTING
    {
        public int windowSize;
        public int windowType;
        public void read(BinaryReader reader)
        {
            windowSize = reader.ReadInt32();
            windowType = reader.ReadInt32();
        }
    }

    public class CONSTANTS
    {
        public double adcVdd;
        public double adcHafVdd;
        public double adcDaynamicRange;
        public double adcLSB;
        public double adcDcOffsetInVolts;
        public double adcDcOffsetInDecimal;
        public double dbRefPoint;
        public double fftFreqResolution;
        public double UppderVoltageThreshold;
        public double LowerVoltageThreshold;
        public int adcResoluationBits;
        public int adcResoluation;
        public int adcSamplingFrequencey;
        public int adcWindowSize;
        public int fftResultSize;
        public void read(BinaryReader reader)
        {
            adcVdd = reader.ReadDouble();
            adcHafVdd = reader.ReadDouble();
            adcDaynamicRange = reader.ReadDouble();
            adcLSB = reader.ReadDouble();
            adcDcOffsetInVolts = reader.ReadDouble();
            adcDcOffsetInDecimal = reader.ReadDouble();
            dbRefPoint = reader.ReadDouble();
            fftFreqResolution = reader.ReadDouble();
            UppderVoltageThreshold = reader.ReadDouble();
            LowerVoltageThreshold = reader.ReadDouble();
            adcResoluationBits = reader.ReadInt32();
            adcResoluation = reader.ReadInt32();
            adcSamplingFrequencey = reader.ReadInt32();
            adcWindowSize = reader.ReadInt32();
            fftResultSize = reader.ReadInt32();
        }
    }

    public class OutputFileHeader
    {
        static public int ChannelMaxCount = 8;
        static public int FunctionMaxCount = 12;

        public int version;
        public FILTERSETTING[] filterSettings = new FILTERSETTING[(int)Channels.channel_max_count];
        public double[] gains = new double[(int)Channels.channel_max_count];
        public CONSTANTS constants = new CONSTANTS();
        public byte[] functions = new byte[(int)CalcFunctions.calc_function_max_count];
        public byte[] channels = new byte[(int)Channels.channel_max_count];
        public uint blockSize;
        public uint blockCount;


        public int channelsInBlock;
        public int functionsInBlock;
        public void read(BinaryReader reader)
        {
            version = reader.ReadInt32();
            int i;
            for(i = 0; i< filterSettings.Length; i++)
            {
                filterSettings[i] = new FILTERSETTING();
                filterSettings[i].read(reader);
            }
            for (i = 0; i < (int)Channels.channel_max_count; i++)
                gains[i] = reader.ReadDouble();
            constants.read(reader);
            for (i = 0; i < (int)CalcFunctions.calc_function_max_count; i++)
                functions[i] = reader.ReadByte();
            for (i = 0; i < (int)Channels.channel_max_count; i++)
                channels[i] = reader.ReadByte();
            blockSize = reader.ReadUInt32();
            blockCount = reader.ReadUInt32();
            channelsInBlock = calcChannelsInBlock();
            functionsInBlock = calcFunctionsInBlock();
        }

        private int calcChannelsInBlock()
        {
            int nChns = 0;
            foreach(var v in channels)
            {
                if (v == 1) nChns++;
            }
            return nChns;
        }
        private int calcFunctionsInBlock()
        {
            int nFns = 0;
            foreach (var v in functions)
            {
                if (v == 1) nFns++;
            }
            return nFns;
        }

        public int getChannelIndex(int chNo)
        {
            int idx = 0, i;
            for (i = 0; i < chNo; i++)
            {
                if (channels[i] == 1) idx++;
            }
            return idx;
        }
        public int getFunctionIndex(int fnNo)
        {
            int idx = 0, i;
            for (i = 0; i < fnNo; i++)
            {
                if (functions[i] == 1) 
                    idx++;
            }
            return idx;
        }
    }

    public class Block4
    {
        static public int BlockHeaderSize = 16;
        static public int FftResultSize = 512;

        public int index;
        public int timeStamp;
        public float adcVolt;
        public float temperature;
        public List<double> voltMins = new List<double>();
        public List<double> voltMaxs = new List<double>();
        public List<double> voltAvgs = new List<double>();
        public List<float[]> results = new List<float[]>();
        public int channels;
        public int functions;

        public Block4(int channelCount, int functionCount)
        {
            channels = channelCount;
            functions = functionCount;
        }
        public void read(BinaryReader reader, bool onlyHeader)
        {
            int i, j;
            index = reader.ReadInt32();
            timeStamp = reader.ReadInt32();
            adcVolt = reader.ReadSingle();
            temperature = reader.ReadSingle();
            for (i = 0; i < channels; i++)
                voltMins.Add(reader.ReadDouble());
            for (i = 0; i < channels; i++)
                voltMaxs.Add(reader.ReadDouble());
            for (i = 0; i < channels; i++)
                voltAvgs.Add(reader.ReadDouble());
            if(!onlyHeader)
            {
                int nDatas = channels * functions;
                for(i = 0; i< nDatas; i++)
                {
                    float[] data = new float[FftResultSize];
                    for (j = 0; j < FftResultSize; j++)
                        data[j] = reader.ReadSingle();
                    results.Add(data);
                }
            }
        }
    }

    public class OutPutFileReader
    {
        static public int HeaderSize = 260;
        private FileStream _binaryFile;
        private BinaryReader _reader;
        private long _inputFileSize;
        public  List<int> _indexes = new List<int>();
        public  List<int> _timeStamps = new List<int>();
        public  List<float> _adcVoltages = new List<float>();
        public  List<float> _temperatures = new List<float>();
        public  OutputFileHeader _header = new OutputFileHeader();

        public void init(string InputFile)
        {
            int i;
            _binaryFile = new FileStream(InputFile, FileMode.Open, FileAccess.Read);
            _reader = new BinaryReader(_binaryFile);
            _inputFileSize = _reader.BaseStream.Length;
            //Read file header
            _header.read(_reader);

            //Read block headers
            for (i = 0; i < _header.blockCount; i++)
            {
                int offset = HeaderSize + i * (int)_header.blockSize;
                _binaryFile.Seek(offset, SeekOrigin.Begin);
                _indexes.Add(_reader.ReadInt32());
                _timeStamps.Add(_reader.ReadInt32());
                _adcVoltages.Add(_reader.ReadSingle());
                _temperatures.Add(_reader.ReadSingle());
            }
        }
        public OutPutFileReader()
        {

        }
        public OutPutFileReader(string InputFile)
        {
            init(InputFile);
        }
        public Block4 ReadBlock(int iBock)
        {
            if (iBock >= _header.blockCount)
                return null;
            int offset = HeaderSize + iBock * (int)_header.blockSize;
            _binaryFile.Seek(offset, SeekOrigin.Begin);
            var block = new Block4(_header.channelsInBlock, _header.functionsInBlock);
            block.read(_reader, false);
            return block;    
        }

        public List<float[]> GetCalcResult(int channelNo, int functionNo)
        {
            int iBlock;
            if (channelNo >= OutputFileHeader.ChannelMaxCount ||
                functionNo >= OutputFileHeader.FunctionMaxCount)
                return null;
            if (_header.channels[channelNo] == 0 || _header.functions[functionNo] == 0)
                return null;
            int offset = 0;
            var buff = new byte[_header.constants.fftResultSize * 4];
            var data = new float[Block4.FftResultSize];
            var list = new List<float[]>();
            int idxChn = _header.getChannelIndex(channelNo);
            int idxFn = _header.getFunctionIndex(functionNo);
            for(iBlock = 0; iBlock < _header.blockCount; iBlock++)
            {
                offset = HeaderSize + Block4.BlockHeaderSize + (iBlock * (int)_header.blockSize);
                offset = offset + _header.channelsInBlock * 8 * 3;
                offset = offset + (idxChn * _header.functionsInBlock + idxFn) * (Block4.FftResultSize * 4);
                _binaryFile.Seek(offset, SeekOrigin.Begin);
                _binaryFile.Read(buff, 0, buff.Length);
                Buffer.BlockCopy(buff, 0, data, 0, buff.Length);
                list.Add(data);
            }
            return list;
        }

        public List<Block4> GetBlockHeaders()
        {
            int offset = 0, iBlock;
            var list = new List<Block4>();
            for (iBlock = 0; iBlock < _header.blockCount; iBlock++)
            {
                offset = HeaderSize + (iBlock * (int)_header.blockSize);
                _binaryFile.Seek(offset, SeekOrigin.Begin);
                var block = new Block4(_header.channelsInBlock, _header.functionsInBlock);
                block.read(_reader, true);
                list.Add(block);
            }
            return list;
        }

        public List<double> GetChannelVoltage(int ChannelNo)
        {
            if (ChannelNo >= OutputFileHeader.ChannelMaxCount)
                return null;
            if (_header.channels[ChannelNo] == 0)
                return null;
            int idx = _header.getChannelIndex(ChannelNo);
            var headers = GetBlockHeaders();
            var res = new List<double>();
            foreach(var hdr in headers)
            {
                res.Add(hdr.voltMins[idx]);
                res.Add(hdr.voltMaxs[idx]);
                res.Add(hdr.voltAvgs[idx]);
            }
            return res;
        }
    }
}

