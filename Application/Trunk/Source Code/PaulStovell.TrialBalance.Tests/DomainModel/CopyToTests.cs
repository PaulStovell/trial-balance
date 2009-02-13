using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaulStovell.TrialBalance.DomainModel;
using System.Reflection;
using PaulStovell.Common.BindingFramework;

namespace PaulStovell.TrialBalance.Tests.DomainModel {
    [TestClass]
    public class CopyToTests {

        [TestMethod]
        public void AssertAllConcreteAccountsOverrideCopyTo() {
            string results = null;

            // Ensure that every single type of Account that we define has overridden the CopyTo method from bottom to top
            foreach (Type t in typeof(Account).Assembly.GetTypes()) {
                if (typeof(Account).IsAssignableFrom(t)) {

                    Type currentType = t;
                    while (currentType != null && currentType != typeof(DomainObject)) {

                        MethodInfo mi = currentType.GetMethod("CopyTo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        if (mi == null) {
                            results += "Type " + currentType.FullName + " has not overridden the CopyTo method." + Environment.NewLine;
                        }

                        currentType = currentType.BaseType;
                    }

                }
            }

            if (results != null) {
                Assert.Fail(results.Trim());
            }
        }


    }
}
