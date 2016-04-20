# CustomAppDomainWebSample

App Domain Usage Sample in ASP.NET

### Important files
1. **CustomAppDomainWebSample/Plugin/PluginManager.cs**
    
    Starts watcher on the plugin directory. Preloads the existing dlls in the directory.
2. **CustomAppDomainWebSample/Plugin/PluginsActivator.cs**
    
    Responsible for creating new app domain, adding assembly in app domain and creating instance of the plugin class
    
3. **CustomAppDomainWebSample/Global.asax.cs**

    Starts monitoring on the directory
    
4. **CustomAppDomainWebSample/Controllers/HomeController.cs** and **CustomAppDomainWebSample/Views/Home/Index.cshtml**

    Example usage of PluginsActivator class

5. **Interface/IMarshalObj.cs**

    Interface which all plugin classes should derive from
    
### Where to place the plugin dlls

Such as SampleDLL_1.dll and supporting dlls (in this example, only one supporting dll InterfaceCommon.dll), all should be placed in the DLLs folder or what ever folder specified in the PluginManager class (directoryToMonitor field).

All comments are present inline with the code.
