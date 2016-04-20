using System;

namespace CustomAppDomainWebSample
{
    public class PluginsActivator : IDisposable // Added IDisposable to provide ease to unload the appdomain
    {
        public PluginsActivator()
        {
            newDomain = null;
        }
        
        private AppDomain newDomain { get; set; }

        public void Dispose()
        {
            UnloadDomain();
        }

        public InterfaceCommon.IMarshalObj LoadAssembly(string path)
        {
            AppDomainSetup newDomainSetup = new AppDomainSetup()
            {
                PrivateBinPath = path
                //ShadowCopyFiles = "true" 
                // ShadowCopyFiles - Makes sure the dll is shadow copied and not locked 
                // and user can update the DLL without stopping the service
                // however if we are unloading the appdomain, then this should not be a problem
            };

            string domainName = System.IO.Path.GetFileNameWithoutExtension(path);
            newDomain = AppDomain.CreateDomain(domainName, null, newDomainSetup);

            // Lets try to keep name of all the plugin classes as "Plugin". That will provide ease of identifying the class implementation
            // And namespace of plugin should be kept same as the output dll
            // Each plugin should expose exactly one class
            var c = (InterfaceCommon.IMarshalObj)(newDomain.CreateInstanceFromAndUnwrap(path, string.Format("{0}.Plugin", domainName)));
            return c;
        }

        public void UnloadDomain()
        {
            if (newDomain != null)
            {
                AppDomain.Unload(newDomain);
                newDomain = null;
            }
        }
    }
}