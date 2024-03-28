using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    public class clsUtility
    {
        public static string GenerateGUID()
        {
            Guid newGuid = Guid.NewGuid();

            return newGuid.ToString();
        }

        public static string ReplaceFileNameWithGUID(string SourceFile)
        {
            string FileName = SourceFile;

            FileInfo fileInfo = new FileInfo(FileName);

            string FileExtenation = fileInfo.Extension;

            return GenerateGUID() + FileExtenation;
        }

        public static bool CreateDestinationFolderIfNoExist(string DestinationFolder)
        {
            if (!Directory.Exists(DestinationFolder))
            {

                try
                {
                    Directory.CreateDirectory(DestinationFolder);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder" + ex.Message,
                                    "Error", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
            
        }
        public static bool CopySourceFileToImagesFolder(ref string SourceFile)
        {
            string DestinationFolder = "C:\\Users\\omerm\\Desktop\\OMS\\PROGRAMING\\C# Language\\Programming Advices\\DVLD_V2\\People-Images\\";

            if (!CreateDestinationFolderIfNoExist(DestinationFolder))
            {
                return false;
            }

            string DestinationFile = DestinationFolder + ReplaceFileNameWithGUID(SourceFile);

            try
            {
                File.Copy(SourceFile, DestinationFile, true);

            }
            catch (IOException ex)
            {
                MessageBox.Show("Error : " + ex.Message, 
                           "Error", 
                           MessageBoxButtons.OK,  
                           MessageBoxIcon.Error);


                return false;
            }

            SourceFile = DestinationFile;
            return true;
        }
    }
}
