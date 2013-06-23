<%@ Page language="c#" MasterPageFile="~/Install/InstallMaster.Master" AutoEventWireup="True" Codebehind="Upgrade.aspx.cs" Inherits="Arashi.Web.Install.Upgrade" %>
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
               <div class="align-left">
                  <img class="block-align wizard-img" src="/Resources/img/128x128/wizard.png" alt="wizard" />
                  <div class="block-align width-300">
                     <br />
                     <p>
                        <strong>
                           Welcome to the upgrade procedure!
                        </strong>
                     </p>
                     <p>We have detected that some components of Arashi needs to be upgraded to the latest version.</p>
                     <br />
                  </div>
                  <div class="clear"></div>
                  <table class="grid  ui-widget ui-state-default ui-corner-all ui-shadow">
                     <thead>
                        <tr>
                           <th>Component</th>
                           <th class="align-center">Current<br />Version</th>
                           <th class="align-center">New<br />Version</th>
                           <th></th>
                        </tr>
                     </thead>
                     <tbody class="ui-widget-content">
                        <asp:Repeater ID="UpgradeInforepeater" runat="server">
                           <ItemTemplate>
                              <tr>
                                 <td><%# Eval("AssemblyName")%></td>
                                 <td class="align-center"><%# Eval("CurrentVersion") %></td>
                                 <td class="align-center"><%# ((Version)Eval("NewVersion")).ToString(3) %></td>
                                 <td>
                                    <img src='/Resources/img/16x16/<%# Convert.ToBoolean(Eval("CanUpgrade")) ? "light_bulb.png" : "tick.png" %>' alt="" />
                                    <%# Convert.ToBoolean(Eval("CanUpgrade")) ? "needs upgrade" : "already updated" %>
                                 </td>
                              </tr>
                           </ItemTemplate>
                        </asp:Repeater>
                     </tbody>
                  </table>
                  <br />
                  <br />
                  <div class="align-center">
                     <asp:Button ID="StartUpgradeButton" runat="server" 
                                 CssClass="button ui-state-default ui-priority-primary ui-corner-all"
                                 Text="Upgrade!" 
                                 OnClick="StartUpgradeButton_Click">
                     </asp:Button>
                  </div>
               </div>
            </asp:Panel>

            <asp:Panel ID="UpgradeCompletedPanel" CssClass="group" runat="server" Visible="False">
               <div class="align-center">
                  <img class="block-align wizard-img" src="/Resources/img/128x128/clicknrun.png" alt="wizard" />
                  <div class="block-align">
                     <h3>Upgrade completed!</h3>
                     <p>Arashi has been successfully upgraded to the latest version!</p>
                     <br />
                     <p>Now you can log in to the site administration with the account you just created</p>
                     <br />
                     <asp:HyperLink ID="hplContinue" runat="server" 
                                    CssClass="button ui-state-default ui-priority-primary ui-corner-all"
                                    NavigateUrl="/Admin/Login">
                        Log in
                     </asp:HyperLink>
                  </div>
               </div>
            </asp:Panel>

         </div>
      </div>   
   
   </div>
</asp:Content> 
