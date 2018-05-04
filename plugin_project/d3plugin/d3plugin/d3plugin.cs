using System;
using System.Collections.Generic;
using System.Text;
using RepetierHostExtender.interfaces;
using RepetierHostExtender.geom;

namespace d3plugin
{
    public class d3plugin : IHostPlugin
    {
        handler handle = new handler();
        IHost host;
        /// <summary>
        /// Called first to allow filling some lists. Host is not fully set up at that moment.
        /// </summary>
        /// <param name="host"></param>
        public void PreInitalize(IHost _host)
        {
            host = _host;
        }
        /// <summary>
        /// Called after everything is initalized to finish parts, that rely on other initializations.
        /// Here you must create and register new Controls and Windows.
        /// </summary>
        public void PostInitialize()
        {
            // Add the handle to the right tab
            handle.Connect(host);
            host.RegisterHostComponent(handle);
            host.AboutDialog.RegisterThirdParty("D3Plugin", "\r\n\r\nD3Plugin");
        }
        /// <summary>
        /// Last round of plugin calls. All controls exist, so now you may modify them to your wishes.
        /// </summary>
        public void FinializeInitialize()
        {

        }
    }
}
