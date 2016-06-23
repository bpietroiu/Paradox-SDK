using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    public class VideoFileList
    {
        public ArrayList videoFiles;

        public VideoFileList()
        {
            videoFiles = new ArrayList();
        }

        ~VideoFileList()
        {
            videoFiles.Clear();
        }

        public VideoFileList fullCopy()
        {
            VideoFileList cloned = new VideoFileList();
            for (int i = 0; i < videoFiles.Count; i++)
                cloned.videoFiles.Add(((VideoFileList)videoFiles[i]).fullCopy());
            return cloned;
        }

        public VideoFile this[UInt32 index]
        {
            get
            {
                if (index < videoFiles.Count)
                {
                    return (VideoFile)videoFiles[(int)index];
                }
                else
                    return null;
            }
            set
            {
                if (index < videoFiles.Count)
                {
                    VideoFile videoFile = (VideoFile)videoFiles[(int)index];

                    videoFile = value;
                }
            }
        }

        public void Clear()
        {
            videoFiles.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            VideoFile videoFile = null;

            for (int i = 0; i < videoFiles.Count; i++)
            {
                videoFile = (VideoFile)videoFiles[i];
                videoFile.Name = string.Format("File{0}", i + 1);
                videoFile.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal bool parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        videoFiles.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TVideoFileXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    VideoFile obj = new VideoFile();
                                    obj.parseXML(sobject);
                                    videoFiles.Add(obj);
                                }
                            }
                        }

                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}