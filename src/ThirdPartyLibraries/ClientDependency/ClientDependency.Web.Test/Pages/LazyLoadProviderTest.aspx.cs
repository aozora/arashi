﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientDependency.Core.Controls;
using ClientDependency.Core.FileRegistration.Providers;
using ClientDependency.Core;

namespace ClientDependency.Web.Test.Pages
{
    public partial class LazyLoadProviderTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var http = new HttpContextWrapper(Context);

            //Changes the provider to be used at runtime
            ClientDependencyLoader.Instance(http).ProviderName = LazyLoadProvider.DefaultName;

            //dynamically register the dependency
            ClientDependencyLoader.Instance(http).RegisterDependency("Content.css", "Styles", ClientDependencyType.Css);

        }
    }
}
