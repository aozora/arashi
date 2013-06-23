using System;
using System.Collections.Generic;
using System.Threading;

namespace xVal.RuleProviders
{
    public abstract class CachingRulesProvider : IRulesProvider
    {
        private Dictionary<Type, RuleSet> cachedRuleSets = new Dictionary<Type, RuleSet>();
        private readonly ReaderWriterLock readerWriterLock = new ReaderWriterLock();

        public RuleSet GetRulesFromType(Type type)
        {
            // Try to return an existing value
            readerWriterLock.AcquireReaderLock(Timeout.Infinite);
            try {
                RuleSet existingValue;
                if (cachedRuleSets.TryGetValue(type, out existingValue))
                    return existingValue;
            }
            finally {
                readerWriterLock.ReleaseReaderLock();
            }

            // Not found - prepare to insert a new value
            readerWriterLock.AcquireWriterLock(Timeout.Infinite);
            try {
                // Double-check locking - see if a new value was inserted while we waited for the writer lock
                RuleSet existingValue;
                if (cachedRuleSets.TryGetValue(type, out existingValue))
                    return existingValue;

                // No, it's time to create one
                cachedRuleSets[type] = GetRulesFromTypeCore(type);
                return cachedRuleSets[type];
            }
            finally {
                readerWriterLock.ReleaseWriterLock();
            }
        }

        protected abstract RuleSet GetRulesFromTypeCore(Type type);
    }
}