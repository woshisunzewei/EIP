using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace EIP.Common.Core.Utils
{
    /// <summary> 
    /// 适用与ZIP压缩 
    /// </summary> 
    public class ZipUtil
    {
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        /// <param name="password">密码</param>
        public static void ZipFile(string fileToZip,
            string zipedFile,
            int compressionLevel,
            int blockSize,
            string password = "")
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream zipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream zipStream = new ZipOutputStream(zipFile))
                {
                    if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                    using (
                        FileStream streamToZip = new FileStream(fileToZip, FileMode.Open,
                            FileAccess.Read))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\", StringComparison.Ordinal) + 1);

                        ZipEntry zipEntry = new ZipEntry(fileName);

                        zipStream.PutNextEntry(zipEntry);

                        zipStream.SetLevel(compressionLevel);

                        byte[] buffer = new byte[blockSize];

                        int sizeRead;
                        do
                        {
                            sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, sizeRead);
                        } while (sizeRead > 0);

                        streamToZip.Close();
                    }

                    zipStream.Finish();
                    zipStream.Close();
                }

                zipFile.Close();
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名</param>
        /// <param name="password">密码</param>
        public static void ZipFile(string fileToZip,
            string zipedFile,
            string password = "")
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream fs = File.OpenRead(fileToZip))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                using (FileStream zipFile = File.Create(zipedFile))
                {
                    using (ZipOutputStream zipStream = new ZipOutputStream(zipFile))
                    {
                        if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                        ZipEntry zipEntry = new ZipEntry(fileName);
                        zipStream.PutNextEntry(zipEntry);
                        zipStream.SetLevel(5);

                        zipStream.Write(buffer, 0, buffer.Length);
                        zipStream.Finish();
                        zipStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="password"></param>
        public static void ZipFileDirectory(string strDirectory, string zipedFile, string password = "")
        {
            using (FileStream zipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream zipStream = new ZipOutputStream(zipFile))
                {
                    if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                    ZipSetp(strDirectory, zipStream, "");
                }
            }
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="s">The ZipOutputStream Object.</param>
        /// <param name="parentPath">The parent path.</param>
        private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            foreach (string file in filenames) // 遍历所有的文件和目录
            {

                if (Directory.Exists(file)) // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                    pPath += "\\";
                    ZipSetp(file, s, pPath);
                }

                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    using (FileStream fs = File.OpenRead(file))
                    {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        string fileName = parentPath + file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                        ZipEntry entry = new ZipEntry(fileName);
                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;

                        fs.Close();

                        crc.Reset();
                        crc.Update(buffer);

                        entry.Crc = crc.Value;
                        s.PutNextEntry(entry);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="strDirectory">The STR directory.</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        public void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {

            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    var pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);

                    if (fileName != "")
                    {
                        if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) ||
                            (!File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                            {
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    var size = s.Read(data, 0, data.Length);

                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                s.Close();
            }
        }
    }
}