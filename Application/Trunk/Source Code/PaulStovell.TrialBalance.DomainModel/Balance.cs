using System;
using System.Globalization;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// A special type used to indicate an accounting balance.
    /// </summary>
    public class Balance : IFormattable {
        private decimal _magnitude;
        private BalanceType _balanceType;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Balance() {
            this.Magnitude = 0;
            this.BalanceType = BalanceType.Debit;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The value of the balance.</param>
        /// <param name="type">The type of balance.</param>
        public Balance(decimal value, BalanceType type) {
            this.Magnitude = value;
            this.BalanceType = type;
        }

        /// <summary>
        /// Gets the value component of the balance.
        /// </summary>
        public decimal Magnitude {
            get { return _magnitude; }
            protected set {
                if (_magnitude < 0) {
                    throw new ArgumentOutOfRangeException("value", value,
                                                          "The value of a balance cannot be less than 0.");
                }
                _magnitude = value;
            }
        }

        /// <summary>
        /// Gets the balance type
        /// </summary>
        public BalanceType BalanceType {
            get { return _balanceType; }
            protected set { _balanceType = value; }
        }

        /// <summary>
        /// Adds two balances.
        /// </summary>
        /// <param name="lhs">The left-hand side.</param>
        /// <param name="rhs">The right-hand side.</param>
        /// <returns>A resulting balance.</returns>
        public static Balance operator +(Balance lhs, Balance rhs) {
            Balance result = new Balance();

            if (lhs.BalanceType == rhs.BalanceType) {
                result.Magnitude = lhs.Magnitude + rhs.Magnitude;
                result.BalanceType = lhs.BalanceType;
            }
            else {
                if (lhs.Magnitude >= rhs.Magnitude) {
                    result.Magnitude = lhs.Magnitude - rhs.Magnitude;
                    result.BalanceType = lhs.BalanceType;
                }
                else {
                    result.Magnitude = rhs.Magnitude - lhs.Magnitude;
                    result.BalanceType = rhs.BalanceType;
                }
            }

            return result;
        }

        /// <summary>
        /// Subtracts two balances.
        /// </summary>
        /// <param name="lhs">The left-hand side.</param>
        /// <param name="rhs">The right-hand side.</param>
        /// <returns>A resulting balance.</returns>
        public static Balance operator -(Balance lhs, Balance rhs) {
            Balance result = new Balance();

            if (lhs.BalanceType == rhs.BalanceType) {
                result.Magnitude = lhs.Magnitude - rhs.Magnitude;
                result.BalanceType = lhs.BalanceType;
            }
            else {
                if (lhs.Magnitude >= rhs.Magnitude) {
                    result.Magnitude = lhs.Magnitude + rhs.Magnitude;
                    result.BalanceType = lhs.BalanceType;
                }
                else {
                    result.Magnitude = rhs.Magnitude - lhs.Magnitude;
                    result.BalanceType = rhs.BalanceType;
                }
            }

            return result;
        }

        /// <summary>
        /// Equals operator.
        /// </summary>
        public static bool operator ==(Balance lhs, Balance rhs) {
            if ((object)lhs == (object)rhs) {
                return true;
            }  else if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) {
                return false;
            } else return (lhs.Magnitude == rhs.Magnitude && lhs.BalanceType == rhs.BalanceType);
        }

        /// <summary>
        /// Not equals operator.
        /// </summary>
        public static bool operator !=(Balance lhs, Balance rhs) {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Checks if this object and another object are equal.
        /// </summary>
        /// <param name="obj">The other object to check.</param>
        /// <returns>True if they are equal, otherwise false.</returns>
        public override bool Equals(object obj) {
            return this == obj as Balance;
        }

        /// <summary>
        /// Gets a hash code for this balance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Gets a string representation of the current balance.
        /// </summary>
        /// <returns>A string representing the current balance.</returns>
        public override string ToString() {
            return this.ToString("c", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets a string representation of the current balance.
        /// </summary>
        /// <param name="format">A format specification.</param>
        /// <param name="formatProvider">An IFormatProvider that supplies culture specific formatting information.</param>
        /// <returns>A string representing the current balance.</returns>
        public string ToString(string format, IFormatProvider formatProvider) {
            string result = this.Magnitude.ToString(format, formatProvider);

            if (this.BalanceType == BalanceType.Credit) {
                result += " CR";
            }
            else {
                result += " DR";
            }

            return result;
        }

        /// <summary>
        /// Parses a Balance given a string and the type of balance expected.
        /// </summary>
        /// <param name="balanceText">The text to attempt to parse.</param>
        /// <returns>The parsed balance.</returns>
        /// <exception cref="FormatException" />
        public static Balance Parse(string balanceText) {
            return Parse(balanceText, null);
        }

        /// <summary>
        /// Parses a Balance given a string and the type of balance expected.
        /// </summary>
        /// <param name="balanceText">The text to attempt to parse.</param>
        /// <param name="expectedBalanceType">The type of balance expected. If null, 'CR' or 'DR' must be specified in the balance text.</param>
        /// <returns>The parsed balance.</returns>
        /// <exception cref="FormatException" />
        public static Balance Parse(string balanceText, BalanceType? expectedBalanceType) {
            Balance result = null;
            if (!TryParse(balanceText, expectedBalanceType, out result)) {
                throw new FormatException();
            }
            return result;
        }

        /// <summary>
        /// Attempts to parse a Balance given a string.
        /// </summary>
        /// <param name="balanceText">The text to attempt to parse.</param>
        /// <param name="resultBalance">If parsing is sucessful, this will contain the parsed balance.</param>
        /// <returns>True if the text could be parsed, otherwise false.</returns>
        public static bool TryParse(string balanceText, out Balance resultBalance) {
            return TryParse(balanceText, null, out resultBalance);
        }

        /// <summary>
        /// Attempts to parse a Balance given a string and the type of balance expected.
        /// </summary>
        /// <param name="balanceText">The text to attempt to parse.</param>
        /// <param name="expectedBalanceType">The type of balance expected. If null, 'CR' or 'DR' must be specified in the balance text.</param>
        /// <param name="resultBalance">If parsing is sucessful, this will contain the parsed balance.</param>
        /// <returns>True if the text could be parsed, otherwise false.</returns>
        public static bool TryParse(string balanceText, BalanceType? expectedBalanceType, out Balance resultBalance) {
            bool result = true;

            decimal magnitude = 0M;
            BalanceType type = BalanceType.Credit;

            balanceText = (balanceText ?? string.Empty).Trim().ToUpper();

            // What type of balance is it?
            if (balanceText.EndsWith("CR")) {
                type = BalanceType.Credit;
                balanceText = balanceText.Substring(0, balanceText.Length - 2);
            } else if (balanceText.EndsWith("DR")) {
                type = BalanceType.Debit;
                balanceText = balanceText.Substring(0, balanceText.Length - 2);
            } else {
                if (expectedBalanceType != null && expectedBalanceType.HasValue) {
                    type = expectedBalanceType.Value;
                } else {
                    result = false;
                }
            }

            // Get the value (magnitude) of the balance
            if (result == true) {
                result = decimal.TryParse(balanceText.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out magnitude);
                magnitude = Math.Abs(magnitude);
            }
            
            
            resultBalance = new Balance(magnitude, type);
            return result;
        }

        /// <summary>
        /// Credits this balance by a given amount, resulting in a new balance.
        /// </summary>
        /// <param name="value">The value to credit this balance.</param>
        /// <returns></returns>
        public Balance Credit(decimal value) {
            return this + new Balance(value, BalanceType.Credit);
        }

        /// <summary>
        /// Debits this balance by a given amount, resulting in a new balance.
        /// </summary>
        /// <param name="value">The value to debit this balance.</param>
        public Balance Debit(decimal value) {
            return this + new Balance(value, BalanceType.Debit);
        }
    }
}