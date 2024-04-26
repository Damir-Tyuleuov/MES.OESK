using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispetcherRes.SqlLayer;

namespace MES.OESK.Class
{
    public  class Appeals : SqlLayer
    {
       

        static List<Appeals> appealsList;
        public List<Appeals> AppealsList
        {
            get { return appealsList; }
            set { appealsList = value; }
        }

        string id;
        public string ID
        {
            get { return id; }
            set {id = value ; }
        }

        DateTime date;
        public DateTime Datetime
        {
            get { return date; }
            set { date = value; }
        }

        string slujbaID;
        public string SlujbaID
        {
            get { return slujbaID; }
            set {slujbaID = value; }
        }

        string slujbaName;
        public string SlujbaName
        {
            get { return slujbaName; }
            set { slujbaName = value; }
        }


        string authorID;
        public string AuthorID
        {
            get { return authorID; }
            set { authorID = value; }
        }

        string authorName;
        public string AuthorName
        {
            get { return authorName; }
            set { authorName = value; }
        }


        string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        string whomsSlujbaID;
        public string WhomsSlujbaID
        {
            get { return whomsSlujbaID; }
            set { whomsSlujbaID = value; }
        }

        string whomsSlujbaName;
        public string WhomsSlujbaName
        {
            get { return whomsSlujbaName; }
            set { whomsSlujbaName = value; }
        }

        string whomsEmployeeID;
        public string WhomsEmployeeID
        {
            get { return whomsEmployeeID; }
            set { whomsEmployeeID = value; }
        }

        string whomsEmployeeName;
        public string WhomsEmployeeName
        {
            get { return whomsEmployeeName; }
            set { whomsEmployeeName = value; }
        }

        byte stateRead;
        public byte StateRead
        {
            get { return stateRead; }
            set { stateRead = value; }
        }

    }
}
