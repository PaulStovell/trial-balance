using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XLinq;

namespace PaulStovell.TrialBalance.DomainModel.PackageDataProvider {
    /// <summary>
    /// An adapter for converting <see cref="T:Transaction">Transactions</see> into <see cref="T:XElement">XElements</see>, and vice-versa.
    /// </summary>
    public class TransactionXElementAdapter {
        /// <summary>
        /// Converts a given <see cref="Transaction"/> into an <see cref="T:XElement"/>.
        /// </summary>
        /// <param name="transaction">The <see cref="Transaction"/> to convert.</param>
        /// <returns>The newly created <see cref="T:XElement"/>.</returns>
        public XElement ToXElement(Transaction transaction) {
            XElement element = new XElement("Transaction");
            element.Add(new XElement("DebitAccountID", transaction.DebitAccount.AccountID));
            element.Add(new XElement("CreditAccountID", transaction.CreditAccount.AccountID));
            element.Add(new XElement("CreatedByUsername", transaction.CreatedByUsername));
            element.Add(new XElement("CreatedDate", transaction.CreatedDate));
            element.Add(new XElement("Date", transaction.Date));
            element.Add(new XElement("Particulars", transaction.Particulars));
            element.Add(new XElement("TransactionID", transaction.TransactionID));
            element.Add(new XElement("Value", transaction.Value));
            element.Add(new XElement("UpdatedByUsername", transaction.UpdatedByUsername));
            element.Add(new XElement("UpdatedDate", transaction.UpdatedDate));

            // Append the change history
            ChangeSetHistoryXElementAdapter adapter = new ChangeSetHistoryXElementAdapter();
            element.Add(adapter.ToXElement(transaction.ChangeSetHistory));

            return element;
        }

        /// <summary>
        /// Converts a given <see cref="T:XElement"/> into a <see cref="T:Transaction"/>.
        /// </summary>
        /// <param name="element">The <see cref="T:XElement"/> to convert.</param>
        /// <param name="workbook">The <see cref="T:Workbook"/> to use to fetch additional information for the <see cref="Transaction"/>.</param>
        /// <returns>The newly created <see cref="T:Transaction"/>.</returns>
        public Transaction FromXElement(XElement element, Workbook workbook) {
            Transaction transaction = new Transaction();
            foreach (XElement childNode in element.Nodes()) {
                switch (childNode.Name.LocalName) {
                    case "DebitAccountID":
                        transaction.DebitAccount = workbook.FetchAccount(new Guid(childNode.Value));
                        break;
                    case "CreditAccountID":
                        transaction.CreditAccount = workbook.FetchAccount(new Guid(childNode.Value));
                        break;
                    case "CreatedByUsername":
                        transaction.CreatedByUsername = childNode.Value;
                        break;
                    case "CreatedDate":
                        transaction.CreatedDate = DateTime.Parse(childNode.Value);
                        break;
                    case "Date":
                        transaction.Date = DateTime.Parse(childNode.Value);
                        break;
                    case "Particulars":
                        transaction.Particulars = childNode.Value;
                        break;
                    case "TransactionID":
                        transaction.TransactionID = new Guid(childNode.Value);
                        break;
                    case "Value":
                        transaction.Value = decimal.Parse(childNode.Value);
                        break;
                    case "UpdatedByUsername":
                        transaction.UpdatedByUsername = childNode.Value;
                        break;
                    case "UpdatedDate":
                        transaction.UpdatedDate = DateTime.Parse(childNode.Value);
                        break;
                    case "ChangeSetHistory":
                        ChangeSetHistoryXElementAdapter adapter = new ChangeSetHistoryXElementAdapter();
                        transaction.ChangeSetHistory = adapter.FromXElement(childNode);
                        break;
                }
            }
            return transaction;
        }
    }
}
