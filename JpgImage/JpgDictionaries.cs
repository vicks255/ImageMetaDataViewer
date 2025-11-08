using System.Text;

namespace JpgImage
{
    public static class JpgDictionaries
    {
        // Information from:
        // https://gist.github.com/RavuAlHemio/82959fb698790781c08716b22496e9fe
        public static Dictionary<byte, string> DictionaryOfMarkers => new Dictionary<byte, string>
        {
            { 0x01, "TEM" },  // temporary value used during arithmetic coding

            { 0xC0, "SOF0" }, // Start-of-Frame, non-differential, Huffman, baseline DCT
            { 0xC1, "SOF1" },  // Start-of-Frame, non-differential, Huffman, extended sequential DCT
            { 0xC2, "SOF2" },  // Start-of-Frame, non-differential, Huffman, progressive DCT
            { 0xC3, "SOF3" },  // Start-of-Frame, non-differential, Huffman, lossless (sequential)
            { 0xC4, "DHT" },   // Define Huffman Tables
            { 0xC5, "SOF5" },  // Start-of-Frame, differential, Huffman, sequential DCT
            { 0xC6, "SOF6" },  // Start-of-Frame, differential, Huffman, progressive DCT
            { 0xC7, "SOF7" },  // Start-of-Frame, differential, Huffman, lossless (sequential)
            { 0xC9, "SOF9" },  // Start-of-Frame, non-differential, arithmetic, extended sequential DCT
            { 0xCA, "SOF10" }, // Start-of-Frame, non-differential, arithmetic, progressive DCT
            { 0xCB, "SOF11" }, // Start-of-Frame, non-differential, arithmetic, lossless (sequential)
            { 0xCC, "DAC" },   // Define Arithmetic Coding conditionings
            { 0xCD, "SOF13" }, // Start-of-Frame, differential, arithmetic, sequential DCT
            { 0xCE, "SOF14" }, // Start-of-Frame, differential, arithmetic, progressive DCT
            { 0xCF, "SOF15" }, // Start-of-Frame, differential, arithmetic, lossless (sequential)

            { 0xD8, "SOI" }, // Start of Image
            { 0xD9, "EOI" }, // End of Image
            { 0xDA, "SOS" }, // Start of Scan (image data)
            { 0xDB, "DQT" }, // Define Quantization Tables
            { 0xDC, "DNL" }, // Define Number of Lines
            { 0xDD, "DRI" }, // Define Restart Interval
            { 0xDE, "DHP" }, // Define Hierarchical Progression
            { 0xDF, "EXP" }, // EXPand reference components

            { 0xE0, "APP0" },
            { 0xE1, "APP1" },
            { 0xE2, "APP2" },
            { 0xE3, "APP3" },
            { 0xE4, "APP4" },
            { 0xE5, "APP5" },
            { 0xE6, "APP6" },
            { 0xE7, "APP7" },
            { 0xE8, "APP8" },
            { 0xE9, "APP9" },
            { 0xEA, "APP10" },
            { 0xEB, "APP11" },
            { 0xEC, "APP12" },
            { 0xED, "APP13" },
            { 0xEE, "APP14" },
            { 0xEF, "APP15" },

            { 0xF0, "VER" }, // VERsion
            { 0xF1, "DTI" }, // Define Tiled Image
            { 0xF2, "DTT" }, // Define Tile
            { 0xF3, "SRF" }, // Selectively Refined Frame
            { 0xF4, "SRS" }, // Selectively Refined Scan
            { 0xF5, "DCR" }, // Define Component Registration
            { 0xF6, "DQS" }, // Define Quantizer Scale selection
            { 0xFE, "COM" }  // COMment
        };



        // Information from:        // https://www.media.mit.edu/pia/Research/deepview/exif.html
        public static Dictionary<string, string> DictionaryOfExifTags => new Dictionary<string, string>
        {
            { "0x00FE", "NewSubfileType" },
            { "0x00FF", "SubFileType" },

            { "0x0100", "ImageWidth" },
            { "0x0101", "ImageHeight" },
            { "0x0102", "BitsPerSample" },
            { "0x0103", "Compression" },
            { "0x0106", "PhotometricInterpretation" },
            { "0x010E", "ImageDescription" },
            { "0x010F", "Make" },
            { "0x0110", "Model" },
            { "0x0111", "StripOffsets" },
            { "0x0112", "Orientation" },
            { "0x0115", "SamplesPerPixel" },
            { "0x0116", "RowsPerStrip" },
            { "0x0117", "StripByteCounts" },
            { "0x011A", "XResolution" },
            { "0x011B", "YResolution" },
            { "0x011C", "PlanarConfiguration" },
            { "0x0128", "ResolutionUnit" },
            { "0x012D", "TransferFunction" },
            { "0x0131", "Software" },
            { "0x0132", "DateTime" },
            { "0x013B", "Artist" },
            { "0x013D", "Predictor" },
            { "0x013E", "WhitePoint" },
            { "0x013F", "PrimaryChromaticities" },
            { "0x0142", "TileWidth" },
            { "0x0143", "TileLength" },
            { "0x0144", "TileOffsets" },
            { "0x0145", "TileByteCounts" },
            { "0x014A", "SubIFDs" },
            { "0x015B", "JPEGTables" },

            { "0x0201", "JpegIFOffset" },
            { "0x0202", "JpegIFByteCount" },
            { "0x0211", "YCbCrCoefficients" },
            { "0x0212", "YCbCrSubSampling" },
            { "0x0213", "YCbCrPositioning" },
            { "0x0214", "ReferenceBlackWhite" },

            { "0x828D", "CFARepatePatternDim" },
            { "0x828E", "CFAPattern" },
            { "0x828F", "BatteryLevel" },
            { "0x8298", "Copyright" },
            { "0x829A", "ExposureTime" },
            { "0x829D", "FNumber" },

            { "0x83BB", "IPTC/NAA" },

            { "0x8769", "ExifOffset" },
            { "0x8773", "InterColorProfile" },

            { "0x8822", "ExposureProgram" },
            { "0x8824", "SpectralSensitivity" },
            { "0x8825", "GPSInfo" },
            { "0x8827", "ISOSpeedRatings" },
            { "0x8828", "OECF" },
            { "0x8829", "Interlace" },
            { "0x882A", "TimeZoneOffset" },
            { "0x882B", "SelfTimerMode" },

            { "0x9000", "ExifVersion" },
            { "0x9003", "DateTimeOriginal" },
            { "0x9004", "DateTimeDigitized" },

            { "0x9101", "ComponentConfiguration" },
            { "0x9102", "CompressedBitsPerPixel" },

            { "0x9201", "ShutterSpeedValue" },
            { "0x9202", "ApertureValue" },
            { "0x9203", "BrightnessValue" },
            { "0x9204", "ExposureBiasValue" },
            { "0x9205", "MaxApertureValue" },
            { "0x9206", "SubjectDistance" },
            { "0x9207", "MeteringMode" },
            { "0x9208", "LightSource" },
            { "0x9209", "Flash" },
            { "0x920A", "FocalLength" },
            { "0x920B", "FlashEnergy" },
            { "0x920C", "SpatialFrequencyResponse" },
            { "0x920D", "Noise" },
            { "0x9211", "ImageNumber" },
            { "0x9212", "SecurityClassification" },
            { "0x9213", "ImageHistory" },
            { "0x9214", "SubjectLocation" },
            { "0x9215", "ExposureIndex" },
            { "0x9216", "TIFF/EPStandardID" },
            { "0x927C", "MakerNote" },
            { "0x9286", "UserComment" },
            { "0x9290", "SubSecTime" },
            { "0x9291", "SubSecTimeOriginal" },
            { "0x9292", "SubSecTimeDigitized" },

            { "0xA000", "FlashPixVersion" },
            { "0xA001", "ColorSpace" },
            { "0xA002", "ExifImageWidth" },
            { "0xA003", "ExifImageHeight" },
            { "0xA004", "RelatedSoundFile" },
            { "0xA005", "ExifInteroperabilityOffset" },

            { "0xA20B", "FlashEnergy" },
            { "0xA20C", "SpatialFrequencyResponse" },
            { "0xA20E", "FocalPlaneXResolution" },
            { "0xA20F", "FocalPlaneYResolution" },
            { "0xA210", "FocalPlaneResolutionUnit" },
            { "0xA214", "SubjectLocation" },
            { "0xA215", "ExposureIndex" },
            { "0xA217", "SensingMethod" },

            { "0xA300", "FileSource" },
            { "0xA301", "SceneType" },
            { "0xA302", "CFAPatter" }
        };


        // https://www.media.mit.edu/pia/Research/deepview/exif.html
        public static Dictionary<int, string> DictionaryOfFormatDescriptions = new Dictionary<int, string>
        {
            { 1, "ubyte" },
            { 2, "ASCII" },
            { 3, "ushort" },
            { 4, "ulong" },
            { 5, "urational" },
            { 6, "byte" },
            { 7, "undefined" },
            { 8, "short" },
            { 9, "long" },
            { 10, "rational" },
            { 11, "float" },
            { 12, "double" }
        };


        // https://www.media.mit.edu/pia/Research/deepview/exif.html
        public static Dictionary<int, int> DictionaryOfFormatLengths = new Dictionary<int, int>
        {
            { 1, 1 },
            { 2, 1 },
            { 3, 2 },
            { 4, 4 },
            { 5, 8 },
            { 6, 1 },
            { 7, 1 },
            { 8, 2 },
            { 9, 4 },
            { 10, 8 },
            { 11, 4 },
            { 12, 8 }
        };


        // https://www.media.mit.edu/pia/Research/deepview/exif.html
        public static string GetDataValueString(int format, byte[] dataBytes, bool isLittleEndian)
        {
            string data = "";
            int index = 0;
            switch (format)
            {
                case 2: // ASCII
                    data = Encoding.ASCII.GetString(dataBytes);
                    break;


                case 3: // Unsigned Short
                    for (index = 0; index < dataBytes.Length; index += 2)
                    {
                        uint uShortValue = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToUInt16([dataBytes[index], dataBytes[index + 1]]) :
                                                                                           BitConverter.ToUInt16([dataBytes[index + 1], dataBytes[index]]);
                        data += $"{uShortValue} ";
                    }
                    break;


                case 4: // Unsigned Long
                    for (index = 0; index < dataBytes.Length; index += 4)
                    {
                        byte[] uLongBytes = [dataBytes[index], dataBytes[index + 1], dataBytes[index + 2], dataBytes[index + 3]];

                        uint uLongValue = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToUInt32(uLongBytes) :
                                                                                          BitConverter.ToUInt32(uLongBytes.Reverse().ToArray());
                        data += $"{uLongValue} ";
                    }
                    break;


                case 5: // Unsigned Rational
                    for (index = 0; index < dataBytes.Length; index += 8)
                    {
                        byte[] uNumeratorBytes = [dataBytes[index], dataBytes[index + 1], dataBytes[index + 2], dataBytes[index + 3]];
                        byte[] uDenomintatorBytes = [dataBytes[index + 4], dataBytes[index + 5], dataBytes[index + 6], dataBytes[index + 7]];

                        uint uNumerator = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToUInt32(uNumeratorBytes) :
                                                                                         BitConverter.ToUInt32(uNumeratorBytes.Reverse().ToArray());

                        uint uDenominator = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToUInt32(uDenomintatorBytes) :
                                                                                            BitConverter.ToUInt32(uDenomintatorBytes.Reverse().ToArray());

                        data += $"{uNumerator}/{uDenominator} ";
                    }
                    break;


                case 8: // Short
                    for (index = 0; index < dataBytes.Length; index += 2)
                    {
                        int shortValue = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt16([dataBytes[index], dataBytes[index + 1]]) :
                                                                                         BitConverter.ToInt16([dataBytes[index + 1], dataBytes[index]]);
                        data += $"{shortValue} ";
                    }
                    break;


                case 9: // Long
                    for (index = 0; index < dataBytes.Length; index += 4)
                    {
                        byte[] longBytes = [dataBytes[index], dataBytes[index + 1], dataBytes[index + 2], dataBytes[index + 3]];

                        int longValue = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt32(longBytes) :
                                                                                        BitConverter.ToInt32(longBytes.Reverse().ToArray());
                        data += $"{longValue} ";
                    }
                    break;


                case 10: // Rational
                    for (index = 0; index < dataBytes.Length; index += 8)
                    {
                        byte[] numeratorBytes = [dataBytes[index], dataBytes[index + 1], dataBytes[index + 2], dataBytes[index + 3]];
                        byte[] denomintatorBytes = [dataBytes[index + 4], dataBytes[index + 5], dataBytes[index + 6], dataBytes[index + 7]];

                        int numerator = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt32(numeratorBytes) :
                                                                                        BitConverter.ToInt32(numeratorBytes.Reverse().ToArray());

                        int denominator = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToInt32(denomintatorBytes) :
                                                                                          BitConverter.ToInt32(denomintatorBytes.Reverse().ToArray());

                        data += $"{numerator}/{denominator} ";
                    }
                    break;


                case 11: // Float
                    for (index = 0; index < dataBytes.Length; index += 4)
                    {
                        byte[] floatBytes = [dataBytes[index], dataBytes[index + 1], dataBytes[index + 2], dataBytes[index + 3]];

                        float floatValue = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToSingle(floatBytes) :
                                                                                           BitConverter.ToSingle(floatBytes.Reverse().ToArray());
                        data += $"{floatValue} ";
                    }
                    break;


                case 12: // Double
                    for (index = 0; index < dataBytes.Length; index += 4)
                    {
                        byte[] doubleBytes = [dataBytes[index], dataBytes[index + 1], dataBytes[index + 2], dataBytes[index + 3]];

                        double doubleValue = isLittleEndian == BitConverter.IsLittleEndian ? BitConverter.ToDouble(doubleBytes) :
                                                                                             BitConverter.ToDouble(doubleBytes.Reverse().ToArray());
                        data += $"{doubleValue} ";
                    }
                    break;


                default: // Display the bytes as hex
                    for (index = 0; index < dataBytes.Length; index++)
                    {
                        data += $"{dataBytes[index].ToString("X2")} ";
                    }
                    break;

            }

            return data;
        }
    }
}