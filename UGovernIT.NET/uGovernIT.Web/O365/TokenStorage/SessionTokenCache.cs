﻿/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System.Threading;
using System.Web;
using Microsoft.Identity.Client;

namespace uGovernIT.Web.TokenStorage
{

    // Store the user's token information.
    // Store the user's token information.
    public class SessionTokenCache
    {
        private static ReaderWriterLockSlim SessionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        string UserId = string.Empty;
        string CacheId = string.Empty;
        HttpContextBase httpContext = null;
        [System.Obsolete]
        TokenCache cache = new TokenCache();

        [System.Obsolete]
        public SessionTokenCache(string userId, HttpContextBase httpcontext)
        {
            // not object, we want the SUB
            UserId = userId;
            CacheId = UserId + "_TokenCache";
            httpContext = httpcontext;
            Load();
        }

        [System.Obsolete]
        public TokenCache GetMsalCacheInstance()
        {
            cache.SetBeforeAccess(BeforeAccessNotification);
            cache.SetAfterAccess(AfterAccessNotification);
            Load();
            return cache;
        }

        public void SaveUserStateValue(string state)
        {
            SessionLock.EnterWriteLock();
            httpContext.Session[CacheId + "_state"] = state;
            SessionLock.ExitWriteLock();
        }
        public string ReadUserStateValue()
        {
            string state = string.Empty;
            SessionLock.EnterReadLock();
            state = (string)httpContext.Session[CacheId + "_state"];
            SessionLock.ExitReadLock();
            return state;
        }

        [System.Obsolete]
        public void Load()
        {
            SessionLock.EnterReadLock();
            cache.Deserialize((byte[])httpContext.Session[CacheId]);
            SessionLock.ExitReadLock();
        }

        [System.Obsolete]
        public void Persist()
        {
            SessionLock.EnterWriteLock();

            // Optimistically set HasStateChanged to false. We need to do it early to avoid losing changes made by a concurrent thread.
            cache.HasStateChanged = false;

            // Reflect changes in the persistent store
            httpContext.Session[CacheId] = cache.Serialize();
            SessionLock.ExitWriteLock();
        }

        // Triggered right before MSAL needs to access the cache.
        // Reload the cache from the persistent store in case it changed since the last access.
        [System.Obsolete]
        void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            Load();
        }

        // Triggered right after MSAL accessed the cache.
        [System.Obsolete]
        void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (cache.HasStateChanged)
            {
                Persist();
            }
        }

    }
}