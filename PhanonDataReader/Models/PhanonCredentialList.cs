using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PhanonDataReader.Models {
    //https://docs.microsoft.com/en-us/dotnet/api/system.net.icredentials?view=net-5.0
    public class PhanonCredentialList : ICredentials {

        class CredInfo {
            public Uri uriObj;
            public String authenType;
            public NetworkCredential netCredObj;

            public CredInfo(Uri uriO, string authType, NetworkCredential credO) {
                this.uriObj = uriO;
                this.authenType = authType;
                this.netCredObj = credO;
            }
        }

        private List<CredInfo> aList;

        public PhanonCredentialList() {
            aList = new List<CredInfo>();
        }

        public void Add( Uri myUri, string aType, NetworkCredential nc) {
            aList.Add(new CredInfo(myUri, aType, nc));
        }

        public void Remove(Uri myUri, string aType, NetworkCredential nc) {
            for (int ndx = 0; ndx < aList.Count; ndx++) {
                CredInfo ci = aList[ndx];
                if (myUri.Equals(ci.uriObj) && aType.Equals(ci.authenType)) {
                    aList.RemoveAt(ndx);
                }
            }
        }

        public NetworkCredential GetCredential (Uri myUri,string aType) {
            NetworkCredential retCred = null;
            for (int ndx = 0; ndx < aList.Count; ndx++) {
                CredInfo ci = aList[ndx];
                if (myUri.Equals(ci.uriObj) && aType.Equals(ci.authenType)) {
                    retCred = ci.netCredObj;
                }
            }
            return retCred;
        }

    }
}
