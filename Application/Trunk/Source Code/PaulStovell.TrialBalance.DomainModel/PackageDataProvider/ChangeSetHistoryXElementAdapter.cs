using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XLinq;

namespace PaulStovell.TrialBalance.DomainModel.PackageDataProvider {
    /// <summary>
    /// An adapter for converting <see cref="ChangeSetHistory">ChangeSetHistory</see> records into <see cref="T:XElement">XElements</see>, and vice-versa.
    /// </summary>
    public class ChangeSetHistoryXElementAdapter {
        /// <summary>
        /// Converts a given <see cref="ChangeSetHistory"/> into an <see cref="T:XElement"/>.
        /// </summary>
        /// <param name="changeSetHistory">The <see cref="ChangeSetHistory"/> to convert.</param>
        /// <returns>The newly created <see cref="T:XElement"/>.</returns>
        public XElement ToXElement(ChangeSetHistory changeSetHistory) {
            XElement historyElement = new XElement("ChangeSetHistory");
            foreach (ChangeSet changeSet in changeSetHistory) {
                XElement changeSetElement = new XElement("ChangeSet",
                    new XElement("Applied", changeSet.Applied),
                    new XElement("Username", changeSet.Username));
                foreach (Change change in changeSet.Changes) {
                    XElement changeElement = new XElement("Change",
                        new XElement("PropertyName", change.PropertyName),
                        new XElement("OldValue", change.OldValue),
                        new XElement("NewValue", change.NewValue));
                    changeSetElement.Add(changeElement);
                }
                historyElement.Add(changeSetElement);
            }
            return historyElement;
        }

        /// <summary>
        /// Converts a given <see cref="T:XElement"/> into a <see cref="T:ChangeSetHistory"/>.
        /// </summary>
        /// <param name="element">The <see cref="T:XElement"/> to convert.</param>
        /// <returns>The newly created <see cref="T:ChangeSetHistory"/>.</returns>
        public ChangeSetHistory FromXElement(XElement element) {
            ChangeSetHistory history = new ChangeSetHistory();
            foreach (XElement changeSetElement in element.Nodes()) {
                ChangeSet changeSet = new ChangeSet();

                // Get the change set details
                foreach (XElement changeSetElementChild in changeSetElement.Nodes()) {
                    if (changeSetElementChild.Name.LocalName == "Applied") {
                        changeSet.Applied = DateTime.Parse(changeSetElementChild.Value);
                    } else if (changeSetElementChild.Name.LocalName == "Username") {
                        changeSet.Username = changeSetElementChild.Value;
                    } else if (changeSetElementChild.Name.LocalName == "Change") {
                        Change change = new Change();
                        foreach (XElement changeElement in changeSetElementChild.Nodes()) {
                            if (changeElement.Name.LocalName == "PropertyName") {
                                change.PropertyName = changeElement.Value;
                            } else if (changeElement.Name.LocalName == "OldValue") {
                                change.OldValue = changeElement.Value;
                            } else if (changeElement.Name.LocalName == "NewValue") {
                                change.NewValue = changeElement.Value;
                            }
                        }
                        changeSet.Changes.Add(change);
                    }
                }
                history.Append(changeSet);
            }
            return history;
        }
    }
}
