#ECHO OFF
#D:
#CD D:\dev\Projects\Azora System\trunk\Web.ControlPanel\Resources\js\src
CLS

java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.comments.min.js        admin.comments.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.core.min.js            admin.core.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.mediamanager.min.js    admin.mediamanager.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.mediamanager.edit.min.js    admin.mediamanager.edit.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.mediamanager.upload.min.js    admin.mediamanager.upload.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.messages.min.js        admin.messages.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.page.edit.min.js       admin.page.edit.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.page.new.min.js        admin.page.new.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.post.edit.min.js       admin.post.edit.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.post.index.min.js      admin.post.index.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.post.new.min.js        admin.post.new.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.site.home.min.js       admin.site.home.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.site.settings.min.js   admin.site.settings.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.user.details.min.js    admin.user.details.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o admin.tutorial.min.js        admin.tutorial.js

java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.blockUI.min.js        jquery.blockUI.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.corner.min.js         jquery.corner.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.form.min.js           jquery.form.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.hoverIntent.min.js    jquery.hoverIntent.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.jgrowl.min.js         jquery.jgrowl.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.url.min.js            jquery.url.js

java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.taconite.min.js       jquery.taconite.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.treeview.min.js       jquery.treeview.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.validate.min.js       jquery.validate.js

java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.jqplot.min.js         jquery.jqplot.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o visualize.jQuery.min.js      visualize.jQuery.js

java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.ui.plupload.min.js      jquery.ui.plupload.js
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar --charset utf-8 -o jquery.colorbox.min.js      jquery.colorbox.js


ECHO Minifization done (YUICompressor Version)!
ECHO Premi un tasto per spostare i file min nella cartella js...
PAUSE 
MOVE /Y *.min.js ..
ECHO DONE!!!

PAUSE
