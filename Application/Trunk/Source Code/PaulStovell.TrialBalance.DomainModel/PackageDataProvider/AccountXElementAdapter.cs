using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XLinq;

namespace PaulStovell.TrialBalance.DomainModel.PackageDataProvider {
    /// <summary>
    /// An adapter for converting <see cref="T:Account">Accounts</see> into <see cref="T:XElement">XElements</see>, and vice-versa.
    /// </summary>
    public class AccountXElementAdapter {
        /// <summary>
        /// Converts a given <see cref="Account"/> into an <see cref="T:XElement"/>.
        /// </summary>
        /// <param name="account">The <see cref="Account"/> to convert.</param>
        /// <returns>The newly created <see cref="T:XElement"/>.</returns>
        public XElement ToXElement(Account account) {
            XElement element = new XElement(account.AccountType.ToString());
            element.Add(new XElement("AccountID", account.AccountID));
            element.Add(new XElement("CreatedByUsername", account.CreatedByUsername));
            element.Add(new XElement("CreatedDate", account.CreatedDate));
            element.Add(new XElement("Description", account.Description));
            element.Add(new XElement("Name", account.Name));
            element.Add(new XElement("UpdatedByUsername", account.UpdatedByUsername));
            element.Add(new XElement("UpdatedDate", account.UpdatedDate));
            
            // Append the change history
            ChangeSetHistoryXElementAdapter adapter = new ChangeSetHistoryXElementAdapter();
            element.Add(adapter.ToXElement(account.ChangeSetHistory));

            return element;
        }

        /// <summary>
        /// Converts a given <see cref="T:XElement"/> into an <see cref="T:Account"/>.
        /// </summary>
        /// <param name="element">The <see cref="T:XElement"/> to convert.</param>
        /// <returns>The newly created <see cref="T:Account"/>.</returns>
        public Account FromXElement(XElement element) {
            Account account = AccountFactory.Create(element.Name.LocalName);
            foreach (XElement childNode in element.Nodes()) {
                switch (childNode.Name.LocalName) {
                    case "AccountID":
                        account.AccountID = new Guid(childNode.Value);
                        break;
                    case "CreatedByUsername":
                        account.CreatedByUsername = childNode.Value;
                        break;
                    case "CreatedDate":
                        account.CreatedDate = DateTime.Parse(childNode.Value);
                        break;
                    case "Description":
                        account.Description = childNode.Value;
                        break;
                    case "Name":
                        account.Name = childNode.Value;
                        break;
                    case "UpdatedByUsername":
                        account.UpdatedByUsername = childNode.Value;
                        break;
                    case "UpdatedDate":
                        account.UpdatedDate = DateTime.Parse(childNode.Value);
                        break;
                    case "ChangeSetHistory":
                        ChangeSetHistoryXElementAdapter adapter = new ChangeSetHistoryXElementAdapter();
                        account.ChangeSetHistory = adapter.FromXElement(childNode);
                        break;
                }
            }
            return account;
        }
    }
}
