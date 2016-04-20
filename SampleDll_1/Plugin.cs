using System;
using System.Diagnostics;

namespace SampleDll_1
{
    // All plugin class should derive from MarshalByRefObject.
    // In adddtion to that all (similar) Plugins should expose same set of methods
    // In our case, the interface is IMarshalObj, which exposes DoSomething method
    [Serializable]
    public class Plugin : MarshalByRefObject, InterfaceCommon.IMarshalObj
    {
        public string DoSomething()
        {
            return "Hello From Plugin";
        }
    }
}
