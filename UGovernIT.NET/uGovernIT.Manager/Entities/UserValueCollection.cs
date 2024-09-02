using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class UserValueCollection : ICollection<UserValue>
    {
        private List<UserValue> userValues;

        public UserValueCollection(ApplicationContext context, string values, bool includeDetail = false)
        {
            userValues = new List<UserValue>();
            List<string> lists = UGITUtility.ConvertStringToList(values, Constants.Separator6);
            lists.ForEach(x => {
                userValues.Add(new UserValue(context,x)); 
                   });

            if (includeDetail)
            {
                List<UserProfile> userCollection = context.UserManager.GetUserInfosById(values);
                if (userCollection != null && userCollection.Count() > 0)
                {
                    foreach (UserProfile usrVal in userCollection)
                    {
                        lists.Add(usrVal.UserName);
                    }
                }

            }
        }

        public void Add(UserValue item)
        {
            userValues.Add(item);
        }

        public void Clear()
        {
            userValues.Clear();
        }

        public bool Contains(UserValue item)
        {
            return userValues.Exists(x => x.ID == item.ID);
        }

        public void CopyTo(UserValue[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex + 1)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < userValues.Count; i++)
            {
                array[i + arrayIndex] = userValues[i];
            }
        }

        public int Count
        {
            get { return userValues.Count; }
        }

        public bool IsReadOnly
        {
            get {
                return false;  
            }
        } 

        public bool Remove(UserValue item)
        {
            return userValues.Remove(item);
        }

        public IEnumerator<UserValue> GetEnumerator()
        {
            return userValues.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return userValues.GetEnumerator();
        }
    }
}
