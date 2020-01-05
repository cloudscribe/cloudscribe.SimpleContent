using cloudscribe.MetaWeblog.Models;

namespace cloudscribe.MetaWeblog
{
    public static class MetaWeblogResultExtensions
    {
        public static MetaWeblogResult AddSecurityFault(this MetaWeblogResult result)
        {
            var faultStruct = new FaultStruct();
            faultStruct.faultCode = "11"; // invalid access
            faultStruct.faultString = "Authentication Failed";
            result.Fault = faultStruct;

            return result;
        }

        public static MetaWeblogResult AddValidatonFault(this MetaWeblogResult result)
        {
            var faultStruct = new FaultStruct();
            faultStruct.faultCode = "802"; // invalid access
            faultStruct.faultString = "invalid request";
            result.Fault = faultStruct;

            return result;
        }
    }
}
