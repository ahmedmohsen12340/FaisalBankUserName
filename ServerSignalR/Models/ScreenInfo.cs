using System.Xml.Serialization;

namespace ServerSignalR.Models
{
    public class ScreenInfo
    {
        [XmlElement("WindowName")]
        public string WindowName { get; set; }
        [XmlElement("WindowNumber")]
        public int WindowNumber { get; set; }
        [XmlElement("SlipNumber")]
        public string SlipNumber { get; set; }
        [XmlElement("CallTime")]
        public string CallTime { get; set; }
        [XmlElement("IsOpened")]
        public string IsOpened { get; set; }
    }
}