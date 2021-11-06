using FreeRedis;
using System;

namespace Chinahoo.RedisLock
{
    public class LockFactory
    {
       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="cli"></param>
       /// <param name="key"></param>
       /// <param name="acquire"></param>
       /// <param name="timeoutSeconds"></param>
       /// <returns></returns>
        public static T CreateLock<T>(RedisClient cli, string key, Func<T> acquire,int timeoutSeconds=1)
        {
            try
            {
                var lockObj = cli.Lock(key, timeoutSeconds);
                if (lockObj is not null)
                {
                    try
                    {
                        var result = acquire();
                        if (result is not null)
                        {
                            lockObj.Unlock(); // 解锁
                            return result;
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                    catch
                    {
                        lockObj.Unlock(); // 解锁
                        return default(T);
                    }
                }
                else
                {
                    return default(T);
                }

            }
            catch
            {
                return default(T);
            }
        }
    }
}
