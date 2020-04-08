/*
 * Copyright (c) 2000, 2020, Oracle and/or its affiliates.
 *
 * Licensed under the Universal Permissive License v 1.0 as shown at
 * http://oss.oracle.com/licenses/upl.
 */
using System.Collections;
using System.IO;

using Tangosol.IO.Pof;
using Tangosol.Net.Cache;
using Tangosol.Util.Processor;

namespace Tangosol.Util.Filter
{
    /// <summary>
    /// <see cref="IFilter"/> which returns <b>true</b> for
    /// <see cref="IInvocableCacheEntry"/> objects that currently exist
    /// in an <see cref="ICache"/>.
    /// </summary>
    /// <remarks>
    /// This filter is intended to be used solely in combination with a
    ///  <see cref="ConditionalProcessor"/> and is unnecessary for standard
    /// <see cref="IQueryCache"/> operations.
    /// </remarks>
    /// <author>Jason Howes  2005.12.16</author>
    /// <author>Goran Milosavljevic  2006.10.24</author>
    /// <author>Tom Beerbower  2009.03.10</author>
    /// <seealso cref="IInvocableCacheEntry.IsPresent" />
    public class PresentFilter: IEntryFilter, IPortableObject
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PresentFilter()
        {}

        #endregion

        #region IFilter implementation

        /// <summary>
        /// Apply the test to the object.
        /// </summary>
        /// <param name="o">
        /// An object to which the test is applied.
        /// </param>
        /// <returns>
        /// <b>true</b> if the test passes, <b>false</b> otherwise.
        /// </returns>
        public virtual bool Evaluate(object o)
        {
            return true;
        }

        #endregion

        #region IEntryFilter implementation

        /// <summary>
        /// Apply the test to an <see cref="ICacheEntry"/>.
        /// </summary>
        /// <param name="entry">
        /// The <b>ICacheEntry</b> to evaluate; never <c>null</c>.
        /// </param>
        /// <returns>
        /// <b>true</b> if the test passes, <b>false</b> otherwise.
        /// </returns>
        public virtual bool EvaluateEntry(ICacheEntry entry)
        {
            return !(entry is IInvocableCacheEntry) || 
                ((IInvocableCacheEntry) entry).IsPresent;
        }

        #endregion

        #region IIndexAwareFilter implementation

        /// <summary>
        /// Given an IDictionary of available indexes, determine if this 
        /// IIndexAwareFilter can use any of the indexes to assist in its 
        /// processing, and if so, determine how effective the use of that 
        /// index would be.
        /// </summary>
        /// <remarks>
        /// <p>
        /// The returned value is an effectiveness estimate of how well this 
        /// filter can use the specified indexes to filter the specified 
        /// keys. An operation that requires no more than a single access to 
        /// the index content (i.e. Equals, NotEquals) has an effectiveness of 
        /// <b>one</b>. Evaluation of a single entry is assumed to have an 
        /// effectiveness that depends on the index implementation and is 
        /// usually measured as a constant number of the single operations.  
        /// This number is referred to as <i>evaluation cost</i>.
        /// </p>
        /// <p>
        /// If the effectiveness of a filter evaluates to a number larger 
        /// than the keys.size() then a user could avoid using the index and 
        /// iterate through the keys calling <tt>Evaluate</tt> rather than 
        /// <tt>ApplyIndex</tt>.
        /// </p>
        /// </remarks>
        /// <param name="indexes">
        /// The available <see cref="ICacheIndex"/> objects keyed by the 
        /// related IValueExtractor; read-only.
        /// </param>
        /// <param name="keys">
        /// The set of keys that will be filtered; read-only.
        /// </param>
        /// <returns>
        /// An effectiveness estimate of how well this filter can use the 
        /// specified indexes to filter the specified keys.
        /// </returns>
        public int CalculateEffectiveness(IDictionary indexes, ICollection keys)
        {
            return 1;
        }

        /// <summary>
        /// Filter remaining keys using an IDictionary of available indexes.
        /// </summary>
        /// <remarks>
        /// The filter is responsible for removing all keys from the passed 
        /// set of keys that the applicable indexes can prove should be 
        /// filtered. If the filter does not fully evaluate the remaining 
        /// keys using just the index information, it must return a filter
        /// (which may be an <see cref="IEntryFilter"/>) that can complete the 
        /// task using an iterating implementation. If, on the other hand, the
        /// filter does fully evaluate the remaining keys using just the index
        /// information, then it should return <c>null</c> to indicate that no 
        /// further filtering is necessary.
        /// </remarks>
        /// <param name="indexes">
        /// The available <see cref="ICacheIndex"/> objects keyed by the 
        /// related IValueExtractor; read-only.
        /// </param>
        /// <param name="keys">
        /// The mutable set of keys that remain to be filtered.
        /// </param>
        /// <returns>
        /// An <see cref="IFilter"/> object that can be used to process the 
        /// remaining keys, or <c>null</c> if no additional filter processing 
        /// is necessary.
        /// </returns>
        public IFilter ApplyIndex(IDictionary indexes, ICollection keys)
        {
            return null;
        }

        #endregion

        #region Object override methods

        /// <summary>
        /// Compare the <b>PresentFilter</b> with another object to
        /// determine equality.
        /// </summary>
        /// <param name="o">
        /// The <b>PresentFilter</b> to compare to.
        /// </param>
        /// <returns>
        /// <b>true</b> if this <b>PresentFilter</b> and the passed object
        /// are equivalent <b>PresentFilter</b> objects.
        /// </returns>
        public override bool Equals(object o)
        {
            return o is PresentFilter;
        }

        /// <summary>
        /// Determine a hash value for the <b>PresentFilter</b> object
        /// according to the general <b>object.GetHashCode()</b> contract.
        /// </summary>
        /// <returns>
        /// An integer hash value for this <b>PresentFilter</b> object.
        /// </returns>
        public override int GetHashCode()
        {
            return 0xEF;
        }

        /// <summary>
        /// Return a human-readable description for this
        /// <b>PresentFilter</b>.
        /// </summary>
        /// <returns>
        /// A string description of the <b>PresentFilter</b>.
        /// </returns>
        public override string ToString()
        {
            return "PresentFilter";
        }

        #endregion

        #region IPortableObject implementation

        /// <summary>
        /// Restore the contents of a user type instance by reading its state
        /// using the specified <see cref="IPofReader"/> object.
        /// </summary>
        /// <param name="reader">
        /// The <b>IPofReader</b> from which to read the object's state.
        /// </param>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        public virtual void ReadExternal(IPofReader reader)
        {}

        /// <summary>
        /// Save the contents of a POF user type instance by writing its
        /// state using the specified <see cref="IPofWriter"/> object.
        /// </summary>
        /// <param name="writer">
        /// The <b>IPofWriter</b> to which to write the object's state.
        /// </param>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        public virtual void WriteExternal(IPofWriter writer)
        {}

        #endregion

        #region Constants

        /// <summary>
        /// An instance of the PresentFilter.
        /// </summary>
        public static readonly PresentFilter Instance = new PresentFilter();

        #endregion
    }
}