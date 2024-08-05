using System.Xml.Serialization;

namespace ServerSignalR.Models
{
    [XmlRoot("QueuingInfo")]
    public class QueuingInfo
    {
        [XmlElement("WindowInfo")]
        public List<ScreenInfo> screenInfoList;
    }
}