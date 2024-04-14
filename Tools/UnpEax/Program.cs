// This is a modified version of UnpEax that only extracts the AppX manifest and Store Assets. It is also adapted to C# 5,
// in order to be compatible with Visual Studio 2012+

// Original version: (c) 2020 LioneL Christopher Chetty

// MIT License

// Copyright (c) 2020 LioneL Christopher Chetty

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using System.Xml;

namespace UnpEax
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "";
            if (args.Length > 0)
            {
                path = args[0];
            }
            else
            {
                Console.WriteLine("\n unpeax <eappx/emsix file>\n");
                return;
            }
            using (var mmap = MemoryMappedFile.CreateFromFile(path, FileMode.Open))
            {
                Extract(mmap, Path.ChangeExtension(path, null), "");
            }
        }

        static Stream ReadData(MemoryMappedFile mmap, long offset, long count, bool unzip = false)
        {
            var strm = (count > 0) ?
                mmap.CreateViewStream(offset, count) as Stream :
                new MemoryStream(0);
            return unzip ?
                new DeflateStream(strm, CompressionMode.Decompress) as Stream :
                strm;
        }

        static void WriteFile(string dir, string path, Stream stream, bool encrypted = false)
        {
            Console.Write(" Extracting: {0}", path);
            if (encrypted)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" [Encrypted]");
                Console.ResetColor();
            }
            Console.WriteLine();

            var fullpath = Path.Combine(dir, path);
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
            using (var file = File.Create(fullpath))
            {
                stream.CopyTo(file);
            }
            stream.Dispose(); // BEWARE
        }

        class Part
        {
            public int id;
            public short zipped;
            public short flags;
            public long pos;
            public long len_orig;
            public long len;
            public string path;
            public bool isPackage;
        }

        static void Extract(MemoryMappedFile mmap, string root_dir, string dir, long offset = 0)
        {
			bool is_bundle = false;
			var header_view_0 = mmap.CreateViewAccessor(offset, 6);
            ushort header_size;
			using (header_view_0)
			{
				var magic = header_view_0.ReadInt32(0);
				if (magic == 0x48425845) // EXBH
				{
					is_bundle = true;
				}
				else if ((magic != 0x48505845)  // EXPH
					&& (magic != 0x48535845))   // EXSH ??
				{
					Console.WriteLine("\n invalid file\n");
					return;
				}
			    header_size = header_view_0.ReadUInt16(4);
			}

            using (var header_view = mmap.CreateViewAccessor(offset, header_size))
            {
                long pos = 6;
                Func<Int64> ReadInt64 = () => { var ret = header_view.ReadInt64(pos); pos += 8; return ret; };
                Func<Int32> ReadInt32 = () => { var ret = header_view.ReadInt32(pos); pos += 4; return ret; };
                Func<Int16> ReadInt16 = () => { var ret = header_view.ReadInt16(pos); pos += 2; return ret; };
                Func<int, byte[]> ReadBytes = count =>
                {
                    var data = new byte[count];
                    header_view.ReadArray(pos, data, 0, count);
                    pos += count;
                    return data;
                };
                Func<int, string> ReadString = count =>
                {
                    return System.Text.Encoding.Unicode.GetString(ReadBytes(count));
                };

                var file_version = ReadInt64();

                var footer_offset = ReadInt64();
                var footer_length = ReadInt64();
                var footer_count = ReadInt64();

                var sig_offset = ReadInt64();
                var sig_zipped = ReadInt16();
                var sig_length_orig = ReadInt32();
                var sig_length = ReadInt32();

                var coin_offset = ReadInt64();
                var coin_zipped = ReadInt16();
                var coin_length_origin = ReadInt32();
                var coin_length = ReadInt32();

                var v7 = ReadInt64();

                var v8 = ReadInt32();
                var outerCount = ReadInt16();
                for (int i = 0; i < outerCount; ++i)
                {
                    var data = ReadBytes(32);
                }

                var v10 = ReadInt16();
                var packname_len = ReadInt16();
                var packname = ReadString(packname_len);

                var algo_len = ReadInt16();
                var algo = ReadString(algo_len);

                var v11 = ReadInt16();
                var hash_method_len = ReadInt16();
                var hash_method = ReadString(hash_method_len);

                var v15_len = ReadInt16();
                var v15 = ReadBytes(v15_len);

                bool end = pos == header_size;

                if ((sig_offset != 0) && (sig_length) != 0)
                {
                    WriteFile(root_dir, Path.Combine(dir, "AppxSignature.p7x"),
                        ReadData(mmap, sig_offset, sig_length, true)
                    );
                }

                if ((coin_offset != 0) && (coin_length) != 0)
                {
                    WriteFile(root_dir, Path.Combine(dir, "AppxMetadata\\CodeIntegrity.cat"),
                        ReadData(mmap, coin_offset, coin_length, true)
                    );
                }

                var parts = new List<Part>();
                using (var parts_acc = mmap.CreateViewAccessor(
                    offset + footer_offset, footer_length
                ))
                {
                    for (var i = 0; i < (parts_acc.Capacity / 40); ++i)
                    {
                        long p_offset = i * 40;
                        parts.Add(new Part()
                        {
                            id = parts_acc.ReadInt32(p_offset + 8),
                            flags = parts_acc.ReadInt16(p_offset + 4),
                            zipped = parts_acc.ReadInt16(p_offset + 6),
                            pos = parts_acc.ReadInt64(p_offset + 16),
                            len_orig = parts_acc.ReadInt64(p_offset + 24),
                            len = parts_acc.ReadInt64(p_offset + 32),
                            path = "part" + i.ToString() + ".dat",
                            isPackage = false,
                        });
                    }

                    if (parts.Count > 0)
                    {
                        var bmap_part = parts[parts.Count - 1]; // Sure ?
                        bmap_part.path = "AppxBlockMap.xml";
                        using (var strm = ReadData(mmap, offset + bmap_part.pos,
                            bmap_part.len, bmap_part.zipped == 1))
                        {
                            var xml = new XmlDocument();
                            xml.Load(strm);
                            foreach (XmlElement elem in xml.DocumentElement.ChildNodes)
                            {
                                var id_str = elem.GetAttribute("Id");
                                var name = elem.GetAttribute("Name");
                                if (!(string.IsNullOrEmpty(id_str) || string.IsNullOrEmpty(name)))
                                {
                                    var id = int.Parse(id_str, System.Globalization.NumberStyles.HexNumber);
                                    var part = parts.Find(p => p.id == id);
                                    if (part is object)
                                    {
                                        part.path = name;
                                    }
                                }
                            }
                        }
                    }
                }

                if (is_bundle)
                {
                    var bman_part = parts.Find(p => p.path == "AppxMetadata\\AppxBundleManifest.xml");
                    if (bman_part is object)
                    {
                        using (var strm = ReadData(mmap,
                            offset + bman_part.pos, bman_part.len,
                            bman_part.zipped == 1
                        ))
                        {
                            var xml = new XmlDocument();
                            xml.Load(strm);
                            foreach (XmlElement elem in xml.DocumentElement.ChildNodes)
                            {
                                if (elem.Name == "Packages")
                                {
                                    foreach (XmlElement elem_ in elem.ChildNodes)
                                    {
                                        var offset_str = elem_.GetAttribute("Offset");
                                        var filename = elem_.GetAttribute("FileName");
                                        if (!(string.IsNullOrEmpty(offset_str) || string.IsNullOrEmpty(filename)))
                                        {
                                            var offset_ = int.Parse(offset_str);
                                            var part = parts.Find(p => p.pos == offset_);
                                            if (part is object)
                                            {
                                                part.path = filename;
                                                part.isPackage = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                foreach (var part in parts)
                {
                    if (part.isPackage)
                    {
                        Extract(mmap, root_dir, Path.Combine(dir, Path.ChangeExtension(part.path, null)), part.pos);
                    }
                    else
                    {
                        if ((part.path.Equals("AppxManifest.xml")) || (part.path.StartsWith("Assets")))
                        {
                            WriteFile(root_dir, Path.Combine(dir, part.path),
                                ReadData(mmap, offset + part.pos, part.len, part.zipped == 1),
                                part.flags == 0);
                        }
                        else
                        {
                            Console.WriteLine("Skipping file {0}", part.path);
                        }
                    }
                }
            }
        }
    }
}
