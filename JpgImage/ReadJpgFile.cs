using System.IO;
using System.Text;

namespace JpgImage
{
    public class ReadJpgFile
    {
        public byte[] JpgFileBytes { get; set; }

        public ushort GetSegmentLength(byte[] segmentLength)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToUInt16([segmentLength[1], segmentLength[0]]);
            else
                return BitConverter.ToUInt16(segmentLength);
        }


        public static bool IsSegmentExif(byte[] segment)
        {
            if (Encoding.ASCII.GetString(segment, 4, 4) == "Exif")
                return true;
            return false;
        }


        public List<byte[]> GetSegments(string fileName)
        {
            JpgFileBytes = File.ReadAllBytes(fileName);
            List<byte[]> listOfSegments = new List<byte[]>();

            int index = 0;
            while(index < JpgFileBytes.Length - 1)
            {
                if (JpgFileBytes[index] == 0xff && JpgDictionaries.DictionaryOfMarkers.ContainsKey(JpgFileBytes[index + 1]))
                {
                    string marker = JpgDictionaries.DictionaryOfMarkers[JpgFileBytes[index + 1]];

                    ushort segmentLength = 2;
                    if(marker != "TEM" && marker != "SOI" && marker != "EOI")
                        segmentLength += GetSegmentLength([JpgFileBytes[index + 2], JpgFileBytes[index + 3]]);

                    byte[] segment = new byte[segmentLength];
                    segment[0] = JpgFileBytes[index];
                    segment[1] = JpgFileBytes[index + 1];

                    for(int i = 0; i < segmentLength; i++)
                    {
                        segment[i] = JpgFileBytes[index + i];
                    }
                    listOfSegments.Add(segment);

                    index += segmentLength;
                }
                else
                {
                    index++;
                }
            }
            return listOfSegments;
        }



        public List<IfdData> GetExifData(byte[] exifSegment)
        {
            List<IfdData> listOfIfds = new List<IfdData>();
            if (IsSegmentExif(exifSegment) == false)
                return listOfIfds;

            byte[] tiffData = new byte[exifSegment.Length - 10];
            for(int i = 0; i < tiffData.Length; i++)
            {
                tiffData[i] = exifSegment[10 + i];
            }


            bool isLittleEndian = Encoding.ASCII.GetString(tiffData, 0, 2) == "II";
            byte[] nextIfdTableBytes =
            [
                tiffData[4],
                tiffData[5],
                tiffData[6],
                tiffData[7]
            ];
            int offset = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt32(nextIfdTableBytes) :
                                                                         BitConverter.ToInt32(nextIfdTableBytes.Reverse().ToArray());

            int indexOfNextDirectory = offset;
            List<byte[]> listOfIfdTableEntries = new List<byte[]>();
            while (indexOfNextDirectory != 0)
            {
                byte[] tableSizeBytes = [tiffData[offset], tiffData[offset + 1]];
                int tableSize = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt16(tableSizeBytes) :
                                                                                BitConverter.ToInt16(tableSizeBytes.Reverse().ToArray());

                offset += 2;
                for(int i = 0; i < tableSize; i++)
                {
                    byte[] tableEntry = new byte[12];
                    for(int x = 0; x < 12; x++)
                    {
                        tableEntry[x] = tiffData[offset + x];
                    }
                    listOfIfdTableEntries.Add(tableEntry);
                    offset += 12;
                }

                nextIfdTableBytes =
                [
                    tiffData[offset],
                    tiffData[offset + 1],
                    tiffData[offset + 2],
                    tiffData[offset + 3]
                ];
                indexOfNextDirectory = offset = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt32(nextIfdTableBytes) :
                                                                                                BitConverter.ToInt32(nextIfdTableBytes.Reverse().ToArray());
            }


            foreach (byte[] entry in listOfIfdTableEntries)
            {
                byte[] tagBytes = [entry[0], entry[1]];
                byte[] formatBytes = [entry[2], entry[3]];
                byte[] componentsBytes = [entry[4], entry[5], entry[6], entry[7]];
                byte[] dataPointerBytes = [entry[8], entry[9], entry[10], entry[11]];
                byte[] dataBytes;

                string tagName = $"0x{Convert.ToHexString(tagBytes)}";
                if (JpgDictionaries.DictionaryOfExifTags.ContainsKey(tagName))
                    tagName = JpgDictionaries.DictionaryOfExifTags[tagName];

                int format = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt16(formatBytes) :
                                                                             BitConverter.ToInt16(formatBytes.Reverse().ToArray());

                int components = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt16(componentsBytes) :
                                                                                 BitConverter.ToInt16(componentsBytes.Reverse().ToArray());

                int dataSize = components * JpgDictionaries.DictionaryOfFormatLengths[format];
                if (dataSize < 4)
                {
                    dataBytes = [dataPointerBytes[0], dataPointerBytes[1], dataPointerBytes[2], dataPointerBytes[3]];
                }
                else
                {
                    int dataPointer = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt32(dataPointerBytes) :
                                                                                      BitConverter.ToInt32(dataPointerBytes.Reverse().ToArray());

                    dataBytes = new byte[dataSize];
                    for(int i = 0; i < dataSize; i++)
                    {
                        dataBytes[i] = tiffData[dataPointer + i];
                    }
                }

                listOfIfds.Add(new IfdData
                {
                    DataType = tagName,
                    DataValue = JpgDictionaries.GetDataValueString(format, dataBytes, isLittleEndian)
                });
            }

            return listOfIfds;
        }
    }
}