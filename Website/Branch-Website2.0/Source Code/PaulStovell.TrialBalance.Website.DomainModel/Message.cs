using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PaulStovell.TrialBalance.Website.DomainModel {
    [XmlRoot("Message")]
    public class Message {
        private string _messageBody;
        private string _messageTitle;
        private DateTime _dateCreated;
        private int _dispatchAttemptCount;

        public Message() {
            _dateCreated = DateTime.Now;
        }

        public string MessageBody {
            get { return _messageBody ?? string.Empty; }
            set { _messageBody = value; }
        }

        public string MessageTitle {
            get { return _messageTitle ?? string.Empty; }
            set { _messageTitle = value; }
        }

        public DateTime DateCreated {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        public int DispatchAttemptCount {
            get { return _dispatchAttemptCount; }
            set { _dispatchAttemptCount = value; }
        }
    }
}
