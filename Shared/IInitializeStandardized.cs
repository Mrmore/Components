using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Renren.Components.Tools
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IInitializeStandardized<T>
        where T : IInitializeStandardized<T>, new()
    {
        static private T _singleton = default(T);
        static private readonly SemaphoreSlim _singleLock = new SemaphoreSlim(1);
        static private readonly IDictionary<string, T> _instances =
            new Dictionary<string, T>();

        public static async Task<T> CreateInitedInstance(string key)
        {
            return await Task.Run<T>(async () =>
            {
                T instance = default(T);
                _singleLock.Wait();
                try
                {
                    if (_instances.ContainsKey(key))
                        instance = _instances[key];
                    else
                    {
                        instance =
                           (T)Activator.CreateInstance(typeof(T));
                        await instance.Initialize();
                        _instances.Add(key, instance);
                        return instance;
                    }
                }
                catch (Exception ex)
                {
                    _instances.Remove(key);
                    instance = null;

                    Debug.WriteLine("Stardard initialization failed during invoking your intialization process with Key = " +  key);
                    Debug.WriteLine("The initialized type = " + typeof(T).FullName);
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    _singleLock.Release();
                }

                return instance;
            });
        }

        public static async Task<T> CreateInitedInstance(bool isSingleton = true)
        {
            return await Task.Run<T>(async () =>
            {
                if (!isSingleton)
                {
                    T instance =
                        (T)Activator.CreateInstance(typeof(T));
                    await instance.Initialize();
                    return instance;
                }
                else
                {
                    if (_singleton == default(T))
                    {
                        _singleLock.Wait();
                        try
                        {
                            if (_singleton == default(T))
                            {
                                _singleton =
                                    (T)Activator.CreateInstance(typeof(T));
                                await _singleton.Initialize();
                            }
                        }
                        catch (Exception ex)
                        {
                            _singleton = null;

                            Debug.WriteLine("Stardard initialization failed during invoking your intialization process!");
                            Debug.WriteLine("The initialized type = " + typeof(T).FullName);
                            Debug.WriteLine(ex.ToString());
                        }
                        finally
                        {
                            _singleLock.Release();
                        }
                    }
                    return _singleton;
                }
            });
        }

        public abstract Task Initialize();
    }

    public abstract class IIInitializeStandardized<T, TInitArg>
        where T : IIInitializeStandardized<T, TInitArg>, new()
    {
        static private T _singleton =  default(T);
        static private SemaphoreSlim _syncLock = new SemaphoreSlim(1);
        static private readonly IDictionary<string, T> _instances =
            new Dictionary<string, T>();

        public static async Task<T> CreateInitedInstance(TInitArg arg, string key)
        {
            return await Task.Run<T>(async () =>
            {
                T instance = default(T);
                _syncLock.Wait();
                try
                {
                    if (_instances.ContainsKey(key))
                        instance = _instances[key];
                    else
                    {
                        instance =
                           (T)Activator.CreateInstance(typeof(T));
                        await instance.Initialize(arg);
                        _instances.Add(key, instance);
                        return instance;
                    }
                }
                catch (Exception ex)
                {
                    _instances.Remove(key);
                    instance = null;

                    Debug.WriteLine("Stardard initialization failed during invoking your intialization process with Key = " + key);
                    Debug.WriteLine("The initialized type = " + typeof(T).FullName);
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    _syncLock.Release();
                }

                return instance;
            });
        }

        public async static Task<T> CreateInitedInstance(TInitArg arg, bool isSingleton = true)
        {
            return await Task.Run<T>(async () =>
            {
                if (!isSingleton)
                {
                    T instance =
                        (T)Activator.CreateInstance(typeof(T));
                    await instance.Initialize(arg);
                    return instance;
                }
                else
                {
                    if (_singleton == default(T))
                    {
                        _syncLock.Wait();
                        try
                        {
                            if (_singleton == default(T))
                            {
                                _singleton =
                                    (T)Activator.CreateInstance(typeof(T));
                                await _singleton.Initialize(arg);
                            }
                        }
                        catch (Exception ex)
                        {
                            _singleton = null;

                            Debug.WriteLine("Stardard initialization failed during invoking your intialization process!");
                            Debug.WriteLine("The initialized type = " + typeof(T).FullName);
                            Debug.WriteLine(ex.ToString());
                        }
                        finally
                        {
                            _syncLock.Release();
                        }
                    }
                    return _singleton;
                }
            });
        }

        public abstract Task Initialize(TInitArg arg);
    }
}
