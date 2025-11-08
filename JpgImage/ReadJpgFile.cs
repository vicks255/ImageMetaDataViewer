using System.IO;
using System.Text;

namespace JpgImage
{
    public class ReadJpgFile
    {
        /******************************************************************************************
         *  Byte order: there are two places where byte order (i.e. little or big endian) needs to
         *  be checked in order to converted to an integer correctly, the byte order in the JPEG
         *  file and the byte order of the processor.  To determine if the array needs to be
         *  reversed or not prior to converting is a simple matter checking to see if the processor
         *  and the file share the same byte order.
         *  
         *  If the byte order of the file and the process are the same convert the bytes.
         *  If not, reverse the byte array and then convert.
         *  
         *  Strategy for retrieving the meta-data from the JPEG file (i.e. sub-problems)
         *      1) Break the file into a list of its segments
         *      2) Scan the segmets for EXIF data
         *      3) Separate the TIFF portion of the EXIF data
         *      4) Retrieve the IDF tables from the TIFF
         *      5) Use the IDF tables to retrieve the meta-data from the TIFF
         ******************************************************************************************/


        public List<byte[]> GetSegments(string fileName)
        {
            byte[] jpgFileBytes = File.ReadAllBytes(fileName);
            List<byte[]> listOfSegments = new List<byte[]>();

            int index = 0;
            while(index < jpgFileBytes.Length - 1)
            {
                // since all markers are 2-bytes consisting of FFXX, I only need to look for 0xFF and then check the next byte
                // to see if it is a segment marker or not.
                if (jpgFileBytes[index] == 0xff && JpgDictionaries.DictionaryOfMarkers.ContainsKey(jpgFileBytes[index + 1]))
                {
                    string marker = JpgDictionaries.DictionaryOfMarkers[jpgFileBytes[index + 1]];

                    // The 2 byte segment marker is being stored in the segment data so that it can be identified in the list
                    // This means that 0-length segment will be 2-bytes.
                    int segmentLength = 2;
                    if(marker != "TEM" && marker != "SOI" && marker != "EOI")
                    {
                        segmentLength = BitConverter.IsLittleEndian  ?  BitConverter.ToUInt16([jpgFileBytes[index + 3], jpgFileBytes[index + 2]]) + 2 :
                                                                        BitConverter.ToUInt16([jpgFileBytes[index + 2], jpgFileBytes[index + 3]]) + 2;
                    }
                        
                    // The first two bytes of each segment in the list is the marker
                    byte[] segment = new byte[segmentLength];
                    segment[0] = jpgFileBytes[index];
                    segment[1] = jpgFileBytes[index + 1];

                    // Copy the bytes from the file into the segment and add it to the list.
                    for(int i = 0; i < segmentLength; i++)
                    {
                        segment[i] = jpgFileBytes[index + i];
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


        private List<byte[]> GetIfdTableEntries(bool isLittleEndian, byte[] tiffData)
        {
            /************************************ TABLE FORMAT ************************************
             *   Table Size:            2-byte integer
             *   Table Entries:         12-bytes each
             *   Pointer to Next Table: 4-byte integer
             *************************************************************************************/

            // get the pointer to the first IFD table (IFD0)
            byte[] nextIfdTableBytes =
            [
                tiffData[4],
                tiffData[5],
                tiffData[6],
                tiffData[7]
            ];
            int offset = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt32(nextIfdTableBytes) :
                                                                         BitConverter.ToInt32(nextIfdTableBytes.Reverse().ToArray());


            // loop through the IFD tables, each table ends with a pointer to the next table
            // a null pointer indicates that you have reached the last table.
            int indexOfNextDirectory = offset;
            List<byte[]> listOfIfdTableEntries = new List<byte[]>();
            while (indexOfNextDirectory != 0)
            {
                // first 2 bytes are an integer containing the number of entries in the table
                byte[] tableSizeBytes = [tiffData[offset], tiffData[offset + 1]];
                int tableSize = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt16(tableSizeBytes) :
                                                                                BitConverter.ToInt16(tableSizeBytes.Reverse().ToArray());

                // each entry in the table contains 12 bytes
                offset += 2;
                for (int i = 0; i < tableSize; i++)
                {
                    byte[] tableEntry = new byte[12];
                    for (int x = 0; x < 12; x++)
                    {
                        tableEntry[x] = tiffData[offset + x];
                    }
                    listOfIfdTableEntries.Add(tableEntry);
                    offset += 12;
                }

                // grab the pointer to the next table, a null pointer indicates that this is the last table in the segment.
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

            return listOfIfdTableEntries;
        }


        public List<IfdData> GetMetaData(byte[] exifSegment)
        {
            // check if this is an EXIF segment, return and empty list of not
            List<IfdData> listOfIfds = new List<IfdData>();
            if (Encoding.ASCII.GetString(exifSegment, 4, 4) != "Exif")
                return listOfIfds;

            // Extracting the TIFF portion of the segment
            byte[] tiffData = new byte[exifSegment.Length - 10];
            for(int i = 0; i < tiffData.Length; i++)
            {
                tiffData[i] = exifSegment[10 + i];
            }

            // II = little-endian, MM = big-endian
            bool isLittleEndian = Encoding.ASCII.GetString(tiffData, 0, 2) == "II";
            List<byte[]> listOfIfdTableEntries = GetIfdTableEntries(isLittleEndian, tiffData);

            // Once a complete list of table entries is generated, loop through them in order to grab the meta-data.
            foreach (byte[] entry in listOfIfdTableEntries)
            {
                byte[] tagBytes = [entry[0], entry[1]];
                byte[] formatBytes = [entry[2], entry[3]];
                byte[] componentsBytes = [entry[4], entry[5], entry[6], entry[7]];
                byte[] dataBytes = [entry[8], entry[9], entry[10], entry[11]];
                byte[] dataValueBytes;

                // Check the dictionary of tags, if the tag name is found use that, if not, display as hex
                string tagName = $"0x{Convert.ToHexString(tagBytes)}";
                if (JpgDictionaries.DictionaryOfExifTags.ContainsKey(tagName))
                    tagName = JpgDictionaries.DictionaryOfExifTags[tagName];

                int format = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt16(formatBytes) :
                                                                             BitConverter.ToInt16(formatBytes.Reverse().ToArray());

                int components = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt16(componentsBytes) :
                                                                                 BitConverter.ToInt16(componentsBytes.Reverse().ToArray());

                // if the dataSize < 4, the dataBytes are the data
                // otherwise the dataBytes are a pointer to the data.
                int dataSize = components * JpgDictionaries.DictionaryOfFormatLengths[format];
                if (dataSize < 4)
                {
                    dataValueBytes = [dataBytes[0], dataBytes[1], dataBytes[2], dataBytes[3]];
                }
                else
                {
                    int dataPointer = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt32(dataBytes) :
                                                                                      BitConverter.ToInt32(dataBytes.Reverse().ToArray());

                    dataValueBytes = new byte[dataSize];
                    for(int i = 0; i < dataSize; i++)
                    {
                        dataValueBytes[i] = tiffData[dataPointer + i];
                    }
                }

                // Add the meta-data to the list
                listOfIfds.Add(new IfdData
                {
                    DataType = tagName,
                    DataValue = JpgDictionaries.GetDataValueString(format, dataValueBytes, isLittleEndian)
                });
            }

            return listOfIfds;
        }
    }
}