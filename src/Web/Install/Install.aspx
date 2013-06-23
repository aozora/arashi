<%@ Page Language="c#" MasterPageFile="~/Install/InstallMaster.Master" AutoEventWireup="false" CodeBehind="Install.aspx.cs" Inherits="Arashi.Web.Install.Install" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   
   <asp:Panel ID="ErrorPanel" CssClass="message ui-widget margin-topbottom" Visible="False" runat="server">
      <div class="ui-state-error ui-helper-clearfix ui-corner-all"> 
         <p>
            <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-alert"></span>
            <asp:Literal ID="ErrorLabel" runat="server"></asp:Literal>
         </p>
      </div>
      <br />
   </asp:Panel>
   <asp:Panel ID="MessagePanel" CssClass="message ui-widget margin-topbottom" Visible="False" runat="server">
      <div class="ui-state-highlight ui-helper-clearfix ui-corner-all"> 
         <p>
            <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-info"></span>
            <asp:Literal ID="MessageLiteral" runat="server"></asp:Literal>
         </p>
      </div>
      <br />
   </asp:Panel>

   <div id="setupbox" class="section ui-widget">
      <div id="setupbox-inner" class="ui-widget-content ui-corner-all ui-shadow">
         <div id="setupbox-title" class="ui-corner-all">
	         <h2 class="align-center">
               <img alt="logo" src="/Resources/img/logo/arashi-project-logo-h43.png"/>
               <img id="setup-logo" alt="setup" src="/Resources/img/setup-logo.png"/>
	         </h2>
         </div>
         <div class="section-body">

            <asp:Panel ID="IntroPanel" runat="server" Visible="False">
               <p>
                  <strong>
                     Welcome to the installation procedure!
                  </strong>
               </p>
               <p>With few simple steps we'll prepare the system for first use.</p>
               <p>Once the setup has finished, you can customize all the settings and add your content.</p>
               <p>
                  <strong>Important:</strong> make sure that in the Web.config file located in the application root the connection string is properly specified.
               </p>
               <br />
               <div class="align-center">
                  <img class="block-align wizard-img" src="/Resources/img/128x128/wizard.png" alt="wizard" />
                  <div class="block-align">
                     <br />
                     <br />
                     <br />
                     <asp:Button ID="StartInstallButton" runat="server" 
                                 CssClass="button call-to-action ui-state-default ui-priority-primary ui-corner-all"
                                 Text="Start the setup" 
                                 OnClick="StartInstallButton_Click">
                     </asp:Button>
                  </div>
               </div>
            </asp:Panel>

            <asp:Panel ID="DatabasePanel" runat="server" Visible="False">
               <ul id="setup-steps" class="block-align" >
                  <li class="current">
                     <div class="ui-widget-header">
                        <img src="/Resources/img/48x48/database.png" alt="database" />
                        <span>Step 1</span>
                     </div>
                  </li>
                  <li>
                     <div class="ui-state-default">
                        <img src="/Resources/img/48x48/web_light.png" alt="site" />
                        <span>Step 2</span>
                     </div>
                  </li>
                  <li>
                     <div class="ui-state-default">
                        <img src="/Resources/img/48x48/user_light.png" alt="admin" />
                        <span>Step 3</span>
                     </div>
                  </li>
               </ul>

               <div id="step-content" class="block-align">
                  <h3>Install Database</h3>
                  <p>The installer will first install the database.</p>
                  <br />
                  <p>The database for the following components wil be installed:</p>
                  <asp:Label ID="lblCoreAssembly" runat="server" Font-Bold="True"></asp:Label>
                  <br />
                  <asp:Label ID="lblModulesAssembly" runat="server" Font-Bold="True"></asp:Label>
                  <br />
                  <div class="align-center">
                     <asp:Button ID="InstallDatabaseButton" runat="server" 
                                 CssClass="button ui-state-default ui-priority-primary ui-corner-all"
                                 Text="Install database" 
                                 OnClick="InstallDatabaseButton_Click">
                     </asp:Button>
                  </div>
                  <br />
               </div>
            </asp:Panel>
            
            <asp:Panel ID="CreateSitePanel" runat="server" Visible="False">
               <ul id="setup-steps" class="block-align" >
                  <li>
                     <div class="ui-state-default">
                        <img src="/Resources/img/48x48/database.png" alt="database" />
                        <span>Step 1</span>
                     </div>
                  </li>
                  <li class="current">
                     <div class="ui-widget-header">
                        <img src="/Resources/img/48x48/web.png" alt="site" />
                        <span>Step 2</span>
                     </div>
                  </li>
                  <li>
                     <div class="ui-state-default">
                        <img src="/Resources/img/48x48/user_light.png" alt="admin" />
                        <span>Step 3</span>
                     </div>
                  </li>
               </ul>

               <div id="step-content" class="block-align ui-form-default">
                  <h3>Create site</h3>
                  <p>Now it's time to create the default site.</p>
                  <p>In the textbox below, please insert the default host name under wich Arashi is configured to run in the web server:</p>
                  <ol>
                     <li class="vertical">
                        <asp:Label AssociatedControlID="SiteHostTextBox" runat="server" EnableViewState="false">Host name</asp:Label>
                        <span>http://</span>
                        <asp:TextBox ID="SiteHostTextBox" runat="server" 
                                     CssClass="mediumtext" 
                                     MaxLength="100"
                                     Text="localhost:8080" />
                        <asp:RequiredFieldValidator ID="SiteHostRequiredFieldValidator" runat="server" 
                                                    CssClass="field-validation-error" 
                                                    EnableClientScript="False" 
                                                    ControlToValidate="SiteHostTextBox" 
                                                    Display="Dynamic" 
                                                    ErrorMessage="The host name is required" />
                     </li>
                  </ol>
                  <br />
                  <div class="align-center">
                     <asp:Button ID="CreateSiteButton" runat="server" 
                                 CssClass="button ui-state-default ui-priority-primary ui-corner-all"
                                 Text="Create a site" 
                                 OnClick="CreateSiteButton_Click">
                     </asp:Button>
                  </div>
                  <br />
                  <asp:HiddenField ID="SiteIdHidden" runat="server" />
               </div>
            </asp:Panel>
            
            <asp:Panel ID="AdminPanel" runat="server" Visible="False">
               <ul id="setup-steps" class="block-align" >
                  <li>
                     <div class="ui-state-default">
                        <img src="/Resources/img/48x48/database.png" alt="database" />
                        <span>Step 1</span>
                     </div>
                  </li>
                  <li>
                     <div class="ui-state-default">
                        <img src="/Resources/img/48x48/web.png" alt="site" />
                        <span>Step 2</span>
                     </div>
                  </li>
                  <li class="current">
                     <div class="ui-widget-header">
                        <img src="/Resources/img/48x48/user.png" alt="admin" />
                        <span>Step 3</span>
                     </div>
                  </li>
               </ul>

               <div id="step-content" class="block-align ui-form-default">
                  <h3>Set administrator password</h3>
                  <p>Please enter a valid email address and a password that will be used for the administrator account.</p>
                  <ol>
                     <li class="vertical">
                        <asp:Label AssociatedControlID="EmailTextBox" runat="server" EnableViewState="false">Email</asp:Label>
                        <asp:TextBox ID="EmailTextBox" runat="server" 
                                     CssClass="mediumtext" />
                        <asp:RequiredFieldValidator ID="EmailRequiredFieldValidator" runat="server" 
                                                    CssClass="field-validation-error" 
                                                    EnableClientScript="False" 
                                                    ControlToValidate="EmailTextBox" 
                                                    Display="Dynamic" 
                                                    ErrorMessage="The email is required" />
                        <asp:RegularExpressionValidator ID="EmailRegularExpressionValidator" runat="server"
                                                    CssClass="field-validation-error" 
                                                    EnableClientScript="False" 
                                                    ControlToValidate="EmailTextBox" 
                                                    Display="Dynamic" 
                                                    ErrorMessage="The email format is not valid"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >
                        </asp:RegularExpressionValidator>
                     </li>
                     <li class="vertical">
                        <asp:Label AssociatedControlID="PasswordTextBox" runat="server" EnableViewState="false">Password</asp:Label>
                        <asp:TextBox ID="PasswordTextBox" runat="server" 
                                     CssClass="mediumtext"
                                     TextMode="Password" />
                        <asp:RequiredFieldValidator ID="PasswordRequiredFieldValidator" runat="server" 
                                                    CssClass="field-validation-error" 
                                                    EnableClientScript="False" 
                                                    ControlToValidate="PasswordTextBox" 
                                                    Display="Dynamic" 
                                                    ErrorMessage="The password is required" />
                     </li>
                     <li class="vertical">
                        <asp:Label AssociatedControlID="ConfirmPasswordTextBox" runat="server" EnableViewState="false">Confirm password</asp:Label>
                        <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" 
                                     CssClass="mediumtext"
                                     TextMode="Password" />
                        <asp:RequiredFieldValidator ID="ConfirmPasswordRequiredFieldValidator" runat="server" 
                                                    CssClass="field-validation-error" 
                                                    EnableClientScript="False" 
                                                    ControlToValidate="ConfirmPasswordTextBox" 
                                                    Display="Dynamic" 
                                                    ErrorMessage="The password is required" />
                        <asp:CompareValidator ID="PasswordCompareValidator" runat="server" 
                                              CssClass="field-validation-error" 
                                              EnableClientScript="False" 
                                              ControlToValidate="ConfirmPasswordTextBox" 
                                              ErrorMessage="The passwords must be the same" 
                                              ControlToCompare="PasswordTextBox" />
                     </li>
                  </ol>
                  
                  <br />
                  <div class="align-center">
                     <asp:Button ID="AdminButton" runat="server" 
                                 CssClass="button ui-state-default ui-priority-primary ui-corner-all"
                                 Text="Create the administrator user" 
                                 OnClick="AdminButton_Click">
                     </asp:Button>
                  </div>
                  <br />
               </div>
            </asp:Panel>
            
            <asp:Panel ID="pnlFinished" CssClass="group" runat="server" Visible="False">
               <div class="align-center">
                  <img class="block-align wizard-img" src="/Resources/img/128x128/clicknrun.png" alt="wizard" />
                  <div class="block-align">
                     <h3>Setup completed!</h3>
                     <p>Arashi is successfully installed!</p>
                     <br />
                     <p>Log in to the site administration with the account you just created</p>
                     <br />
                     <asp:HyperLink ID="hplContinue" runat="server" 
                                    CssClass="button call-to-action ui-state-default ui-priority-primary ui-corner-all"
                                    NavigateUrl="/Admin/Login">
                        Log in
                     </asp:HyperLink>
                  </div>
               </div>
            </asp:Panel>
            
         </div>
      </div>
      <br />
   </div>
</asp:Content> 
