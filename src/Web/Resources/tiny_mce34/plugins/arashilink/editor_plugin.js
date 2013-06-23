/**
* ARASHI Link Plugin
*/

(function () {
   tinymce.create('tinymce.plugins.ArashiLinkPlugin', {
      init: function (ed, url) {
         this.editor = ed;

         var arashiCurrentSiteId = currentSiteId;

         // Register commands
         ed.addCommand('mceArashiLink', function () {
            var se = ed.selection;

            // No selection and not in link
            if (se.isCollapsed() && !ed.dom.getParent(se.getNode(), 'A'))
               return;

            ed.windowManager.open({
               file: url + '/link.htm?id=' + arashiCurrentSiteId,
               width: 480 + parseInt(ed.getLang('arashilink.delta_width', 0)),
               height: 400 + parseInt(ed.getLang('arashilink.delta_height', 0)),
               inline: 1
            }, {
               plugin_url: url
            });
         });

         // Register buttons
         ed.addButton('arashilink', {
            title: 'Link',
            image: url + '/img/icons.gif',
            cmd: 'mceArashiLink'
         });

         //ed.addShortcut('ctrl+k', 'arashilink.advlink_desc', 'arashiLink');

         ed.onNodeChange.add(function (ed, cm, n, co) {
            cm.setDisabled('arashilink', co && n.nodeName != 'A');
            cm.setActive('arashilink', n.nodeName == 'A' && !n.name);
         });
      },

      /**
      * Creates control instances based in the incomming name. This method is normally not
      * needed since the addButton method of the tinymce.Editor class is a more easy way of adding buttons
      * but you sometimes need to create more complex controls like listboxes, split buttons etc then this
      * method can be used to create those.
      *
      * @param {String} n Name of the control to create.
      * @param {tinymce.ControlManager} cm Control manager to use inorder to create new control.
      * @return {tinymce.ui.Control} New control instance or null if no control was created.
      */
      createControl: function (n, cm) {
         return null;
      },

      getInfo: function () {
         return {
            longname: 'link',
            author: 'Arashi Project',
            authorurl: 'http://www.arashi-project.com',
            infourl: 'http://www.arashi-project.com',
            version: "1.0"
         };
      }
   });

   // Register plugin
   tinymce.PluginManager.add('arashilink', tinymce.plugins.ArashiLinkPlugin);
})();